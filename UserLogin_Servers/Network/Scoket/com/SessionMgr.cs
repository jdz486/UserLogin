using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class SessionMgr : Singleton<SessionMgr>
{
    private int _instanceInter;
    private Dictionary<int, Session> _sessionDic = new Dictionary<int, Session>();

    public void AddSession(Session session, int sessionId = -1)
    {
        if (sessionId <= 0)
        {
            sessionId = GteInstanceInter();
        }

        if (!_sessionDic.ContainsKey(sessionId))
        {
            session.SessionId = sessionId;
            _sessionDic.Add(sessionId, session);
        }
    }

    public void RemoveSession(int sessionId)
    {
        if (_sessionDic.ContainsKey(sessionId))
        {
            _sessionDic.Remove(sessionId);
        }
    }

    public Session GetSession(int sessionId)
    {
        if (_sessionDic.ContainsKey(sessionId))
        {
            return _sessionDic[sessionId];
        }
        return null;
    }

    public int GetSessionCount()
    {
        return _sessionDic.Count;
    }

    public int GteInstanceInter()
    {
        return Interlocked.Increment(ref _instanceInter);
    }
}