using FluentValidation;
using UzmanCrm.CrmService.WebAPI.Models.Endorsement;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class EndorsementRequestValidator : AbstractValidator<EndorsementRequest>
    {
        public EndorsementRequestValidator()
        {
            RuleFor(x => x.ErpId).NotEmpty().Length(3, 10);
            RuleFor(x => x.InvoiceNumber).NotEmpty().Length(4, 50);
            RuleFor(x => x.BillType).IsInEnum();
            RuleFor(x => x.StoreCode).NotEmpty().Length(3, 20);
            RuleFor(x => x.TransactionId).NotEmpty().Length(1, 100);
            RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
            RuleFor(x => x.CardNo).Length(10, 20);
            RuleFor(x => x.GiftCardAmount).GreaterThanOrEqualTo(0);
        }
    }
}



//RuleFor(sample => sample.NotNull).NotNull();
//RuleFor(sample => sample.NotEmpty).NotEmpty();
//RuleFor(sample => sample.EmailAddress).EmailAddress();
//RuleFor(sample => sample.RegexField).Matches(@"(\d{4})-(\d{2})-(\d{2})");

//RuleFor(sample => sample.ValueInRange).GreaterThanOrEqualTo(5).LessThanOrEqualTo(10);
//RuleFor(sample => sample.ValueInRangeExclusive).GreaterThan(5).LessThan(10);

//// WARNING: Swashbuckle implements minimum and maximim as int so you will loss fraction part of float and double numbers
//RuleFor(sample => sample.ValueInRangeFloat).InclusiveBetween(1.1f, 5.3f);
//RuleFor(sample => sample.ValueInRangeDouble).ExclusiveBetween(2.2, 7.5f);