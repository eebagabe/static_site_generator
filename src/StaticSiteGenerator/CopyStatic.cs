namespace StaticSiteGenerator;

public static class CopyStatic
{
    public static void SourceToPublicDir(string source, string destination)
    {
        foreach (var file in Directory.GetFiles(source))
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));

        foreach (var directory in Directory.GetDirectories(source))
        {
            var destinationPath = Path.Combine(destination, Path.GetFileName(directory));

            Directory.CreateDirectory(destinationPath);
            SourceToPublicDir(directory, destinationPath);
        }
    }

    public static void CopyStaticToPublic(string source, string destination)
    {
        if (Directory.Exists(destination))
            Directory.Delete(destination, true);

        Directory.CreateDirectory(destination);

        SourceToPublicDir(source, destination);
    }
}
