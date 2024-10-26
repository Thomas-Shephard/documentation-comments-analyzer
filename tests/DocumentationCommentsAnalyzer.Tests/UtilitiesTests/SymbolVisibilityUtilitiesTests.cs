using DocumentationCommentsAnalyzer.Utilities;
using Microsoft.CodeAnalysis;
using Moq;

namespace DocumentationCommentsAnalyzer.Tests.UtilitiesTests;

public class SymbolVisibilityUtilitiesTests {
    [TestCase(Accessibility.Public)]
    [TestCase(Accessibility.Protected)]
    [TestCase(Accessibility.Public, Accessibility.Public)]
    [TestCase(Accessibility.Public, Accessibility.Protected)]
    [TestCase(Accessibility.Public, Accessibility.Protected, Accessibility.ProtectedOrInternal)]
    [TestCase(Accessibility.Protected, Accessibility.Public, Accessibility.Public)]
    public void IsSymbolAccessibleExternally_AccessibleExternally_ReturnsTrue(params Accessibility[] accessibilities) {
        ISymbol publicSymbol = CreateMockSymbol(accessibilities);

        bool isVisibleToExternalAssemblies = publicSymbol.IsSymbolAccessibleExternally();

        Assert.That(isVisibleToExternalAssemblies, Is.EqualTo(true));
    }

    [TestCase(Accessibility.Internal)]
    [TestCase(Accessibility.NotApplicable)]
    [TestCase(Accessibility.Public, Accessibility.Private)]
    [TestCase(Accessibility.Internal, Accessibility.Protected)]
    [TestCase(Accessibility.Internal, Accessibility.Public, Accessibility.Protected)]
    [TestCase(Accessibility.Public, Accessibility.NotApplicable, Accessibility.ProtectedAndInternal)]

    public void IsSymbolAccessibleExternally_NotAccessibleExternally_ReturnsFalse(params Accessibility[] accessibilities) {
        ISymbol privateSymbol = CreateMockSymbol(accessibilities);

        bool isVisibleToExternalAssemblies = privateSymbol.IsSymbolAccessibleExternally();

        Assert.That(isVisibleToExternalAssemblies, Is.EqualTo(false));
    }

    private static ISymbol CreateMockSymbol(params Accessibility[] accessibilities) {
        if (accessibilities.Length == 0) {
            throw new ArgumentException("There must be at least one accessibility", nameof(accessibilities));
        }

        Mock<INamedTypeSymbol>? symbol = null;

        foreach (Accessibility accessibility in accessibilities) {
            Mock<INamedTypeSymbol> newSymbol = new();
            newSymbol.SetupGet(s => s.DeclaredAccessibility).Returns(accessibility);
            newSymbol.SetupGet(s => s.ContainingType).Returns(symbol?.Object!);

            symbol = newSymbol;
        }

        return symbol?.Object!;
    }
}