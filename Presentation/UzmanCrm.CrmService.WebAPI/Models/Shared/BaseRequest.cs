using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.WebAPI.Models.Shared
{
    public class BaseRequest
    {
        public ChannelEnum ChannelId { get; set; } = ChannelEnum.Bilinmiyor;

        /// <summary>
        /// Kaydın Aktif veya Pasif olacağı bilgisi. Varsayılan değeri : Aktif
        /// </summary>
        public StatusType StatusEnum { get; set; } = StatusType.Aktif;

        /// <summary>
        ///Crm sisteminde bulunan Vakko ve Vakko için Mağaza kodu bilgisidir
        /// </summary>
        public string Location { get; set; } = null;

        /// <summary>
        ///Crm sisteminde bulunan Vakko ve Vakko için Personel Numarası bilgisidir. Crm de Portal Kullanıcıları menüsünden ulaşılabilir
        /// </summary>
        public string PersonNo { get; set; } = null;

        /// <summary>
        /// Firma bilgisi
        /// </summary>
        public CompanyEnum Company { get; set; } = CompanyEnum.KD;
    }
}