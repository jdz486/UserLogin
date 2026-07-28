using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Session : ServerBase
{
    public int SessionId { get; set; }

    public Session(Dictionary<int, IContainer> cmdDic, NetClient client)
    {
        _cmdDic = cmdDic;
        _client = client;
        SessionMgr.Instance.AddSession(this);
    }

    /// 开始接收客户端发来的数据
    public void ReceiveData(Socket socket)
    {
        _socket = socket;
        BeginReceive();
    }

    protected override void HandleCommand(BasePackage basePackage)
    {
        IContainer container = _cmdDic[basePackage.ProtoCode];
        if (container == null)
        {
            LogMsg.Info("command not regist..");
            return;
        }

        if (_client != null)
        {
            if (_client._clientType == ClientType.LoginServer)
            {
                basePackage.UnitySessionId = SessionId;
            }
        }

        container.OnServerCommand(this, basePackage);
    }

    public override void Disconnect()
    {
        LogMsg.Info("Disconnect::" + _socket.RemoteEndPoint + "断开了连接...");
        base.Disconnect();
    }
}