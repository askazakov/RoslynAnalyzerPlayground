using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Rename;

namespace PlaygroundAnalyzers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(UseOrganizationCodeFixProvider)), Shared]
    public class UseOrganizationCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            ImmutableArray.Create(UseOrganizationAnalyzer.DiagnosticId);

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            var declaration = root
                .FindToken(diagnosticSpan.Start)
                .Parent
                .AncestorsAndSelf()
                .OfType<LocalDeclarationStatementSyntax>()
                .First();

            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken);
            var variableSymbol = semanticModel.GetDeclaredSymbol(declaration.Declaration.Variables.First(), context.CancellationToken);

            if (variableSymbol is null)
                return;

            var solution = context.Document.Project.Solution;

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: "Rename",
                    createChangedSolution: ct => Renamer.RenameSymbolAsync(
                        solution,
                        variableSymbol,
                        "organizationId",
                        solution.Workspace.Options,
                        ct)),
                diagnostic);
        }
    }
}
