using StaticSiteGenerator.Enums;
using StaticSiteGenerator.Models;

namespace StaticSiteGenerator.Tests;

public class TextNodeTests
{
    [Fact]
    public void Equal()
    {
        var node = new TextNode("This is a text node", TextType.Bold);
        var node2 = new TextNode("This is a text node", TextType.Bold);

        Assert.Equal(node, node2);
    }

    [Fact]
    public void NotEqual()
    {
        var node = new TextNode("This is a text node", TextType.Bold);
        var node2 = new TextNode("This is not a text node", TextType.Text);

        Assert.NotEqual(node, node2);
    }

    [Fact]
    public void UrlIsNull()
    {
        var node = new TextNode("This is a text node", TextType.Bold);

        Assert.Null(node.Url);
    }
}
