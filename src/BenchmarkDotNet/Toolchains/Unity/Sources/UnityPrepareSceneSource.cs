namespace BenchmarkDotNet.Toolchains.Unity.Sources;

internal static class UnityPrepareSceneSource
{
    public static readonly string Source = """
using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UnityPrepareScene
{
    public static void Prepare()
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
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        GameObject go = new()
        {
            name = "UnityBenchmarkRunner",
            isStatic = true
        };
        go.AddComponent<UnityBenchmarkRunner>();

        const string path = "Assets/BenchmarkScene.unity";
        EditorSceneManager.SaveScene(scene, path, false);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(path, true) };

        EditorSceneManager.MarkAllScenesDirty();
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        return 0;
    }
}
""";
}