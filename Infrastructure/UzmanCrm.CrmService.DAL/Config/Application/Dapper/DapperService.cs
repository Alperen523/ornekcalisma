using Dapper;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UzmanCrm.CrmService.Application.Abstractions.Service.LogService;
using UzmanCrm.CrmService.Application.Abstractions.Service.Shared;
using UzmanCrm.CrmService.Common.Enums;
using UzmanCrm.CrmService.Common.Helpers;
using UzmanCrm.CrmService.DAL.Config.Abstractions.Dapper;
using UzmanCrm.CrmService.DAL.Config.Application.Common;
using UzmanCrm.CrmService.DAL.Config.Application.Dapper.Model;
using UzmanCrm.CrmService.DAL.Helper;

namespace UzmanCrm.CrmService.DAL.Config.Application.Dapper
{
    public class DapperService : IDapperService
    {
        private string DapperRowContains = "Dapper";

        /// <summary>
        /// Dapper kullanırken entity'lerin FilteredView göünümleri üzerinden sorgu yapabilmek için eklenir
        /// </summary>
        private string QueryPrefix = $@"DECLARE @uid uniqueidentifier = '{ValidationHelper.GetUserId()}' SET CONTEXT_INFO @uid ";
        private readonly ILogService logService;
        private readonly IMemoryCache _memoryCache;


        /// <summary>
        /// Sql connection object
        /// </summary>
        private SqlConnection sqlConnection;

        /// <summary>
        /// Connection string
        /// </summary>
        private string connectionString;


