using Dapper;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using SmallCoder.Entities;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace SmallCoder
{
    /// <summary>
    /// oracle sql语结尾不要加`;`封号，否则报错
    /// </summary>
    internal class SqlUtils
    {
        private string _conn;
        private DbType _dbType;
        public SqlUtils(string conn, DbType dbType)
        {
            this._conn = conn;
            this._dbType = dbType;
        }

        private IDbConnection CreateSqlCon()
        {
            switch (_dbType)
            {
                case DbType.SqlServer:
                    return new SqlConnection(_conn);
                case DbType.Oracle:
                    return new OracleConnection(_conn);
                case DbType.MySql:
                default: return new MySqlConnection(_conn);
            }
        }

        public List<string> getAllDatabase()
        {
            using (var sqlcon = CreateSqlCon())
            {
                switch (_dbType)
                {
                    case DbType.SqlServer:
                        return sqlcon.Query<string>("SELECT NAME FROM MASTER.DBO.SYSDATABASES WHERE NAME NOT IN ('master','model','msdb','tempdb');").ToList();
                    case DbType.Oracle:
                        // 获取OWNER 做为库名
                        return sqlcon.Query<string>(@"
                                                    SELECT
	                                                     DISTINCT a.OWNER
                                                    FROM all_tables a
                                                    WHERE a.OWNER NOT IN ('SYS','SYSMAN','OWBSYS','SCOTT'
                                                                            ,'SYSTEM','OUTLN','FLOWS_FILES','MDSYS','ORDSYS','EXFSYS','DBSNMP'
                                                                            ,'WMSYS','APPQOSSYS','APEX_030200','ORDDATA','CTXSYS','ANONYMOUS','XDB'
                                                                            ,'ORDPLUGINS','SI_INFORMTN_SCHEMA','OLAPSYS','ORACLE_OCM','XS$NULL','MDDATA'
                                                                            ,'DIP','APEX_PUBLIC_USER','SPATIAL_CSW_ADMIN_USR','SPATIAL_WFS_ADMIN_USR'
																			,'DBSFWUSER','AUDSYS','GSMADMIN_INTERNAL','OJVMSYS','LBACSYS','DVSYS'
                                                                            )
                                                    ").ToList();
                    case DbType.MySql:
                    default:
                        return sqlcon.Query<string>("SHOW DATABASES WHERE `Database` NOT IN ('mysql', 'performance_schema', 'sys', 'information_schema');").ToList();
                }
            }
        }

        public List<TableInfo> getAllTable(string data_base)
        {
            using (var sqlcon = CreateSqlCon())
            {
                switch (_dbType)
                {
                    case DbType.SqlServer:
                        return sqlcon.Query<TableInfo>($@"USE {data_base};
                                                        SELECT
	                                                        a.name AS table_name,
	                                                        g.[value] AS table_comment
                                                        FROM sys.tables a
                                                        LEFT JOIN sys.extended_properties g ON a.object_id = g.major_id AND g.minor_id = 0;
                                                        ", new { database = data_base }).ToList();
                    case DbType.Oracle:
                        return sqlcon.Query<TableInfo>($@"SELECT
                                                            a.TABLE_NAME AS table_name,
                                                            b.COMMENTS AS table_comment
                                                          FROM all_tables a 
                                                          LEFT JOIN all_tab_comments b ON a.TABLE_NAME = b.TABLE_NAME
                                                          WHERE a.OWNER = UPPER('{data_base}')", new { database = data_base }).ToList();
                    case DbType.MySql:
                    default:
                        return sqlcon.Query<TableInfo>("select table_name,table_comment from information_schema.tables where table_schema = @database", new { database = data_base }).ToList();
                }
            }
        }

        public List<TableColumn> getTableColumns(string data_base, string table_name)
        {
            using (var sqlcon = CreateSqlCon())
            {
                var dataColumns = new List<TableColumn>();
                switch (_dbType)
                {
                    case DbType.SqlServer:
                        {
                            dataColumns = sqlcon.Query<TableColumn>($@"USE {data_base};
                                            SELECT
	                                            c.Column_Name AS name,
	                                            p.[value] AS comment,
	                                            c.DATA_TYPE AS data_type_code,
	                                            CASE 
                                                  WHEN tp.[name] IN ('varchar', 'char', 'varbinary','bit') THEN tp.[name] + '(' + IIF(cc.max_length = -1, 'max', CAST(cc.max_length AS VARCHAR(25))) + ')' 
                                                  WHEN tp.[name] IN ('nvarchar','nchar') THEN tp.[name] + '(' + IIF(cc.max_length = -1, 'max', CAST(cc.max_length / 2 AS VARCHAR(25)))+ ')'      
                                                  WHEN tp.[name] IN ('decimal', 'numeric') THEN tp.[name] + '(' + CAST(cc.[precision] AS VARCHAR(25)) + ', ' + CAST(cc.[scale] AS VARCHAR(25)) + ')'
                                                  WHEN tp.[name] IN ('datetime2') THEN tp.[name] + '(' + CAST(cc.[scale] AS VARCHAR(25)) + ')'
			                                      WHEN tp.[name] IN ('int','bigint') THEN tp.[name] + '(' + CAST(cc.[precision] AS VARCHAR(25)) + ')'
                                                  ELSE tp.[name] END AS column_type,
	                                            c.CHARACTER_MAXIMUM_LENGTH AS [char_length], -- 字符串类型长度
	                                            CASE WHEN k.COLUMN_NAME IS NULL THEN 0 ELSE 1 END AS is_pri,
	                                            c.NUMERIC_PRECISION AS number_precision,   -- 数值类型长度
	                                            c.NUMERIC_SCALE AS number_scale,           -- 数值精确度
	                                            cc.is_identity AS is_auto_increment,
	                                            cc.is_nullable AS is_nullable 			   -- 是否可空
                                            FROM information_schema.columns c
                                            INNER JOIN sys.tables t ON t.name = c.table_name
                                            INNER JOIN sys.columns cc ON cc.object_id = t.object_id AND cc.name = c.Column_Name
                                            INNER JOIN sys.types tp ON cC.user_type_id = tp.user_type_id
                                            LEFT JOIN sys.extended_properties p ON p.major_id = cc.object_id AND p.minor_id = cc.column_id
                                            LEFT JOIN information_schema.key_column_usage k ON k.table_Catalog = c.table_Catalog AND k.table_name = c.table_name AND k.column_name = c.Column_Name
                                            WHERE c.table_Catalog = @database and c.table_name = @table_name;
                                            ", new { database = data_base, table_name = table_name }).ToList();
                        }
                        break;
                    case DbType.Oracle:
                        {
                            dataColumns = sqlcon.Query<TableColumn>($@"SELECT
	                                                                        c.COLUMN_NAME AS name,
	                                                                        tc.COMMENTS AS ""comment"",
	                                                                        c.data_type AS data_type_code,
	                                                                        c.data_type AS column_type,
	                                                                        c.DATA_LENGTH AS char_length, -- 字符串类型长度
	                                                                        CASE WHEN ct.CONSTRAINT_type='P' THEN 1 ELSE 0 END AS is_pri,      -- PRI 主键
	                                                                        c.DATA_PRECISION AS number_precision,   -- 数值类型长度
	                                                                        c.DATA_SCALE AS number_scale,           -- 数值精确度
	                                                                        0 AS is_auto_increment,                 -- 是否自增列
	                                                                        CASE WHEN c.NULLABLE='N' THEN 0 ELSE 1 END AS is_nullable  -- 是否可空
                                                                        FROM all_tab_columns c 
                                                                        LEFT JOIN all_col_comments tc ON tc.OWNER = c.OWNER AND tc.TABLE_NAME = c.TABLE_NAME AND tc.COLUMN_NAME = c.COLUMN_NAME
                                                                        LEFT JOIN all_cons_columns cc ON cc.OWNER = c.OWNER AND cc.TABLE_NAME = c.TABLE_NAME AND cc.COLUMN_NAME = c.COLUMN_NAME AND cc.POSITION IS NOT NULL
                                                                        LEFT JOIN all_constraints ct ON ct.OWNER = c.OWNER AND ct.TABLE_NAME = c.TABLE_NAME AND ct.CONSTRAINT_NAME = cc.CONSTRAINT_NAME
                                                                        WHERE c.Table_Name = '{table_name}' AND c.OWNER = UPPER('{data_base}')", new { database = data_base, table_name = table_name }).ToList();
                        }
                        break;
                    case DbType.MySql:
                    default:
                        {
                            dataColumns = sqlcon.Query<TableColumn>(@"
                                            SELECT
                                                column_name AS name,
                                                column_comment AS comment,
                                                data_type AS data_type_code,
                                                column_type AS column_type,
                                                character_maximum_length AS char_length, -- 字符串类型长度
                                                IF(column_key='PRI',1,0) AS is_pri,      -- PRI 主键
		                                        NUMERIC_PRECISION AS number_precision,   -- 数值类型长度
		                                        NUMERIC_SCALE AS number_scale,           -- 数值精确度
                                                IF(extra LIKE '%auto_increment%',1,0) AS is_auto_increment,  -- 是否自增列
                                                IF(IS_NULLABLE='NO',0,1) AS is_nullable  -- 是否可空
                                            FROM information_schema.columns
                                            WHERE table_schema = @database AND table_name = @table_name
                                            ", new { database = data_base, table_name = table_name }).ToList();
                        }
                        break;
                }
                Utils.ChangeDataType(dataColumns);
                return dataColumns;
            }
        }

        /// <summary>
        /// 测试数据库连通性
        /// </summary>
        /// <returns></returns>
        public bool testConnection()
        {
            using (var sqlCon = CreateSqlCon())
            {
                sqlCon.Open();
                int? rst = null;
                switch (_dbType)
                {
                    case DbType.Oracle:
                        rst = sqlCon.Query<int?>("SELECT 1 FROM dual").FirstOrDefault();
                        break;
                    case DbType.MySql:
                    case DbType.SqlServer:
                    default:
                        rst = sqlCon.Query<int?>("SELECT 1;").FirstOrDefault();
                        break;
                }
                if (rst == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
