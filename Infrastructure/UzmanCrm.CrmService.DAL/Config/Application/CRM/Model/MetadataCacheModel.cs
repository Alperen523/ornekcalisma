using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.DAL.Config.Application.CRM.Model
{
    public class MetadataCacheModel
    {
        public string EntityName { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public EntityMetadata EntityMetadata { get; set; }
    }
}
