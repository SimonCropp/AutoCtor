﻿using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AutoCtor;

[Generator(LanguageNames.CSharp)]
public class AutoConstructAttributeGenerator : IIncrementalGenerator
{
    private static readonly string Source =
$@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by {ThisAssembly.Project.PackageProjectUrl}
//     Version: {ThisAssembly.Project.Version}
//     SHA: {ThisAssembly.Git.Sha}
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

#if AUTOCTOR_EMBED_ATTRIBUTES
namespace AutoCtor
{{
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute(""{ThisAssembly.Project.AssemblyName}"", ""{ThisAssembly.Project.Version}"")]
    [global::System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    [global::System.Diagnostics.Conditional(""AUTOCTOR_USAGES"")]
    internal sealed class AutoConstructAttribute : System.Attribute
    {{
        [global::System.Runtime.CompilerServices.CompilerGenerated]
        public AutoConstructAttribute()
        {{
        }}
    }}
}}
#endif";

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(static c =>
        {
            c.AddSource("AutoConstructAttribute.g.cs", SourceText.From(Source, Encoding.UTF8));
        });
    }
}
