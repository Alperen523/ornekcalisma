using Newtonsoft.Json;
using NLog;
using System;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Infrastructure.NLogConfigService;

namespace UzmanCrm.CrmService.Application.Service.LogService
{
    public class LogService : ILogService
    {
        private readonly ILogger logger;
        public Guid instanceId;
        public LogService(ILogger logger)
        {
            this.logger = logger;
            instanceId = Guid.NewGuid();
        }

        public async Task LogSave(LogEventEnum logEvent, string LoggerName, string MethodName, CompanyEnum company, LogTypeEnum logtype,
            object MessageAndModel)
        {
            try
            {
                var jsonText = JsonConvert.SerializeObject(MessageAndModel);

                LogLevel loglevel = LogLevel.Info;
                switch (logEvent)
                {
                    case LogEventEnum.DbInfo:
                        loglevel = LogLevel.Info;
                        break;
                    case LogEventEnum.DbError:
                        loglevel = LogLevel.Error;
                        break;
                    case LogEventEnum.DbWarning:
                        loglevel = LogLevel.Warn;
                        break;
                    case LogEventEnum.FileInfoLog:
                        loglevel = LogLevel.Trace;
                        break;
                    default:
                        break;

                }

                logger.Log(NLogConfigService.Logger(loglevel, instanceId.ToString(), LoggerName, MethodName, company.ToString(), logtype.ToString(), jsonText));
            }
            catch (System.Exception ex)
            {

                logger.Log(NLogConfigService.Logger(NLog.LogLevel.Error, instanceId.ToString(), LoggerName, MethodName, company.ToString(), logtype.ToString(), ex.ToString()));
            }



        }

        public async Task LogSaveSync(LogEventEnum logEvent, string LoggerName, string MethodName, CompanyEnum company, LogTypeEnum logtype,
            object MessageAndModel)
        {
            try
            {
                var jsonText = JsonConvert.SerializeObject(MessageAndModel);

                LogLevel loglevel = LogLevel.Info;
                switch (logEvent)
                {
                    case LogEventEnum.DbInfo:
                        loglevel = LogLevel.Info;
                        break;
                    case LogEventEnum.DbError:
                        loglevel = LogLevel.Error;
                        break;
                    case LogEventEnum.DbWarning:
                        loglevel = LogLevel.Warn;
                        break;
                    case LogEventEnum.FileInfoLog:
                        loglevel = LogLevel.Trace;
                        break;
                    default:
                        break;

                }

                logger.Log(NLogConfigService.Logger(loglevel, instanceId.ToString(), LoggerName, MethodName, company.ToString(), logtype.ToString(), jsonText));
            }
            catch (System.Exception ex)
            {

                logger.Log(NLogConfigService.Logger(NLog.LogLevel.Error, instanceId.ToString(), LoggerName, MethodName, company.ToString(), logtype.ToString(), ex.ToString()));
            }



        }



        #region Log Tablosu SQL scripti


        //CREATE TABLE[dbo].[logs] (    [Id][int] IDENTITY(1,1) NOT NULL,  [InstanceId] [nvarchar] (40) NULL,	[Date] [datetime] NULL,	[Level] [nvarchar] (500) NULL,	[Message] [nvarchar] (max) NULL, [MachineName] [nvarchar] (300) NULL,	[Ipaddress] [nvarchar] (30) NULL,	[Macaddress] [nvarchar] (500) NULL,	[Organization] [nvarchar] (100) NULL,	[Logger] [nvarchar] (500) NULL,	[Methodname] [nvarchar] (500) NULL,	[Type] [nvarchar] (100) NULL, CONSTRAINT[PK__logs__3214EC074532B342] PRIMARY KEY CLUSTERED([Id] ASC)WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 70) ON[PRIMARY]) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY]  GO

        #endregion
    }
}
