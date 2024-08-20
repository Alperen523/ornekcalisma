using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.DataSource
{
    public class DataSourceSaveResponse
    {
        /// <summary>
        /// Email id bilgisi
        /// </summary>
        public Guid? Id { get; set; } = null;

        /// <summary>
        /// Müşteri crm id bilgisi
        /// </summary>
        public Guid? CustomerCrmId { get; set; } = null;

    }
}
