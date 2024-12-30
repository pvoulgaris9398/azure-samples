using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Copernicus.Function
{
    public class LocalTimer
    {
        [FunctionName("LocalTimer")]
        public void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
