namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Erp
{
    public class ErpSaveCustomerResponseModel
    {
        public string erpId { get; set; }
        public bool error { get; set; }
        public string message { get; set; }
        public string customerNo { get; set; }
        public string cardNo { get; set; }
    }
}
