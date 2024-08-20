using Autofac.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Metadata;
using System.Runtime.CompilerServices;
using UzmanCrm.CrmService.Application.Abstractions;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.DAL.Config.Abstractions.CRM
{
    public interface IXRMConfigService : IApplicationService
    {
        OrganizationServiceContext OrganizationServiceContextVakko { get; }
        OrganizationServiceContext OrganizationServiceContextLatelier { get; }
        IOrganizationService OrganizationServiceVakko { get; }
        IOrganizationService OrganizationServiceLT { get; }
//OrganizationServiceContext DevServiceContext { get; }
//IOrganizationService DevOrganizationService { get; }
//Guid ControlGuid { get; }
//ResponseList<ResponseCrm<Entity>> BulkInsert(EntityCollection input, IOrganizationService organizationService = null);
//ResponseList<ResponseCrm<Entity>> DevBulkInsert(EntityCollection input, IOrganizationService organizationService = null);
//ResponseList<ResponseCrm<Entity>> BulkUpdate(EntityCollection input, IOrganizationService organizationService = null);
        EntityMetadata GetEntityMetaData(string entityName, CompanyEnum company, ref IOrganizationService service);
        IOrganizationService GetService([CallerMemberName] string callerMethodName = null);
        void ReleaseService(IOrganizationService releasedService);
    }
}
