using Newtonsoft.Json;
using System;
using System.Globalization;

namespace UzmanCrm.CrmService.Common.Helpers
{
    public static class FormatHelper
    {
        public static string RemoveTurkishAndUpper(this string Value)
        {
            if (Value.IsNotNullAndEmpty()) return "";
            Value = Value.ToUpper();
            return RemoveTurkish(Value);
        }

        public static string RemoveTurkish(this string Value)
        {
            if (string.IsNullOrEmpty(Value)) return Value;
            Value = Value.Replace('Ğ', 'G');
            Value = Value.Replace('Ü', 'U');
            Value = Value.Replace('Ş', 'S');
            Value = Value.Replace('İ', 'I');
            Value = Value.Replace('Ö', 'O');
            Value = Value.Replace('Ç', 'C');

            Value = Value.Replace('ğ', 'g');
            Value = Value.Replace('ü', 'u');
            Value = Value.Replace('ş', 's');
            Value = Value.Replace('ı', 'i');
            Value = Value.Replace('ö', 'o');
            Value = Value.Replace('ç', 'c');

            return Value;
        }

        public static string RemoveNonAlpha(this string Text)
        {
            char[] arr = Text.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsLetter(c) || char.IsWhiteSpace(c))));
            return new string(arr);
        }

        public static string RemoveNumeric(this string Text)
        {
            char[] arr = Text.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsDigit(c) == false)));
            return new string(arr);
        }

        public static string RemoveNonNumeric(this string Text)
        {
            char[] arr = Text.ToCharArray();
            arr = Array.FindAll<char>(arr, (c => (char.IsDigit(c))));
            return new string(arr);
        }

        public static string FormatTelephoneNumber(this string phoneNumber)
        {
            bool Plus90 = false;
            //if (phoneNumber.StartsWith("905") && phoneNumber.Length == 12)
            //    Plus90 = true;
            if (!phoneNumber.IsNotNullAndEmpty())
                return "";
            phoneNumber = RemoveNonNumeric(phoneNumber);
            #region İki Numara ve Dahili yazanları ayırmak 
            string sphoneNumber = phoneNumber.Replace(" ", "").Replace("+90", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("_", "");
            if (sphoneNumber.LastIndexOf('-') >= 7 && (sphoneNumber.Split('-').Length - 1) == 1)
            {
                phoneNumber = phoneNumber.Substring(0, phoneNumber.LastIndexOf('-'));
            }
            else if (sphoneNumber.LastIndexOf('/') >= 7 && (sphoneNumber.Split('/').Length - 1) == 1)
            {
                phoneNumber = phoneNumber.Substring(0, phoneNumber.LastIndexOf('/'));
            }
            #endregion
            //phoneNumber = RemoveNonNumeric(phoneNumber);
            //bool Plus90 = phoneNumber.StartsWith("+90");

            //Eğer +90 dışında başka bir ülkenin ülke kodu ile gelmiş ise formatında herhangi bir değişiklik yapmadan çık. 
            if (Plus90 == false && phoneNumber.StartsWith("+"))
                return phoneNumber;

            phoneNumber = phoneNumber.Replace(" ", "").Replace("+90", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("_", "");

            if (phoneNumber.Contains("111111") || phoneNumber.Contains("1234567"))
                phoneNumber = "";

            if (phoneNumber.StartsWith("90"))
                phoneNumber = phoneNumber.Substring(2);
            else if (phoneNumber.StartsWith("0"))
                phoneNumber = phoneNumber.Substring(1);

            if (phoneNumber == "")
            {
                //Do nothing
            }
            else if (phoneNumber.Length <= 4)
            {
                phoneNumber = "";
            }
            else if (phoneNumber.Length <= 7)
            {
                if (Plus90)
                    phoneNumber = string.Format("+90 {0} {1}{2}", phoneNumber.Substring(0, phoneNumber.Length - 4), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
                else
                    phoneNumber = string.Format("{0}{1}{2}", phoneNumber.Substring(0, phoneNumber.Length - 4), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
            }
            else if (phoneNumber.Length < 10)
            {
                if (Plus90)
                    phoneNumber = string.Format("+90 ({0}) {1} {2}{3}", phoneNumber.Substring(0, phoneNumber.Length - 7), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
                else
                    phoneNumber = string.Format("{0}{1}{2}{3}", phoneNumber.Substring(0, phoneNumber.Length - 7), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
            }
            else if (phoneNumber.Length == 10)
            {
                if (Plus90)
                    phoneNumber = string.Format("+90 ({0}) {1} {2}{3}", phoneNumber.Substring(0, phoneNumber.Length - 7), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
                else
                    phoneNumber = string.Format("({0}) {1} {2}{3}", phoneNumber.Substring(0, phoneNumber.Length - 7), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
            }
            else if (phoneNumber.Length <= 14)
            {
                if (Plus90)
                    phoneNumber = string.Format("+90 ({1}) {2} {3}{4}", phoneNumber.Substring(0, phoneNumber.Length - 10), phoneNumber.Substring(phoneNumber.Length - 10, 3), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
                else
                    phoneNumber = string.Format("{0}{1}{2}{3}{4}", phoneNumber.Substring(0, phoneNumber.Length - 10), phoneNumber.Substring(phoneNumber.Length - 10, 3), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
            }
            else
            {
                if (Plus90)
                    phoneNumber = string.Format("+90 ({1}) {2} {3}{4}", phoneNumber.Substring(phoneNumber.Length - 14, 4), phoneNumber.Substring(phoneNumber.Length - 10, 3), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
                else
                    phoneNumber = string.Format("{0}{1}{2}{3}{4}", phoneNumber.Substring(phoneNumber.Length - 14, 4), phoneNumber.Substring(phoneNumber.Length - 10, 3), phoneNumber.Substring(phoneNumber.Length - 7, 3), phoneNumber.Substring(phoneNumber.Length - 4, 2), phoneNumber.Substring(phoneNumber.Length - 2, 2));
            }

            phoneNumber = phoneNumber.TrimStart(new Char[] { '0' }); //Baştaki sıfırları silme
            phoneNumber = phoneNumber.TrimStart(new Char[] { '-' }); //Baştaki karakterleri silme
            phoneNumber = phoneNumber.TrimStart(new Char[] { '/' }); //Baştaki karakterleri silme

            return phoneNumber;
        }

        public static DateTime? ConvertToDateTime(this string date)
        {
            if (!date.IsNotNullAndEmpty())
                return null;
            try
            {
                var cInfo = new CultureInfo("tr-TR");
                
                return DateTime.Parse(date, cInfo);
            }
            catch
            {
                return null;
            }
        }

        public static string JsonSerializeObject(this object obj, bool? includeNull = false)
        {
            string json = includeNull == true ? JsonConvert.SerializeObject(obj) : JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            return json;
        }

        public static DateTime? EpochTimeToDateTime(this string epochSeconds)
        {
            if (!epochSeconds.IsNotNullAndEmpty())
                return null;

            try
            {
                DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(epochSeconds));

                return dateTimeOffset.LocalDateTime;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static int ToInt(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue);
        }

        public static string ToValueAsString(this Enum enumValue)
        {
            return Convert.ToInt32(enumValue).ToString();
        }

        public static int? ToInt(this string str)
        {
            int? result = null;
            try
            {
                result = int.Parse(str);
            }
            catch
            {
            }
            return result;
        }

        public static int? ToInt(this decimal? str)
        {
            int? result = null;
            try
            {
                result = (int?)str;
            }
            catch
            {
            }
            return result;
        }

        public static decimal? ToDecimal(this string str)
        {
            decimal? result = null;
            try
            {
                result = decimal.Parse(str);
            }
            catch
            {
            }
            return result;
        }
    }
}
