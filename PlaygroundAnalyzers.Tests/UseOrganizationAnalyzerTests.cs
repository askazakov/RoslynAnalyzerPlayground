using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using NUnit.Framework;

namespace PlaygroundAnalyzers.Tests;

[FixtureLifeCycle(LifeCycle.InstancePerTestCase)] // base class shares TestState between cases
public class UseOrganizationAnalyzerTests : CSharpAnalyzerTest<UseOrganizationAnalyzer, NUnitVerifier>
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
                    int organizationId = 0;
                }
            }
            ";
        ExpectedDiagnostics.Clear();

        await RunAsync();
    }

    [Test]
    public async Task WhenInconsistentName_ShowsWarning()
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

        await RunAsync();
    }
}