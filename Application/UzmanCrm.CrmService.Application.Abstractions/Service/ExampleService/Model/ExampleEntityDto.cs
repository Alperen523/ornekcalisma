using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ExampleService.Model
{
	public class ExampleEntityDto : BaseSchemaDto<Guid>
	{
		public string FirstName { get; set; }
	}
}
