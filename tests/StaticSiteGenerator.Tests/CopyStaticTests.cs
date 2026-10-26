namespace StaticSiteGenerator.Tests;

public class CopyStaticTests
{
    [Fact]
    public void CopyStaticToPublic_CopiesNestedFilesAndClearsDestination()
    {
        var root = Directory.CreateTempSubdirectory().FullName;
        var source = Path.Combine(root, "static");
        var destination = Path.Combine(root, "public");

        Directory.CreateDirectory(Path.Combine(source, "images"));
        File.WriteAllText(Path.Combine(source, "index.css"), "body {}");
        File.WriteAllText(Path.Combine(source, "images", "a.png"), "png");
        Directory.CreateDirectory(destination);
        File.WriteAllText(Path.Combine(destination, "old.html"), "old");

        CopyStatic.CopyStaticToPublic(source, destination);

        Assert.Equal("body {}", File.ReadAllText(Path.Combine(destination, "index.css")));
        Assert.Equal("png", File.ReadAllText(Path.Combine(destination, "images", "a.png")));
        Assert.False(File.Exists(Path.Combine(destination, "old.html")));

        Directory.Delete(root, true);
    }
}
