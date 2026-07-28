﻿﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;

public class LoginModel
{
    private readonly SqlSugarClient _db;

    public LoginModel(SqlSugarClient db)
    {
        _db = db;
    }

    internal LoginRet Login(LoginReq req)
    {
        LoginRet ret = new LoginRet();

        AccountTable accountTable = _db.Queryable<AccountTable>()
            .Where(v => v.UserName == req.UserName || v.PhoneNumber == req.UserName).First();

        if (accountTable == null)
        {
            ret.CmdCode = CmdCode.AcctNotExist;
            return ret;
        }

        if (!string.Equals(accountTable.Password, req.Password, StringComparison.Ordinal))
        {
            ret.CmdCode = CmdCode.PasswordError;
            return ret;
        }

        ret.CmdCode = CmdCode.Succeed;
        ret.AccountId = accountTable.Id;
        ret.UserName = accountTable.UserName;
        ret.Password = accountTable.Password;
        ret.PhoneNumber = accountTable.PhoneNumber;
        ret.Gender = accountTable.Gender;
        ret.IsMember = accountTable.IsMember;
        return ret;
    }

    internal RegistRet Regist(RegistReq req)
    {
        RegistRet ret = new RegistRet();

        bool userExists = _db.Queryable<AccountTable>()
            .Any(v => v.UserName == req.UserName || v.PhoneNumber == req.PhoneNumber);

        if (userExists)
        {
            ret.CmdCode = CmdCode.AcctExist;
            return ret;
        }

        AccountTable accountTable = new AccountTable
        {
            UserName = req.UserName,
            PhoneNumber = req.PhoneNumber,
            Password = req.Password,
            Gender = req.Gender,
            IsMember = req.IsMember,
        };

        int id = _db.Insertable(accountTable).ExecuteCommand();
        if (id <= 0)
        {
            ret.CmdCode = CmdCode.ServerError;
            return ret;
        }
        ret.CmdCode = CmdCode.Succeed;
        return ret;
    }

    internal ChangeMemberRet ChangeMember(ChangeMemberReq req)
    {
        ChangeMemberRet ret = new ChangeMemberRet();

        AccountTable accountTable = _db.Queryable<AccountTable>()
            .Where(v => v.Id == req.AccountId).First();

        if (accountTable == null)
        {
            ret.CmdCode = CmdCode.AcctNotExist;
            return ret;
        }

        accountTable.IsMember = req.IsMember;
        int rows = _db.Updateable(accountTable).ExecuteCommand();

        if (rows <= 0)
        {
            ret.CmdCode = CmdCode.ServerError;
            return ret;
        }

        ret.CmdCode = CmdCode.Succeed;
        return ret;
    }
}