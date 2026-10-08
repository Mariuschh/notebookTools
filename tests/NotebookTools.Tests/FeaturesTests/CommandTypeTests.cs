using NotebookTools.Features;
using NotebookTools.Notebook;
using System.Text.Json;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandTypeTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void Type_CodeZuMarkdownBehaeltInhaltUndEntferntAusgaben()
    {
        EingabeSetzen("\n");
        CommandType.Type(new string[] { originalPfad, "type", "2", "markdown" }, originalPfad);
        string zielPfad = Path.Combine(testOrdner, "original_bearbeitet.ipynb");
        Notebooks gespeichert = NotebookReader.LoadNotebook(zielPfad);
        Assert.AreEqual("markdown", gespeichert.Cells[1].Type);
        Assert.AreEqual("Hallo Hallo", gespeichert.Cells[1].Source[0]);
        Assert.AreEqual("behalten", gespeichert.Cells[1].Metadata["tag"].ToString());
        using JsonDocument datei = JsonDocument.Parse(File.ReadAllText(zielPfad));
        JsonElement zelle = datei.RootElement.GetProperty("cells")[1];
        Assert.IsFalse(zelle.TryGetProperty("outputs", out _));
        Assert.IsFalse(zelle.TryGetProperty("execution_count", out _));
        OriginalPruefen();
    }

    [TestMethod]
    public void Type_UngueltigerTypSpeichertNichts()
    {
        CommandType.Type(new string[] { originalPfad, "type", "1", "text" }, originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "Bitte gib als Zelltyp");
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }
}
