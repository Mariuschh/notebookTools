using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookCellTests
{
    [TestMethod]
    public void NeueZelle_IstLeer()
    {
        NotebookCell zelle = new NotebookCell();

        Assert.AreEqual("", zelle.Type);
        Assert.IsEmpty(zelle.Source);
        Assert.IsEmpty(zelle.Outputs);
        Assert.IsEmpty(zelle.Metadata);
        Assert.IsEmpty(zelle.AdditionalData);
        Assert.IsNull(zelle.ExecutionCount);
    }

    [TestMethod]
    public void TextHinzufuegen_AendertNurEineZelle()
    {
        NotebookCell ersteZelle = new NotebookCell();
        NotebookCell zweiteZelle = new NotebookCell();

        ersteZelle.Source.Add("Hallo");

        Assert.HasCount(1, ersteZelle.Source);
        Assert.IsEmpty(zweiteZelle.Source);
    }

    [TestMethod]
    public void ZelleInJsonUmwandeln_SpeichertTypUndText()
    {
        NotebookCell zelle = new NotebookCell();
        zelle.Type = "code";
        zelle.Source.Add("print(1)");
        zelle.ExecutionCount = 2;

        string json = JsonSerializer.Serialize(zelle);
        using JsonDocument dokument = JsonDocument.Parse(json);

        Assert.AreEqual("code", dokument.RootElement.GetProperty("cell_type").GetString());
        Assert.AreEqual("print(1)", dokument.RootElement.GetProperty("source")[0].GetString());
        Assert.AreEqual(2, dokument.RootElement.GetProperty("execution_count").GetInt32());
    }

    [TestMethod]
    public void JsonEinlesen_LiestZellendaten()
    {
        string json = """
            { "cell_type": "code", "source": ["print(1)"], "execution_count": 2 }
            """;

        NotebookCell? zelle = JsonSerializer.Deserialize<NotebookCell>(json);

        Assert.IsNotNull(zelle);
        Assert.AreEqual("code", zelle.Type);
        Assert.AreEqual("print(1)", zelle.Source[0]);
        Assert.AreEqual(2, zelle.ExecutionCount);
    }

    [TestMethod]
    public void JsonEinlesen_OhneAusfuehrung_ZaehlerBleibtNull()
    {
        NotebookCell? zelle = JsonSerializer.Deserialize<NotebookCell>("""{ "execution_count": null }""");

        Assert.IsNotNull(zelle);
        Assert.IsNull(zelle.ExecutionCount);
    }

    [TestMethod]
    public void ZellenId_BleibtBeimUmwandelnErhalten()
    {
        NotebookCell? zelle = JsonSerializer.Deserialize<NotebookCell>("""{ "id": "zelle-1" }""");
        Assert.IsNotNull(zelle);
        Assert.AreEqual("zelle-1", zelle.AdditionalData["id"].GetString());

        string json = JsonSerializer.Serialize(zelle);
        using JsonDocument dokument = JsonDocument.Parse(json);

        Assert.AreEqual("zelle-1", dokument.RootElement.GetProperty("id").GetString());
    }
}
