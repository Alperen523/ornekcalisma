namespace UzmanCrm.CrmService.Application.Abstractions.Service.ContactService.Model.Erp
{
    public class ErpSaveCustomerRequestModel
    {
        public string birthDateString { get; set; } = null;  // Format : yyyy-MM-dd        
        public string creatorPerson { get; set; } = null;  // required Kaydeden sicil. Erp db de kaydı olmalı.
        public string crm_id { get; set; } = null;  // required
        public string email { get; set; } = null;
        public string erp_id { get; set; } = null;
        public string genderId { get; set; } = null; // required
        public string gsm { get; set; } = null;  // Ev ülkesi 90 olan kayıtlar için : Gsm Numarası yada Ev telefonundan en az biri dolu olmalı. Format : 0905XXXXXXXXX
        public string homeAdress1 { get; set; } = null;
        public string homeAdress2 { get; set; } = null;
        public int homeCityId { get; set; } // required IL tablosu IL_NO kolonu! 
        public int homeCountryId { get; set; }// required ULKE tablosu ID!
        public int homeCountyId { get; set; } // required ILCE tablosu ID!
        public int homeDistrictId { get; set; }// required SEMT tablosu ID!
        public string homePostCode { get; set; } = null;
        public string homeTel { get; set; } = null;
        public int isKvkk { get; set; }  // required Kvkk izinli ise :1, değil ise 0!
        public string name { get; set; } = null; // required
        public string surname { get; set; } = null; // required
        public string taxAdministration { get; set; } = null;
        public string taxNumber { get; set; } = null;
        public string tckn { get; set; } = null;
        public string creatorChannel { get; set; } = null;
        public string cardType { get; set; } = null;
        public string customerType { get; set; } = null;
    }
}
