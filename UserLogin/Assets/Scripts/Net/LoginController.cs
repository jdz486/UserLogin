using Google.Protobuf;
using UnityEngine;

public class LoginController
{
    public static LoginController Instance { get; private set; }

    public LoginController()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;
        RegistCommand();
    }

    public void SendLoginRequest(string userName, string password)
    {
        LoginReq req = new LoginReq
        {
            UserName = userName,
            Password = password,
        };

        BasePackage package = new BasePackage
        {
            ProtoCode = NetDefine.CMD_LoginCode,
            Data = req.ToByteString(),
        };

        NetSocketManager.Client?.SendData(package);
    }

    public void SendRegisterRequest(string userName, string password, string phoneNumber, string gender)
    {
        RegistReq req = new RegistReq
        {
            UserName = userName,
            Password = password,
            PhoneNumber = phoneNumber,
            Gender = gender,
            IsMember = false,
        };

        BasePackage package = new BasePackage
        {
            ProtoCode = NetDefine.CMD_RegistCode,
            Data = req.ToByteString(),
        };

        NetSocketManager.Client?.SendData(package);
    }

    public void SendChangeMemberRequest(int accountId, bool isMember)
    {
        ChangeMemberReq req = new ChangeMemberReq
        {
            AccountId = accountId,
            IsMember = isMember,
        };

        BasePackage package = new BasePackage
        {
            ProtoCode = NetDefine.CMD_ChangeMemberCode,
            Data = req.ToByteString(),
        };

        NetSocketManager.Client?.SendData(package);
    }

    private void RegistCommand()
    {
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_RegistCode, OnRegistHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_LoginCode, OnLoginHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_ChangeMemberCode, OnChangeMemberHandle);
        SocketDispatcher.Instance.AddEventHandler(NetDefine.CMD_ErrCode, OnErrorHandle);
    }

    private void OnRegistHandle(ByteString data)
    {
        RegistRet ret = RegistRet.Parser.ParseFrom(data);
        if (ret.CmdCode == CmdCode.Succeed)
        {
            RegisterManager.Instance?.OnRegisterSuccess();
        }
        else
        {
            RegisterManager.Instance?.OnRegisterFailed(GetErrorMessage(ret.CmdCode));
        }
    }

    private void OnLoginHandle(ByteString data)
    {
        LoginRet ret = LoginRet.Parser.ParseFrom(data);
        if (ret.CmdCode == CmdCode.Succeed)
        {
            PlayerPrefs.SetString("CurrentUserId", ret.AccountId.ToString());
            PlayerPrefs.SetString("CurrentUsername", ret.UserName);
            PlayerPrefs.SetString("CurrentPhoneNumber", ret.PhoneNumber);
            PlayerPrefs.SetString("CurrentGender", ret.Gender);
            PlayerPrefs.SetInt("CurrentIsMember", ret.IsMember ? 1 : 0);
            PlayerPrefs.Save();
            
            LoginManager.Instance?.OnLoginSuccess(ret.UserName, ret.Password);
        }
        else
        {
            LoginManager.Instance?.OnLoginFailed(GetErrorMessage(ret.CmdCode));
        }
    }

    private void OnChangeMemberHandle(ByteString data)
    {
        ChangeMemberRet ret = ChangeMemberRet.Parser.ParseFrom(data);
        if (ret.CmdCode == CmdCode.Succeed)
        {
            UserManager.Instance?.OnChangeMemberSuccess();
        }
        else
        {
            UserManager.Instance?.OnChangeMemberFailed(GetErrorMessage(ret.CmdCode));
        }
    }

    private void OnErrorHandle(ByteString data)
    {
        ErrMsg err = ErrMsg.Parser.ParseFrom(data);
        if (err.CmdCode == CmdCode.Succeed)
        {
            return;
        }

        LoginManager.Instance?.OnLoginFailed(GetErrorMessage(err.CmdCode));
        RegisterManager.Instance?.OnRegisterFailed(GetErrorMessage(err.CmdCode));
    }

    private string GetErrorMessage(CmdCode cmdCode)
    {
        switch (cmdCode)
        {
            case CmdCode.AcctExist:
                return "账号已存在";
            case CmdCode.AcctNotExist:
                return "账号不存在";
            case CmdCode.PasswordError:
                return "密码错误";
            case CmdCode.PasswordIllegal:
                return "密码格式不合法";
            case CmdCode.UserNameIllegal:
                return "用户名格式不合法";
            case CmdCode.PhoneNumIllegal:
                return "手机号格式不合法";
            case CmdCode.UserOftenLogin:
                return "操作过于频繁，请稍后再试";
            default:
                return "请求失败";
        }
    }
}