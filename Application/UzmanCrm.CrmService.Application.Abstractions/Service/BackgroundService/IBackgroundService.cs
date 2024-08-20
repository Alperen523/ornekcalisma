using Hangfire;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.BackgroundService
{
    public interface IBackgroundService
    {
        [DisableConcurrentExecution(10)]
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        Task<Response<object>> Customer_Endorsement_Data_Processing();

        Task<Response<object>> Card_Exception_Discount_Send_Email_By_ArrivalChannel();

        Task<Response<object>> Will_Be_Expired_Soon_Card_Exception_Discount_Send_Email();

        Task<Response<object>> Set_Status_Expired_Today_Card_Exception_Discount();

        [DisableConcurrentExecution(10)]
        [AutomaticRetry(Attempts = 0, LogEvents = false, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        Task<Response<object>> Batch_Approval_List_Data_Processing();
    }
}