        /// <summary>
        /// Getting connection string from web config and set global variable property
        /// Create sql connection object
        /// </summary>
        public DapperService(ILogService logService, IMemoryCache memoryCache)
        {
            this.logService = logService;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Connection open
        /// </summary>
        private void ConOpen(ConnectionStringNames dbName = (ConnectionStringNames)0)
        {
            bool isTest = false;
            Boolean.TryParse(ConfigurationManager.AppSettings["IsTest"], out isTest);
            if (isTest)
            {
                dbName = DatabaseHelper.GetTestConnectionStringByConnectionString(dbName);
            }

            // If dispose connection should be new
            var str = Enum.GetName(typeof(ConnectionStringNames), dbName);
            connectionString = ConfigurationManager.ConnectionStrings[str].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            if (sqlConnection.State == ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        /// <summary>
        /// Connection close
        /// </summary>
        private void ConClose()
        {
            if (sqlConnection.State == ConnectionState.Open)
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        /// <summary>
        /// Getting stored procedure, return first or default item
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return first or default item</returns>
        public async Task<Response<TRes>> GetItem<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QueryFirstOrDefaultAsync<TRes>(storedProcedureName,
                                                                                      param: paramObject,
                                                                                      commandTimeout: 180,
                                                                                      commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                SetValueAndCloseConnection(res, procedureRes);
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetItem),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Getting sql query, return first or default item
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return first or default item</returns>
        public async Task<Response<TRes>> GetItem<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QueryFirstOrDefaultAsync<TRes>(sql: QueryPrefix + query,
                                                                                      param: args,
                                                                                      commandTimeout: 180,
                                                                                      commandType: CommandType.Text);
                SetValueAndCloseConnection(res, procedureRes);
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetItem),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Getting sql query by param model, return list
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return list</returns>
        public async Task<Response<TRes>> GetItemParam<TReq, TRes>(string query, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QueryFirstOrDefaultAsync<TRes>(sql: QueryPrefix + query,
                                                                        param: paramObject,
                                                                        commandTimeout: 180,
                                                                        commandType: CommandType.Text);

                SetValueAndCloseConnection(res, procedureRes);
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetItemParam),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Getting stored procedure, return list async
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return list</returns>
        public async Task<Response<List<TRes>>> GetListAsync<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<List<TRes>>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QueryAsync<TRes>(sql: storedProcedureName,
                                                                        paramObject,
                                                                        commandTimeout: 180,
                                                                        commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                SetValueAndCloseConnection(res, procedureRes.ToList());
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetListAsync),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Getting sql query, return list async
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return list</returns>
        public async Task<Response<List<TRes>>> GetListAsync<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<List<TRes>>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QueryAsync<TRes>(sql: QueryPrefix + query,
                                                                        param: args,
                                                                        commandTimeout: 180,
                                                                        commandType: CommandType.Text).ConfigureAwait(false);
                SetValueAndCloseConnection(res, procedureRes.ToList());
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetListAsync),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Getting sql query by param model, return list
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return list</returns>
        public async Task<Response<List<TRes>>> GetListByParamAsync<TReq, TRes>(string query, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<List<TRes>>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QueryAsync<TRes>(sql: QueryPrefix + query,
                                                                        param: paramObject,
                                                                        commandTimeout: 180,
                                                                        commandType: CommandType.Text).ConfigureAwait(false);
                SetValueAndCloseConnection(res, procedureRes.ToList());
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetListByParamAsync),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Save method like a update,delete insert
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        public async Task<Response<TRes>> Save<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QuerySingleAsync<TRes>(sql: storedProcedureName,
                                                                              param: paramObject,
                                                                              commandTimeout: 180,
                                                                              commandType: CommandType.StoredProcedure).ConfigureAwait(false);
                if (procedureRes.GetType().Name.Contains(DapperRowContains))
                {
                    var matchObject = CommonMethod.DynamicToClass(procedureRes, res.Data);
                    SetValueAndCloseConnection(res, matchObject);
                }
                else
                {
                    SetValueAndCloseConnection(res, procedureRes);
                }
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(Save),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Save method with query like a update,delete and insert
        /// Procedure should be output params
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="query">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        public async Task<Response<TRes>> Save<TRes>(string query, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QuerySingleAsync<TRes>(sql: QueryPrefix + query,
                                                                              commandTimeout: 180,
                                                                              commandType: CommandType.Text).ConfigureAwait(false);
                if (procedureRes.GetType().Name.Contains(DapperRowContains))
                {
                    var matchObject = CommonMethod.DynamicToClass(procedureRes, res.Data);
                    SetValueAndCloseConnection(res, matchObject);
                }
                else
                {
                    SetValueAndCloseConnection(res, procedureRes);
                }
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(Save),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }


        /// <summary>
        /// Save method with query like a update,delete and insert
        /// Procedure should be output params
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="query">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        public async Task<Response<TRes>> Save<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = 0) where TRes : new()
        {
            var res = new Response<TRes>();
            SqlConnection sqlConnection = null;
            try
            {
                ConOpen(dbName);

                var procedureRes = await sqlConnection.QuerySingleAsync<TRes>(sql: QueryPrefix + query,
                                                                              param: args,
                                                                              commandTimeout: 86400,
                                                                              commandType: CommandType.Text).ConfigureAwait(false);
                if (procedureRes.GetType().Name.Contains(DapperRowContains))
                {
                    var matchObject = CommonMethod.DynamicToClass(procedureRes, res.Data);
                    SetValueAndCloseConnection(res, matchObject);
                }
                else
                {
                    SetValueAndCloseConnection(res, procedureRes);
                }
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(Save),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Save method with query like a update,delete and insert by param
        /// Procedure should be output params
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <param name="query">Query procedure name</param>
        /// <param name="paramObject">Query procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Effected rows int</returns>
        public async Task<Response<TRes>> SaveQueryParam<TReq, TRes>(string query, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                ConOpen(dbName);
                var procedureRes = await sqlConnection.QuerySingleAsync<TRes>(sql: QueryPrefix + query,
                                                                              param: paramObject,
                                                                              commandTimeout: 180,
                                                                              commandType: CommandType.Text).ConfigureAwait(false);
                if (procedureRes.GetType().Name.Contains(DapperRowContains))
                {
                    var matchObject = CommonMethod.DynamicToClass(procedureRes, res.Data);
                    SetValueAndCloseConnection(res, matchObject);
                }
                else
                {
                    SetValueAndCloseConnection(res, procedureRes);
                }
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(SaveQueryParam),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    $@"ExceptionDetail:{ex}
                       Query:{query}
                       ParamObj:{JsonConvert.SerializeObject(paramObject)}"
                );
            }

            return res;
        }


        /// <summary>
        /// Getting stored procedure, return list
        /// </summary>
        /// <typeparam name="TReq">Request type</typeparam>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="storedProcedureName">Stored procedure name</param>
        /// <param name="paramObject">Stored procedure parameters object</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Stored procedure, return list</returns>
        public Response<List<TRes>> GetList<TReq, TRes>(string storedProcedureName, TReq paramObject, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<List<TRes>>();
            try
            {
                ConOpen(dbName);
                var procedureRes = sqlConnection.Query<TRes>(sql: storedProcedureName,
                                                                        paramObject,
                                                                        commandTimeout: 180,
                                                                        commandType: CommandType.StoredProcedure);
                SetValueAndCloseConnection(res, procedureRes.ToList());
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                logService.LogSaveSync(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetList),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Getting sql query, return list
        /// </summary>
        /// <typeparam name="TRes">Response type</typeparam>
        /// <param name="query">Sql query</param>
        /// <param name="args">Parameters if exists</param>
        /// <param name="dbName">Db name enum for multiple db</param>
        /// <returns>Sql query, return list</returns>
        public Response<List<TRes>> GetList<TRes>(string query, Dictionary<string, object> args = null, ConnectionStringNames dbName = (ConnectionStringNames)0) where TRes : new()
        {
            var res = new Response<List<TRes>>();
            try
            {
                ConOpen(dbName);
                var procedureRes = sqlConnection.Query<TRes>(sql: QueryPrefix + query,
                                                                        param: args,
                                                                        commandTimeout: 180,
                                                                        commandType: CommandType.Text);
                SetValueAndCloseConnection(res, procedureRes.ToList());
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                logService.LogSaveSync(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(GetList),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    ex
                );
            }

            return res;
        }

        /// <summary>
        /// Set value for res and close connectinon
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="res"></param>
        /// <param name="procedureRes"></param>
        private void SetValueAndCloseConnection<T>(Response<T> res, T procedureRes) where T : new()
        {
            if (procedureRes == null)
                CommonMethod.SetError(res, "No record found.");
            else if (procedureRes is IEnumerable && procedureRes is not string)
            {
                if (((IList)procedureRes).Count == 0)
                    CommonMethod.SetError(res, "No record found.");
                else
                    CommonMethod.SetSuccess(res);
            }
            else
                CommonMethod.SetSuccess(res);

            res.Data = procedureRes;
            ConClose();
        }


        /// <summary>
        /// Dapper da Modelin sql olarak db ye kaydı için kullanılan metotlar.
        /// </summary>

        public async Task<Response<TRes>> SaveAsync<TRes>(object req, string tableName = null, SaveModeEnum saveMode = SaveModeEnum.Auto, ConnectionStringNames dbName = ConnectionStringNames.CRM) where TRes : new()
        {
            var res = new Response<TRes>();
            try
            {
                tableName = tableName ?? req.GetType().Name;
                tableName = tableName.EndsWith("Base") ? tableName.Substring(0, tableName.Length - 4) : tableName;
                var idColumn = dbName == ConnectionStringNames.CRM ? req.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual(tableName + "Id")) : req.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual("Id"));
                var id = idColumn != null ? idColumn.GetValue(req, null) : null;
                if (saveMode == SaveModeEnum.Auto)
                    saveMode = id != null ? SaveModeEnum.Update : SaveModeEnum.Insert;
                SqlQueryModel query = new SqlQueryModel();
                if (saveMode == SaveModeEnum.Insert)
                {
                    if (dbName == ConnectionStringNames.CRM)
                        FillBaseParameters(ref req);
                    query = await ToInsertQueryAsync(req, tableName, dbName);
                }
                else
                {
                    if (id == null)
                        throw new Exception("Id cannot be empty for records that will be updated!");
                    query = await ToUpdateQueryAsync(req, tableName, dbName);
                }
                if (dbName == ConnectionStringNames.CRM && (typeof(TRes) == typeof(Guid) || typeof(TRes) == typeof(Guid?)))
                    query.Query += $" SELECT @" + tableName + "Id";
                else if (id != null && id.GetType() == typeof(TRes).GenericTypeArguments[0])
                {
                    var key = query.Parameters.Keys.FirstOrDefault(x => x.IsEqual(idColumn.Name));
                    query.Query += $" SELECT @{(key != null ? key : idColumn.Name)}";
                }
                else if (typeof(TRes) == typeof(int) || typeof(TRes) == typeof(int?))
                    query.Query += " SELECT @@ROWCOUNT resp";
                res = await Save<TRes>(query.Query, query.Parameters, dbName);
            }
            catch (Exception ex)
            {
                res.Message = ex.Message;
                res.Success = true;
            }
            return res;
        }

        public async Task<SqlQueryModel> ToUpdateQueryAsync(object obj, string tableName, ConnectionStringNames dbName = ConnectionStringNames.CRM)
        {
            var queryModel = new SqlQueryModel();
            tableName = tableName.EndsWith("Base") ? tableName.Substring(0, tableName.Length - 4) : tableName;
            var schemaRes = await GetTableSchemaAsync(dbName == ConnectionStringNames.CRM ? tableName + "Base" : tableName, dbName);
            if (!schemaRes.Success)
                throw new Exception("Schema Error: " + schemaRes.Message);
            var idColumn = dbName == ConnectionStringNames.CRM ? obj.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual(tableName + "Id")) : obj.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual("Id"));
            var id = idColumn != null ? idColumn.GetValue(obj, null) : null;
            var idName = "";
            queryModel.Parameters = new Dictionary<string, object>();
            var query = $@"UPDATE {(dbName == ConnectionStringNames.CRM ? tableName + "Base" : tableName)} SET ";
            var values = "";
            var where = $@"WHERE ";
            foreach (var prop in obj.GetType().GetProperties())
            {
                var column = schemaRes.Data.TableSchema.FirstOrDefault(x => x.column_name.IsEqual(prop.Name.ToLower()));
                if (column == null)
                    continue;
                var columnName = column.column_name;
                if (columnName.IsEqual(tableName + "Id") || columnName.IsEqual("id"))
                {
                    prop.SetValue(obj, id);
                    idName = columnName;
                    where += $"{columnName} = @{columnName}";
                }
                else if (prop.GetValue(obj, null) != null && column != null)
                {
                    values += @$"{columnName} = @{columnName}, ";
                }

                var value = prop.GetValue(obj, null);
                if (value == null)
                    continue;
                if ((prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?)) && dbName == ConnectionStringNames.CRM)
                {
                    var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
                    value = ((DateTime)value).AddMinutes(-1 * offset);
                }

