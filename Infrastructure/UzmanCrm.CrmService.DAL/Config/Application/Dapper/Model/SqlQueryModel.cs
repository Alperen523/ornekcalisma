using System.Collections.Generic;

namespace UzmanCrm.CrmService.DAL.Config.Application.Dapper.Model
{
    public class SqlQueryModel
    {
        public string Query { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
