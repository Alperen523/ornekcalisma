using UzmanCrm.CrmService.DAL.Config.Application.Dapper;

namespace UzmanCrm.CrmService.DAL.Helper
{
    public static class DatabaseHelper
    {

        public static ConnectionStringNames GetTestConnectionStringByConnectionString(ConnectionStringNames connectionString)
        {
            switch (connectionString)
            {
                case ConnectionStringNames.OVM:
                    return ConnectionStringNames.OVM_TEST;
                case ConnectionStringNames.CRM:
                    return ConnectionStringNames.CRM_TEST;
                case ConnectionStringNames.LOVM:
                    return ConnectionStringNames.LOVM_TEST;
                case ConnectionStringNames.LatelierCRM:
                    return ConnectionStringNames.LatelierCRM_TEST;
                case ConnectionStringNames.JOB:
                    return ConnectionStringNames.JOB_TEST;
                case ConnectionStringNames.Integration:
                    return ConnectionStringNames.Integration_TEST;
                default:
                    return connectionString;
            }
        }
    }
}
