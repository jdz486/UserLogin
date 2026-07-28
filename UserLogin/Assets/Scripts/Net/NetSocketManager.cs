using Google.Protobuf;
using System.Threading;
using UnityEngine;

public class NetSocketManager : MonoBehaviour
{
    public static NetSocketManager Instance { get; private set; }
    private static NetClient _client;
    public static NetClient Client { get => _client; }
    private SynchronizationContext synchronizationContext;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        new LoginController();
    }

    void Start()
    {
        Init();
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void Init()
    {
        synchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
        ConnectServer(NetDefine.IPHost, NetDefine.LoginServerPort);
    }

    public void ConnectServer(string host, int port)
    {
        Disconnect();
        _client = new NetClient(host, port, ClientType.Unity);
        _client.OnReceiveMsg += OnReceiveMsgHandle;
        _client.StartConnect();
    }

    private void OnReceiveMsgHandle(int protoCode, ByteString data)
    {
        synchronizationContext.Post(_ =>
        {
            SocketDispatcher.Instance.DispatcherEvent(protoCode, data);
        }, null);
    }

    public void Disconnect()
    {
        if (_client != null)
        {
            _client._isNeedReconn = false;
            _client.Disconnect();
            _client = null;
        }
    }
}