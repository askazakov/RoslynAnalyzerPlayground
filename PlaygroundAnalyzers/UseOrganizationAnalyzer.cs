using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace PlaygroundAnalyzers
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseOrganizationAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "EV0001";

        private const string Title = "Short form `org` is prohibited ";
        private const string MessageFormat = "Variable '{0}' should be renamed";
        private const string Description = "";
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat,
            Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.LocalDeclarationStatement);
        }

        private void AnalyzeNode(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;
            // Retrieve the local symbol for each variable in the local declaration
            var variable = localDeclaration.Declaration.Variables.Single();

            if (variable.Identifier.ValueText == "orgId")
            {
                context.ReportDiagnostic(
                    Diagnostic.Create(Rule, context.Node.GetLocation(), variable.Identifier.ValueText));
            }
        }
    }
}