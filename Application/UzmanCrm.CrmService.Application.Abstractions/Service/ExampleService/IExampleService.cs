using System;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.ExampleService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ExampleService
{
	public interface IExampleService : IApplicationService
	{
		//method interface example
		Task<ExampleEntityDto> ExampleMethodAsync();

		Task<Response<Guid>> DateTestSave(DataTestDto dataTestDto);

	}
}
