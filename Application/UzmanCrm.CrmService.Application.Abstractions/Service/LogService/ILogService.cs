using System.Threading.Tasks;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.LogService
{
    public interface ILogService : IApplicationService
    {
        Task LogSave(LogEventEnum logEvent, string LoggerName, string MethodName, CompanyEnum company,
            LogTypeEnum logtype, object MessageAndModel);
        Task LogSaveSync(LogEventEnum logEvent, string LoggerName, string MethodName, CompanyEnum company,
            LogTypeEnum logtype, object MessageAndModel);
    }
}
