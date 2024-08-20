using Hangfire;
using System;
using UzmanCrm.CrmService.Application.Abstractions.Service.BackgroundService;

namespace UzmanCrm.CrmService.Hangfire.Helper
{
    public static class HangFireJob
    {
        public static void DoJob()
        {
            Customer_Endorsement_Data_Processing();
            Will_Be_Expired_Soon_Card_Exception_Discount_Send_Email();
            Set_Status_Expired_Today_Card_Exception_Discount();
            Batch_Approval_List_Data_Processing();
        }

        /// <summary>
        /// Gelen ciro kayıtlarını işleme
        /// </summary>
        public static void Customer_Endorsement_Data_Processing()
        {
            string name = nameof(Customer_Endorsement_Data_Processing);
            RecurringJob.RemoveIfExists(name);
            RecurringJob.AddOrUpdate<IBackgroundService>(name, _ => _.Customer_Endorsement_Data_Processing(), "*/3 * * * *", TimeZoneInfo.Local);
        }

        public static void Card_Exception_Discount_Send_Email_By_ArrivalChannel()
        {
            string name = nameof(Card_Exception_Discount_Send_Email_By_ArrivalChannel);
            RecurringJob.RemoveIfExists(name);
            RecurringJob.AddOrUpdate<IBackgroundService>(name, _ => _.Card_Exception_Discount_Send_Email_By_ArrivalChannel(), Cron.Monthly, TimeZoneInfo.Local);
        }

        /// <summary>
        /// Bitiş tarihine kart sınıfı segmenti kaydındaki süre kadar zaman kalan istisna kayıtlarını ilgili kişilere mail ile gönderme
        /// </summary>
        public static void Will_Be_Expired_Soon_Card_Exception_Discount_Send_Email()
        {
            string name = nameof(Will_Be_Expired_Soon_Card_Exception_Discount_Send_Email);
            RecurringJob.RemoveIfExists(name);
            RecurringJob.AddOrUpdate<IBackgroundService>(name, _ => _.Will_Be_Expired_Soon_Card_Exception_Discount_Send_Email(), "0 5 1 * *", TimeZoneInfo.Local); //0 5 1 * * : Her ayın 1 inde saat 5te
        }

        /// <summary>
        /// Bitiş tarihi bugün olan kart istisna kayıtlarının durumlarını süresi doldu olarak işaretleme
        /// </summary>
        public static void Set_Status_Expired_Today_Card_Exception_Discount()
        {
            string name = nameof(Set_Status_Expired_Today_Card_Exception_Discount);
            RecurringJob.RemoveIfExists(name);
            RecurringJob.AddOrUpdate<IBackgroundService>(name, _ => _.Set_Status_Expired_Today_Card_Exception_Discount(), Cron.Daily(5), TimeZoneInfo.Local);
        }

        /// <summary>
        /// Gelen toplu istisna onay kayıtlarını işleme
        /// </summary>
        public static void Batch_Approval_List_Data_Processing()
        {
            string name = nameof(Batch_Approval_List_Data_Processing);
            RecurringJob.RemoveIfExists(name);
            RecurringJob.AddOrUpdate<IBackgroundService>(name, _ => _.Batch_Approval_List_Data_Processing(), "*/5 * * * *", TimeZoneInfo.Local);
        }





    }
}



// */10 * * * *  10 dk bir.
// 0 * * * *     her saat başı
// 0 6 * * *    Her gün saat 6 da
// 0 5 1 * *    Her ayın 1'inde saat 5'te