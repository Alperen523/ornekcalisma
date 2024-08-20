using Hangfire;
using Hangfire.SqlServer;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using UzmanCrm.CrmService.Hangfire.Helper;

[assembly: OwinStartup(typeof(UzmanCrm.CrmService.Hangfire.Startup))]

namespace UzmanCrm.CrmService.Hangfire
{
    public class Startup
    {
        private IEnumerable<IDisposable> GetHangfireServers()
        {
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            string strcon = isTest ? ConfigurationManager.ConnectionStrings["JOB_TEST"].ConnectionString : ConfigurationManager.ConnectionStrings["JOB"].ConnectionString;

            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(strcon, new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                });

            yield return new BackgroundJobServer();
        }

        /// <summary>
        ///  HangFireJob.DoJob(); Scheduler olmayan yerlerde comment alınması gerekiyor. Sadece sunucu içerisinden dışa kapalı olarak çalışması gerekir.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            app.UseHangfireAspNet(GetHangfireServers);
            app.UseHangfireDashboard();

            HangFireJob.DoJob();

        }

    }
}