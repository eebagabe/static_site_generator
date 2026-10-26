using StaticSiteGenerator.Enums;
using StaticSiteGenerator.Models;

namespace StaticSiteGenerator.Tests;

public class InlineMarkdownTests
{
    [Theory]
    [InlineData(TextType.Text, null)]
    [InlineData(TextType.Bold, "b")]
    [InlineData(TextType.Italic, "i")]
    [InlineData(TextType.Code, "code")]
    public void TextNodeToHtmlNode_SimpleTypes(TextType textType, string? tag)
    {
        var htmlNode = InlineMarkdown.TextNodeToHtmlNode(new TextNode("This is a text", textType));

        Assert.Equal(tag, htmlNode.Tag);
        Assert.Equal("This is a text", htmlNode.Value);
        Assert.Null(htmlNode.Props);
    }

    [Fact]
    public void TextNodeToHtmlNode_Link()
    {
        var htmlNode = InlineMarkdown.TextNodeToHtmlNode(new TextNode("This is a link", TextType.Link, "https://google.com"));

        Assert.Equal("a", htmlNode.Tag);
        Assert.Equal("This is a link", htmlNode.Value);
        Assert.Equal(new Dictionary<string, string> { ["href"] = "https://google.com" }, htmlNode.Props);
    }

    [Fact]
    public void TextNodeToHtmlNode_Image()
    {
        var htmlNode = InlineMarkdown.TextNodeToHtmlNode(new TextNode("Image of Google", TextType.Image, "https://i.imgur.com/zjjcJKZ.png"));

        Assert.Equal("img", htmlNode.Tag);
        Assert.Equal("", htmlNode.Value);
        Assert.Equal(
            new Dictionary<string, string> { ["src"] = "https://i.imgur.com/zjjcJKZ.png", ["alt"] = "Image of Google" },
            htmlNode.Props);
    }

    [Fact]
    public void ExtractMarkdownLinks_IgnoresImages()
    {
        var links = InlineMarkdown.ExtractMarkdownLinks("a ![img](https://a.png) and [link](https://boot.dev)");

        Assert.Equal([("link", "https://boot.dev")], links);
    }

    [Fact]
    public void SplitNodesImage()
    {
        var node = new TextNode(
            "This is text with an ![image](https://i.imgur.com/zjjcJKZ.png) and another ![second image](https://i.imgur.com/3elNhQu.png)",
            TextType.Text);

        Assert.Equal(
            [
                new TextNode("This is text with an ", TextType.Text),
                new TextNode("image", TextType.Image, "https://i.imgur.com/zjjcJKZ.png"),
                new TextNode(" and another ", TextType.Text),
                new TextNode("second image", TextType.Image, "https://i.imgur.com/3elNhQu.png"),
            ],
            InlineMarkdown.SplitNodesImage([node]));
    }

    [Fact]
    public void TextToTextNodes()
    {
        var text = "This is **text** with an _italic_ word and a `code block` and an ![obi wan image](https://i.imgur.com/fJRm4Vk.jpeg) and a [link](https://boot.dev)";

        Assert.Equal(
            [
                new TextNode("This is ", TextType.Text),
                new TextNode("text", TextType.Bold),
                new TextNode(" with an ", TextType.Text),
                new TextNode("italic", TextType.Italic),
                new TextNode(" word and a ", TextType.Text),
                new TextNode("code block", TextType.Code),
                new TextNode(" and an ", TextType.Text),
                new TextNode("obi wan image", TextType.Image, "https://i.imgur.com/fJRm4Vk.jpeg"),
                new TextNode(" and a ", TextType.Text),
                new TextNode("link", TextType.Link, "https://boot.dev"),
            ],
            InlineMarkdown.TextToTextNodes(text));
    }
}
