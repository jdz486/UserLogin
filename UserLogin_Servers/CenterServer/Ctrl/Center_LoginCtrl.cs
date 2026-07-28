using Google.Protobuf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Center_LoginCtrl : IContainer
{
    private LoginModel _loginModel;

    public Center_LoginCtrl(LoginModel loginModel)
    {
        _loginModel = loginModel;
    }

    public void OnClientCommand(ServerBase serverBase, BasePackage basePackage)
    {
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
        LogMsg.Info("OnLoginHandle=>req::" + req.ToString());

        LoginRet ret = _loginModel.Login(req);
        LogMsg.Info("OnLoginHandle=>ret::" + ret.ToString());
        serverBase.SendData(basePackage, basePackage.ProtoCode, ret.ToByteString());
    }

    private void OnRegistHandle(ServerBase serverBase, BasePackage basePackage)
    {
        RegistReq req = RegistReq.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info("OnRegistHandle=>req::" + req.ToString());

        RegistRet ret = _loginModel.Regist(req);
        LogMsg.Info("OnRegistHandle=>ret::" + ret.ToString());

        serverBase.SendData(basePackage, basePackage.ProtoCode, ret.ToByteString());
    }

    private void OnChangeMemberHandle(ServerBase serverBase, BasePackage basePackage)
    {
        ChangeMemberReq req = ChangeMemberReq.Parser.ParseFrom(basePackage.Data);
        LogMsg.Info("OnChangeMemberHandle=>req::" + req.ToString());

        ChangeMemberRet ret = _loginModel.ChangeMember(req);
        LogMsg.Info("OnChangeMemberHandle=>ret::" + ret.ToString());

        serverBase.SendData(basePackage, basePackage.ProtoCode, ret.ToByteString());
    }
}