using StaticSiteGenerator.Models;

namespace StaticSiteGenerator.Tests;

public class HtmlNodeTests
{
    [Fact]
    public void ToHtml_ThrowsNotImplemented()
    {
        var node = new HtmlNode("p", "testing p");

        Assert.Throws<NotImplementedException>(() => node.ToHtml());
    }

    [Fact]
    public void PropsToHtml_WithoutProps_ReturnsEmpty()
    {
        var node = new HtmlNode("p", "testing p");

        Assert.Equal("", node.PropsToHtml());
    }

    [Fact]
    public void PropsToHtml_WithProps()
    {
        var props = new Dictionary<string, string>
        {
            ["href"] = "https://www.google.com",
            ["target"] = "_blank",
        };

        var node = new HtmlNode("a", "Teste de link", props: props);

        Assert.Equal(" href=\"https://www.google.com\" target=\"_blank\"", node.PropsToHtml());
    }
}

public class LeafNodeTests
{
    [Fact]
    public void ToHtml_Paragraph()
    {
        var node = new LeafNode("p", "Hello, world!");

        Assert.Equal("<p>Hello, world!</p>", node.ToHtml());
    }

    [Fact]
    public void ToHtml_WithoutValue_Throws()
    {
        var node = new LeafNode("p", null);

        Assert.Throws<InvalidOperationException>(() => node.ToHtml());
    }
}

public class ParentNodeTests
{
    [Fact]
    public void ToHtml_WithChildren()
    {
        var child = new LeafNode("span", "child");
        var parent = new ParentNode("div", [child]);

        Assert.Equal("<div><span>child</span></div>", parent.ToHtml());
    }

    [Fact]
    public void ToHtml_WithGrandchildren()
    {
        var grandchild = new LeafNode("b", "grandchild");
        var child = new ParentNode("span", [grandchild]);
        var parent = new ParentNode("div", [child]);

        Assert.Equal("<div><span><b>grandchild</b></span></div>", parent.ToHtml());
    }
}
