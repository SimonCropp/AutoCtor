﻿using AutoSource;

internal static class CodeBuilderExtensions
{
    public static CodeBuilder AppendHeader(this CodeBuilder source)
    {
        source.AppendLine($"//------------------------------------------------------------------------------");
        source.AppendLine($"// <auto-generated>");
        source.AppendLine($"//     This code was generated by {ThisAssembly.Project.PackageProjectUrl}");
        source.AppendLine($"//     Version: {ThisAssembly.Project.Version}");
        source.AppendLine($"//     SHA: {ThisAssembly.Git.Sha}");
        source.AppendLine($"//");
        source.AppendLine($"//     Changes to this file may cause incorrect behavior and will be lost if");
        source.AppendLine($"//     the code is regenerated.");
        source.AppendLine($"// </auto-generated>");
        source.AppendLine($"//------------------------------------------------------------------------------");
        return source;
    }

    public static CodeBuilder AddCompilerGeneratedAttribute(this CodeBuilder source) => source.AppendLine("[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]");
    public static CodeBuilder AddGeneratedCodeAttribute(this CodeBuilder source) => source.AppendLine($"[global::System.CodeDom.Compiler.GeneratedCodeAttribute(\"{ThisAssembly.Project.AssemblyName}\", \"{ThisAssembly.Project.Version}\")]");
}