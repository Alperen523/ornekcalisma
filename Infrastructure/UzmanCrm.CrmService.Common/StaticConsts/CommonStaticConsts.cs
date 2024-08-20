namespace UzmanCrm.CrmService.Common
{
    public static class CommonStaticConsts
    {
        public static class CustomerIdType
        {
            public const string Account = "account";
            public const string Contact = "contact";
        }
        public static class Message
        {
            public const string Success = "İşlem Başarılı";
            public const string Unsuccess = "İşlem Başarısız";

            public const string PersonnelNotFound = "Personel numarasına ait bir personel bulunamadı.";
            public const string LocationNotFound = "Lokasyon bulunamadı.";
            public const string EmailSaveError = "EmailSave işleminde hata alındı. ";
            public const string PhoneSaveError = "PhoneSave işleminde hata alındı. ";
            public const string DataSourceSaveError = "DataSourceSave işleminde hata alındı. ";
            public const string CreateCustomerFormError = "CreateCustomerForm işleminde hata alındı. ";
            public const string AddressSaveError = "Adres kayıt işleminde hata alındı. ";
            public const string AddressDeleteError = "Adres silme işleminde hata alındı. ";
            public const string CustomerDuplicateError = "Bilgilere sahip başka bir kayıtlı müşteri mevcut. ";

            public const string SmsUserNotFoundError = "Girilen bilgilere ait sms sağlayıcı bilgisi bulunamadı. ";

            public const string CustomerIdIsNotValidFormat = "Girilen crm id bilgisi doğru formatta değil. ";

            public const string CustomerNotFound = "Müşteri bulunamadı";
            public const string ContactInsertUpdateError = "Müşteri kaydetme veya güncelleme işlemi başarısız";
            public const string ContactResponseError = "Müşteri kaydetme veya güncelleme işleminde CrmId ve ErpId işlemi başarısız";

            public const string ApiUserError = "Api kullanıcı bilgileri hatalı";
            public const string Unauthorized = "Unauthorized";
            public const string UnauthorizedDesc = "Api kullanımı için token almanız gerekmektedir";
            public const string DataNotFound = "Veri bulunamadı.";
            public const string SystemError = "Sistem hatası.";

            public const string InternalServerError = "Sunucu hatası";
            public const string PointBlacklist = "Müşteri kara listede. Puan yüklenemez.";

            public const string SmsProviderError = "Sms sağlayıcı hatası";
            public const string InvoiceNumberError = "Aynı faturadan mevcut. Puan yüklenemez";
            public const string CampaignCodeError = "Kampanya kodu yok veya kampanya başlamadı";
            public const string PointTotalAmountError = "Fatura miktarı puan kazanımı için yetersiz";
            public const string PointGeneralError = "Puan kazandırılamadı";

            public const string SqlGetError = "Sql getirme işlemi başarısız";
            public const string ErpApiLoginError = "Erp Login servis hatası ";
            public const string LoyaltyApiLoginError = "Loyalty Login servis hatası ";
            public const string ErpSaveCustomerError = "ErpSaveCustomer servis hatası ";
            /// <summary>
            /// LOYALTY FOR.....
            /// </summary>
            /// 
            public const string EndorsementCreateError = "Müşteri ciro kayıt işlemi başarısız";
            public const string EndorsementGetError = "Müşteri ciro veri çekme işlemi başarısız";
            public const string EndorsementInvoiceExistError = "Aynı fatura bilgisi ile müşteri ciro kayıt işlemi yapılamaz";
            public const string LoyaltyCardGetError = "Müşteri sadakat kartı verisi çekme işlemi başarısız";
            public const string CustomerGroupListGetError = "Müşteri grubu listesi çekme işlemi başarısız";
            public const string CardClassSegmentSaveError = "Kart sınıfı segment kayıt işleminde hata alındı";
            public const string CardClassSegmentListGetError = "Kart sınıfı segmenti listesi çekme işlemi başarısız";
            public const string CardExceptionDiscountSaveError = "Kart istisna indirimi kayıt işleminde hata alındı";
            public const string LoyaltyCardAvailable = "Müşteri sadakat kartı mevcut.Var olan kartı kullanmalısınız";
            public const string SaveLoyaltyCardCustomerTypeError = "Müşteri sadakat kartı oluşturmak için ilgili müşteri M tipli olmak zorundadır";
            public const string CustomerErpNotFound = "ErpId ile eşleşen müşteri bulunamadı";
            public const string Customer_EcomId_AddresId_LocationId_NotFound = "EcomId, AddresId ve LocationId ile eşleşen kayıt bulunamadı";
            public const string CardTypeNotFound = "Kart tipi bulunamadı";
            public const string CustomerEndorsementDataProcessingWagescaleFindError = "Barem kaydı bulunamadığı için işlem gerçekleştirilmedi";
            public const string CustomerEndorsementDataProcessingWagescaleSelectError = "Geçerli ciro ve kart tipi için eşleşen barem kaydı bulunamadı";
            public const string GetUnintegratedEndorsementListError = "Entegre edilmemiş ciro kaydı bulunamadı";
            public const string LoyaltyCardInfoOnContactUpdateError = "Müşteri Kartı ile Loyalty kartı eşleştirilemedi.";
            public const string EndorsementIsDeleteError = "Silinecek kayıt için eşleşen fatura numarası bulunamadı.";
            public const string LoyaltyCardSaveError = "Müşteri sadakat kartı kayıt işleminde hata alındı";
            public const string SearchCustomerError = "Müşteri arama işleminde hata alındı";
            public const string ContactGetEcomIdError = "EcomId verisi ile müşteri çekme işlemi başarısız\".";
            public const string BatchApprovalListSaveError = "Toplu Onay Listesi kayıt işleminde hata alındı";
        }


        public static class StaticUrl
        {
            public const string DigitalSeriviceTr = "http://udi.dijitalizin.com/OtpService.svc";
            public const string DigitalSeriviceGdpr = "http://gdpr.dijitalizin.com/OtpService.svc";

            public const string InfobipSendSms = "https://api.infobip.com/sms/2/text/advanced";
            public const string InfobipOutboundDeliveryReport = "https://api.infobip.com/sms/1/reports";
            public const string InfobipOutboundSmsLogs = "https://api.infobip.com/sms/2/logs";
        }
    }
}
