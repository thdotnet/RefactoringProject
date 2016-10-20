using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace RefactoringProject
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ShortNameAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ShortNameAnalyzer";

        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.ShortNameTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.ShortNameMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.ShortNameDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.LocalDeclarationStatement);
        }

        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var localDeclaration = (LocalDeclarationStatementSyntax)context.Node;

            var variableDeclaration = context.Node.ChildNodes().OfType<VariableDeclarationSyntax>().FirstOrDefault();

            foreach (var variable in variableDeclaration.Variables)
            {
                if (variable.Identifier.Text.Length <= 3)
                {
                    var diagnostic = Diagnostic.Create(Rule, variableDeclaration.Type.GetLocation());
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
