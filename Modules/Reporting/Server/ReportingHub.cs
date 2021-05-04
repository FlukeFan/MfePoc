using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;

namespace MfePoc.Reporting.Server
{
    public class ReportingHub : Hub
    {
        private readonly ReportingDb _reportingDb;

        public ReportingHub(ReportingDb reportingDb)
        {
            _reportingDb = reportingDb;
        }

        public string RequestHostDetail()
        {
            return $"This response was from process Id={Process.GetCurrentProcess().Id} Framework={RuntimeInformation.FrameworkDescription}";
        }
    }
}
