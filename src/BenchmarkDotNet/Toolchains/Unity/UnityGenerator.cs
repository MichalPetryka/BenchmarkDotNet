using BenchmarkDotNet.Characteristics;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.Results;

namespace BenchmarkDotNet.Toolchains.Mono
{
    public class UnityGenerator : IGenerator
    {
        public UnityGenerator(string targetFrameworkMoniker, string cliPath, string packagesPath, string runtimeFrameworkVersion) : base(targetFrameworkMoniker, cliPath, packagesPath, runtimeFrameworkVersion, true)
        {
        }

        public GenerateResult GenerateProject(BuildPartition buildPartition, ILogger logger, string rootArtifactsFolderPath)
        { 
            
        }
    }
}
