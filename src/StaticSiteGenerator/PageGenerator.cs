namespace StaticSiteGenerator;

public static class PageGenerator
{
    public static string ExtractTitle(string markdown)
    {
        foreach (var line in markdown.Split("\n"))
        {
            if (line.StartsWith("# "))
                return line["# ".Length..].Trim();
        }

        throw new InvalidOperationException("Don't have a title in the markdown");
    }

    public static void GeneratePage(string fromPath, string templatePath, string destPath, string basePath = "/")
    {
        Console.WriteLine($"Generating page from {fromPath} to {destPath} using {templatePath}");

        var markdown = File.ReadAllText(fromPath);
        var template = File.ReadAllText(templatePath);

        var html = template
            .Replace("{{ Title }}", ExtractTitle(markdown))
            .Replace("{{ Content }}", MarkdownToHtmlNode.Convert(markdown).ToHtml())
            .Replace("href=\"/", $"href=\"{basePath}")
            .Replace("src=\"/", $"src=\"{basePath}");

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(destPath))!);
        File.WriteAllText(destPath, html);
    }

    public static void GeneratePagesRecursive(string dirPathContent, string templatePath, string destDirPath, string basePath = "/")
    {
        foreach (var file in Directory.GetFiles(dirPathContent, "*.md", SearchOption.AllDirectories))
        {
            var relativePath = Path.ChangeExtension(Path.GetRelativePath(dirPathContent, file), ".html");
            GeneratePage(file, templatePath, Path.Combine(destDirPath, relativePath), basePath);
        }
    }
}
