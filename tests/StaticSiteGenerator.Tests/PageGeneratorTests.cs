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

    [Fact]
    public void GeneratePagesRecursive_MirrorsContentTree()
    {
        var root = Directory.CreateTempSubdirectory().FullName;
        var content = Path.Combine(root, "content");
        var template = Path.Combine(root, "template.html");
        var dest = Path.Combine(root, "public");

        Directory.CreateDirectory(Path.Combine(content, "blog", "tom"));
        File.WriteAllText(Path.Combine(content, "index.md"), "# Home");
        File.WriteAllText(Path.Combine(content, "blog", "tom", "index.md"), "# Tom");
        File.WriteAllText(template, "{{ Title }}");

        PageGenerator.GeneratePagesRecursive(content, template, dest);

        Assert.Equal("Home", File.ReadAllText(Path.Combine(dest, "index.html")));
        Assert.Equal("Tom", File.ReadAllText(Path.Combine(dest, "blog", "tom", "index.html")));

        Directory.Delete(root, true);
    }
}
