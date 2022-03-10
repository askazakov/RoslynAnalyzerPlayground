using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using NUnit.Framework;

namespace PlaygroundAnalyzers.Tests;

public class UseOrganizationCodeFixFacts
    : CSharpCodeFixTest<UseOrganizationAnalyzer, UseOrganizationCodeFixProvider, NUnitVerifier>
{
    [Test]
    public async Task WhenInconsistentName_AddsExceptionPostfix()
    {
        TestCode = @"
            using System;

            class Program
            {
                static void Main()
                {
                    int orgId = 0;
                }
            }
            ";

        ExpectedDiagnostics.Add(
            new DiagnosticResult(UseOrganizationAnalyzer.DiagnosticId, DiagnosticSeverity.Error)
                .WithMessage("Variable 'orgId' should be renamed")
                .WithSpan(8, 21, 8, 35));

        FixedCode = @"
            using System;

            class Program
            {
                static void Main()
                {
                    int orgId_1 = 0;
                }
            }
            ";

        await RunAsync();
    }
}