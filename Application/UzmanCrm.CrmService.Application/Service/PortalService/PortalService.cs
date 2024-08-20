using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.PortalService;
using UzmanCrm.CrmService.Application.Abstractions.Service.PortalService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;

namespace UzmanCrm.CrmService.Application.Service.PortalService
{
    public class PortalService : IPortalService
    {
        private readonly IDapperService _dapperService;

        public PortalService(IDapperService dapperService)
        {
            _dapperService = dapperService;
        }

        public async Task<Response<PortalUserResponseDto>> GetPortalUserAndApprovedBy(PortalUserRequestDto req)
        {
            //var query = @$"SELECT pu.uzm_fullname, pu.uzm_portaluserId, pu.uzm_username,pu.uzm_approvingsupervisorid, pub.uzm_firstname,pub.uzm_lastname
            //               FROM [uzm_portaluserBase] pu WITH(NOLOCK)
            //               LEFT JOIN uzm_portaluserBase pub ON pub.uzm_portaluserId = pu.uzm_approvingsupervisorid
            //               WHERE pu.uzm_portaluserId = '{req.uzm_portaluserid}'";
            var query = $@"SELECT 
                           	  pu.uzm_portaluserid,
                              pu.uzm_fullname,
                           	  pu.statecode,
                              pu.statecodename,
                              pu.statuscode,
                              pu.uzm_storecode,
                              pu.uzm_storeid,
                              pu.uzm_storeidname,
                              pu.uzm_organizationid,
                              pu.uzm_organizationidname,
                              pu.uzm_suborganization,
                              pu.uzm_suborganizationname,
                              pu.uzm_username,
                              bu.uzm_cardexceptiondiscountapprover,
                           	  bu.uzm_cardexceptiondiscountapproverName,
                              bu.BusinessUnitId
                           FROM Filtereduzm_portaluser pu WITH(NOLOCK)
                           JOIN BusinessUnit bu ON bu.BusinessUnitId = pu.uzm_storeid
                           WHERE pu.uzm_portaluserid = '{req.uzm_portaluserid}'";
            var response = await _dapperService.GetItemParam<PortalUserRequestDto, PortalUserResponseDto>(query, req, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
            return response;
        }
    }
}
