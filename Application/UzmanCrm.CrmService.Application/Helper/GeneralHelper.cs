using System;
using System.Security.Cryptography;
using System.Text;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.DAL.Config.Application.Dapper;

namespace UzmanCrm.CrmService.Application.Helper
{
    public static class GeneralHelper
    {
        public static string GetLocalIpAddress()
        {
            return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        public static Guid? GetChannelIdByChannelEnum(ChannelEnum channel)
        {
            switch (channel)
            {
                case ChannelEnum.Bilinmiyor:
                    return new Guid("28794560-0841-EF11-90A4-005056BC90FD");
                case ChannelEnum.ETicaret:
                    return new Guid("49B6EB08-E93E-EF11-90A3-005056BC90FD");
                case ChannelEnum.Magaza:
                    return new Guid("4E20B018-E93E-EF11-90A3-005056BC90FD");
                case ChannelEnum.MobilApp:
                    return new Guid("99454124-E93E-EF11-90A3-005056BC90FD");
                case ChannelEnum.Crm:
                    return new Guid("DC2BB22D-0841-EF11-90A4-005056BC90FD");
                default:
                    return new Guid("28794560-0841-EF11-90A4-005056BC90FD");
            }
        }

        public static PointType GetPointTypeByCategoryCode(int? categoryCode)
        {
            switch (categoryCode)
            {
                case null:
                    return PointType.Bilinmiyor;
                case 1:
                    return PointType.Yuzdelik;
                case 2:
                    return PointType.Katlanir;
                case 3:
                    return PointType.Sabit;
                case 4:
                    return PointType.LimitliKatlanir;
                case 5:
                    return PointType.Yuzde50;
                default:
                    return PointType.Bilinmiyor;
            }
        }

        public static ConnectionStringNames GetCrmConnectionStringByCompany(CompanyEnum company)
        {
            switch (company)
            {
                case CompanyEnum.KD:
                    return ConnectionStringNames.CRM;
                case CompanyEnum.LT:
                    return ConnectionStringNames.LatelierCRM;
                default:
                    return ConnectionStringNames.CRM;
            }
        }

        public static ConnectionStringNames GetOvmConnectionStringByCompany(CompanyEnum company)
        {
            switch (company)
            {
                case CompanyEnum.KD:
                    return ConnectionStringNames.OVM;
                case CompanyEnum.LT:
                    return ConnectionStringNames.LOVM;
                default:
                    return ConnectionStringNames.OVM;
            }
        }

        public static string GetOvmDbNameByCompany(CompanyEnum company)
        {
            switch (company)
            {
                case CompanyEnum.KD:
                    return "OVM";
                case CompanyEnum.LT:
                    return "LOVM";
                default:
                    return "OVM";
            }
        }

        public static string GetCrmDbNameByCompany(CompanyEnum company)
        {
            switch (company)
            {
                case CompanyEnum.KD:
                    return "KahveDunyasi_MSCRM";
                case CompanyEnum.LT:
                    return "KahveDunyasi_MSCRM";// farklı bir organizasyon olursa değiştirilebilir.
                default:
                    return "KahveDunyasi_MSCRM";
            }
        }


        public static double GetTotalMillisecondsDateTime(DateTime dateTime)
        {
            return dateTime
                    .ToUniversalTime()
                    .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                    .TotalMilliseconds;
        }

        // Import this Dll
        public static string Get8Digits()
        {
            var bytes = new byte[8];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            uint random = BitConverter.ToUInt32(bytes, 0) % 1000000000;
            return String.Format("{0:D12}", random);
        }

        private static Random RNG = new Random();

        public static string Create16DigitString()
        {
            var builder = new StringBuilder();
            while (builder.Length < 16)
            {
                builder.Append(RNG.Next(10).ToString());
            }
            return builder.ToString();
        }

        public static Guid GetCardIdByCardTypeEnum(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.V:
                    return new Guid("{DC6B7D06-3E33-ED11-915C-00505685232B}");
                case CardTypeEnum.W:
                    return new Guid("{3CDAC57E-CC29-ED11-915B-00505685232B}");
                case CardTypeEnum.R:
                    return new Guid("{7ECA170E-3E33-ED11-915C-00505685232B}");
                default:
                    return new Guid("00000000-0000-0000-0000-000000000000");
            }
        }
        public static CardTypeEnum GetLocationIdByCardTypeEnum(string LocationId)
        {
            switch (LocationId)
            {
                case "2890":
                    return CardTypeEnum.V;
                case "2891":
                    return CardTypeEnum.R;
                case "2893":
                    return CardTypeEnum.W;
                default:
                    return CardTypeEnum.V;
            }
        }
        //00028A98-A614-E911-812C-005056991930  2890 Vakko E-ticaret
        //ABD066B4-A614-E911-812C-005056991930  2891 Vakkorama E-ticaret
        //3C517AD1-A614-E911-812C-005056991930  2893 WCollection E-ticaret
        public static Guid? GetLocationIdByDataSourceId(string LocationId)
        {
            switch (LocationId)
            {
                case "2890":
                    return new Guid("00028A98-A614-E911-812C-005056991930");
                case "2891":
                    return new Guid("ABD066B4-A614-E911-812C-005056991930");
                case "2893":
                    return new Guid("3C517AD1-A614-E911-812C-005056991930");
                default:
                    return new Guid("00028A98-A614-E911-812C-005056991930");
            }
        }

        public static string FieldNullReplace(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                value = "-";
            }

            return value;
        }

        public static string GetCardTypeFilter(CardTypeEnum cardType)
        {
            switch (cardType)
            {
                case CardTypeEnum.W:
                    return " in ('W')";
                default:
                    return " not in ('W')"; // Kart tipi W değil ise diğer kart tipeleri için sorgulayacak şekilde sorgu atılır
            }
        }
    }
}
