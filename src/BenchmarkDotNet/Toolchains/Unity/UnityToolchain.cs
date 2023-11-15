using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BenchmarkDotNet.Characteristics;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.Mono;
using BenchmarkDotNet.Validators;
using JetBrains.Annotations;

namespace BenchmarkDotNet.Toolchains.Unity
{
    [PublicAPI]
    public class UnityToolchain : Toolchain
    {
        [PublicAPI] public static readonly UnityToolchain Mono = From();
        [PublicAPI] public static readonly UnityToolchain Il2Cpp = From();

        [PublicAPI] public Version UnityVersion { get; }

        private static readonly char[] VersionSeparators = {'.', 'f', 'b', 'a'};
        private const string BaseArgs = "-batchmode -nographics -quit -accept-apiupdate -ignorecompilererrors -releaseCodeOptimization -silent-crashes -disableManagedDebugger -silent-crashes -enableIncompatibleAssetDowngrade -force-free -vcsMode \"Visible Meta Files\" -buildTarget \"Standalone\" ";

        [PublicAPI]
        public UnityToolchain(string path, ILogger logger) : base("MonoAot", new UnityGenerator(), new UnityBuilder(), new Executor())
        {
            var (exitCode, output) = ProcessHelper.RunAndReadOutputLineByLine(
                fileName: path,
                arguments: BaseArgs + "-version",
                workingDirectory: Path.GetDirectoryName(path),
                environmentVariables: null,
                includeErrors: true,
                logger: logger);
            var version = output.First(line => line.Count(c => c == '.') == 3).Trim().Split(VersionSeparators, 4);
            UnityVersion = new Version(int.Parse(version[0]), int.Parse(version[1]), int.Parse(version[2]), int.Parse(version[3]));
        }

        public override IEnumerable<ValidationError> Validate(BenchmarkCase benchmarkCase, IResolver resolver)
        {
            foreach (var validationError in base.Validate(benchmarkCase, resolver))
            {
                yield return validationError;
            }

            if (!benchmarkCase.Job.Environment.HasValue(EnvironmentMode.RuntimeCharacteristic) || benchmarkCase.Job.Environment.Runtime is not UnityRuntime)
            {
                yield return new ValidationError(true,
                    "The MonoAOT toolchain requires the Runtime property to be configured explicitly to an instance of MonoRuntime class",
                    benchmarkCase);
            }

            if ((benchmarkCase.Job.Environment.Runtime is UnityRuntime unityRuntime) && !string.IsNullOrEmpty(unityRuntime.MonoBclPath) && !Directory.Exists(unityRuntime.MonoBclPath))
            {
                yield return new ValidationError(true,
                    $"The MonoBclPath provided for MonoAOT toolchain: {unityRuntime.MonoBclPath} does NOT exist.",
                    benchmarkCase);
            }

            if (benchmarkCase.Job.HasValue(InfrastructureMode.BuildConfigurationCharacteristic)
                && benchmarkCase.Job.ResolveValue(InfrastructureMode.BuildConfigurationCharacteristic, resolver) != InfrastructureMode.ReleaseConfigurationName)
            {
                yield return new ValidationError(true,
                    "The Unity toolchain does not allow to rebuild source project, so defining custom build configuration makes no sense",
                    benchmarkCase);
            }

            if (benchmarkCase.Job.HasValue(InfrastructureMode.NuGetReferencesCharacteristic))
            {
                yield return new ValidationError(true,
                    "The Unity toolchain does not allow specifying NuGet package dependencies",
                    benchmarkCase);
            }
        }
    }
}