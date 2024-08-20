using FluentValidation;
using UzmanCrm.CrmService.WebAPI.Models.CardExceptionDiscount;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class CardExceptionDiscountRequestValidator : AbstractValidator<CardExceptionDiscountRequest>
    {
        public CardExceptionDiscountRequestValidator()
        {
            RuleFor(x => x.ApprovalStatus).IsInEnum();
            // RuleFor(x => x.CardClassSegmentId).NotEmpty();  Son durumda İstisna oluşturulurken önce müşteri grubu seçileceği için bu alanının zorunululuğu kaldırılmıştır. 
            RuleFor(x => x.CustomerGroupId).NotEmpty();
            RuleFor(x => x.DemandDate).NotEmpty();
            RuleFor(x => x.DemandedUserId).NotEmpty();
            RuleFor(x => x.Description).Length(0, 250);
            RuleFor(x => x.DiscountRate).NotEmpty().InclusiveBetween(0, 100); // 0-100 arası değer alabilir sadece
            RuleFor(x => x.EndDate).NotEmpty();
            RuleFor(x => x.LoyaltyCardId).NotEmpty();
            RuleFor(x => x.StartDate).NotEmpty();
            RuleFor(x => x.StatusCode).IsInEnum();
        }
    }
}