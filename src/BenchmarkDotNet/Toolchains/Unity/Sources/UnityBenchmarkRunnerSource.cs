namespace BenchmarkDotNet.Toolchains.Unity.Sources;

internal static class UnityBenchmarkRunnerSource
{
    public static readonly string Source = """
using System;
using UnityEngine;

public class UnityBenchmarkRunner : MonoBehaviour
{
    public void Start()
    {
        try
        {
            Application.Quit(Run());
        }
        catch (Exception exception)
        {
            int code = exception.HResult;
            if (code == 0)
                code = -1;
            Application.Quit(code);
        }
    }

    private static int Run()
    {
        Application.logMessageReceivedThreaded += HandleLog;
        return 0;
    }

    private static void HandleLog(string logString, string stackTrace, LogType type)
    {
        
    }
}
""";
}