using FluentValidation;
using System;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.WebAPI.Models.Customer;

namespace UzmanCrm.CrmService.WebAPI.Validation.Rules
{
    public class SaveCustomerEcomRequestValidator : AbstractValidator<SaveCustomerRequest>
    {
        public SaveCustomerEcomRequestValidator()
        {
            RuleFor(x => x.FirstName).
                Must(x => x.Length >= 2 && x.Length <= 100).
                WithMessage("FirstName karakter sayısı 2-100 arasında olmak zorundadır");

            RuleFor(x => x.LastName).
                Must(x => x.Length >= 2 && x.Length <= 100).
                WithMessage("LastName karakter sayısı 2-100 arasında olmak zorundadır");

            //RuleFor(x => x.Location).
            //    Must(x => x.Length >= 2 && x.Length <= 8).
            //    WithMessage("Location karakter sayısı 2-8 arasında olmak zorundadır").IsNotNullAndEmpty();


            //RuleFor(x => x.EcomId).
            //    Must(x => x.Length >= 2 && x.Length <= 15).
            //    WithMessage("EcomId karakter sayısı 2-15 arasında olmak zorundadır").IsNotNullAndEmpty();

            //RuleFor(x => x.PhoneNumber).Matches(@"(^(\d{9,14})\d$)|(^$)").WithMessage("Geçerli bir telefon formatı giriniz");

            //RuleFor(x => x.EmailAddress).Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$|^$").WithMessage("Geçerli bir email formatı giriniz");

            //RuleFor(x => x).Must(x => (x.FirstName.IsNotNullAndEmpty() && x.LastName.IsNotNullAndEmpty()) &&
            //                         (x.PhoneNumber.IsNotNullAndEmpty() || x.EmailAddress.IsNotNullAndEmpty())).
            //WithMessage("FirstName,LastName ile PhoneNumber veya EmailAddress alanlarından biri dolu olmak zorundadır");

            When(x => x.BirthDate.IsNotNullAndEmpty(), () =>
            {
                RuleFor(x => x.BirthDate).Must(BeValidDateFormat).WithMessage("Invalid date. Birthdate year must be after 1900");
            });
        }

        private bool BeValidDateFormat(DateTime? date)
        {
            if (date == DateTime.MinValue)
                return false;

            if (((DateTime)date).Year <= 1900)
                return false;

            return true;
        }
    }
}