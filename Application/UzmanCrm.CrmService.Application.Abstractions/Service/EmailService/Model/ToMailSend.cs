using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.EmailService.Model
{
    public class ToMailSend
    {
        public Guid uzm_customreportmailid { get; set; }
        public string uzm_email { get; set; }
        public string uzm_mailtype { get; set; }
        public string uzm_report { get; set; }
    }
}
