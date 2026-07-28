using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CenterServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //开启服务端
            NetServer server = new NetServer(null);
            server.StartServer(NetDefine.IPHost, NetDefine.CenterServerPort);
            //初始化数据库
            SqlSugarClient db = DBMgr.Instance.InitDB();

            Center_LoginCtrl loginCtrl = new Center_LoginCtrl(new LoginModel(db));
            //注册指令集
            server.RegistCommand(NetDefine.CMD_RegistCode, loginCtrl);//注册接口指令集
            server.RegistCommand(NetDefine.CMD_LoginCode, loginCtrl);//登录接口指令集
            server.RegistCommand(NetDefine.CMD_ChangeMemberCode, loginCtrl);//变更会员状态接口指令集
            server.RegistCommand(NetDefine.CMD_GetServerListCode, loginCtrl);//获取服务器列表接口指令集
            server.RegistCommand(NetDefine.CMD_LoginGameServerCode, loginCtrl);//登录游戏服务器接口指令集
            server.RegistCommand(NetDefine.CMD_CreateRoleCode, loginCtrl);//创建角色接口指令集

            while (true)
            {
                Thread.Sleep(1);
            }
        }
    }
}