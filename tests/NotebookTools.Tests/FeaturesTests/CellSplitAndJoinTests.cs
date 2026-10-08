using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CellSplitAndJoinTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void Split_TeiltInhaltUndBehaeltReihenfolge()
    {
        EingabeSetzen("\n");
        CommandSplit.Split(new string[] { originalPfad, "split", "1", "1" }, originalPfad);
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.HasCount(4, gespeichert.Cells);
        Assert.AreEqual("Hallo Welt\n", gespeichert.Cells[0].Source[0]);
        Assert.AreEqual("Noch eine Zeile", gespeichert.Cells[1].Source[0]);
        Assert.AreEqual("markdown", gespeichert.Cells[1].Type);
        Assert.AreEqual("Hallo Hallo", gespeichert.Cells[2].Source[0]);
        Assert.AreNotEqual(gespeichert.Cells[0].AdditionalData["id"].GetString(), gespeichert.Cells[1].AdditionalData["id"].GetString());
        OriginalPruefen();
    }

    [TestMethod]
    public void Split_CodeSetztAusgabenInBeidenZellenZurueck()
    {
        Notebooks notebook = NotebookLaden();
        notebook.Cells[1].Source.Clear();
        notebook.Cells[1].Source.Add("Erste Zeile\r\nZweite Zeile");
        string quellPfad = Path.Combine(testOrdner, "code.ipynb");
        NotebookWriter.SaveNotebook(quellPfad, notebook);
        EingabeSetzen("\n");
        CommandSplit.Split(new string[] { quellPfad, "split", "2", "1" }, quellPfad);
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "code_bearbeitet.ipynb"));
        Assert.AreEqual("Erste Zeile\r\n", gespeichert.Cells[1].Source[0]);
        Assert.AreEqual("Zweite Zeile", gespeichert.Cells[2].Source[0]);
        Assert.IsEmpty(gespeichert.Cells[1].Outputs);
        Assert.IsEmpty(gespeichert.Cells[2].Outputs);
        Assert.IsNull(gespeichert.Cells[1].ExecutionCount);
        Assert.AreEqual("behalten", gespeichert.Cells[2].Metadata["tag"].ToString());
    }

    [TestMethod]
    public void Split_UngueltigeZeileSpeichertNichts()
    {
        CommandSplit.Split(new string[] { originalPfad, "split", "1", "5" }, originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "Bitte wähle eine Zeile");
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void Join_VerbindetNichtBenachbarteZellen()
    {
        EingabeSetzen("\n");
        CommandJoin.Join(new string[] { originalPfad, "join", "1,3" }, originalPfad);
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.HasCount(2, gespeichert.Cells);
        Assert.AreEqual("Hallo Welt\nNoch eine Zeile" + Environment.NewLine + "Ende", gespeichert.Cells[0].Source[0]);
        Assert.AreEqual("Hallo Hallo", gespeichert.Cells[1].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void Join_UnterschiedlicheTypenSpeichertNichts()
    {
        CommandJoin.Join(new string[] { originalPfad, "join", "1,2" }, originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "gleichen Typ");
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }
}
