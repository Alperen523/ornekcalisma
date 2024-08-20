using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Loyalty
{
    public class SaveLoyaltyCardRequestModel
    {
        public string ErpId { get; set; }
        public int CardType { get; set; }
        public string StoreCode { get; set; }
    }
}

