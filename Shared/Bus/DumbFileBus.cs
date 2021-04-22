using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MfePoc.Shared.Bus
{
    public class DumbFileBus : IBus, IBusControl
    {
        private readonly ILogger _logger;
        private readonly IServiceProvider _services;
        private readonly string _busFolder;

        private string _name;
        private string _queueFolder;
        private FileSystemWatcher _fsw;

        public DumbFileBus(
            ILogger<DumbFileBus> logger,
            IServiceProvider services)
        {
            _logger = logger;
            _logger.LogInformation($"Bus instantiated");

            _services = services;

            _busFolder = Path.Combine("c:\\temp", "DumbFileBus");
        }

        public async Task PublishAsync<T>(T message)
        {
            var json = JsonConvert.SerializeObject(message, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All,

            });

            var queues = Directory.GetDirectories(_busFolder);

            foreach (var queue in queues)
            {
                var messageId = $"{DateTime.UtcNow.Ticks}_{_name}";
                var tmpFile = Path.Combine(queue, $"{messageId}.tmp");

                await File.WriteAllTextAsync(tmpFile, json);

                var file = Path.Combine(queue, $"{messageId}.txt");
                File.Move(tmpFile, file);
            }
        }

        public async Task StartAsync(string name)
        {
            _name = name;
            _queueFolder = Path.Combine(_busFolder, _name);

            if (!Directory.Exists(_queueFolder))
                Directory.CreateDirectory(_queueFolder);

            _logger.LogInformation($"Incoming messages folder: {_queueFolder}");

            _fsw = new FileSystemWatcher(_queueFolder, "*.txt");
            _fsw.Renamed += (s, e) => OnMessageDetected();
            _fsw.Changed += (s, e) => OnMessageDetected();
            _fsw.Created += (s, e) => OnMessageDetected();
            _fsw.Error += (s, e) => OnMessageDetected();

            await Task.Run(OnMessageDetected);
        }

        private void OnMessageDetected()
        {
            lock (_fsw)
            {
                _fsw.EnableRaisingEvents = false;
                object message = null;

                try
                {
                    var messageFiles = GetMessages();

                    _logger.LogInformation($"Messages: {messageFiles.Length}");

                    var firstMessageFile = messageFiles
                        .OrderBy(m => m)
                        .FirstOrDefault();

                    if (firstMessageFile == null)
                        return;

                    var messageJson = File.ReadAllText(firstMessageFile);
                    File.Delete(firstMessageFile); // non transactional - if we fail after here, message is (deliberately) lost

                    message = JsonConvert.DeserializeObject(messageJson, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    });

                    var messageType = message.GetType();
                    _logger.LogInformation($"Received: {messageType}");

                    var handlerType = typeof(IHandle<>).MakeGenericType(messageType);
                    var handler = _services.GetService(handlerType);

                    if (handler != null)
                    {
                        var method = handlerType.GetMethod("HandleAsync");
                        Task.Run(async () =>
                        {
                            var task = (Task)method.Invoke(handler, new[] { message });
                            await task;
                        });
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error handling '{message}'");
                }
                finally
                {
                    try
                    {
                        if (GetMessages().Length > 1)
                            Task.Run(OnMessageDetected);
                        else
                            _fsw.EnableRaisingEvents = true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error in finally '{message}'");
                    }
                }
            }
        }

        private string[] GetMessages()
        {
            return Directory.GetFiles(_queueFolder, "*.txt");
        }
    }
}
