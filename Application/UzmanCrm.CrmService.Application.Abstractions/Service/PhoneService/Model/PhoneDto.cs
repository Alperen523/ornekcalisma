﻿using System;

namespace UzmanCrm.CrmService.Application.Abstractions.Service.PhoneService.Model
{
    public class PhoneDto//uzm_customerphone
    {
        public Guid? createdby { get; set; } = null;
        public string createdbyname { get; set; } = null;
        public DateTime? createdon { get; set; } = null;
        public int? importsequencenumber { get; set; } = null;
        public Guid? modifiedby { get; set; } = null;
        public string modifiedbyname { get; set; } = null;
        public DateTime? modifiedon { get; set; } = null;
        public Guid? organizationid { get; set; } = null;
        public string organizationidname { get; set; } = null;
        public int? statecode { get; set; } = null;
        public int? statuscode { get; set; } = null;
        public Guid? uzm_call_unsubscribe_channelid { get; set; } = null;
        public string uzm_call_unsubscribe_channelidname { get; set; } = null;
        public bool? uzm_callisiystrigger { get; set; } = null;
        public bool? uzm_calliyssend { get; set; } = null;
        public bool? uzm_callpermission { get; set; } = null;
        public DateTime? uzm_callunsubscribedate { get; set; } = null;
        public bool? uzm_chocpatismspermission { get; set; } = null;
        public bool? uzm_christoflesmspermission { get; set; } = null;
        public string uzm_countryphonecode { get; set; } = null;
        public Guid? uzm_createdlocationid { get; set; } = null;
        public string uzm_createdlocationidname { get; set; } = null;
        public Guid? uzm_createdpersonid { get; set; } = null;
        public string uzm_createdpersonidname { get; set; } = null;
        public Guid? uzm_customerid { get; set; } = null;
        public Guid? uzm_customeridbeforemerge { get; set; } = null;
        public string uzm_customeridbeforemergename { get; set; } = null;
        public string uzm_customeridname { get; set; } = null;
        public Guid? uzm_customerphoneid { get; set; } = null;
        public string uzm_customerphonenumber { get; set; } = null;
        public int? uzm_datasourceid { get; set; } = null;
        public DateTime? uzm_erpcreatedon { get; set; } = null;
        public DateTime? uzm_erpmodifiedon { get; set; } = null;
        public bool? uzm_esmodsmspermission { get; set; } = null;
        public bool? uzm_espassmspermission { get; set; } = null;
        public string uzm_exmessage { get; set; } = null;
        public bool? uzm_folliefolliesmspermission { get; set; } = null;
        public bool? uzm_isdeleted { get; set; } = null;
        public bool? uzm_isiystrigger { get; set; } = null;
        public string uzm_iyscallid { get; set; } = null;
        public bool? uzm_iyscallpermit { get; set; } = null;
        public DateTime? uzm_iyscallpermitdate { get; set; } = null;
        public Guid? uzm_iyscallunsubscribe_channelid { get; set; } = null;
        public string uzm_iyscallunsubscribe_channelidname { get; set; } = null;
        public DateTime? uzm_iyscallunsubscribedate { get; set; } = null;
        public string uzm_iysphoneid { get; set; } = null;
        public bool? uzm_iysphonepermit { get; set; } = null;
        public DateTime? uzm_iysphonepermitdate { get; set; } = null;
        public bool? uzm_iyssend { get; set; } = null;
        public Guid? uzm_iysunsubscribe_channelid { get; set; } = null;
        public string uzm_iysunsubscribe_channelidname { get; set; } = null;
        public DateTime? uzm_iysunsubscribedate { get; set; } = null;
        public bool? uzm_leonarasmspermission { get; set; } = null;
        public DateTime? uzm_mergeddate { get; set; } = null;
        public Guid? uzm_modifiedbylocationid { get; set; } = null;
        public string uzm_modifiedbylocationidname { get; set; } = null;
        public Guid? uzm_modifiedbypersonid { get; set; } = null;
        public string uzm_modifiedbypersonidname { get; set; } = null;
        public bool? uzm_ozeldikimsmspermission { get; set; } = null;
        public bool? uzm_phonepermission { get; set; } = null;
        public int? uzm_phonetype { get; set; } = null;
        public bool? uzm_powersmspermission { get; set; } = null;
        public bool? uzm_pronoviassmspermission { get; set; } = null;
        public Guid? uzm_releatedpermissionid { get; set; } = null;
        public string uzm_releatedpermissionidname { get; set; } = null;
        public bool? uzm_statuschangedduetocustomer { get; set; } = null;
        public bool? uzm_tomssmspermission { get; set; } = null;
        public Guid? uzm_unsubscribe_channelid { get; set; } = null;
        public string uzm_unsubscribe_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribechocpati_channelid { get; set; } = null;
        public string uzm_unsubscribechocpati_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribechristofle_channelid { get; set; } = null;
        public string uzm_unsubscribechristofle_channelidname { get; set; } = null;
        public DateTime? uzm_unsubscribedate { get; set; } = null;
        public DateTime? uzm_unsubscribedatechopati { get; set; } = null;
        public DateTime? uzm_unsubscribedatechristofle { get; set; } = null;
        public DateTime? uzm_unsubscribedateesmod { get; set; } = null;
        public DateTime? uzm_unsubscribedateespas { get; set; } = null;
        public DateTime? uzm_unsubscribedatefollie { get; set; } = null;
        public DateTime? uzm_unsubscribedateleonara { get; set; } = null;
        public DateTime? uzm_unsubscribedateoutlet { get; set; } = null;
        public DateTime? uzm_unsubscribedateozeldikim { get; set; } = null;
        public DateTime? uzm_unsubscribedatepower { get; set; } = null;
        public DateTime? uzm_unsubscribedatepronovias { get; set; } = null;
        public DateTime? uzm_unsubscribedatetoms { get; set; } = null;
        public DateTime? uzm_unsubscribedatev2k { get; set; } = null;
        public DateTime? uzm_unsubscribedatevr { get; set; } = null;
        public DateTime? uzm_unsubscribedatewcoll { get; set; } = null;
        public DateTime? uzm_unsubscribedatewedding { get; set; } = null;
        public Guid? uzm_unsubscribeesmod_channelid { get; set; } = null;
        public string uzm_unsubscribeesmod_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribeespas_channelid { get; set; } = null;
        public string uzm_unsubscribeespas_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribefollie_channelid { get; set; } = null;
        public string uzm_unsubscribefollie_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribeleonora_channelid { get; set; } = null;
        public string uzm_unsubscribeleonora_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribeoutlet_channelid { get; set; } = null;
        public string uzm_unsubscribeoutlet_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribeozeldikim_channelid { get; set; } = null;
        public string uzm_unsubscribeozeldikim_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribepower_channelid { get; set; } = null;
        public string uzm_unsubscribepower_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribepronovias_channelid { get; set; } = null;
        public string uzm_unsubscribepronovias_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribetoms_channelid { get; set; } = null;
        public string uzm_unsubscribetoms_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribev2k_channelid { get; set; } = null;
        public string uzm_unsubscribev2k_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribevr_channelid { get; set; } = null;
        public string uzm_unsubscribevr_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribewcoll_channelid { get; set; } = null;
        public string uzm_unsubscribewcoll_channelidname { get; set; } = null;
        public Guid? uzm_unsubscribewedding_channelid { get; set; } = null;
        public string uzm_unsubscribewedding_channelidname { get; set; } = null;
        public bool? uzm_v2ksmspermission { get; set; } = null;
        public bool? uzm_vakkooutletsmspermission { get; set; } = null;
        public bool? uzm_vrsmspermission { get; set; } = null;
        public bool? uzm_wcollectionsmspermission { get; set; } = null;
        public bool? uzm_weddingsmspermission { get; set; } = null;
        public Guid? uzm_whatsapp_unsubscribe_channelid { get; set; } = null;
        public string uzm_whatsapp_unsubscribe_channelidname { get; set; } = null;
        public bool? uzm_whatsapppermission { get; set; } = null;
        public DateTime? uzm_whatsappunsubscribedate { get; set; } = null;


        public string createdpersonno { get; set; } = null;
        public string updatedpersonno { get; set; } = null;
    }

}
