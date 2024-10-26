using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DocumentationCommentsAnalyzer.Utilities;

internal static class DocumentationCommentUtilities {
    internal static DocumentationCommentTriviaSyntax[] GetDocumentationComments(this SyntaxNode syntaxNode) {
        return syntaxNode.GetLeadingTrivia()
                         .Select(syntaxTrivia => syntaxTrivia.GetStructure())
                         .OfType<DocumentationCommentTriviaSyntax>()
                         .ToArray();
    }
}