using SqlSugar;

internal class DBMgr : Singleton<DBMgr>
{
    public SqlSugarClient InitDB()
    {
        ConnectionConfig connectionConfig = new ConnectionConfig()
        {
            ConnectionString = "Server=127.0.0.1;Port=3306;Database=userlogin;Uid=root;Pwd=123456;Charset=utf8mb4;",
            DbType = DbType.MySql,
            IsAutoCloseConnection = true,
        };

        SqlSugarClient db = new SqlSugarClient(connectionConfig);
        db.DbMaintenance.CreateDatabase();
        db.CodeFirst.InitTables(typeof(AccountTable));
        return db;
    }
}