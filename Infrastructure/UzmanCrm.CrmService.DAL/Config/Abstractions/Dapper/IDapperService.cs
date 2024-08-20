using System.Collections.Generic;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.DAL.Config.Application.Dapper;

namespace UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper
{
    public interface IDapperService : IApplicationService
    {

        /// <summary>
        /// Getting stored procedure, return first or default item
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return first or default item</returns>
        Task<Response<TRes>> GetItem<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting sql query, return first or default item
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return first or default item</returns>
        Task<Response<TRes>> GetItem<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting sql query, return first or default item
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="paramObject">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return first or default item</returns>
        Task<Response<TRes>> GetItemParam<TReq, TRes>(string query, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting stored procedure, return list async
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return list</returns>
        Task<Response<List<TRes>>> GetListAsync<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting sql query, return list async
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return list</returns>
        Task<Response<List<TRes>>> GetListAsync<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting sql query, return list
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return list</returns>
        Task<Response<List<TRes>>> GetListByParamAsync<TReq, TRes>(string query, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Save method like a update,delete insert
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        Task<Response<TRes>> Save<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Save method with query like a update,delete and insert
        /// Procedure should be output params set res type this params
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="query">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        Task<Response<TRes>> Save<TRes>(string query, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Save method with query like a update,delete and insert with param
        /// Procedure should be output params set res type this params
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="query">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        Task<Response<TRes>> SaveQueryParam<TReq, TRes>(string query, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting stored procedure, return list
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return list</returns>
        Response<List<TRes>> GetList<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();

        /// <summary>
        /// Getting sql query, return list
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return list</returns>
        Response<List<TRes>> GetList<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new();
    }
}