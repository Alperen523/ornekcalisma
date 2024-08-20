using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Models;
using UzmanCrm.CrmService.Application.Abstractions.Service.BusinessUnitService;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Application.Abstractions.Service.UserService;
using UzmanCrm.CrmService.Application.Helper;
using UzmanCrm.CrmService.Common;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Abstractions.CRM;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.DAL.Config.Application.Common;
using UzmanCrm.CrmService.DAL.Config.Application.Dapper;
using UzmanCrm.CrmService.Domain.Entity.CRM.Adress;
using UzmanCrm.CrmService.Domain.Entity.CRM.Contact;

namespace UzmanCrm.CrmService.Application.Service.AddressService
{
    public class AddressService : IAddressService
    {
        private readonly IMapper mapper;
        private readonly ICRMService crmService;
        private readonly ILogService logService;
        private readonly IDapperService dapperService;
        private readonly IUserService userService;
        private readonly IBusinessUnitService businessService;


        public static List<CountryDto> staticCountryList = null;
        public static List<CityDto> staticCityList = null;
        public static List<NeighborhoodDto> staticNeighborhoodList = null;
        public static List<DistrictDto> staticDistrictList = null;

        public AddressService(IMapper mapper, ICRMService crmService, ILogService logService, IDapperService dapperService, IUserService userService, IBusinessUnitService businessService)
        {
            this.crmService = crmService;
            this.logService = logService;
            this.dapperService = dapperService;
            this.userService = userService;
            this.businessService = businessService;
            this.mapper = mapper;
        }

        #region ADDRESS_PROCESS

