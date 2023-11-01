namespace BenchmarkDotNet.Toolchains.Unity.Sources;

internal static class UnityProjectSetupSource
{
    public static readonly string Source = """
using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

public static class UnityProjectSetup
{
    public static void Setup()
    {
        try
        {
            EditorApplication.Exit(Run());
        }
        catch (Exception exception)
        {
            int code = exception.HResult;
            if (code == 0)
                code = -1;
            EditorApplication.Exit(code);
        }
    }

    private static int Run()
    {
        AssetDatabase.ActiveRefreshImportMode = RefreshImportMode.OutOfProcessPerQueue;
        AssetDatabase.DesiredWorkerCount = Environment.ProcessorCount;

        EditorSettings.asyncShaderCompilation = true;
        EditorSettings.assetPipelineMode = AssetPipelineMode.Version2;
        EditorSettings.refreshImportMode = RefreshImportMode.OutOfProcessPerQueue;
        EditorSettings.serializeInlineMappingsOnOneLine = true;

        PlayerSettings.allowUnsafeCode = true;
        PlayerSettings.runInBackground = true;
        PlayerSettings.usePlayerLog = false;
        PlayerSettings.forceSingleInstance = false;
        PlayerSettings.bakeCollisionMeshes = true;
        PlayerSettings.stripEngineCode = true;
        PlayerSettings.companyName = "{companyName}";
        PlayerSettings.productName = "{productName}";

        PlayerSettings.gcIncremental = {incrementalGC};
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.{scriptingRuntime});
        PlayerSettings.SetIl2CppCodeGeneration(NamedBuildTarget.Standalone, Il2CppCodeGeneration.{il2cppCodeGeneration});
        PlayerSettings.SetIl2CppCompilerConfiguration(NamedBuildTarget.Standalone, Il2CppCompilerConfiguration.{il2cppConfiguration});

        PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.{logStacktraces});
        PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.{warningStacktraces});
        PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.{errorStacktraces});
        PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.{exceptionStacktraces});
        PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.{assertStacktraces});

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        return 0;
    }
}
""";
}