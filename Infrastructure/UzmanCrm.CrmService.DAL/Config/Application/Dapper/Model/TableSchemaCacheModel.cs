using System;
using System.Collections.Generic;

namespace UzmanCrm.CrmService.DAL.Config.Application.Dapper.Model
{
    public class TableSchemaCacheModel
    {
        public string EntityName { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public List<TableSchemaModel> TableSchema { get; set; }
    }

    public class TableSchemaModel
    {
        public int? character_maximum_length { get; set; }
        public int? character_octet_length { get; set; }
        public string character_set_catalog { get; set; }
        public string character_set_name { get; set; }
        public string character_set_schema { get; set; }
        public string collation_catalog { get; set; }
        public string collation_name { get; set; }
        public string collation_schema { get; set; }
        public string column_default { get; set; }
        public string column_name { get; set; }
        public string data_type { get; set; }
        public short? datetime_precision { get; set; }
        public string domain_catalog { get; set; }
        public string domain_name { get; set; }
        public string domain_schema { get; set; }
        public string is_nullable { get; set; }
        public byte? numeric_precision { get; set; }
        public short? numeric_precision_radix { get; set; }
        public int? numeric_scale { get; set; }
        public int? ordinal_position { get; set; }
        public string table_catalog { get; set; }
        public string table_name { get; set; }
        public string table_schema { get; set; }
    }
}
