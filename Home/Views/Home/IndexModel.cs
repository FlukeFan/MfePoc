using System.Collections.Generic;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace MfePoc.Home.Views.Home
{
    public static class IndexModel
    {
        public static IEnumerable<string> WarmupUrls(IWebHostEnvironment host)
        {
            var warmupUrls = new List<string>();
            var rootPath = Path.Combine(host.ContentRootPath, "..");

            if (host.IsDevelopment())
                while (!File.Exists(Path.Combine(rootPath, "MfePoc.sln")))
                    rootPath = Path.Combine(rootPath, "..");

            var moduleFiles = Directory.GetFiles(rootPath, "module.xml", SearchOption.AllDirectories);

            foreach (var moduleFile in moduleFiles)
            {
                if (moduleFile.Contains($"{Path.DirectorySeparatorChar}bin"))
                    continue; // skip any build artefacts

                var doc = new XmlDocument();
                doc.Load(moduleFile);

                var warmupElement = (XmlElement)doc.SelectSingleNode("//Warmup");
                warmupUrls.Add(warmupElement.GetAttribute("Url"));
            }

            return warmupUrls;
        }
    }
}
