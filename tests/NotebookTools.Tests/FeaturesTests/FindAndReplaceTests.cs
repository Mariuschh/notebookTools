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
        Assert.AreEqual("Neu Welt\nNoch eine Zeile", string.Join("", notebook.Cells[0].Source));
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
    public void FindAndReplace_FindetTextZwischenZweiTextstuecken()
    {
        Notebooks notebook = NotebookLaden();
        notebook.Cells[0].Source = new List<string> { "Hal", "lo Welt" };
        EingabeSetzen("Hallo\nNeu\nabbrechen\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.AreEqual("Neu Welt", string.Join("", notebook.Cells[0].Source));
        OriginalPruefen();
    }

    [TestMethod]
    public void FindAndReplace_EntferntNurAusgabenGeaenderterCodeZellen()
    {
        Notebooks notebook = NotebookLaden();
        EingabeSetzen("Hallo\nNeu\nabbrechen\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.IsEmpty(notebook.Cells[1].Outputs);
        Assert.IsNull(notebook.Cells[1].ExecutionCount);
        Assert.AreEqual("behalten", notebook.Cells[1].Metadata["tag"].ToString());
    }

    [TestMethod]
    public void FindAndReplace_GleicherErsatzAendertUndSpeichertNichts()
    {
        Notebooks notebook = NotebookLaden();
        EingabeSetzen("Hallo\nHallo\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.HasCount(1, notebook.Cells[1].Outputs);
        Assert.AreEqual(2, notebook.Cells[1].ExecutionCount);
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void FindAndReplace_EingabeEndeBeimErsatzBrichtAb()
    {
        Notebooks notebook = NotebookLaden();
        EingabeSetzen("Hallo\n");
        FindAndReplace.FindAndReplaces(notebook, originalPfad);
        Assert.AreEqual("Hallo Hallo", notebook.Cells[1].Source[0]);
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
    }

    [TestMethod]
    public void FindAndReplace_StelltTerminalfarbeNachErfolgWiederHer()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        ConsoleColor vorherigeFarbe = Console.ForegroundColor;
        EingabeSetzen("Hallo\nNeu\nabbrechen\n");
        FindAndReplace.FindAndReplaces(NotebookLaden(), originalPfad);
        Assert.AreEqual(vorherigeFarbe, Console.ForegroundColor);
    }

    [TestMethod]
    public void FindAndReplace_LeererSuchtextBehaeltTerminalfarbe()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        ConsoleColor vorherigeFarbe = Console.ForegroundColor;
        EingabeSetzen("\n");
        FindAndReplace.FindAndReplaces(NotebookLaden(), originalPfad);
        Assert.AreEqual(vorherigeFarbe, Console.ForegroundColor);
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
