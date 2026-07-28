using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LoginServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //登录服务端  去连接中心服务器
            NetClient client = new NetClient(NetDefine.IPHost, NetDefine.CenterServerPort, ClientType.LoginServer);
            client.StartConnect();

            //登录服务器  开启服务端
            NetServer server = new NetServer(client);
            server.StartServer(NetDefine.IPHost, NetDefine.LoginServerPort);

            LoginCtrl loginCtrl = new LoginCtrl();
            //注册指令集
            server.RegistCommand(NetDefine.CMD_RegistCode, loginCtrl);
            server.RegistCommand(NetDefine.CMD_LoginCode, loginCtrl);
            server.RegistCommand(NetDefine.CMD_ChangeMemberCode, loginCtrl);
            server.RegistCommand(NetDefine.CMD_GetServerListCode, loginCtrl);
            server.RegistCommand(NetDefine.CMD_LoginGameServerCode, loginCtrl);
            server.RegistCommand(NetDefine.CMD_CreateRoleCode, loginCtrl);

            client.RegistCommand(NetDefine.CMD_RegistCode, loginCtrl);
            client.RegistCommand(NetDefine.CMD_LoginCode, loginCtrl);
            client.RegistCommand(NetDefine.CMD_ChangeMemberCode, loginCtrl);
            client.RegistCommand(NetDefine.CMD_GetServerListCode, loginCtrl);
            client.RegistCommand(NetDefine.CMD_LoginGameServerCode, loginCtrl);
            client.RegistCommand(NetDefine.CMD_CreateRoleCode, loginCtrl);

            while (true)
            {
                Thread.Sleep(1);
            }
        }
    }
}