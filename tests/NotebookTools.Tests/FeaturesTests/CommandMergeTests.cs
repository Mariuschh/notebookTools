using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandMergeTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void Merge_SpeichertAuswahlInAngegebenerReihenfolge()
    {
        string ziel = Path.Combine(testOrdner, "zusammen");
        int ergebnis = CommandMerge.Merge(new string[] { ziel, "merge", originalPfad, "3,1", originalPfad, "2" });
        Assert.AreEqual(0, ergebnis);
        Notebooks gespeichert = NotebookReader.LoadNotebook(ziel + ".ipynb");
        Assert.HasCount(3, gespeichert.Cells);
        Assert.AreEqual("Ende", gespeichert.Cells[0].Source[0]);
        Assert.AreEqual("Hallo Welt\n", gespeichert.Cells[1].Source[0]);
        Assert.AreEqual("Hallo Hallo", gespeichert.Cells[2].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void Merge_UngueltigeZellnummerErstelltKeineDatei()
    {
        string ziel = Path.Combine(testOrdner, "zusammen");
        int ergebnis = CommandMerge.Merge(new string[] { ziel, "merge", originalPfad, "99", originalPfad, "1" });
        Assert.AreEqual(1, ergebnis);
        Assert.IsFalse(File.Exists(ziel + ".ipynb"));
        OriginalPruefen();
    }

    [TestMethod]
    public void Merge_VorhandeneZieldateiWirdNichtUeberschrieben()
    {
        int ergebnis = CommandMerge.Merge(new string[] { originalPfad, "merge", originalPfad, "1", originalPfad, "2" });
        Assert.AreEqual(1, ergebnis);
        OriginalPruefen();
    }

    [TestMethod]
    public void Merge_FehlendeArgumenteZeigenVerwendung()
    {
        Assert.AreEqual(1, CommandMerge.Merge(new string[] { "ziel", "merge" }));
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
    }
}
