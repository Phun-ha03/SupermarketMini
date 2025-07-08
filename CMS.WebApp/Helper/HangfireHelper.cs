using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace CMS.WebApp.Helper
{
    public static class HangfireHelper
    {
        public static void StartRecurringJobs(this WebApplication app)
        {
            //var jobQueueExecutionService = app.Services.GetService<IJobQueueExecutionService>();
            //RecurringJob.AddOrUpdate("Job_Queue_Execution", () => jobQueueExecutionService.Execute(), Cron.MinuteInterval(1));
        }
    }
}
