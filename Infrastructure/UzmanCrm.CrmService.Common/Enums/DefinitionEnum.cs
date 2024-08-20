namespace UzmanCrm.CrmService.Common.Enums
{
    public enum TwoOptionsEnum
    {
        No = 0,
        Yes = 1
    }

    public enum ChannelEnum
    {
        Bilinmiyor = 0,
        ETicaret = 1,
        Magaza = 2,
        MobilApp = 3,
        Crm = 4


    }
    public enum GenderEnum
    {
        Erkek = 1,
        Kadin = 2,
        Bilinmiyor = 3
    }

    public enum DigitalFormEnum
    {
        Yazili = 1,
        Dijital = 2,
        Personel = 3,
        Internet = 4
    }

    public enum OrganizationEnum
    {
        TR = 1,
        TR_LT = 20,
        TR_TEST = 30,
        TR_LT_TEST = 40,
    }

    public enum CompanyEnum
    {
        KD = 0,
        LT = 1
    }

    public enum LogTypeEnum
    {
        Request = 1,
        Response = 2
    }
    public enum PhoneTypeEnum
    {
        MobilePhone = 0,
        LandPhone = 1
    }

    public enum LogEventEnum
    {
        DbInfo = 1,// Db olarak Info tipi kaydetmek
        DbError = 2,// Db olarak Error tipi kaydetmek
        DbWarning = 3,// Db olarak Warning tipi kaydetmek
        FileInfoLog = 4 // File olarak dosyayı kaydetmek
    }

    public enum CreateType
    {
        Create = 0,
        Update = 1
    }


    //GetEmailOrPhoneInfo metodunda kullanıldı.
    public enum EntityTypeEnum
    {
        Email = 1,
        Phone = 2,
        Contact = 3
    }

    public enum StatusType
    {
        Aktif = 1,
        Pasif = 2
    }

    public enum PointType
    {
        Bilinmiyor = 0,
        Yuzdelik = 1,
        Katlanir = 2,
        Sabit = 3,
        LimitliKatlanir = 4,
        Yuzde50 = 5
    }
    public enum CountryCodeEnum
    {
        Bilinmiyor = 0,
        TR = 90
    }

    public enum AddressTypeEnum
    {
        Ev = 0,
        Is = 1,
        Fatura = 2,
        Tatil_Yazlik = 3,
        Teslimat_Adresi = 4
    }

    public enum BillTypeEnum
    {
        Unknown = 0,
        ZP01 = 1, // P.Satış - Normal Fatura
        ZP02 = 2, // P.Satış - KDVsiz Fatura
        ZP03 = 3, // P.Satış - Taxfree Fatura
        ZP04 = 4, // P.Satış - Web Fatura
        ZP05 = 5, // P.Satış - E-fatura
        ZP08 = 6, // P.Satış - Gift Card
        ZP09 = 7, // P.Satış - Toplu Gift Kart
        ZP12 = 8, // P.Satış - RB Değişim
        ZP16 = 9, // P.Satış - Tahsilat --------Kullanılmayacak
        ZP61 = 10, // P.Satış - Kapora --------Kullanılmayacak
        ZR01 = 11, // P.İade -  Normal Fatura
        ZR02 = 12, // P.İade -  KDVsiz Fatura
        ZR03 = 13, // P.İade -  Taxfree Fatura
        ZR04 = 14, // P.İade -  Web Fatura
        ZR05 = 15, // P.İade -  E-fatura
        ZR08 = 16, // P.İade -  Gift Card
        ZR09 = 17, // P.İade -  Toplu Gift Kart
        ZR12 = 18, // P.İade -  RB Değişim
        ZR17 = 19, // P.İade -  Tediye --------Kullanılmayacak
        ZR63 = 20, // P.İade -  Kapora İade --------Kullanılmayacak
        IsDelete = 30,// IsDelete - SAP nin gönderdiği bu tip ile ciro tablosundaki fatura iptal edilecek.
        IsDeleteReturn = 40// IsDeleteReturn - SAP nin gönderdiği bu tip ile ciro tablosundaki iade faturası iptal edilecek.
    }

    public enum CardTypeEnum
    {
        Unknown = 0,
        V = 1, // Vakko
        R = 2, // Vakkorama
        W = 3, // WCollection
        P = 4 // Personel
    }

    public enum OperationType
    {
        create = 1,
        update = 2,
        delete = 3
    }

    public enum ApprovalStatusType
    {
        Draft = 1, // Taslak
        Denied = 2, // Reddedildi
        Approved = 3, // Onaylandi
        WaitingForApproval = 4, // Onay Beklemede
    }

    public enum LoyaltyCardStatusCodeType
    {
        InUse = 1, // Kullanımda
        Blocked = 2, // Bloke Edildi
        Canceled = 3, // İptal Edildi
    }

    public enum CardDiscountStatusCodeType
    {
        Draft = 0, // Taslak
        InUse = 1, // Kullanımda
        Blocked = 2, // Bloke Edildi
        Expired = 3, // Süresi Doldu
        Canceled = 4, // İptal Edildi
    }

    public enum EndorsementType
    {
        Standart = 0, // Cirodan hesaplanan segment
        Dost = 1 // İstisna kaydı var
    }

    public enum ArrivalChannelType
    {
        CRM = 0,
        Portal = 1
    }

    public enum EcomChannelTypeEnum
    {
        Unknown = 0,
        V = 2890, // Vakko
        R = 2891, // Vakkorama
        W = 2893  // WCollection
    }

    public enum SaveModeEnum
    {
        Auto,
        Insert,
        Update
    }

    public enum CustomerTypeEnum
    {
        Bilinmiyor = 0,
        PersonelMusteri = 1, // Personel Müşteri
        GercekMusteri = 2, // Gerçek Müşteri
        AVMPersoneli = 3, // AVM Personeli
        Ogrenci = 4, // Öğrenci
        SaglikCalisani = 5, // Sağlık Çalışanı
        Esnaf = 6, // Esnaf
    }

}
