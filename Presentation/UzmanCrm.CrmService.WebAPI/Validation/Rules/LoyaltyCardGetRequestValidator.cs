using FluentValidation;
using System;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.WebAPI.Models.LoyaltyCard;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class LoyaltyCardGetRequestValidator : AbstractValidator<LoyaltyCardGetRequest>
    {
        public LoyaltyCardGetRequestValidator()
        {
            RuleFor(x => x).Must(x => x.ErpId.IsNotNullAndEmpty() || x.CardNo.IsNotNullAndEmpty()).WithMessage("İki alandan biri dolu olmak zorundadır");
            RuleFor(x => x.ErpId).Must(x => x == null || x == "" || (x.Length >= 3 && x.Length <= 10)).WithMessage("ErpId değeri Null olabilir. Ancak değilse karakter sayısı 3-10 arasında olmak zorundadır");
            RuleFor(x => x.CardNo).Must(x => x == null || x == "" || (x.Length >= 10 && x.Length <= 20)).WithMessage("CardNo değeri Null olabilir. Ancak değilse karakter sayısı 10-20 arasında olmak zorundadır");
            RuleFor(x => x.ChannelType).NotNull().NotEmpty().IsInEnum();
            /*When(x => !x.ErpId.IsNotNullAndEmpty() && (x.CrmId == null || x.CrmId == Guid.Empty), () =>
            {
                RuleFor(x => x.ErpId).Length(3, 10);
                RuleFor(x => x.CrmId).NotNull().NotEmpty();
            });*/
        }
    }
}