        public async Task<Response<AddressSaveResponseDto>> AddressSaveAsync(AddressSaveRequestDto requestDto)
        {
            await logService.LogSave(LogEventEnum.FileInfoLog, this.GetType().Name, nameof(AddressSaveAsync), CompanyEnum.KD, LogTypeEnum.Request, requestDto);

            var responseModel = new Response<AddressSaveResponseDto>();

            #region Personel ve Lokasyon Bilgileri doğrumu kontrolleri...

            var portalUserResponse = await userService.GetEmployeeNumberAsync(requestDto.Location, CompanyEnum.KD);
            if (!portalUserResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<AddressSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.PersonnelNotFound, ErrorStaticConsts.SearchErrorStaticConsts.S010)));
            }
            else
                requestDto.PersonId = portalUserResponse.Data.uzm_employeeid;

            var businessResponse = await businessService.GetStoreCodeAsync(requestDto.Location, CompanyEnum.KD);
            if (!businessResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<AddressSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.LocationNotFound, ErrorStaticConsts.SearchErrorStaticConsts.S011)));
            }
            else
                requestDto.LocationId = businessResponse.Data.uzm_storeid;

            var crmIdResponse = await GetContactByErpIdAsync(requestDto);
            if (!crmIdResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<AddressSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.CustomerErpNotFound, ErrorStaticConsts.LoyaltyCardStaticConsts.LC0003)));
            }
            else
                requestDto.CustomerCrmId = crmIdResponse.Data.uzm_customerid;



            #endregion

            #region ADDRESS_DATA
            var cityResponse = new Response<CityDto>();
            var districtResponse = new Response<DistrictDto>();
            var neighborhoodResponse = new Response<NeighborhoodDto>();
            var countryResponse = await GetCountryByCodeAsync(requestDto.CountryCode, CompanyEnum.KD);
            if (countryResponse.Success && countryResponse.Data != null)
            {
                cityResponse = await GetCityByCodeAsync(requestDto.CityCode, countryResponse.Data.uzm_countriesid.ToString(), CompanyEnum.KD);
                if (cityResponse.Success)
                {
                    districtResponse = await GetDistrictByCodeAsync(requestDto.DistrictCode, cityResponse.Data.uzm_citiesid.ToString(), CompanyEnum.KD);
                    if (districtResponse.Success)
                        neighborhoodResponse = await GetNeighborhoodByCodeAsync(requestDto.NeighborhoodCode, districtResponse.Data.uzm_districtid.ToString(), CompanyEnum.KD);
                }
            }


            #endregion ADDRESS_DATA

            #region ADRESS_PARAMETERS_CONVERT

            if (countryResponse.Success && countryResponse.Data != null)
                requestDto.CountryId = countryResponse.Data.uzm_countriesid;


            if (cityResponse.Success && cityResponse.Data != null)
                requestDto.CityId = cityResponse.Data.uzm_citiesid;


            if (districtResponse.Success && districtResponse.Data != null)
                requestDto.DistrictId = districtResponse.Data.uzm_districtid;


            if (neighborhoodResponse.Success && neighborhoodResponse.Data != null)
                requestDto.NeighborhoodId = neighborhoodResponse.Data.uzm_neighborhoodsid;

            #endregion ADRESS_PARAMETERS_CONVERT

            var addressDto = mapper.Map<AddressDto>(requestDto);
            var resService = await GetAddressItemAsync(requestDto);
            if (resService.Success)
            {
                addressDto.uzm_customeraddressid = resService.Data.uzm_customeraddressid;
                addressDto.uzm_createdlocationid = null;
                addressDto.uzm_createdbypersonid = null;
                responseModel.Data.Type = CreateType.Update;
            }
            else
                responseModel.Data.Type = CreateType.Create;

            try
            {
                var entityModel = mapper.Map<Address>(addressDto);
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var result = crmService.Save<Address>(entityModel, "uzm_customeraddress", "uzm_customeraddress", CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Data.CrmId = addressDto.uzm_customerid;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;

                await logService.LogSave(LogEventEnum.FileInfoLog, this.GetType().Name, nameof(AddressSaveAsync), CompanyEnum.KD, LogTypeEnum.Response, responseModel);

                if (result.Success)
                {
                    var updateContactAddress = await UpdateContactAddressAsync(requestDto);
                }

            }
            catch (Exception ex)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<AddressSaveResponseDto>(new ErrorModel(System.Net.HttpStatusCode.NotFound,
                    CommonStaticConsts.Message.AddressSaveError + ex.ToString(), "")));
            }

            return responseModel;
        }
        private async Task<Response<int>> UpdateContactAddressAsync(AddressSaveRequestDto requestDto)
        {
            //Satır Pasif gönderildi ise adres listesinden son aktif adresi çek ve son aktif adres ile hem müşteri kartını hemde ovm talosuna update geçiyoruz..
            #region UpdateLastActiveAddress

            if (requestDto.StatusEnum == StatusType.Pasif)
            {
                var query = String.Format(@"
							select top 1 uzm_customeraddressId,uzm_countryid,uzm_cityid,uzm_districtid,uzm_neighborhoodid,uzm_fulladdress,uzm_postcode,uzm_isdefaultaddress
							from uzm_customeraddressBase with(nolock)
                            where uzm_customerId=@CustomerCrmId and uzm_isdefaultaddress=1  and statecode=0 order by ModifiedOn desc");
                var lastAddressRow = await dapperService.GetItemParam<AddressSaveRequestDto, AddressDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);
                if (lastAddressRow.Success && lastAddressRow.Data != null)
                {
                    requestDto.CountryId = lastAddressRow.Data?.uzm_countryid;
                    requestDto.CityId = lastAddressRow.Data?.uzm_cityid;
                    requestDto.DistrictId = lastAddressRow.Data?.uzm_districtid;
                    requestDto.NeighborhoodId = lastAddressRow.Data?.uzm_neighborhoodid;
                    requestDto.AddressLine = lastAddressRow.Data.uzm_fulladdress;
                    requestDto.PostCode = lastAddressRow.Data.uzm_postcode;
                    requestDto.IsDefaultAddress = lastAddressRow.Data.uzm_isdefaultaddress;

                    #region ADDRESS_DATA
                    var countryResponse = await GetCountryByIdAsync(requestDto.CountryId.Value, CompanyEnum.KD);
                    requestDto.CountryCode = (CountryCodeEnum)Enum.Parse(typeof(CountryCodeEnum), countryResponse?.Data?.uzm_countrycode);
                    var cityResponse = await GetCityByIdAsync(requestDto.CityId.Value, CompanyEnum.KD);
                    requestDto.CityCode = cityResponse?.Data?.uzm_citycode;
                    var districtResponse = await GetDistrictByIdAsync(requestDto.DistrictId.Value, CompanyEnum.KD);
                    requestDto.DistrictCode = districtResponse?.Data?.uzm_districtcode;
                    var neighborhoodResponse = await GetNeighborhoodByIdAsync(requestDto.NeighborhoodId ?? Guid.Empty, CompanyEnum.KD);
                    requestDto.NeighborhoodCode = neighborhoodResponse?.Data?.uzm_neighborhoodcode.ToString();
                    #endregion ADDRESS_DATA

                }
            }

            #endregion

            string queryContactSet = "";
            string querySet = "";
            if (requestDto?.IsDefaultAddress != null && (bool)requestDto?.IsDefaultAddress)
            {
                var entityContact = new Contact();
                entityContact.contactid = requestDto.CustomerCrmId;
                //entityContact.uzm_contact_countryid = requestDto.CountryId;
                //entityContact.uzm_contact_cityid = requestDto.CityId;
                //entityContact.uzm_contact_districtid = requestDto.DistrictId;
                //entityContact.uzm_neighborhoodid = requestDto.NeighborhoodId;
                //entityContact.uzm_address1_line1 = requestDto.AddressLine;
                //entityContact.uzm_postcode = requestDto.PostCode;
                var result = crmService.Save<Contact>(entityContact, "contact", "contact", CompanyEnum.KD);

                #region Ükle,İl,İlçe Sql ile crm özet adres bilgilerini güncelle...

                if (requestDto.CountryId == null)
                    querySet += "uzm_contact_countryid=null,";
                if (requestDto.CityId == null)
                    querySet += "uzm_contact_cityid=null,";
                if (requestDto.DistrictId == null)
                    querySet += "uzm_contact_districtid=null,";
                if (requestDto.NeighborhoodCode == null)
                    querySet += "uzm_neighborhoodid=null,";
                if (requestDto.AddressLine == null)
                    querySet += "uzm_address1_line1=null,";
                if (requestDto.PostCode == null)
                    querySet += "uzm_postcode=null,";


                if (querySet != string.Empty)
                {
                    querySet = querySet.Remove(querySet.Length - 1, 1);
                    queryContactSet = string.Format(@"Update c SET {0} from KahveDunyasi_MSCRM..ContactBase c where ContactId=@CustomerCrmId  select @@ROWCOUNT ", querySet);
                }

                #endregion
            }
            var queryOvm = queryContactSet + @"Update c SET CountryId=@CountryCode,CityId=@CityCode,DisctrictId=@DistrictCode,NeighborhoodId=@NeighborhoodCode from Customer c 
            where CrmId=@CustomerCrmId select @@ROWCOUNT";

            var responseOvm = await dapperService.SaveQueryParam<AddressSaveRequestDto, int>(queryOvm, requestDto, GeneralHelper.GetOvmConnectionStringByCompany(CompanyEnum.KD));

            return responseOvm;
        }
        private async Task<Response<AddressDto>> GetAddressItemAsync(AddressSaveRequestDto requestDto)
        {
            var query = String.Format(@"select uzm_customeraddressId from uzm_customeraddressBase a with(nolock) 
                                        inner join KahveDunyasi_MSCRM..uzm_customerdatasourceBase d with(nolock) on d.uzm_customerexternalid='4720'
                                        where 
									    uzm_customerId=@CustomerCrmId and
										uzm_addressecomidstr=@AddressId and uzm_createdlocationid=@LocationId and a.statecode=0 and
										d.uzm_datasourceid=@LocationId and ISNULL(d.uzm_unusedflag,0)=0 and d.statecode=0
                                        ");
            var resService = await dapperService.GetItemParam<object, AddressDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;

        }
        private async Task<Response<AddressDto>> GetContactByErpIdAsync(AddressSaveRequestDto requestDto)
        {
            var query = String.Format(@"select ContactId as uzm_customerid from ContactBase a with(nolock) where uzm_ErpId=@ErpId and a.statecode=0");
            var resService = await dapperService.GetItemParam<object, AddressDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;

        }



        public async Task<Response<DeleteAddressResponseDto>> DeleteAddressAsync(DeleteAddressRequestDto requestDto)
        {

            await logService.LogSave(LogEventEnum.FileInfoLog, this.GetType().Name, nameof(DeleteAddressAsync), CompanyEnum.KD, LogTypeEnum.Request, requestDto);
            var ErpId = "";
            var responseModel = new Response<DeleteAddressResponseDto>();

            #region Personel ve Lokasyon Bilgileri doğrumu kontrolleri...

            var portalUserResponse = await userService.GetEmployeeNumberAsync(requestDto.Location, CompanyEnum.KD);
            if (!portalUserResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<DeleteAddressResponseDto>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.PersonnelNotFound, ErrorStaticConsts.SearchErrorStaticConsts.S010)));
            }
            else
                requestDto.PersonId = portalUserResponse.Data.uzm_employeeid;

            var businessResponse = await businessService.GetStoreCodeAsync(requestDto.Location, CompanyEnum.KD);
            if (!businessResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<DeleteAddressResponseDto>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.LocationNotFound, ErrorStaticConsts.SearchErrorStaticConsts.S011)));
            }
            else
                requestDto.LocationId = businessResponse.Data.uzm_storeid;

            var crmIdResponse = await GetDeleteAddressIdAsync(requestDto);
            if (!crmIdResponse.Success)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<DeleteAddressResponseDto>(new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.Customer_EcomId_AddresId_LocationId_NotFound, ErrorStaticConsts.LoyaltyCardStaticConsts.LC0003)));
            }
            else
            {
                requestDto.CustomerCrmId = crmIdResponse.Data.uzm_customerid;
                requestDto.AddressCrmId = crmIdResponse.Data.uzm_customeraddressid;
                ErpId = crmIdResponse.Data.uzm_ErpId;

            }

            #endregion

            try
            {
                requestDto.StatusEnum = StatusType.Pasif;
                var addressDto = mapper.Map<AddressDto>(requestDto);


                var entityModel = mapper.Map<Address>(addressDto);
                entityModel = ContactHelper.EntityModelSetStateAndStatusCode(requestDto, entityModel);

                var result = crmService.Save<Address>(entityModel, "uzm_customeraddress", "uzm_customeraddress", CompanyEnum.KD);

                responseModel.Data.Id = result.Data;
                responseModel.Data.CrmId = addressDto.uzm_customerid;
                responseModel.Data.ErpId = ErpId;
                responseModel.Success = result.Success;
                responseModel.Message = result.Message;

                await logService.LogSave(LogEventEnum.FileInfoLog, this.GetType().Name, nameof(AddressSaveAsync), CompanyEnum.KD, LogTypeEnum.Response, responseModel);

                if (result.Success)
                {
                    var updateDto = new AddressSaveRequestDto { CustomerCrmId = requestDto.CustomerCrmId, StatusEnum = requestDto.StatusEnum };
                    var updateContactAddress = await UpdateContactAddressAsync(updateDto);
                }

            }
            catch (Exception ex)
            {
                return await Task.FromResult(ResponseHelper.SetSingleError<DeleteAddressResponseDto>(new ErrorModel(System.Net.HttpStatusCode.NotFound,
                    CommonStaticConsts.Message.AddressDeleteError + ex.ToString(), "")));
            }

            return responseModel;
        }
        private async Task<Response<AddressDto>> GetDeleteAddressIdAsync(DeleteAddressRequestDto requestDto)
        {
            var query = String.Format(@"select uzm_customeraddressId,a.uzm_customerId,c.uzm_ErpId from uzm_customeraddressBase a with(nolock) 
inner join KahveDunyasi_MSCRM..uzm_customerdatasourceBase d with(nolock) on d.uzm_customerexternalid=@EcomId
inner join KahveDunyasi_MSCRM..ContactBase c with(nolock) on c.ContactId=d.uzm_customerid
                                        where 
										uzm_addressecomidstr=@AddressId and a.uzm_createdlocationid=@LocationId and a.statecode=0 and
										d.uzm_datasourceid=@LocationId and ISNULL(d.uzm_unusedflag,0)=0 and d.statecode=0 and c.StateCode=0 ");
            var resService = await dapperService.GetItemParam<object, AddressDto>(query, requestDto, GeneralHelper.GetCrmConnectionStringByCompany(CompanyEnum.KD)).ConfigureAwait(false);

            return resService;

        }

        #endregion ADDRESS_PROCESS

        #region GET_BY_ID

        /// <summary>
        /// Get city by id
        /// </summary>
        /// <param name="id">District id</param>
        /// <returns></returns>
        public async Task<Response<CityDto>> GetCityByIdAsync(Guid id, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_citiesId,
	                                        CreatedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        uzm_city_countryidName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber,
	                                        uzm_cityname,
	                                        uzm_city_countryid,
	                                        uzm_citycode,
	                                        uzm_oldid
                                        FROM
	                                        uzm_cities WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND
                                            uzm_citiesId = @id");

            var resService = await dapperService.GetItemParam<object, City>(query, new { id = id }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<CityDto>>(resService);
            return response;
        }

        /// <summary>
        /// Get country by id
        /// </summary>
        /// <param name="id">District id</param>
        /// <returns></returns>
        public async Task<Response<CountryDto>> GetCountryByIdAsync(Guid id, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_countriesId,
	                                        uzm_countryname,
	                                        uzm_countrycode,
	                                        CreatedOn,
	                                        ModifiedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber
                                        FROM
	                                        uzm_countries WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND
                                            uzm_countriesId = @id");

            var resService = await dapperService.GetItemParam<object, Country>(query, new { id = id }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<CountryDto>>(resService);
            return response;
        }

        /// <summary>
        /// Get district by id
        /// </summary>
        /// <param name="id">District id</param>
        /// <returns></returns>
        public async Task<Response<DistrictDto>> GetDistrictByIdAsync(Guid id, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_district_cityidName,
	                                        uzm_districtId,
	                                        uzm_districtname,
	                                        uzm_district_cityid,
	                                        uzm_districtcode,
	                                        uzm_ilce_sub_code,
	                                        uzm_old_district_code,                    
	                                        CreatedOn,
	                                        ModifiedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode
                                        FROM
	                                        uzm_district WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND
                                            uzm_districtId = @id");

            var resService = await dapperService.GetItemParam<object, District>(query, new { id = id }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<DistrictDto>>(resService);
            return response;
        }

        /// <summary>
        /// Get neighborhood by id
        /// </summary>
        /// <param name="id">Neighborhood id</param>
        /// <returns></returns>
        public async Task<Response<NeighborhoodDto>> GetNeighborhoodByIdAsync(Guid id, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_neighborhood_districtidName,											
											uzm_neighborhoodsId,
											uzm_neighborhoodname,
											uzm_neighborhoodcode,
											uzm_neighborhood_districtid,											                        
											CreatedOn,
											ModifiedOn,
											CreatedBy,
											CreatedByName,
											OrganizationId,
											OrganizationIdname,
											ModifiedByName,
											CreatedBy,
											ModifiedBy,
											OrganizationId,
											statecode,
											statuscode
										FROM
											uzm_neighborhoods WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND
                                            uzm_neighborhoodsId = @id");

            var resService = await dapperService.GetItemParam<object, Neighborhood>(query, new { id = id }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<NeighborhoodDto>>(resService);
            return response;
        }

        #endregion GET_BY_ID

        #region GET_BY_CODE

        /// <summary>
        /// Get city by code with fallback
        /// </summary>
        /// <param name="code">City code</param>
        /// <returns></returns>
        public async Task<Response<CityDto>> GetCityByCodeAsync(string code, string uzm_city_countryid, CompanyEnum company)
        {

            if (staticCityList == null || string.IsNullOrEmpty(code) || company == CompanyEnum.KD)
                return await GetCityByCodeDbAsync(code, uzm_city_countryid, company);

            if (staticCityList.Count == 0)
                return await GetCityByCodeDbAsync(code, uzm_city_countryid, company);

            var city = staticCityList.Where(_ => _.uzm_citycode == code && _.uzm_city_countryid == Guid.Parse(uzm_city_countryid)).FirstOrDefault();

            if (city == null)
                return await GetCityByCodeDbAsync(code, uzm_city_countryid, company);

            return CommonMethod.SetResponseSuccess(city);
        }

        /// <summary>
        /// Get district by code with fallback
        /// </summary>
        /// <param name="code">City code</param>
        /// <returns></returns>
        public async Task<Response<DistrictDto>> GetDistrictByCodeAsync(string code, string uzm_district_cityid, CompanyEnum company)
        {
            if (staticDistrictList == null || string.IsNullOrEmpty(code))
                return await GetDistrictByDistrictDbAsync(code, uzm_district_cityid, company);

            if (staticDistrictList.Count == 0)
                return await GetDistrictByDistrictDbAsync(code, uzm_district_cityid, company);

            var district = staticDistrictList.Where(_ => _.uzm_districtcode == code).FirstOrDefault();

            if (district == null)
                return await GetDistrictByDistrictDbAsync(code, uzm_district_cityid, company);

            return CommonMethod.SetResponseSuccess(district);
        }

        /// <summary>
        /// Get country by code with fallback
        /// </summary>
        /// <param name="code">Country code</param>
        /// <returns></returns>
        public async Task<Response<CountryDto>> GetCountryByCodeAsync(CountryCodeEnum code, CompanyEnum company)
        {
            if (staticCountryList == null || code != CountryCodeEnum.Bilinmiyor)
                return await GetCountryByCodeDbAsync(code, company);

            if (staticCountryList.Count == 0)
                return await GetCountryByCodeDbAsync(code, company);



            var country = staticCountryList.Where(_ => _.uzm_countrycode == code.ToString()).FirstOrDefault();

            if (country == null)
                return await GetCountryByCodeDbAsync(code, company);

            return CommonMethod.SetResponseSuccess(country);
        }

        /// <summary>
        /// Get neighborhood by code with fallback
        /// </summary>
        /// <param name="code">City code</param>
        /// <returns></returns>
        public async Task<Response<NeighborhoodDto>> GetNeighborhoodByCodeAsync(string code, string uzm_neighborhood_districtid, CompanyEnum company)
        {

            if (staticNeighborhoodList == null || string.IsNullOrEmpty(code))
                return await GetNeighborhoodByCodeDbAsync(code, uzm_neighborhood_districtid, company);

            if (staticNeighborhoodList.Count == 0)
                return await GetNeighborhoodByCodeDbAsync(code, uzm_neighborhood_districtid, company);

            var neighborhood = staticNeighborhoodList.Where(_ => _.uzm_neighborhoodcode?.ToString() == code).FirstOrDefault();

            if (neighborhood == null)
                return await GetNeighborhoodByCodeDbAsync(code, uzm_neighborhood_districtid, company);

            return CommonMethod.SetResponseSuccess(neighborhood);
        }

        #endregion GET_BY_CODE

        #region GET_BY_CODE_FORM_DB

        /// <summary>
        /// Get city by code from database
        /// </summary>
        /// <param name="code">City code</param>
        /// <returns></returns>
        private async Task<Response<CityDto>> GetCityByCodeDbAsync(string code, string uzm_city_countryid, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_citiesId,
	                                        CreatedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        uzm_city_countryidName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber,
	                                        uzm_cityname,
	                                        uzm_city_countryid,
	                                        uzm_citycode,
	                                        uzm_oldid
                                        FROM
	                                        uzm_cities WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND
                                            uzm_citycode = @code and uzm_city_countryid=@uzm_city_countryid");

            var resService = await dapperService.GetItemParam<object, City>(query, new { code = code, uzm_city_countryid = uzm_city_countryid }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<CityDto>>(resService);
            return response;
        }

        /// <summary>
        /// Get country by code from database
        /// </summary>
        /// <param name="code">Country code</param>
        /// <returns></returns>
        private async Task<Response<CountryDto>> GetCountryByCodeDbAsync(CountryCodeEnum code, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_countriesId,
	                                        uzm_countryname,
	                                        uzm_countrycode,
	                                        CreatedOn,
	                                        ModifiedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber
                                        FROM
	                                        uzm_countries WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND
                                            uzm_countrycode = @code");

            var resService = await dapperService.GetItemParam<object, Country>(query, new { code = code }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<CountryDto>>(resService);
            return response;
        }

        /// <summary>
        /// Get district by code from database
        /// </summary>
        /// <param name="code">District code</param>
        /// <returns></returns>
        private async Task<Response<DistrictDto>> GetDistrictByDistrictDbAsync(string code, string uzm_district_cityid, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_district_cityidName,
	                                        uzm_districtId,
	                                        uzm_districtname,
	                                        uzm_district_cityid,
	                                        uzm_districtcode,
	                                        uzm_ilce_sub_code,
	                                        uzm_old_district_code,              
	                                        CreatedOn,
	                                        ModifiedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode
                                        FROM
	                                        uzm_district WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND uzm_district_cityid=@uzm_district_cityid
                                            AND uzm_districtcode = @code");

            var resService = await dapperService.GetItemParam<object, District>(query, new { code = code, uzm_district_cityid = uzm_district_cityid }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<DistrictDto>>(resService);
            return response;
        }

        /// <summary>
        /// Get neighborhood by code from database
        /// </summary>
        /// <param name="code">Neighborhood code</param>
        /// <returns></returns>
        private async Task<Response<NeighborhoodDto>> GetNeighborhoodByCodeDbAsync(string code, string uzm_neighborhood_districtid, CompanyEnum company)
        {
            var query = String.Format(@"SELECT
	                                        uzm_neighborhood_districtidName,											
											uzm_neighborhoodsId,
											uzm_neighborhoodname,
											uzm_neighborhoodcode,
											uzm_neighborhood_districtid,										               
											CreatedOn,
											ModifiedOn,
											CreatedBy,
											CreatedByName,
											OrganizationId,
											OrganizationIdname,
											ModifiedByName,
											CreatedBy,
											ModifiedBy,
											OrganizationId,
											statecode,
											statuscode
										FROM
											uzm_neighborhoods WITH(NOLOCK)
                                        WHERE
                                            statecode = 0 AND uzm_neighborhood_districtid=@uzm_neighborhood_districtid
                                            AND uzm_neighborhoodcode = @code ");

            var resService = await dapperService.GetItemParam<object, Neighborhood>(query, new { code = code, uzm_neighborhood_districtid = uzm_neighborhood_districtid }, GeneralHelper.GetCrmConnectionStringByCompany(company)).ConfigureAwait(false);

            var response = mapper.Map<Response<NeighborhoodDto>>(resService);
            return response;
        }

        #endregion GET_BY_CODE_FORM_DB

        #region STATIC_METHODS

        /// <summary>
        /// Set static city list
        /// </summary>
        /// <returns></returns>
        public void SetStaticCityList()
        {
            var query = String.Format(@"SELECT
	                                        uzm_citiesId,
	                                        CreatedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        uzm_city_countryidName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber,
	                                        uzm_cityname,
	                                        uzm_city_countryid,
	                                        uzm_citycode,
	                                        uzm_oldid
                                        FROM
	                                        uzm_cities WITH(NOLOCK)
                                        WHERE
                                            statecode = 0");

            var resService = dapperService.GetList<City>(query, null, ConnectionStringNames.CRM);

            var response = mapper.Map<Response<List<CityDto>>>(resService);

            if (response.Success)
            {
                staticCityList = new List<CityDto>();
                staticCityList = mapper.Map<List<CityDto>>(response.Data);
            }
        }

        /// <summary>
        /// Set static country list
        /// </summary>
        /// <returns></returns>
        public void SetStaticCountryList()
        {
            var query = String.Format(@"SELECT
	                                        uzm_countriesId,
	                                        uzm_countryname,
	                                        uzm_countrycode,
	                                        CreatedOn,
	                                        ModifiedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber
                                        FROM
	                                        uzm_countries WITH(NOLOCK)
                                        WHERE
                                            statecode = 0");

            var resService = dapperService.GetList<Country>(query, null, ConnectionStringNames.CRM);

            var response = mapper.Map<Response<List<CountryDto>>>(resService);

            if (response.Success)
            {
                staticCountryList = new List<CountryDto>();
                staticCountryList = mapper.Map<List<CountryDto>>(response.Data);
            }
        }

        /// <summary>
        /// Set static district list
        /// </summary>
        /// <returns></returns>
        public void SetStaticDistrictList()
        {
            var query = String.Format(@"SELECT
	                                        uzm_district_cityidName,
	                                        uzm_districtId,
	                                        uzm_districtname,
	                                        uzm_district_cityid,
	                                        uzm_districtcode,
	                                        uzm_ilce_sub_code,
	                                        uzm_old_district_code,                          
	                                        CreatedOn,
	                                        ModifiedOn,
	                                        CreatedBy,
	                                        CreatedByName,
	                                        OrganizationIdName,
	                                        ModifiedByName,
	                                        CreatedBy,
	                                        ModifiedBy,
	                                        OrganizationId,
	                                        statecode,
	                                        statuscode,
	                                        ImportSequenceNumber
                                        FROM
	                                        uzm_district WITH(NOLOCK)
                                        WHERE
                                            statecode = 0");

            var resService = dapperService.GetList<District>(query, null, ConnectionStringNames.CRM);

            var response = mapper.Map<Response<List<DistrictDto>>>(resService);

            if (response.Success)
            {
                staticDistrictList = new List<DistrictDto>();
                staticDistrictList = mapper.Map<List<DistrictDto>>(response.Data);
            }
        }

        /// <summary>
        /// Set static neighborhood list
        /// </summary>
        /// <returns></returns>
        public void SetStaticNeighborhoodList()
        {
            var query = String.Format(@"SELECT
	                                        uzm_neighborhood_districtidName,
											uzm_neighborhoodsId,
											uzm_neighborhoodname,
											uzm_neighborhoodcode,
											uzm_neighborhood_districtid,                       
											CreatedOn,
											ModifiedOn,
											CreatedBy,
											CreatedByName,
											OrganizationId,
											OrganizationIdname,
											ModifiedByName,
											CreatedBy,
											ModifiedBy,
											OrganizationId,
											statecode,
											statuscode,
											ImportSequenceNumber
										FROM
											uzm_neighborhoods WITH(NOLOCK)
                                        WHERE
                                            statecode = 0");

            var resService = dapperService.GetList<Neighborhood>(query, null, ConnectionStringNames.CRM);

            var response = mapper.Map<Response<List<NeighborhoodDto>>>(resService);

            if (response.Success)
            {
                staticNeighborhoodList = new List<NeighborhoodDto>();
                staticNeighborhoodList = mapper.Map<List<NeighborhoodDto>>(response.Data);
            }
        }

        #endregion STATIC_METHODS
    }
}
