namespace StaticSiteGenerator;

public static class Program
{
    public static void Main(string[] args)
    {
        CopyStatic.CopyStaticToPublic("static", "public");
        PageGenerator.GeneratePage("content/index.md", "template.html", "public/index.html");
    }
}
