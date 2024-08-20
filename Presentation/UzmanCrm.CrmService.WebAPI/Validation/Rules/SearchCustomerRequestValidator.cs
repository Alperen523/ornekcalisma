using FluentValidation;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class SearchCustomerRequestValidator : AbstractValidator<GetCustomerRequest>
    {
        public SearchCustomerRequestValidator()
        {

            RuleFor(x => x.MobilePhone).Matches(@"(^(\d{8,14})\d$)|(^$)").WithMessage("Geçerli bir telefon formatı giriniz");


        }
    }
}