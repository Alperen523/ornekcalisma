using System.Collections.Generic;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model
{
    public class EmailRequestDto
    {
        public string Subject { get; set; } = null;
        public string Message { get; set; } = null;
        public string ToType { get; set; } = null;
        public string[] PortalUserId { get; set; } = null;
        public List<string> MailList { get; set; } = null;
        public object Attachment { get; set; } = null;
        public string AttachType { get; set; } = null;
        public string AttachName { get; set; } = null;
    }
}