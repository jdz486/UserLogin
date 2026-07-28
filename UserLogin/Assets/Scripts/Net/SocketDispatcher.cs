using Google.Protobuf;
using System.Collections.Generic;

public delegate void OnActionHandler(ByteString data);

public class SocketDispatcher : Singleton<SocketDispatcher>
{
    private Dictionary<int, OnActionHandler> _actionDic = new Dictionary<int, OnActionHandler>();

    public void AddEventHandler(int protoCode, OnActionHandler handler)
    {
        if (!_actionDic.ContainsKey(protoCode) && handler != null)
        {
            _actionDic.Add(protoCode, handler);
        }
    }
    public void RemoveEventHandler(int protoCode)
    {
        if (_actionDic.ContainsKey(protoCode))
        {
            _actionDic.Remove(protoCode);
        }
    }
    public void DispatcherEvent(int protoCode, ByteString data)
    {
        if (_actionDic.ContainsKey(protoCode))
        {
            _actionDic[protoCode]?.Invoke(data);
        }
    }
}