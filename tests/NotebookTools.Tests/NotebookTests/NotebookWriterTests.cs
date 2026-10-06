using System.Text.Json;
using System.Text.Json.Nodes;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookWriterTests
{
    private string dateiPfad = "";

    [TestInitialize]
    public void Vorbereiten()
    {
        dateiPfad = Path.GetTempFileName();
        // Der Writer darf nur neue Dateien erstellen.
        File.Delete(dateiPfad);
    }

    [TestCleanup]
    public void Aufraeumen()
    {
        File.Delete(dateiPfad);
    }

    [TestMethod]
    public void SaveNotebook_ErstelltDatei()
    {
        Notebooks notebook = new Notebooks();
        notebook.Format = 4;
        notebook.MinorFormat = 5;

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        Assert.IsTrue(File.Exists(dateiPfad));
        string json = File.ReadAllText(dateiPfad);
        using JsonDocument dokument = JsonDocument.Parse(json);
        Assert.AreEqual(4, dokument.RootElement.GetProperty("nbformat").GetInt32());
        Assert.AreEqual(5, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
    }

    [TestMethod]
    public void SaveNotebook_CodeZelle_BehaeltAusgabe()
    {
        Notebooks notebook = new Notebooks();
        NotebookCell zelle = new NotebookCell();
        zelle.Type = "code";
        zelle.Source.Add("print(1)");
        string ausgabe = """{ "output_type": "stream", "text": ["1"] }""";
        zelle.Outputs.Add(JsonSerializer.Deserialize<JsonElement>(ausgabe));
        zelle.ExecutionCount = 2;
        notebook.Cells.Add(zelle);

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        using JsonDocument dokument = JsonDocument.Parse(File.ReadAllText(dateiPfad));
        JsonElement gespeicherteZelle = dokument.RootElement.GetProperty("cells")[0];
        Assert.AreEqual("print(1)", gespeicherteZelle.GetProperty("source")[0].GetString());
        Assert.AreEqual(1, gespeicherteZelle.GetProperty("outputs").GetArrayLength());
        Assert.AreEqual(2, gespeicherteZelle.GetProperty("execution_count").GetInt32());
    }

    [TestMethod]
    public void SaveNotebook_MarkdownZelle_HatKeineAusgabe()
    {
        Notebooks notebook = new Notebooks();
        NotebookCell zelle = new NotebookCell();
        zelle.Type = "markdown";
        zelle.ExecutionCount = 2;
        notebook.Cells.Add(zelle);

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        JsonObject? gespeichert = JsonNode.Parse(File.ReadAllText(dateiPfad)) as JsonObject;
        Assert.IsNotNull(gespeichert);
        JsonObject? gespeicherteZelle = gespeichert["cells"]?[0] as JsonObject;
        Assert.IsNotNull(gespeicherteZelle);
        Assert.IsFalse(gespeicherteZelle.ContainsKey("outputs"));
        Assert.IsFalse(gespeicherteZelle.ContainsKey("execution_count"));
    }

    [TestMethod]
    public void SaveNotebook_ErstelltZellenId()
    {
        Notebooks notebook = new Notebooks();
        notebook.Cells.Add(new NotebookCell());

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        using JsonDocument dokument = JsonDocument.Parse(File.ReadAllText(dateiPfad));
        string? id = dokument.RootElement.GetProperty("cells")[0].GetProperty("id").GetString();
        Assert.IsFalse(string.IsNullOrEmpty(id));
    }

    [TestMethod]
    public void SaveNotebook_BehaeltVorhandeneId()
    {
        Notebooks notebook = new Notebooks();
        NotebookCell zelle = new NotebookCell();
        zelle.AdditionalData.Add("id", JsonSerializer.SerializeToElement("zelle-1"));
        notebook.Cells.Add(zelle);

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        using JsonDocument dokument = JsonDocument.Parse(File.ReadAllText(dateiPfad));
        Assert.AreEqual("zelle-1", dokument.RootElement.GetProperty("cells")[0].GetProperty("id").GetString());
    }

    [TestMethod]
    public void SaveNotebook_ErhoehtAlteUnterversionAufFuenf()
    {
        Notebooks notebook = new Notebooks();
        notebook.MinorFormat = 2;

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        using JsonDocument dokument = JsonDocument.Parse(File.ReadAllText(dateiPfad));
        Assert.AreEqual(5, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
        Assert.AreEqual(2, notebook.MinorFormat);
    }

    [TestMethod]
    public void SaveNotebook_BehaeltHoehereUnterversion()
    {
        Notebooks notebook = new Notebooks();
        notebook.MinorFormat = 6;

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        using JsonDocument dokument = JsonDocument.Parse(File.ReadAllText(dateiPfad));
        Assert.AreEqual(6, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
    }

    [TestMethod]
    public void SaveNotebook_JsonObjekt_WirdGespeichert()
    {
        JsonObject notebook = new JsonObject();
        notebook["nbformat"] = 4;
        notebook["nbformat_minor"] = 2;

        NotebookWriter.SaveNotebook(dateiPfad, notebook);

        using JsonDocument dokument = JsonDocument.Parse(File.ReadAllText(dateiPfad));
        Assert.AreEqual(4, dokument.RootElement.GetProperty("nbformat").GetInt32());
        Assert.AreEqual(2, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
    }

    [TestMethod]
    public void SaveNotebook_VorhandeneDatei_WirdNichtUeberschrieben()
    {
        File.WriteAllText(dateiPfad, "Alter Inhalt");
        Notebooks notebook = new Notebooks();

        Assert.Throws<IOException>(() => NotebookWriter.SaveNotebook(dateiPfad, notebook));

        Assert.AreEqual("Alter Inhalt", File.ReadAllText(dateiPfad));
    }
}
