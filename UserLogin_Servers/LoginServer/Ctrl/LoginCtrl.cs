using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class LoginCtrl : IContainer
{
    public void OnClientCommand(ServerBase serverBase, BasePackage basePackage)
    {
        Session session = SessionMgr.Instance.GetSession(basePackage.UnitySessionId);

        switch (basePackage.ProtoCode)
        {
            case NetDefine.CMD_RegistCode:
                OnRegistResultHandle(session, basePackage);
                break;
            case NetDefine.CMD_LoginCode:
                OnLoginResultHandle(session, basePackage);
                break;
            case NetDefine.CMD_ChangeMemberCode:
                OnChangeMemberResultHandle(session, basePackage);
                break;
        }
    }

    private void OnLoginResultHandle(Session session, BasePackage basePackage)
    {
        LoginRet ret = LoginRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info("OnLoginResultHandle::" + ret.ToString());
        if (ret.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, ret.CmdCode);
            return;
        }
        session.SendData(basePackage);
    }

    private void OnRegistResultHandle(Session session, BasePackage basePackage)
    {
        RegistRet ret = RegistRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info("OnRegistResultHandle::" + ret.ToString());
        if (ret.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, ret.CmdCode);
            return;
        }
        session.SendData(basePackage);
    }

    private void OnChangeMemberResultHandle(Session session, BasePackage basePackage)
    {
        ChangeMemberRet ret = ChangeMemberRet.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info("OnChangeMemberResultHandle::" + ret.ToString());
        if (ret.CmdCode != CmdCode.Succeed)
        {
            session.SendError(basePackage, ret.CmdCode);
            return;
        }
        session.SendData(basePackage);
    }

    public void OnInit()
    {
    }

    public void OnServerCommand(ServerBase serverBase, BasePackage basePackage)
    {
        switch (basePackage.ProtoCode)
        {
            case NetDefine.CMD_RegistCode:
                OnRegistHandle(serverBase, basePackage);
                break;
            case NetDefine.CMD_LoginCode:
                OnLoginHandle(serverBase, basePackage);
                break;
            case NetDefine.CMD_ChangeMemberCode:
                OnChangeMemberHandle(serverBase, basePackage);
                break;
        }
    }

    private void OnLoginHandle(ServerBase serverBase, BasePackage basePackage)
    {
        LoginReq req = LoginReq.Parser.ParseFrom(basePackage.Data);
        long timer = DataUtils.Instance.GetLoginMilliseconds(req.UserName);
        if (timer > 0 && DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - timer < 300)
        {
            serverBase.SendError(basePackage, CmdCode.UserOftenLogin);
            return;
        }

        DataUtils.Instance.AddLoginMilliseconds(req.UserName, DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        serverBase._client.SendData(basePackage);

        LogMsg.Info("OnLoginHandle::" + req.ToString());
    }

    private void OnRegistHandle(ServerBase serverBase, BasePackage basePackage)
    {

        RegistReq req = RegistReq.Parser.ParseFrom(basePackage.Data);
        if (!DataUtils.IsValidUserName(req.UserName))
        {
            serverBase.SendError(basePackage, CmdCode.UserNameIllegal);
            return;
        }
        if (!DataUtils.IsValidMobile(req.PhoneNumber))
        {
            serverBase.SendError(basePackage, CmdCode.PhoneNumIllegal);
            return;
        }
        if (!DataUtils.IsValidPassword(req.Password))
        {
            serverBase.SendError(basePackage, CmdCode.PasswordIllegal);
            return;
        }

        serverBase._client.SendData(basePackage);

        LogMsg.Info("OnRegistHandle::" + req.ToString());
    }

    private void OnChangeMemberHandle(ServerBase serverBase, BasePackage basePackage)
    {
        ChangeMemberReq req = ChangeMemberReq.Parser.ParseFrom(basePackage.Data);
        serverBase._client.SendData(basePackage);
        LogMsg.Info("OnChangeMemberHandle::" + req.ToString());
    }
}