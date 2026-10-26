using Microsoft.Extensions.FileProviders;

namespace StaticSiteGenerator;

public static class Program
{
    public static void Main(string[] args)
    {
        var serve = args.Contains("--serve");
        var basePath = args.FirstOrDefault(arg => arg != "--serve") ?? "/";

        CopyStatic.CopyStaticToPublic("static", "docs");
        PageGenerator.GeneratePagesRecursive("content", "template.html", "docs", basePath);

        if (serve)
            Serve("docs", "http://localhost:8888");
    }

    private static void Serve(string directory, string url)
    {
        var app = WebApplication.Create();
        var fileProvider = new PhysicalFileProvider(Path.GetFullPath(directory));

        app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = fileProvider });
        app.UseStaticFiles(new StaticFileOptions { FileProvider = fileProvider });

        app.Run(url);
    }
}
