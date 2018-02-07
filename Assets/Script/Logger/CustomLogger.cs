using System.Collections;
using System.Collections.Generic;
using System;

public static class CustomLogger{

    public static LoggerType type = LoggerType.ScreenLogger;
    public static string currentLogString = string.Empty;

    public static void Log(string messaggio, params Object [] args)
    {

        switch (type)
        {
            case LoggerType.UnityLogger:
            case LoggerType.ScreenLogger:
                currentLogString = String.Format(messaggio,args) + Environment.NewLine + currentLogString;
                UnityEngine.Debug.LogFormat(messaggio, args);
                break;
            default:
                UnityEngine.Debug.LogFormat(messaggio, args);
                break;
        }
    }

}

public enum LoggerType
{
    UnityLogger,
    ScreenLogger,
}
