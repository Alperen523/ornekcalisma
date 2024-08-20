using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService
{
    public interface IEmailService : IApplicationService
    {

        Response<EmailResponse> SendEmail(string _subject, string _body, string ToType, string[] _portalUserId = null, List<string> _toMail = null, List<string> _ccMail = null, object _attachment = null, string _attachtype = "", string _attachname = "");

        Task<Response<EmailSaveResponse>> EmailSaveAsync(EmailSaveRequestDto requestDto);
    }
}