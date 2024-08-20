using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.AddressService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.AddressService
{
    public interface IAddressService : IApplicationService
    {

        /// <summary>
        /// Address Save 
        /// </summary>
        /// <param name="requestDto">AddressSaveRequestDto</param>
        /// <returns></returns>
        Task<Response<AddressSaveResponseDto>> AddressSaveAsync(AddressSaveRequestDto requestDto);

        /// <summary>
        /// Address Save 
        /// </summary>
        /// <param name="requestDto">DeleteAddressAsync</param>
        /// <returns></returns>
        Task<Response<DeleteAddressResponseDto>> DeleteAddressAsync(DeleteAddressRequestDto requestDto);

        /// <summary>
        /// Set static city list
        /// </summary>
        /// <returns></returns>
        public void SetStaticCityList();

        /// <summary>
        /// Set static country list
        /// </summary>
        /// <returns></returns>
        public void SetStaticCountryList();

        /// <summary>
        /// Set static district list
        /// </summary>
        /// <returns></returns>
        public void SetStaticDistrictList();

        /// <summary>
        /// Set static neighborhood list
        /// </summary>
        /// <returns></returns>
        public void SetStaticNeighborhoodList();
    }
}
