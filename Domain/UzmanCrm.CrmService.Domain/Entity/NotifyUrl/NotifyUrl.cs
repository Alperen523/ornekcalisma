using System;

namespace UzmanCrm.CrmService.Domain.Entity.NotifyUrl
{
    public class NotifyUrl
    {
        public int Id { get; set; }
        public DateTime SmsSentDate { get; set; }
        public string MobilePhone { get; set; }
        public Guid ContactId { get; set; }
        public int DeliveryStatus { get; set; }
        public DateTime DeliveryDate { get; set; }
        public Guid CampanyId { get; set; }
        public string Message { get; set; }
        public string MessageId { get; set; }
        public string StatusName { get; set; }
        public string StatusDescription { get; set; }
        public string ErrorName { get; set; }
        public string Error { get; set; }
        public string CurrencyType { get; set; }
        public int Channel { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
