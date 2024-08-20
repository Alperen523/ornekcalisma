using FluentValidation;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.WebAPI.Models.Address;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class AddressSaveRequestValidator : AbstractValidator<AddressSaveRequest>
    {
        public AddressSaveRequestValidator()
        {

            RuleFor(x => x.ErpId).
                Must(x => x.Length >= 2 && x.Length <= 10).
                WithMessage("ErpId karakter sayısı 2-10 arasında olmak zorundadır");

            RuleFor(x => x.CountryCode).IsInEnum().NotNull().WithMessage("*Required");
            RuleFor(x => x.AddressType).IsInEnum().NotNull().WithMessage("*Required");
            RuleFor(x => x.ChannelId).IsInEnum().NotNull().WithMessage("*Required");

            RuleFor(x => x.Location).
                Must(x => x.Length >= 2 && x.Length <= 8).
                WithMessage("Location karakter sayısı 2-8 arasında olmak zorundadır").IsNotNullAndEmpty();


            RuleFor(x => x.AddressId).
                Must(x => x.Length >= 2 && x.Length <= 20).
                WithMessage("AddressId karakter sayısı 2-20 arasında olmak zorundadır").IsNotNullAndEmpty();

            RuleFor(x => x.IsDefaultAddress).Must(x => x == false || x == true).
                WithMessage("IsDefaultAddress true veya false olmak zorundadır").IsNotNullAndEmpty();

        }
    }
}
