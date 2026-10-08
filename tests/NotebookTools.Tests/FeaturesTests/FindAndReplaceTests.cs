using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class FindAndReplaceTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void FindAndReplace_ErsetztAlleTrefferUndSpeichertKopie()
    {
        Notebooks notebook = NotebookLaden();
        EingabeSetzen("Hallo\nNeu\n\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.AreEqual("Neu Welt\n", notebook.Cells[0].Source[0]);
        Assert.AreEqual("Neu Neu", notebook.Cells[1].Source[0]);
        Assert.AreEqual("Ende", notebook.Cells[2].Source[0]);
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.AreEqual("Neu Neu", gespeichert.Cells[1].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void FindAndReplace_LeererSuchtextAendertNichts()
    {
        Notebooks notebook = NotebookLaden();
        EingabeSetzen("\nNeu\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.AreEqual("Hallo Hallo", notebook.Cells[1].Source[0]);
        StringAssert.Contains(ausgabe.ToString(), "Kein Suchtext");
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
    }

    [TestMethod]
    public void FindAndReplace_KeinTrefferSpeichertNichts()
    {
        EingabeSetzen("Fehlt\nNeu\n");
        FindAndReplace.FindAndReplaces(NotebookLaden(), originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "kein Suchtext gefunden");
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void FindAndReplace_LeererErsatzEntferntSuchtext()
    {
        Notebooks notebook = NotebookLaden();
        EingabeSetzen("Hallo\n\nabbrechen\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.AreEqual(" ", notebook.Cells[1].Source[0]);
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }
}
