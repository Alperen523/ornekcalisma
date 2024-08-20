using System.ComponentModel.DataAnnotations;

namespace UzmanCrm.CrmService.Common
{
    public static class ErrorStaticConsts
    {
        #region SEARCH_ERRORS

        public static class SearchErrorStaticConsts
        {
            [Display(Name = "Kayıtlı cep telefonu tespit edildi")]
            public const string S001 = "S001";
            [Display(Name = "Müşteri bulunamadı")]
            public const string S002 = "S002";
            [Display(Name = "Personel numarasına ait bir personel bulunamadı")]
            public const string S010 = "S010";
            [Display(Name = "Lokasyon bulunamadı")]
            public const string S011 = "S011";
            [Display(Name = "EcomId verisi ile müşteri bulunamadı.")]
            public const string S012 = "S012";
        }

        #endregion SEARCH_ERRORS

        #region DUPLICATE_ERRORS

        public static class DuplicateErrorStaticConsts
        {
            [Display(Name = "Kayıtlı cep telefonu eşleşmesi")]
            public const string D001 = "D001";
            [Display(Name = "Kayıtlı ad - soyad - cep telefonu - doğum tarihi eşleşmesi")]
            public const string D002 = "D002";
            [Display(Name = "Kayıtlı ad - soyad - cep telefonu - email eşleşmesi")]
            public const string D003 = "D003";
            [Display(Name = "Kayıtlı ad - soyad - doğum tarihi - email eşleşmesi")]
            public const string D004 = "D004";
            [Display(Name = "Kayıtlı ad - soyad - cep telefonu eşleşmesi")]
            public const string D005 = "D005";
            [Display(Name = "Kayıtlı ad - soyad - doğum tarihi eşleşmesi")]
            public const string D006 = "D006";
            [Display(Name = "Kayıtlı ad - soyad - email eşleşmesi")]
            public const string D007 = "D007";
            [Display(Name = "Kayıtlı ad - cep telefonu - email eşleşmesi")]
            public const string D008 = "D008";
            [Display(Name = "Kayıtlı ad - cep telefonu - doğum tarihi eşleşmesi")]
            public const string D009 = "D009";
            [Display(Name = "Kayıtlı ad - doğum tarihi - email eşleşmesi")]
            public const string D010 = "D010";
            [Display(Name = "Kayıtlı CrmId eşleşmesi")]
            public const string D011 = "D011";
            [Display(Name = "Kayıtlı ErpId eşleşmesi")]
            public const string D012 = "D012";

        }

        #endregion DUPLICATE_ERRORS

        #region DIGITAL_SERVICE

        public static class DigitalServiceErrorStaticConsts
        {
            [Display(Name = "Sms gönderme işlemi başarısız")]
            public const string J001 = "J001";

            [Display(Name = "Otp kodu doğrulunamadı")]
            public const string J002 = "J002";
        }

        #endregion DIGITAL_SERVICE

        #region SMS_ERRORS

        public static class SmsErrorStaticConsts
        {
            [Display(Name = "Girilen bilgilere ait sms sağlayıcı bilgisi bulunamadı")]
            public const string P001 = "P001";

            [Display(Name = "Sms gönderilemedi")]
            public const string P002 = "P002";

            [Display(Name = "Sms sağlayıcı hatası")]
            public const string P003 = "P003";

        }

        #endregion SMS_ERRORS

        #region FORMAT_ERRORS

        public static class FormatErrorStaticConsts
        {
            [Display(Name = "CrmId bilgisi doğru formatta değil")]
            public const string F001 = "F001";
        }

        #endregion FORMAT_ERRORS

        #region GENERAL_ERRORS

        public static class GeneralErrorStaticConsts
        {
            [Display(Name = "Validation model error")]
            public const string V001 = "V001";

            [Display(Name = "Veri bulunamadı")]
            public const string V002 = "V002";

            [Display(Name = "Sunucu hatası")]
            public const string V003 = "V003";

            [Display(Name = "Sistem hatası")]
            public const string V004 = "V004";
        }

        #endregion GENERAL_ERRORS

        #region LOGIN_ERRORS

        public static class LoginErrorStaticConsts
        {
            [Display(Name = "Kullanıcı bilgileri hatalı")]
            public const string L001 = "L001";

            [Display(Name = "Unauthorized")]
            public const string L002 = "L002";
        }

        #endregion LOGIN_ERRORS

        #region CUSTOMER_INSERT

        public static class CustomerStaticConsts
        {

            [Display(Name = "Müşteri kaydetme veya güncelleme işlemi başarısız")]
            public const string C001 = "C001";
            [Display(Name = "Müşteri kaydetme veya güncelleme işleminde CrmId ve ErpId işlemi başarısız")]
            public const string C002 = "C002";
        }

        #endregion CUSTOMER_INSERT

        #region POINT_ERRORS

        public static class PointStaticConsts
        {
            [Display(Name = "Müşteri kara listede. Puan yüklenemez")]
            public const string B001 = "B001";
            [Display(Name = "Aynı faturadan mevcut. Puan yüklenemez")]
            public const string B002 = "B002";
            [Display(Name = "Kampanya kodu yok veya kampanya başlamadı")]
            public const string B003 = "B003";
            [Display(Name = "Fatura miktarı puan kazanımı için yetersiz")]
            public const string B004 = "B004";
            [Display(Name = "Puan kazandırılamadı")]
            public const string B005 = "B005";
        }

        #endregion POINT_ERROR

        /// <summary>
        /// LOYALTY FOR.....................
        /// </summary>

        #region ENDORSEMENT

        public static class EndorsementStaticConsts
        {
            [Display(Name = "Müşteri ciro kayıt işlemi başarısız")]
            public const string E0001 = "E0001";

            [Display(Name = "Müşteri ciro kayıt işlemi sırasında aynı fatura bilgisi olamaz")]
            public const string E0002 = "E0002";

            [Display(Name = "Müşteri ciro detay bilgisi çekme işlemi başarısız")]
            public const string E0003 = "E0003";

            [Display(Name = "Silinecek kayıt için eşleşen fatura numarası bulunamadı.")]
            public const string E0004 = "E0004";
        }
        #endregion ENDORSEMENT

        #region LOYALTY CARD
        public static class LoyaltyCardStaticConsts
        {
            [Display(Name = "Müşteri sadakat kartı verisi çekme işlemi başarısız")]
            public const string LC0001 = "LC0001";

            [Display(Name = "Müşteri sadakat kartı mevcut.Var olan kartı kullanmalısınız")]
            public const string LC0002 = "LC0002";

            [Display(Name = "ErpId ile eşleşen müşteri bulunamadı")]
            public const string LC0003 = "LC0003";

            [Display(Name = "Kart tipi bulunamadı")]
            public const string LC0004 = "LC0004";

            [Display(Name = "Müşteri sadakat kartı oluşturmak için ilgili müşteri M tipli olmak zorundadır")]
            public const string LC0005 = "LC0005";

        }
        #endregion LOYALTY CARD

        #region CUSTOMER GROUP
        public static class CustomerGroupStaticConsts
        {
            [Display(Name = "Müşteri grubu listesi çekme işlemi başarısız")]
            public const string CG0001 = "CG0001";
        }
        #endregion CUSTOMER GROUP

        #region CARD CLASS SEGMENT
        public static class CardClassSegmentStaticConsts
        {
            [Display(Name = "Müşteri grubu listesi çekme işlemi başarısız")]
            public const string CCS0001 = "CCS0001";
        }
        #endregion CARD CLASS SEGMENT

    }
}
