using Microsoft.CodeAnalysis;

namespace DocumentationCommentsAnalyzer.Utilities;

internal static class SymbolVisibilityUtilities {
    internal static bool IsSymbolAccessibleExternally(this ISymbol symbol) {
        bool isVisibleToExternalAssemblies = symbol.DeclaredAccessibility.IsAccessibleExternally();
        INamedTypeSymbol? containingType = symbol.ContainingType;

        while (isVisibleToExternalAssemblies && containingType is not null) {
            isVisibleToExternalAssemblies = containingType.DeclaredAccessibility.IsAccessibleExternally();
            containingType = containingType.ContainingType;
        }

        return isVisibleToExternalAssemblies;
    }

    private static bool IsAccessibleExternally(this Accessibility accessibility) {
        // Visible to external assemblies when the access modifier is one of 'public', 'protected internal', or 'protected'
        return accessibility is Accessibility.Public or Accessibility.ProtectedOrInternal or Accessibility.Protected;
    }
}