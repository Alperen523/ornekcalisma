using FluentValidation.Attributes;
using UzmanCrm.CrmService.WebAPI.Validation.Rules;

namespace UzmanCrm.CrmService.WebAPI.Models.Customer
{
    [Validator(typeof(SearchCustomerRequestValidator))]
    /// <summary>
    /// Müşteri Arama request modeli
    /// </summary>
    public class GetCustomerRequest
    {


        /// <summary>
        /// Müşteri Cep telefonu bilgisidir
        /// </summary>
        public string MobilePhone { get; set; } = null;





    }
}