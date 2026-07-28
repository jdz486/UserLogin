using System;
using System.Runtime.InteropServices;

public class LogMsg
{
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll", EntryPoint = "SetWindowText")]
    public static extern int SetWindowText(IntPtr hwnd, string lpString);

    public static Action<string> logCB;

    public static void SetWindowInfo(string text)
    {
        SetWindowText(GetConsoleWindow(), text);
    }

    public static void Info(string msg, LogMsgType lv = LogMsgType.None)
    {
        logCB?.Invoke(msg);
        msg = DateTime.Now.ToLongTimeString() + " >> " + msg;

        if (lv == LogMsgType.None)
        {
            Console.WriteLine(msg);
        }
        else if (lv == LogMsgType.Warn)
        {
            Console.WriteLine("//--------------------Warn--------------------//");
            Console.WriteLine(msg);
        }
        else if (lv == LogMsgType.Error)
        {
            Console.WriteLine("//--------------------ErrorCode--------------------//");
            Console.WriteLine(msg);
        }
        else if (lv == LogMsgType.Info)
        {
            Console.WriteLine("//--------------------Info--------------------//");
            Console.WriteLine(msg);
        }
        else
        {
            Console.WriteLine("//--------------------ErrorCode--------------------//");
            Console.WriteLine(msg + " >> Unknow LogMsg Type\n");
        }
    }
}

public enum LogMsgType
{
    None = 0,// None
    Warn = 1,//Yellow
    Error = 2,//Red
    Info = 3//Green
}