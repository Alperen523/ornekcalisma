using System;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;

namespace UzmanCrm.CrmService.DAL.Config.Abstractions.CRM
{
    public interface ICRMService : IApplicationService
    {
        /// <summary>
        /// Create and update entity
        /// Example entityName=account if inside model accountid property is not null it's mean update
        /// Example entityName=account if inside model accountid property is null it's mean create
        /// </summary>
        /// <typeparam name="TReq">Model type</typeparam>
        /// <param name="req">Model</param>
        /// <param name="entityName">Entity name(if cant send it's mean model name=entity name)</param>
        /// <returns></returns>
        Response<Guid> Save<TReq>(TReq req, string entityName = null, string customeridtype = Common.CommonStaticConsts.CustomerIdType.Contact, CompanyEnum co = CompanyEnum.KD);

        /// <summary>
        /// Delete entity by name or model name
        /// </summary>
        /// <typeparam name="TReq">Entity type</typeparam>
        /// <param name="req">Request entity</param>
        /// <param name="entityName">Entity name(if cant send it's mean model name=entity name)</param>
        void Delete<TReq>(TReq req, string entityName = null, CompanyEnum co = CompanyEnum.KD);

        /// <summary>
        /// SendCrmEmail
        /// </summary>
        /// <typeparam name="TReq">Entity type</typeparam>
        /// <param name="req">Request entity</param>
        /// <param name="entityName">Entity name(if cant send it's mean model name=entity name)</param>
        Guid SendCrmEmail(string[] ToGuids, string ToType, string Subject, string Body, List<string> toMail = null, List<string> ccMail = null, object attachment = null, string attachtype = "", string attachname = "");
    }
}