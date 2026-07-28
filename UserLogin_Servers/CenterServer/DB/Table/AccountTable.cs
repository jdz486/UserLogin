using SqlSugar;
using System;

[SugarTable("users", TableDescription = "用户表")]
internal class AccountTable
{
    //数据库是自增才配自增 IsPrimaryKey:表示是否是主键，IsIdentity:表示是否自增长
    [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
    public int Id { get; set; }

    //用户名
    [SugarColumn(Length = 30)]
    public string UserName { get; set; }

    //手机号
    [SugarColumn(Length = 20)]
    public string PhoneNumber { get; set; }

    //密码
    [SugarColumn(Length = 20)]
    public string Password { get; set; }

    //性别
    [SugarColumn(Length = 10)]
    public string Gender { get; set; }

    //是否会员
    public bool IsMember { get; set; }
}