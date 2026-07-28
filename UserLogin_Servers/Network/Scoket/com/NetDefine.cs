using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class NetDefine
{
    public const string IPHost = "127.0.0.1";//本机IP
    public const int CenterServerPort = 10110;//中心服务器端口号
    public const int LoginServerPort = 10120;//登录服务器的端口号
    public const int GateServerPort = 10120;//网关服务器的端口号

    public const ushort CMD_ErrCode = 10001;//错误码，

    public const ushort CMD_RegistCode = 11010;//注册请求码

    public const ushort CMD_LoginCode = 11020;//登录请求码

    public const ushort CMD_GetServerListCode = 11030;//获取服务器列表请求码

    public const ushort CMD_LoginGameServerCode = 11040;//登录游戏服务器请求码

    public const ushort CMD_CreateRoleCode = 11050;//创建角色请求码
    public const ushort CMD_ChangeMemberCode = 11060;//变更会员状态请求码
}

/// 连接状态
public enum ConnState
{
    Connected,
    Disconnected,
}

/// 客户端类型
public enum ClientType
{
    Unity,
    LoginServer,
}