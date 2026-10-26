namespace StaticSiteGenerator.Tests;

public class MarkdownToHtmlNodeTests
{
    [Fact]
    public void Headings()
    {
        var md = """
            # This is a heading

            ## This is a heading with **bold** text
            """;

        Assert.Equal(
            "<div><h1>This is a heading</h1><h2>This is a heading with <b>bold</b> text</h2></div>",
            MarkdownToHtmlNode.Convert(md).ToHtml());
    }

    [Fact]
    public void Paragraphs()
    {
        var md = """
            This is **bolded** paragraph
            text in a p
            tag here

            This is another paragraph with _italic_ text and `code` here
            """;

        Assert.Equal(
            "<div><p>This is <b>bolded</b> paragraph text in a p tag here</p><p>This is another paragraph with <i>italic</i> text and <code>code</code> here</p></div>",
            MarkdownToHtmlNode.Convert(md).ToHtml());
    }

    [Fact]
    public void CodeBlock()
    {
        var md = """
            ```
            This is text that _should_ remain
            the **same** even with inline stuff
            ```
            """;

        Assert.Equal(
            "<div><pre><code>This is text that _should_ remain\nthe **same** even with inline stuff</code></pre></div>",
            MarkdownToHtmlNode.Convert(md).ToHtml());
    }

    [Fact]
    public void Quote()
    {
        var md = """
            > This is a quote
            > that spans multiple lines
            > with **bold** text
            """;

        Assert.Equal(
            "<div><blockquote>This is a quote that spans multiple lines with <b>bold</b> text</blockquote></div>",
            MarkdownToHtmlNode.Convert(md).ToHtml());
    }

    [Fact]
    public void UnorderedList()
    {
        var md = """
            - Primeiro item
            - Segundo item com **negrito**
            - Terceiro item com _itálico_
            """;

        Assert.Equal(
            "<div><ul><li>Primeiro item</li><li>Segundo item com <b>negrito</b></li><li>Terceiro item com <i>itálico</i></li></ul></div>",
            MarkdownToHtmlNode.Convert(md).ToHtml());
    }

    [Fact]
    public void OrderedList()
    {
        var md = """
            1. Primeiro item
            2. Segundo item com **negrito**
            3. Terceiro item com _itálico_
            """;

        Assert.Equal(
            "<div><ol><li>Primeiro item</li><li>Segundo item com <b>negrito</b></li><li>Terceiro item com <i>itálico</i></li></ol></div>",
            MarkdownToHtmlNode.Convert(md).ToHtml());
    }
}
