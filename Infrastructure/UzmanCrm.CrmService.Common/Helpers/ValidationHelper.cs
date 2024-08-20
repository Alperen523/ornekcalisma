using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace UzmanCrm.CrmService.Common.Helpers
{
    public static class ValidationHelper
    {
        public static bool IsNotNullAndEmpty(this string obj)
        {
            return obj != null && obj.Trim() != "" && obj.Trim().ToLower() != "null";
        }

        public static bool IsNullOrEmpty(this string obj)
        {
            return !(obj != null && obj.Trim() != "" && obj.Trim().ToLower() != "null");
        }

        public static bool IsNotNullAndEmpty(this int? obj)
        {
            return obj != null;
        }

        public static bool IsNotNullAndEmpty(this object obj)
        {
            return obj != null && obj.ToString().IsNotNullAndEmpty();
        }

        public static bool IsNotNullAndEmpty<T>(this T model, string propName)
        {
            if (model == null)
                return false;
            return model.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual(propName)) != null;
        }

        public static bool IsNotNullAndEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Count() > 0 && list.FirstOrDefault() != null;
        }

        public static bool IsNotNullAndEmpty(this Guid obj)
        {
            return obj != Guid.Empty;
        }

        public static bool IsNotNullAndEmpty(this Guid? obj)
        {
            return obj != null && obj != Guid.Empty;
        }

        public static bool IsNullOrEmpty(this Guid? obj)
        {
            return !obj.IsNotNullAndEmpty();
        }

        public static bool IsNotNullAndEmpty(this DateTime obj)
        {
            return obj != DateTime.MinValue && obj.Year != 1900 && obj.Year != 1901;
        }

        public static bool IsNotNullAndEmpty(this DateTime? obj)
        {
            return obj != null && obj.Value.IsNotNullAndEmpty();
        }

        public static bool IsNullOrEmpty(this DateTime? obj)
        {
            return !obj.IsNotNullAndEmpty();
        }

        public static bool IsNotNullAndEmpty(this bool? obj, bool? isFalseAsNull = false)
        {
            return isFalseAsNull == true ? obj == true : obj != null;
        }

        public static bool IsNullOrEmpty(this bool? obj, bool? isFalseAsNull = false)
        {
            return !obj.IsNotNullAndEmpty();
        }

        public static string SetValueIfEmpty(this string strToCheck, string strToSet)
        {
            return strToCheck.IsNotNullAndEmpty() ? strToCheck : strToSet;
        }

        public static string GetEnumName<T>(this int enumValue)
        {
            return Enum.GetName(typeof(T), enumValue);
        }

        public static object GetValue<T>(this T model, string propName)
        {
            if (!model.IsNotNullAndEmpty(propName))
                return null;
            var prop = model.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual(propName));
            return prop.GetValue(model, null);
        }

        public static bool SetValue<T>(this T model, string propName, object value)
        {
            if (!model.IsNotNullAndEmpty(propName))
                return false;
            var prop = model.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual(propName));
            prop.SetValue(model, value);
            return true;
        }

        public static bool IsValidEmail(this string strEMail)
        {
            if (!strEMail.IsNotNullAndEmpty())
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            strEMail = Regex.Replace(strEMail, @"(@)(.+)$", DomainMapper);
            if (!strEMail.IsNotNullAndEmpty())
                return false;

            if (strEMail.Contains("\\")) return false;
            if (strEMail.Contains("@test") || strEMail.Contains("@deneme")) return false;
            // Return true if strIn is in valid e-mail format.
            return Regex.IsMatch(strEMail,
                   @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                   RegexOptions.IgnoreCase);
        }

        public static string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();
            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                return string.Empty;
            }
            return match.Groups[1].Value + domainName;
        }

        public static bool IsEqual(this string str1, string str2, bool? isCaseInsensitive = true, bool? isCompareUniversal = true)
        {
            if ((str1 == null && str2 != null) || (str1 != null && str2 == null))
                return false;

            if (isCaseInsensitive.Value && isCompareUniversal.Value)
                return str1.Trim().RemoveTurkishAndUpper() == str2.Trim().RemoveTurkishAndUpper();
            else if (!isCaseInsensitive.Value && isCompareUniversal.Value)
                return FormatHelper.RemoveTurkish(str1.Trim()) == FormatHelper.RemoveTurkish(str2.Trim());
            else if (isCaseInsensitive.Value && !isCompareUniversal.Value)
                return str1.ToLower().Trim() == str2.ToLower().Trim();
            else
                return str1.Trim() == str2.Trim();
        }

        public static bool Includes(this string str1, string str2, bool? isCaseInsensitive = true, bool? isCompareUniversal = true)
        {
            if ((str1 == null && str2 != null) || (str1 != null && str2 == null))
                return false;

            if (isCaseInsensitive.Value && isCompareUniversal.Value)
                return str1.Trim().RemoveTurkishAndUpper().Contains(str2.Trim().RemoveTurkishAndUpper());
            else if (!isCaseInsensitive.Value && isCompareUniversal.Value)
                return FormatHelper.RemoveTurkish(str1.Trim()).Contains(FormatHelper.RemoveTurkish(str2.Trim()));
            else if (isCaseInsensitive.Value && !isCompareUniversal.Value)
                return str1.ToLower().Trim().Contains(str2.ToLower().Trim());
            else
                return str1.Trim().Contains(str2.Trim());
        }

        //public static bool IsNotNullAndEmpty(this Entity entity, string str)
        //{
        //    return entity != null && entity.Attributes.Contains(str) && entity.Attributes[str] != null 
        //        && entity.Attributes[str].ToString().IsNotNullAndEmpty();
        //}

        //public static bool IsNotNullAndEmpty(this DataRow entity, string obj)
        //{
        //    return entity.Table.Columns.Contains(obj) && entity[obj] != null && entity[obj].ToString().IsNotNullAndEmpty();
        //}

        public static string GetUserId()
        {
            var item = "";
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            if (isTest)
                item = ConfigurationManager.AppSettings["UserId"];
            else
                item = ConfigurationManager.AppSettings["UserId_TEST"];

            return item;
        }


        public static string GetBusinessUnitId()
        {
            var item = "";
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            if (isTest)
                item = ConfigurationManager.AppSettings["BusinessUnitId"];
            else
                item = ConfigurationManager.AppSettings["BusinessUnitId_TEST"];

            return item;
        }

        public static string GetOrganizationId()
        {
            var item = "";
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            if (isTest)
                item = ConfigurationManager.AppSettings["OrganizationId"];
            else
                item = ConfigurationManager.AppSettings["OrganizationId_TEST"];

            return item;
        }

    }
}
