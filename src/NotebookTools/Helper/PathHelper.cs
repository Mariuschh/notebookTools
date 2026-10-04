namespace NotebookTools.Help;

public static class PathHelper
{
    public static string NormalizeInput(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("Es wurde kein Dateiname angegeben.");
        }
        if (filePath.EndsWith(".ipynb", StringComparison.OrdinalIgnoreCase))
        {
            return filePath;
        }
        else
        {
            filePath += ".ipynb";
            return filePath;
        }
    }
    public static string? GetLocalPath(string filePath)
    {
        string normalizedPath = NormalizeInput(filePath);
        string absolutePath = Path.GetFullPath(normalizedPath);

        if (File.Exists(absolutePath))
        {
            return absolutePath;
        }

        return null;
    }
}