using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.CustomerGroupService.Model
{
    public class CustomerGroupGetDto
    {
        public Guid? uzm_customergroupid { get; set; } = null;

        public DateTime? createdon { get; set; } = null;

        public DateTime? modifiedon { get; set; } = null;

        public int? statecode { get; set; } = null;

        public string uzm_name { get; set; } = null;

        public int? uzm_groupcode { get; set; } = null;
    }
}
