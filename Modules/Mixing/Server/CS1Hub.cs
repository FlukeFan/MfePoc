using System.Diagnostics;
using System.Runtime.InteropServices;
using MfePoc.Mixing.Client;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Mixing.Server
{
    public class CS1Hub : Hub, ClientHub.IRequests
    {
        public string RequestHostDetail()
        {
            return $"This response was from process Id={Process.GetCurrentProcess().Id} Framework={RuntimeInformation.FrameworkDescription}";
        }
    }
}
