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
