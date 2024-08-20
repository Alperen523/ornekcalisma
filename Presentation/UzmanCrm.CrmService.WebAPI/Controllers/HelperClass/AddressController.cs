using AutoMapper;
using Swashbuckle.Examples;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.WebAPI.Examples.Request.Address;
using UzmanCrm.CrmService.WebAPI.Examples.Response.Address;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    // GET: Address
    [Authorize(Roles = "3")]
    public class AddressController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ILogService logService;
        private readonly IAddressService addressService;

        public AddressController(IMapper mapper, ILogService logService, IAddressService addressService)
        {
            this.mapper = mapper;
            this.logService = logService;
            this.addressService = addressService;
        }

        /// <summary>
        /// Address save
        /// </summary>
        /// <remarks>
        /// Bu metot, müşterinin Erp Id(**ErpId**),Mağaza(**Location**) ve AdresId(**addressId**) bilgisine göre adres eklenmesi veya güncellenmesi işlemini sağlar.
        /// <br/>
        /// </remarks>
        /// <returns>AddressSave Response</returns>
        [HttpPost]
        [SwaggerRequestExample(typeof(AddressSaveRequest), typeof(AddressSaveRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(AddressSaveResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<AddressSaveResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/address/save-address")]
        public async Task<IHttpActionResult> AddressSaveAsync(AddressSaveRequest request)
        {
            await logService.LogSave(LogEventEnum.DbInfo, "Request", nameof(AddressSaveAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var mappingModel = mapper.Map<AddressSaveRequest, AddressSaveRequestDto>(request);
            var response = await addressService.AddressSaveAsync(mappingModel);

            await logService.LogSave(LogEventEnum.DbInfo, "Response", nameof(AddressSaveAsync), CompanyEnum.KD, LogTypeEnum.Response, response);

            if (!response.Success)
                return Content(response.Error.StatusCode, response);

            return Ok(response);
        }

        /// <summary>
        /// Address delete
        /// </summary>
        /// <remarks>
        /// Bu metot, müşterinin Veri Kaynağı Id(**EcomId**), Mağaza(**Location**) ve AdresId(**addressId**)  bilgisine göre adres silme işlemini sağlar.
        /// <br/>
        /// </remarks>
        /// <returns>DeleteAddressAsync Response</returns>
        [HttpDelete]
        [SwaggerRequestExample(typeof(DeleteAddressRequest), typeof(DeleteAddressRequestExample))]
        [SwaggerResponseExample(System.Net.HttpStatusCode.OK, typeof(DeleteAddressResponseExample))]
        [SwaggerResponse(System.Net.HttpStatusCode.OK, Type = typeof(Response<DeleteAddressResponse>))]
        [SwaggerResponse(System.Net.HttpStatusCode.BadRequest, Type = typeof(Response<object>))]
        [Route("api/address/delete-address-ecom")]
        public async Task<IHttpActionResult> DeleteAddressAsync(DeleteAddressRequest request)
        {

            await logService.LogSave(LogEventEnum.DbInfo, "Request", nameof(DeleteAddressAsync), CompanyEnum.KD, LogTypeEnum.Request, request);

            var mappingModel = mapper.Map<DeleteAddressRequest, DeleteAddressRequestDto>(request);

            var response = await addressService.DeleteAddressAsync(mappingModel);

            await logService.LogSave(LogEventEnum.DbInfo, "Response", nameof(DeleteAddressAsync), CompanyEnum.KD, LogTypeEnum.Response, response);

            if (!response.Success)
                return Content(response.Error.StatusCode, response);

            return Ok(response);
        }
    }
}
