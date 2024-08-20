namespace UzmanCrm.CrmService.DAL.Config.Application.Dapper
{
    public enum ConnectionStringNames
    {
        // If have a write webconfig  connection strings name and dont send all function. 0 is default
        OVM = 0,
        CRM = 1,
        LOVM = 2,
        LatelierCRM = 3,
        JOB = 4,
        Integration = 5,
        //TEST DB
        OVM_TEST = 15,
        CRM_TEST = 16,
        LOVM_TEST = 17,
        LatelierCRM_TEST = 18,
        JOB_TEST = 19,
        Integration_TEST = 20
    }
}