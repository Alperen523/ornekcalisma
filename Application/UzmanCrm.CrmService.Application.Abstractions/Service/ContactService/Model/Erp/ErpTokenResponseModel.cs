using System.Collections.Generic;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Erp
{
    public class ErpTokenResponseModel
    {
        public string access_token { get; set; } = null;
        public string token_type { get; set; } = null;
        public string refresh_token { get; set; } = null;
        public int expires_in { get; set; }
        public string scope { get; set; } = null;
        public List<object> resourceGroups { get; set; }
        public int id { get; set; }
    }
}