                queryModel.Parameters.Add(columnName, value);
            }
            idName = idName.IsNotNullAndEmpty() ? idName : tableName + "Id";
            query = query + values + where;
            query = query.Replace(", WHERE", " WHERE");
            queryModel.Query = query;
            return queryModel;
        }

        public async Task<SqlQueryModel> ToInsertQueryAsync(object obj, string tableName, ConnectionStringNames dbName = ConnectionStringNames.CRM)
        {
            var queryModel = new SqlQueryModel();
            tableName = tableName.EndsWith("Base") ? tableName.Substring(0, tableName.Length - 4) : tableName;
            var schemaRes = await GetTableSchemaAsync(dbName == ConnectionStringNames.CRM ? tableName + "Base" : tableName, dbName);
            if (!schemaRes.Success)
                throw new Exception("Schema Error: " + schemaRes.Message);

            var idColumn = obj.GetType().GetProperties().FirstOrDefault(x => x.Name.IsEqual(tableName + "Id"));
            var id = idColumn != null ? idColumn.GetValue(obj, null) : null;
            var idName = "";
            queryModel.Parameters = new Dictionary<string, object>();
            var query = $@"INSERT INTO {(dbName == ConnectionStringNames.CRM ? tableName + "Base" : tableName)} (";
            var values = $@") VALUES (";
            foreach (var prop in obj.GetType().GetProperties())
            {
                var column = schemaRes.Data.TableSchema.FirstOrDefault(x => x.column_name.IsEqual(prop.Name.ToLower()));
                if (column == null)
                    continue;
                var columnName = column.column_name;
                if (columnName.IsEqual(tableName + "Id") || columnName.IsEqual(tableName + "id"))
                {
                    prop.SetValue(obj, id ?? Guid.NewGuid());
                    idName = columnName;
                }

                if (prop.GetValue(obj, null) != null && column != null)
                {
                    var value = prop.GetValue(obj, null);
                    if ((prop.PropertyType == typeof(DateTime) || prop.PropertyType == typeof(DateTime?)) && dbName == ConnectionStringNames.CRM)
                    {
                        var offset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
                        value = ((DateTime)value).AddMinutes(-1 * offset);
                    }

                    query += columnName + ",";
                    values += "@" + columnName + ",";
                    queryModel.Parameters.Add(columnName, value);
                }
            }
            idName = idName.IsNotNullAndEmpty() ? idName : tableName + "Id";
            query = query + values + ")";
            query = query.Replace(",)", ")");
            queryModel.Query = query;
            return queryModel;
        }

        public async Task<Response<TableSchemaCacheModel>> GetTableSchemaAsync(string tableName, ConnectionStringNames dbName)
        {
            ConOpen(dbName);
            var res = new Response<TableSchemaCacheModel>();
            var query = "";
            try
            {

                var hasMetadata = _memoryCache.TryGetValue("tableschema-" + tableName, out TableSchemaCacheModel metadata);
                if (!hasMetadata || metadata.TableSchema == null || metadata.LastUpdatedDate < DateTime.Now.AddDays(-3))
                {
                    var connectionString = sqlConnection.ConnectionString;
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
                    query = $@"SELECT COLUMN_NAME, ORDINAL_POSITION, IS_NULLABLE, DATA_TYPE FROM {builder.InitialCatalog}.INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = @TableName";
                    var parameters = new Dictionary<string, object>();
                    parameters.Add("TableName", tableName);
                    var response = await GetListAsync<TableSchemaModel>(query, parameters, dbName);
                    if (!response.Success)
                        throw new Exception(response.Message);

                    metadata = new TableSchemaCacheModel()
                    {
                        EntityName = tableName,
                        TableSchema = response.Data,
                        LastUpdatedDate = DateTime.Now
                    };
                    var setting = _memoryCache.Set("tableschema-" + tableName, metadata);
                }
                else
                {
                    metadata = _memoryCache.Get<TableSchemaCacheModel>("tableschema-" + tableName);
                }
                res.Success = metadata != null ? true : false;
                res.Data = metadata;
                res.Message = "Success";

                ConClose();
            }
            catch (Exception ex)
            {
                ConClose();
                CommonMethod.SetError(res, ex.Message, true);
                await logService.LogSave(CrmService.Common.Enums.LogEventEnum.DbError,
                    this.GetType().Name,
                    nameof(SaveQueryParam),
                    CrmService.Common.Enums.CompanyEnum.KD,
                    LogTypeEnum.Response,
                    $@"ExceptionDetail:{ex}
                       Query:{query}"
                );

            }


            return res;

        }

        private void FillBaseParameters<TReq>(ref TReq req)
        {
            if (req.GetValue("createdon") == null)
                req.SetValue("CreatedOn", DateTime.Now);
            if (req.GetValue("modifiedon") == null)
                req.SetValue("ModifiedOn", DateTime.Now);
            if (req.GetValue("createdby") == null)
                req.SetValue("CreatedBy", new Guid(ValidationHelper.GetUserId()));
            if (req.GetValue("ModifiedBy") == null)
                req.SetValue("ModifiedBy", new Guid(ValidationHelper.GetUserId()));
            if (req.GetValue("StateCode") == null)
                req.SetValue("StateCode", 0);
            if (req.GetValue("StatusCode") == null)
                req.SetValue("StatusCode", 1);
            if (req.IsNotNullAndEmpty("OwnerId") && req.GetValue("OwnerId") == null)
                req.SetValue("OwnerId", new Guid(ValidationHelper.GetUserId()));
            if (req.IsNotNullAndEmpty("OwnerIdType") && req.GetValue("OwnerIdType") == null)
                req.SetValue("OwnerIdType", 8);
            if (req.IsNotNullAndEmpty("OwningBusinessUnit") && req.GetValue("OwningBusinessUnit") == null)
                req.SetValue("OwningBusinessUnit", new Guid(ValidationHelper.GetBusinessUnitId()));
            if (req.IsNotNullAndEmpty("OrganizationId") && req.GetValue("OrganizationId") == null)
                req.SetValue("OrganizationId", new Guid(ValidationHelper.GetOrganizationId()));
        }

    }
}
