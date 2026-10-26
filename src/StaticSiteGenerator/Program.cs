namespace StaticSiteGenerator;

public static class Program
{
    public static void Main(string[] args)
    {
        var basePath = args.Length > 0 ? args[0] : "/";

        CopyStatic.CopyStaticToPublic("static", "docs");
        PageGenerator.GeneratePagesRecursive("content", "template.html", "docs", basePath);
    }
}
