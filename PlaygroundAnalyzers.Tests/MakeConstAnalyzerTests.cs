using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using NUnit.Framework;

namespace PlaygroundAnalyzers.Tests;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class MakeConstAnalyzerTests : CSharpAnalyzerTest<UseOrganizationAnalyzer, NUnitVerifier>
{
    [Test]
    public async Task WhenCorrectName_Ignores()
    {
        TestCode = @"
            using System;

            class Program
            {
                static void Main()
                {
                    int i = 0;
                    Console.WriteLine(i);
                }
            }
            ";
        ExpectedDiagnostics.Add(
            new DiagnosticResult(UseOrganizationAnalyzer.DiagnosticId, DiagnosticSeverity.Warning)
                .WithMessage("Variable 'i' can be made constant")
                .WithSpan(8, 9, 8, 19));

        await RunAsync();
    }
}