using DocumentationCommentsAnalyzer.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DocumentationCommentsAnalyzer.Tests.UtilitiesTests;

public class DocumentationCommentUtilitiesTests {
    [TestCase(TypeArgs = [typeof(ClassDeclarationSyntax)])]
    [TestCase(TypeArgs = [typeof(MethodDeclarationSyntax)])]
    public void GetDocumentationComments_NoDocumentationCommentExists_ReturnsEmpty<TSyntaxNode>() where TSyntaxNode : SyntaxNode {
        const string sourceCode = """
                                  public class TestClass {
                                      public int TestMethod() {
                                          return 1;
                                      }
                                  }
                                  """;
        TSyntaxNode syntaxNode = CreateSyntaxTree<TSyntaxNode>(sourceCode);

        DocumentationCommentTriviaSyntax[] documentationComments = syntaxNode.GetDocumentationComments();

        Assert.That(documentationComments, Is.Empty);
    }

    [TestCase(TypeArgs = [typeof(ClassDeclarationSyntax)])]
    [TestCase(TypeArgs = [typeof(MethodDeclarationSyntax)])]
    public void GetDocumentationComments_SingleDocumentationCommentExists_ReturnsSingleDocumentationComment<TSyntaxNode>() where TSyntaxNode : SyntaxNode {
        const string sourceCode = """
                                  /**
                                   * <TestTagOne>
                                   * TestContentsOne
                                   * </TestTagOne>
                                   */
                                  public class TestClass {
                                      /// <TestTagOne>TestContentsOne</TestTagOne>
                                      /// <TestTagTwo>
                                      /// TestContentsTwo
                                      /// </TestTagTwo>
                                      public int TestMethod() {
                                          return 1;
                                      }
                                  }
                                  """;
        TSyntaxNode syntaxNode = CreateSyntaxTree<TSyntaxNode>(sourceCode);

        DocumentationCommentTriviaSyntax[] documentationComments = syntaxNode.GetDocumentationComments();

        Assert.That(documentationComments, Has.Length.EqualTo(1));
    }

    [TestCase(2, TypeArgs = [typeof(ClassDeclarationSyntax)])]
    [TestCase(3, TypeArgs = [typeof(MethodDeclarationSyntax)])]
    public void GetDocumentationComments_MultipleDocumentationCommentsExists_ReturnsMultipleDocumentationComments<TSyntaxNode>(int expectedNumberOfComments) where TSyntaxNode : SyntaxNode {
        const string sourceCode = """
                                  /// <TestTagOne>
                                  /// TestContentsOne
                                  /// </TestTagOne>
                                  
                                  /// <TestTagTwo>
                                  /// TestContentsTwo
                                  /// </TestTagTwo>
                                  public class TestClass {
                                      /// <TestTagOne>TestContentsOne</TestTagOne>
                                      
                                      /// <TestTagTwo>
                                      /// TestContentsTwo
                                      /// </TestTagTwo>
                                      /** <TestTagThree>TestContentsThree</TestTagThree> */
                                      public int TestMethod() {
                                          return 1;
                                      }
                                  }
                                  """;
        TSyntaxNode syntaxNode = CreateSyntaxTree<TSyntaxNode>(sourceCode);

        DocumentationCommentTriviaSyntax[] documentationComments = syntaxNode.GetDocumentationComments();

        Assert.That(documentationComments, Has.Length.EqualTo(expectedNumberOfComments));
    }

    private static TSyntaxNode CreateSyntaxTree<TSyntaxNode>(string code) {
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
        SyntaxNode rootNode = syntaxTree.GetRoot();
        return rootNode.DescendantNodes().OfType<TSyntaxNode>().First();
    }
}