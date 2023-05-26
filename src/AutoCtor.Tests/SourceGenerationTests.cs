﻿using AutoCtor;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit.Abstractions;

[UsesVerify]
public class SourceGenerationTests
{
    private readonly VerifySettings _settings;

    public SourceGenerationTests()
    {
        _settings = new();
        _settings.ScrubLinesContaining("Version:", "SHA:");
    }

    [Theory]
    [InlineData("[AutoConstruct]")]
    [InlineData("[AutoConstructAttribute]")]
    public Task AttributeTest(string attribute)
    {
        var code = @$"
{attribute}public partial class AttributeTestClass {{}}
";
        var compilation = Compile(code);

        var generator = new AutoConstructSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

        return Verify(driver, _settings)
            .UseParameters(attribute);
    }

    [Theory]
    [InlineData("bool")]
    [InlineData("int")]
    [InlineData("string")]
    [InlineData("object")]
    [InlineData("IEnumerable<string>")]
    [InlineData("List<int>")]
    [InlineData("int[]")]
    [InlineData("int?")]
    [InlineData("Nullable<int>")]
    public Task TypesTest(string type)
    {
        var code = @$"
using System;
using System.Collections.Generic;

[AutoConstruct]public partial class TypesTestClass {{ private readonly {type} _item; }}
";
        var compilation = Compile(code);

        var generator = new AutoConstructSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

        return Verify(driver, _settings)
            .UseParameters(type);
    }

    [Theory]
    [MemberData(nameof(GetExamples))]
    public Task ExamplesTest(CodeFileTheoryData theoryData)
    {
        var baseDir = new DirectoryInfo(Environment.CurrentDirectory)?.Parent?.Parent?.Parent;
        var exampleInterfaces = File.ReadAllText(Path.Combine(baseDir.FullName, "Examples", "IExampleInterfaces.cs"));

        var compilation = Compile(theoryData.Code, exampleInterfaces);

        var generator = new AutoConstructSourceGenerator();
        var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

        return Verify(driver, _settings)
            .UseDirectory("Examples")
            .UseTypeName(theoryData.Name);
    }

    private static CSharpCompilation Compile(params string[] code)
    {
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(assembly => !assembly.IsDynamic)
            .Select(assembly => MetadataReference.CreateFromFile(assembly.Location))
            .Cast<MetadataReference>();

        return CSharpCompilation.Create(
            "AutoCtorTest",
            code.Select(c => CSharpSyntaxTree.ParseText(c)),
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
    }

    public static IEnumerable<object[]> GetExamples()
    {
        var baseDir = new DirectoryInfo(Environment.CurrentDirectory)?.Parent?.Parent?.Parent;

        if (baseDir == null)
        {
            yield break;
        }

        var examples = Directory.GetFiles(Path.Combine(baseDir.FullName, "Examples"), "*.cs");
        foreach (var example in examples)
        {
            if (example.Contains(".g.") || example.Contains("IExampleInterfaces"))
                continue;

            var code = File.ReadAllText(example);
            yield return new object[] {
                new CodeFileTheoryData {
                    Code = code,
                    Name = Path.GetFileNameWithoutExtension(example)
                }
            };
        }
    }

    public class CodeFileTheoryData : IXunitSerializable
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public void Deserialize(IXunitSerializationInfo info)
        {
            Name = info.GetValue<string>("Name");
            Code = info.GetValue<string>("Code");
        }

        public void Serialize(IXunitSerializationInfo info)
        {
            info.AddValue("Name", Name);
            info.AddValue("Code", Code);
        }

        public override string ToString() => Name + ".cs";
    }
}
