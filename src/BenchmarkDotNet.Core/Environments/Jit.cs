﻿namespace BenchmarkDotNet.Environments
{
    public enum Jit
    {
        /// <summary>
        /// LegacyJIT
        /// <remarks>Supported only for Full Framework</remarks>
        /// </summary>
        LegacyJit,

        /// <summary>
        /// RyuJIT
        /// <remarks>Supported only for x64; Full Framework or CoreCLR</remarks>
        /// </summary>
        RyuJit,

        /// <summary>
        /// LLVM
        /// <remarks>Supported only for Mono</remarks>
        /// </summary>
        Llvm
    }
}