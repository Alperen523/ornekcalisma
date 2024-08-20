using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Helper
{
    public static class OrganizationHelper
    {

        public static string GetOrganizationNameByCountryCode(int countrycode, CompanyEnum organization)
        {
            switch (organization)
            {
                case CompanyEnum.KD:
                    switch (countrycode)
                    {
                        case 90:
                            return "VK.TR";

                        default:
                            return null;
                    }
                case CompanyEnum.LT:
                    switch (countrycode)
                    {
                        case 90:
                            return "LT.TR";
                        default:
                            return null;
                    }
                default:
                    return null;
            }
        }

        public static DigitalServiceUserInfoModel GetDigitalServiceUserInfoByOrganization(string organization)
        {
            switch (organization)
            {
                case "VK.TR":
                    return new DigitalServiceUserInfoModel
                    {
                        Organization = "VakkoTR",
                        Token = "5nIh8lY2hTw1yp47YUHooOOgh3rdd9oenkrVsp8oklpI3wC0MR9d"
                    };

                case "LT.TR":
                    return new DigitalServiceUserInfoModel
                    {
                        Organization = "VakkoLT",
                        Token = "6nIh8lL3Vnxlj47YUHAfFfgh4dfd9oenkrVsp7cjSpI3wC0MR9f"
                    };
                default:
                    return null;
            }
        }

        public static UserInfo GetSmsOtpProviderUserByCountryAndCompanyCode(CompanyEnum co, int countryCode)
        {
            UserInfo user = new UserInfo();

            switch (co)
            {
                case CompanyEnum.KD:
                    {
                        switch (countryCode)
                        {
                            case 90:
                                user.Username = "VakkoTRotp";
                                user.Password = "Vakko@83";
                                break;
                            case 7:
                                user.Username = "VakkoRU";
                                user.Password = "DwFXcXEO";
                                break;
                            case 380:
                                user.Username = "Vakko";
                                user.Password = "Sghj56mnb";
                                break;
                            case 375:
                                user.Username = "Vakkoby1";
                                user.Password = "1q2w3e4r*";
                                break;
                            case 40:
                                user.Username = "Vakkorom3";
                                user.Password = "Qz12345!";
                                break;
                            case 212:
                                user.Username = "fas.it";
                                user.Password = "Fasit123.!";
                                break;
                            default:
                                return null;
                        }
                    }
                    break;
                case CompanyEnum.LT:
                    {
                        user.Username = "VakkoOTP";
                        user.Password = "Vakko2018tr";
                    }
                    break;
                default:
                    return null;
            }

            return user;
        }

        public static string GetOrganizationNameByOrganizationEnum(OrganizationEnum organization)
        {
            switch (organization)
            {
                case OrganizationEnum.TR:
                case OrganizationEnum.TR_TEST:
                    return "VK.TR";
                case OrganizationEnum.TR_LT:
                case OrganizationEnum.TR_LT_TEST:
                    return "LT.TR";
                default:
                    return null;
            }
        }

        public static string GetOrganizationDigitalServiceNameByOrganizationEnum(OrganizationEnum organization)
        {
            switch (organization)
            {
                case OrganizationEnum.TR:
                case OrganizationEnum.TR_TEST:
                    return "VakkoTR";
                case OrganizationEnum.TR_LT:
                case OrganizationEnum.TR_LT_TEST:
                    return "Vakko";
                default:
                    return null;
            }
        }

        public static string GetOrganizationDigitalServiceTokenByOrganizationEnum(OrganizationEnum organization)
        {
            switch (organization)
            {
                case OrganizationEnum.TR:
                case OrganizationEnum.TR_TEST:
                    return "5nIh8lY2hTw1yp47YUHooOOgh3rdd9oenkrVsp8oklpI3wC0MR9dwwww";
                case OrganizationEnum.TR_LT:
                case OrganizationEnum.TR_LT_TEST:
                    return "6nIh8lL3Vnxlj47YUHAfFfgh4dfd9oenkrVsp7cjSpI3wC0MR9fwwwww";
                default:
                    return null;
            }
        }

        public static CompanyEnum GetCompanyByOrganizationEnum(OrganizationEnum organization)
        {
            switch (organization)
            {
                case OrganizationEnum.TR:
                    return CompanyEnum.KD;
                case OrganizationEnum.TR_LT:
                    return CompanyEnum.LT;
                case OrganizationEnum.TR_TEST:
                    return CompanyEnum.KD;
                case OrganizationEnum.TR_LT_TEST:
                    return CompanyEnum.LT;
                default:
                    return CompanyEnum.KD;
            }
        }
    }
}
