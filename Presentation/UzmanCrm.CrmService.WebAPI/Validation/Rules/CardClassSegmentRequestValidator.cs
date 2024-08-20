using FluentValidation;
using UzmanCrm.CrmService.WebAPI.Models.CardClassSegment;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class CardClassSegmentRequestValidator : AbstractValidator<CardClassSegmentRequest>
    {
        public CardClassSegmentRequestValidator()
        {
            RuleFor(x => x.SegmentName).NotEmpty().Length(1, 10);
            RuleFor(x => x.ValidityPeriod).NotEmpty().GreaterThan(0);
            RuleFor(x => x.FirstNotificationPeriod).NotEmpty().GreaterThan(0).LessThan(x => x.ValidityPeriod).WithMessage("FirstNotificationPeriod alanı ValidityPeriod alanından küçük olmak zorundadır");
            RuleFor(x => x.SecondNotificationPeriod).NotEmpty().GreaterThan(0).LessThan(x => x.FirstNotificationPeriod).WithMessage("SecondNotificationPeriod alanı FirstNotificationPeriod alanından küçük olmak zorundadır");
        }
    }
}