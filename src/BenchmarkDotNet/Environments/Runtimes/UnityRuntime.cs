using System;
using BenchmarkDotNet.Jobs;

namespace BenchmarkDotNet.Environments
{
    public class UnityRuntime : Runtime, IEquatable<UnityRuntime>
    {
        public static readonly UnityRuntime Mono = new ("Mono");
        public static readonly UnityRuntime Il2cpp = new ("Mono with .NET 6.0", RuntimeMoniker.Mono60, "net6.0", isDotNetBuiltIn: true);

        public string CustomPath { get; }

        public string AotArgs { get; }

        public override bool IsAOT => !string.IsNullOrEmpty(AotArgs);

        public string MonoBclPath { get; }

        internal bool IsDotNetBuiltIn { get; }

        private UnityRuntime(string name) : base(RuntimeMoniker.Mono, "mono", name) { }

        private UnityRuntime(string name, RuntimeMoniker runtimeMoniker, string msBuildMoniker, bool isDotNetBuiltIn) : base(runtimeMoniker, msBuildMoniker, name)
        {
            IsDotNetBuiltIn = isDotNetBuiltIn;
        }

        public UnityRuntime(string name, string customPath) : this(name) => CustomPath = customPath;

        public UnityRuntime(string name, string customPath, string aotArgs, string monoBclPath) : this(name)
        {
            CustomPath = customPath;
            AotArgs = aotArgs;
            MonoBclPath = monoBclPath;
        }

        public override bool Equals(object obj) => obj is UnityRuntime other && Equals(other);

        public bool Equals(UnityRuntime other)
            => base.Equals(other) && Name == other?.Name && CustomPath == other?.CustomPath && AotArgs == other?.AotArgs && MonoBclPath == other?.MonoBclPath;

        public override int GetHashCode()
            => HashCode.Combine(base.GetHashCode(), Name, CustomPath, AotArgs, MonoBclPath);

        internal static Runtime GetCurrentVersion()
        {
            Version version = Environment.Version;
            return version.Major switch
            {
                6 => Mono60,
                7 => Mono70,
                8 => Mono80,
                _ => new UnityRuntime($"Mono with .NET {version.Major}.{version.Minor}", RuntimeMoniker.NotRecognized, $"net{version.Major}.{version.Minor}", isDotNetBuiltIn: true)
            };
        }
    }
}