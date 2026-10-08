using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookTests
{
    [TestMethod]
    public void NeuesNotebook_IstLeer()
    {
        Notebooks notebook = new Notebooks();

        Assert.IsEmpty(notebook.Cells);
        Assert.IsEmpty(notebook.Metadata);
        Assert.IsEmpty(notebook.AdditionalData);
        Assert.AreEqual(0, notebook.Format);
        Assert.AreEqual(0, notebook.MinorFormat);
    }

    [TestMethod]
    public void ZusaetzlichesFeld_BleibtBeimUmwandelnErhalten()
    {
        Notebooks? notebook = JsonSerializer.Deserialize<Notebooks>("""{ "titel": "Test" }""");
        Assert.IsNotNull(notebook);
        Assert.AreEqual("Test", notebook.AdditionalData["titel"].GetString());

        string json = JsonSerializer.Serialize(notebook);
        using JsonDocument dokument = JsonDocument.Parse(json);

        Assert.AreEqual("Test", dokument.RootElement.GetProperty("titel").GetString());
    }
}
