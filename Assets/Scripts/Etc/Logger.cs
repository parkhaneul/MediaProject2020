using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public sealed class Logger
{
    public const string ENABLE_LOGS = "ENABLE_LOG";

    public static bool isDebugBuild
    {
        get { return Debug.isDebugBuild; }
    }

    [Conditional(ENABLE_LOGS)]
    public static void Log(object message)
    {
        //Debug.Log(message);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void Log(object message, Object context)
    {
        Debug.Log(message,context);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void LogWarning(object message, Object context)
    {
        Debug.LogWarning(message,context);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void LogError(object message, Object context)
    {
        Debug.LogError(message,context);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void LogException(Exception exception)
    {
        Debug.LogException(exception);
    }
    
    [Conditional(ENABLE_LOGS)]
    public static void LogException(Exception exception, Object context)
    {
        Debug.LogException(exception,context);
    }
}
