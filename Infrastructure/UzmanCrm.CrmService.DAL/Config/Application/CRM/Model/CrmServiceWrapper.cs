using Microsoft.Xrm.Sdk;
using System;

namespace UzmanCrm.CrmService.DAL.Config.Application.CRM.Model
{
    public class CrmServiceWrapper
	{
		public IOrganizationService Service { get; set; }
		public bool IsInUse { get; set; }
		public DateTime TimeInstance { get; set; }
		public DateTime? UsedDate { get; set; }
		public string CallerMethodName { get; set; }
	}
}
