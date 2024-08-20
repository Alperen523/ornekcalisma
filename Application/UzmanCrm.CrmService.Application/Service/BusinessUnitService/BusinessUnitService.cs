using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.BusinessUnit;

namespace UzmanCrm.CrmService.Application.Service.BusinessUnitService
{
    public class BusinessUnitService : IBusinessUnitService
    {
        private readonly IMapper mapper;
        private readonly IDapperService dapperService;

        public BusinessUnitService(IMapper mapper, IDapperService dapperService)
        {
            this.mapper = mapper;
            this.dapperService = dapperService;
        }

        public async Task<Response<StoreDto>> GetStoreCodeAsync(string storeCode, CompanyEnum company)
        {
            var query = String.Format(@"SELECT [uzm_storeId]
      ,[CreatedOn]
      ,[CreatedBy]
      ,[ModifiedOn]
      ,[ModifiedBy]
      ,[CreatedOnBehalfBy]
      ,[ModifiedOnBehalfBy]
      ,[OwnerId]
      ,[OwnerIdType]
      ,[OwningBusinessUnit]
      ,[statecode]
      ,[statuscode]
      ,[VersionNumber]
      ,[ImportSequenceNumber]
      ,[OverriddenCreatedOn]
      ,[TimeZoneRuleVersionNumber]
      ,[UTCConversionTimeZoneCode]
      ,[uzm_name]
      ,[uzm_code]
      ,[uzm_countryid]
      ,[uzm_regionid]
      ,[uzm_regionname]
      ,[uzm_cityid]
      ,[uzm_cityname]
      ,[uzm_countyid]
      ,[uzm_countyname]
      ,[uzm_distributionchannel]
      ,[uzm_latitude]
      ,[uzm_longitude]
      ,[uzm_isxlstore]
      ,[uzm_istouristic]
      ,[uzm_location]
      ,[uzm_pentiyoung]
      ,[uzm_cashregistercount]
      ,[uzm_storecode]
      ,[uzm_casename]
      ,[uzm_phonenumber]
      ,[uzm_storeopeninghours]
      ,[uzm_storeclosingtime]
      ,[uzm_grabandgostore]
      ,[uzm_visibleonmobileapp]
      ,[uzm_segment]
      ,[uzm_address]
      ,[uzm_outdoorseatingarea]
      ,[uzm_carpark]
      ,[uzm_remoteorder]
      ,[uzm_wifi]
      ,[uzm_icecreamproduction]
      ,[uzm_mondayopeningtime]
      ,[uzm_mondayclosingtime]
      ,[uzm_tuesdayopeningtime]
      ,[uzm_tuesdayclosingtime]
      ,[uzm_wednesdayopeningtime]
      ,[uzm_wednesdayclosingtime]
      ,[uzm_thursdayopeningtime]
      ,[uzm_thursdayclosingtime]
      ,[uzm_fridayopeningtime]
      ,[uzm_fridayclosingtime]
      ,[uzm_saturdayopeningtime]
      ,[uzm_saturdayclosingtime]
      ,[uzm_sondayopeningtime]
      ,[uzm_sondayclosingtime]
      ,[uzm_storeopeningdate]
      ,[uzm_storeemailaddress]
      ,[uzm_chocolateproduction]
      ,[uzm_storegroupid]
      ,[uzm_storeconceptid]
      ,[uzm_regionalmanagerid]
      ,[uzm_districtid]
      ,[uzm_storemanagerid]
  FROM [uzm_storeBase] with(nolock)
                                        WHERE statecode = 0 AND
                                        uzm_storecode = @storeCode");

            var resService = await dapperService.GetItemParam<object, StoreDto>(query, new { storeCode = storeCode }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<StoreDto>>(resService);

            return response;
        }

        /// <summary>
        /// Get BusinessUnit list
        /// </summary>
        /// <returns></returns>
        public async Task<Response<List<BusinessUnitDto>>> GetBusinessUnitListAsync(CompanyEnum company)
        {
            var query = String.Format(@"SELECT
                                            modifiedbyname,
                                            createdbyname,
                                            OrganizationIdName,
                                            TransactionCurrencyIdName,
                                            ParentBusinessUnitIdName,
                                            uzm_bucountryidname,
                                            uzm_bucityidname,
                                            address1_addresstypecode,
                                            address1_city,
                                            address1_composite,
                                            address1_country,
                                            address1_county,
                                            address1_fax,
                                            address1_AddressId,
                                            address1_latitude,
                                            address1_line1,
                                            address1_line2,
                                            address1_line3,
                                            address1_longitude,
                                            address1_name,
                                            address1_postalcode,
                                            address1_postofficebox,
                                            address1_shippingmethodcode,
                                            address1_stateorprovince,
                                            address1_telephone1,
                                            address1_telephone2,
                                            address1_telephone3,
                                            address1_upszone,
                                            address1_utcoffset,
                                            address2_addresstypecode,
                                            address2_city,
                                            address2_composite,
                                            address2_country,
                                            address2_county,
                                            address2_fax,
                                            address2_AddressId,
                                            address2_latitude,
                                            address2_line1,
                                            address2_line2,
                                            address2_line3,
                                            address2_longitude,
                                            address2_name,
                                            address2_postalcode,
                                            address2_postofficebox,
                                            address2_shippingmethodcode,
                                            address2_stateorprovince,
                                            address2_telephone1,
                                            address2_telephone2,
                                            address2_telephone3,
                                            address2_upszone,
                                            address2_utcoffset,
                                            BusinessUnitId,
                                            OrganizationId,
                                            UserGroupId,
                                            name,
                                            description,
                                            divisionname,
                                            fileasname,
                                            tickersymbol,
                                            stockexchange,
                                            utcoffset,
                                            createdon,
                                            modifiedon,
                                            creditlimit,
                                            costcenter,
                                            websiteurl,
                                            ftpsiteurl,
                                            emailaddress,
                                            InheritanceMask,
                                            createdby,
                                            modifiedby,
                                            workflowsuspended,
                                            ParentBusinessUnitId,
                                            IsDisabled,
                                            disabledreason,
                                            picture,
                                            CalendarId,
                                            ImportSequenceNumber,
                                            TransactionCurrencyId,
                                            exchangerate,
                                            uzm_accountcode,
                                            uzm_markettype,
                                            uzm_channeltype,
                                            uzm_isrefit,
                                            uzm_refitstartdate,
                                            uzm_refitenddate,
                                            uzm_transfereestorecode,
                                            uzm_seasontype,
                                            uzm_bucountryid,
                                            uzm_bucityid,
                                            uzm_shortname,
                                            uzm_openingdate,
                                            uzm_closingdate
                                        FROM
                                            BusinessUnit with(nolock)
                                        WHERE                                           
                                            IsDisabled = 0");

            var resService = await dapperService.GetListByParamAsync<object, BusinessUnit>(query, null, GeneralHelper.GetCrmConnectionStringByCompany(company));

            var response = mapper.Map<Response<List<BusinessUnitDto>>>(resService);

            return response;
        }

        public async Task<Response<BusinessUnitDto>> GetBusinessUnitById(Guid id, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
                                            modifiedbyname,
                                            createdbyname,
                                            OrganizationIdName,
                                            TransactionCurrencyIdName,
                                            ParentBusinessUnitIdName,
                                            uzm_bucountryidname,
                                            uzm_bucityidname,
                                            address1_addresstypecode,
                                            address1_city,
                                            address1_composite,
                                            address1_country,
                                            address1_county,
                                            address1_fax,
                                            address1_AddressId,
                                            address1_latitude,
                                            address1_line1,
                                            address1_line2,
                                            address1_line3,
                                            address1_longitude,
                                            address1_name,
                                            address1_postalcode,
                                            address1_postofficebox,
                                            address1_shippingmethodcode,
                                            address1_stateorprovince,
                                            address1_telephone1,
                                            address1_telephone2,
                                            address1_telephone3,
                                            address1_upszone,
                                            address1_utcoffset,
                                            address2_addresstypecode,
                                            address2_city,
                                            address2_composite,
                                            address2_country,
                                            address2_county,
                                            address2_fax,
                                            address2_AddressId,
                                            address2_latitude,
                                            address2_line1,
                                            address2_line2,
                                            address2_line3,
                                            address2_longitude,
                                            address2_name,
                                            address2_postalcode,
                                            address2_postofficebox,
                                            address2_shippingmethodcode,
                                            address2_stateorprovince,
                                            address2_telephone1,
                                            address2_telephone2,
                                            address2_telephone3,
                                            address2_upszone,
                                            address2_utcoffset,
                                            BusinessUnitId,
                                            OrganizationId,
                                            UserGroupId,
                                            name,
                                            description,
                                            divisionname,
                                            fileasname,
                                            tickersymbol,
                                            stockexchange,
                                            utcoffset,
                                            createdon,
                                            modifiedon,
                                            creditlimit,
                                            costcenter,
                                            websiteurl,
                                            ftpsiteurl,
                                            emailaddress,
                                            InheritanceMask,
                                            createdby,
                                            modifiedby,
                                            workflowsuspended,
                                            ParentBusinessUnitId,
                                            IsDisabled,
                                            disabledreason,
                                            picture,
                                            CalendarId,
                                            ImportSequenceNumber,
                                            TransactionCurrencyId,
                                            exchangerate,
                                            uzm_accountcode,
                                            uzm_markettype,
                                            uzm_channeltype,
                                            uzm_isrefit,
                                            uzm_refitstartdate,
                                            uzm_refitenddate,
                                            uzm_transfereestorecode,
                                            uzm_seasontype,
                                            uzm_bucountryid,
                                            uzm_bucityid,
                                            uzm_shortname,
                                            uzm_openingdate,
                                            uzm_closingdate
                                        FROM
                                            BusinessUnit with(nolock)
                                        WHERE
                                            BusinessUnitId = @id AND
                                            IsDisabled = 0");

            var resService = await dapperService.GetItemParam<object, BusinessUnit>(query, new { id = id }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<BusinessUnitDto>>(resService);

            return response;
        }
    }
}
