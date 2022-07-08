﻿using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AutoCtor;

[Generator]
public class AutoConstructAttributeGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    public void Execute(GeneratorExecutionContext context)
    {
        var source = @"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by https://github.com/distantcam/AutoCtor.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutoCtor
{
    public sealed class AutoConstructAttribute : Attribute
    {
        public AutoConstructAttribute()
        {
        }
    }
}";

        context.AddSource("AutoConstructAttribute.g.cs", SourceText.From(source, Encoding.UTF8));
    }
}
