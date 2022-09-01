using Dapper;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;

namespace SmallCoder
{
    internal class SqlUtils
    {
        private string _conn;
        public SqlUtils(string conn)
        {
            this._conn = conn;
        }


        private MySqlConnection CreateSqlCon()
        {
            return new MySqlConnection(_conn);
        }

        public List<string> getAllDatabase()
        {
            using (var sqlcon = CreateSqlCon())
            {
                return sqlcon.Query<string>("SHOW DATABASES WHERE `Database` NOT IN ('mysql', 'performance_schema', 'sys', 'information_schema');").ToList();
            }
        }

        public List<TableColumn> getTableColumns(string data_base, string table_name)
        {
            using (var sqlcon = CreateSqlCon())
            {
                var dataColumns = sqlcon.Query<TableColumn>(@"
                    SELECT
                        column_name AS name,
                        column_comment AS comment,
                        data_type AS data_type_code,
                        column_type AS column_type,
                        character_maximum_length AS char_length, -- 字符串类型长度
                        IF(column_key='PRI',1,0) AS is_pri,      -- PRI 主键
		                NUMERIC_PRECISION AS number_precision,  -- 数值类型长度
		                NUMERIC_SCALE AS number_scale           -- 数值精确度
                    FROM information_schema.columns
                    WHERE table_schema = @database AND table_name = @table_name
                    ", new { database = data_base, table_name = table_name }).ToList();
                Utils.ChangeDataType(dataColumns);
                return dataColumns;
            }
        }

        public List<string> getAllTable(string data_base)
        {
            using (var sqlcon = CreateSqlCon())
            {
                return sqlcon.Query<string>("select table_name from information_schema.tables where table_schema = @database", new { database = data_base }).ToList();
            }
        }
    }
}
