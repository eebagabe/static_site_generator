namespace StaticSiteGenerator.Tests;

public class PageGeneratorTests
{
    [Fact]
    public void ExtractTitle_FindsHeader()
    {
        Assert.Equal("Hello", PageGenerator.ExtractTitle("# Hello"));
    }

    [Fact]
    public void ExtractTitle_IgnoresSubheadings()
    {
        Assert.Equal("Title", PageGenerator.ExtractTitle("## Sub\n\n#  Title  "));
    }

    [Fact]
    public void ExtractTitle_WithoutHeader_Throws()
    {
        Assert.Throws<InvalidOperationException>(() => PageGenerator.ExtractTitle("no title"));
    }

    [Fact]
    public void GeneratePage_FillsTemplate()
    {
        var root = Directory.CreateTempSubdirectory().FullName;
        var from = Path.Combine(root, "index.md");
        var template = Path.Combine(root, "template.html");
        var dest = Path.Combine(root, "out", "index.html");

        File.WriteAllText(from, "# Hello\n\nSome **text**");
        File.WriteAllText(template, "<title>{{ Title }}</title><article>{{ Content }}</article>");

        PageGenerator.GeneratePage(from, template, dest);

        Assert.Equal(
            "<title>Hello</title><article><div><h1>Hello</h1><p>Some <b>text</b></p></div></article>",
            File.ReadAllText(dest));

        Directory.Delete(root, true);
    }
}
