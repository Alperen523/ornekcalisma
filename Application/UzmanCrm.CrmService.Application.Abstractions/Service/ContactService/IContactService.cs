using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model;
using UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Contact;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService
{
    public interface IContactService : IApplicationService
    {

        Task<Response<GetCustomerResponseDto>> GetCustomerSearchAsync(GetCustomerRequestDto request);
        Task<Response<SaveCustomerResponseDto>> SaveCustomerAsync(SaveCustomerRequestDto request);

        //to do
        Task<Response<ContactByErpIdDto>> GetContactItemByErpIdAsync(string erpId);
        Task<Response<ContactByErpAndCardIdDto>> GetContactByErpAndCardId(ContactByErpAndCardIdDto requestDto);
        Task<Response<UpdateResponse>> UpdateLoyaltyCardInfoOnContactAsync(LoyaltyCardInfoOnContactRequest requestDto);
        Task<Response<List<ContactDto>>> GetContactListForCardExceptionDiscount(CardExceptionContactRequestDto requestDto);
        Task<Response<ContactByErpIdDto>> GetMainContactItemBySubContactIdAsync(string subContactId);
        Task<Response<List<CustomerDto>>> SearchCustomerAsync(GetCustomerRequestDto requestDto);
        Task<Response<GetCustomerByEcomIdResponseDto>> GetCustomerByEcomIdAsync(string EcomId, EcomChannelTypeEnum ChannelId);





    }
}
