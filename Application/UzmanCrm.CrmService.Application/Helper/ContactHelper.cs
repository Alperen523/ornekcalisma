using System;
using System.Configuration;
using System.Reflection;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.Application.Helper
{
    public static class ContactHelper
    {
        public static Guid GetContactChannelId(ChannelEnum channel)
        {
            switch (channel)
            {
                case ChannelEnum.Crm:
                    return new Guid("F4FA0CB6-AECF-40F4-A4BA-87ECD21129A8");
                case ChannelEnum.ETicaret:
                    return new Guid("1041F0E8-1E7D-4BC5-95C9-C9DBCF6E492B");
                case ChannelEnum.Magaza:
                    return new Guid("8BBEF3D9-6B7B-4787-8B9F-F4B4013C1530");
                case ChannelEnum.MobilApp:
                    return new Guid("45A8EF9C-E2AD-4363-9A81-2AA96133E073");
                case ChannelEnum.Bilinmiyor:
                    return Guid.Empty;
                default:
                    return Guid.Empty;
            }
        }

        public static bool IsOnlineContact(ChannelEnum channel)
        {
            switch (channel)
            {
                case ChannelEnum.Crm:
                    return false;
                case ChannelEnum.ETicaret:
                    return true;
                case ChannelEnum.Magaza:
                    return false;
                case ChannelEnum.MobilApp:
                    return true;
                case ChannelEnum.Bilinmiyor:
                    return false;
                default:
                    return false;
            }
        }
        public static bool IsOfflineContact(ChannelEnum channel)
        {
            switch (channel)
            {
                case ChannelEnum.Crm:
                    return true;
                case ChannelEnum.ETicaret:
                    return false;
                case ChannelEnum.Magaza:
                    return true;
                case ChannelEnum.MobilApp:
                    return true;
                case ChannelEnum.Bilinmiyor:
                    return false;
                default:
                    return false;
            }
        }

        public static string GetCrmDbName(OrganizationEnum organizations)
        {
            switch (organizations)
            {
                case OrganizationEnum.TR_LT:
                    return "KahveDunyasi_MSCRM";
                case OrganizationEnum.TR_LT_TEST:
                    return "KahveDunyasi_MSCRM";
                case OrganizationEnum.TR:
                    return "KahveDunyasi_MSCRM";
                case OrganizationEnum.TR_TEST:
                    return "KahveDunyasi_MSCRM";
                default:
                    return "KahveDunyasi_MSCRM";
            }
        }

        /// <summary>
        /// TModel içerisindeki statecode ve statuscode alanlarını set etme işlemi.
        /// Crm de statecode ve statuscode alanları sabit olduğu için genel bir fonksiyon yazıldı.  
        /// StatusType enum değerine göre çalışmaktadır.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="requestDto"></param>
        /// <param name="entityModel"></param>
        /// <param name="StatusEnumFieldName"></param>
        /// <returns></returns>
        public static TModel EntityModelSetStateAndStatusCode<T, TModel>(T requestDto, TModel entityModel, string StatusEnumFieldName = "StatusEnum")
        {

            StatusType StatusEnum = (StatusType)requestDto.GetType().GetProperty(StatusEnumFieldName).GetValue(requestDto);

            if (StatusEnum == StatusType.Pasif)
            {
                PropertyInfo propertyInfoStatecode = entityModel.GetType().GetProperty("statecode");
                propertyInfoStatecode.SetValue(entityModel, 1, null);
                PropertyInfo propertyInfoStatuscode = entityModel.GetType().GetProperty("statuscode");
                propertyInfoStatuscode.SetValue(entityModel, 2, null);
            }
            if (StatusEnum == StatusType.Aktif)
            {
                PropertyInfo propertyInfoStatecode = entityModel.GetType().GetProperty("statecode");
                propertyInfoStatecode.SetValue(entityModel, 0, null);
                PropertyInfo propertyInfoStatuscode = entityModel.GetType().GetProperty("statuscode");
                propertyInfoStatuscode.SetValue(entityModel, 1, null);
            }

            return entityModel;

            //T model alandaki verileri çekmek.
            //var stateCode = entityModel.GetType().GetProperty("statecode").GetValue(entityModel);
            //var statusCode = entityModel.GetType().GetProperty("statuscode").GetValue(entityModel);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static string Get_VakkoServiceLink(string parameter)
        {

            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            switch (parameter)
            {
                case "Erp_Customer_Save_Url":
                    return
                    isTest ? ConfigurationManager.AppSettings["Erp_Customer_Save_Url_TEST"]
                    : ConfigurationManager.AppSettings["Erp_Customer_Save_Url"];
                case "Erp_Api_Login_Token_Url":
                    return
                    isTest ? ConfigurationManager.AppSettings["Erp_Api_Login_Token_Url_TEST"]
                    : ConfigurationManager.AppSettings["Erp_Api_Login_Token_Url"];
                case "Erp_Api_Login_User":
                    return
                    isTest ? ConfigurationManager.AppSettings["Erp_Api_Login_User_TEST"]
                    : ConfigurationManager.AppSettings["Erp_Api_Login_User"];
                case "Loyalty_Api_Url":
                    return
                    isTest ? ConfigurationManager.AppSettings["Loyalty_Api_Url_TEST"]
                    : ConfigurationManager.AppSettings["Loyalty_Api_Url"];
                default:
                    return "";
            }
        }

    }
}