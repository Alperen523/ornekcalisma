using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace UzmanCrm.CrmService.Plugins.DbConnection
{
    public class CrmConnectionDal
    {
        private static DateTime time_instance;
        private static CrmConnectionDal serviceInstance;
        private IOrganizationService organizationService;
        private static object lockobject = new object();


        string connection = ResourceSettings.ConnectionCanli;
        //canli: "Data Source=10.189.1.5;Initial Catalog=KahveDunyasi_MSCRM;User Id=uzmancrm; Password =uZm@408C;Connection Timeout=9999;Max Pool Size=200;";
        //test: "Data Source=10.189.1.13;Initial Catalog=KahveDunyasi_MSCRM;User Id=uzmancrm; Password =uZm@408C;Connection Timeout=9999;Max Pool Size=200;";
        //test: "Data Source=195.87.102.135;Initial Catalog=KahveDunyasi_MSCRM;User Id=uzmancrm; Password =uZm@408C;Connection Timeout=9999;Max Pool Size=200;";

        public IOrganizationService OrganizationService
        {
            get { return organizationService; }
        }
        public CrmConnectionDal()
        {
            if (ResourceSettings.CurrentEnvironment == "canli")
                connection = ResourceSettings.ConnectionCanli;
            else if (ResourceSettings.CurrentEnvironment == "test")
                connection = ResourceSettings.ConnectionTest;
            else if (ResourceSettings.CurrentEnvironment == "localtest")
                connection = ResourceSettings.ConnectionLocalTest;
        }

        public DataTable ToDataTable(string sql)
        {
            SqlDataAdapter da = new SqlDataAdapter(sql, new SqlConnection(connection));
            da.SelectCommand.CommandTimeout = 1200;
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            return dataSet.Tables[0];
        }

        public DataTable ToDataTable(string sql, List<SqlParameter> parameters)
        {
            SqlCommand cmd = new SqlCommand(sql, new SqlConnection(connection));
            if (parameters != null)
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet dataSet = new DataSet();
            da.Fill(dataSet);
            return dataSet.Tables[0];
        }

        public int ExecuteNonQuery<T>(string sql, List<SqlParameter> parameters)
        {
            int result = 0;
            try
            {
                using (SqlConnection cnn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters.ToArray());
                        cnn.Open();
                        var scalar = cmd.ExecuteNonQuery();
                        result = scalar;
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("[ExecuteNonQuery]" + e.Message);
            }
            return result;
        }

        public T GetScalar<T>(string sql, List<SqlParameter> parameters)
        {
            T result = default(T);

            try
            {
                using (SqlConnection cnn = new SqlConnection(connection))
                {
                    using (SqlCommand cmd = new SqlCommand(sql, cnn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters.ToArray());
                        cnn.Open();
                        var scalar = cmd.ExecuteScalar();
                        if (scalar != null && scalar != DBNull.Value)
                        {
                            result = scalar is T ? (T)scalar : (T)Convert.ChangeType(scalar, typeof(T));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("[GetScalar]" + e.Message);
            }
            return result;
        }

    }
}
