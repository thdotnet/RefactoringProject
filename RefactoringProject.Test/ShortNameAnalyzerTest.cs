using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using RefactoringProject;

namespace RefactoringProject.Test
{
    [TestClass]
    public class ShortNameAnalyzerTest : CodeFixVerifier
    {
        [TestMethod]
        public void ShortNameDeclaration()
        {
            const string test = @"
                    using System;
                    namespace ConsoleApplication1
                    {
                        class Foo
                        {
                            void Bar()
                            {
                                string x = ""Me"";
                            }
                        }
                    }";

            var expected = new DiagnosticResult
            {
                Id = "ShortNameAnalyzer",
                Message = "Try to use a better name",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 9, 33)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ShortNameAnalyzer();
        }
    }
}