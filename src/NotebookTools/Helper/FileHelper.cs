using System.Data.OleDb;

namespace NotebookTools.Help;

public class FileHelper
{
    private const string ConnectionString =
        "Provider=Search.CollatorDSO.1;" +
        "Extended Properties='Application=Windows';";

    public static IReadOnlyList<string> FindByFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            throw new ArgumentException("Es wurde kein Dateiname angegeben.");
        }

        List<string> results = [];

        string escapedFileName = fileName.Replace("'", "''");

        string query = $"""
                        SELECT System.ItemPathDisplay
                        FROM SystemIndex
                        WHERE SCOPE = 'file:'
                          AND System.FileName = '{escapedFileName}'
                        """;

        using OleDbConnection connection = new(ConnectionString);
        connection.Open();

        using OleDbCommand command = new(query, connection);
        using OleDbDataReader? reader = command.ExecuteReader();

        if (reader is null)
        {
            return results;
        }

        while (reader.Read())
        {
            if (reader.IsDBNull(0))
            {
                continue;
            }

            string path = reader.GetString(0);

            if (File.Exists(path))
            {
                results.Add(path);
            }
        }

        return results;
    }
}