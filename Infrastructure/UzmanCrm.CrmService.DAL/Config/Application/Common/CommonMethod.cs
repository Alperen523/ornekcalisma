using System;
using System.Collections.Generic;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common;

namespace UzmanCrm.CrmService.DAL.Config.Application.Common
{
    public static class CommonMethod
    {

        /// <summary>
        /// Throw if null object
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="res"></param>
        public static void ThrowIfNull<T>(T res)
        {
            if (res == null)
            {
                throw new ArgumentNullException("Result is null!");
            }
        }

        /// <summary>
        /// Set reponse for success
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="res">Response object</param>
        /// <param name="message">Success message</param>
        public static void SetSuccess<T>(Response<T> res, string message = "Process completed.") where T : new()
        {
            res.Success = true;
            res.Message = message;
        }

        /// <summary>
        /// Set reponse for error
        /// </summary>
        /// <typeparam name="T">Response type</typeparam>
        /// <param name="res">Response object</param>
        /// <param name="message">Error message</param>
        public static void SetError<T>(Response<T> res, string message = "An error occurred!", bool isInternalServer = false) where T : new()
        {
            res.Success = false;
            res.Message = message;

            if (isInternalServer)
                res.Error = new ErrorModel(System.Net.HttpStatusCode.InternalServerError, CommonStaticConsts.Message.InternalServerError, ErrorStaticConsts.GeneralErrorStaticConsts.V003);
            else
                res.Error = new ErrorModel(System.Net.HttpStatusCode.NotFound, CommonStaticConsts.Message.DataNotFound, ErrorStaticConsts.GeneralErrorStaticConsts.V002);
        }

        /// <summary>
        /// Matching dynamic response and dapper response
        /// Usually used for string
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="dynamicModels">Dapper response object type DapperRow</param>
        /// <param name="res">Response model</param>
        /// <returns>TRes type object</returns>
        public static TRes DynamicToClass<TRes>(dynamic dynamicModels, TRes res) where TRes : new()
        {
            var count = 0;
            foreach (var item in res.GetType().GetProperties())
            {
                count += 1;
            }

            foreach (var rows in dynamicModels)
            {
                var fields = (KeyValuePair<string, object>)rows;
                try
                {
                    var propName = fields.Key;
                    var propValue = fields.Value;
                    if (count == 0)
                    {
                        return (TRes)propValue;
                    }
                    else
                    {
                        if (res.GetType().GetProperty(propName) != null)
                        {
                            res.GetType().GetProperty(propName).SetValue(res, propValue);
                        }
                    }
                }
                catch
                {
                    //this property type doesn't match
                    continue;
                }
            }

            return res;
        }

        /// <summary>
        /// Get model property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model">Search entity</param>
        /// <param name="propertyName">Property name</param>
        /// <returns>Property value or null</returns>
        public static object GetPropertyByName<T>(T model, string propertyName)
        {
            if (model.GetType().GetProperty(propertyName) != null)
            {
                return model.GetType().GetProperty(propertyName).GetValue(model);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        public static Response<T> SetResponseSuccess<T>(T response) where T : new()
        {
            Response<T> res = new Response<T>();
            res.Data = response;
            SetSuccess(res);

            return res;
        }
    }
}
