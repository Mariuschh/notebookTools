using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandMoveTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void ZelleVerschieben_NachHinten()
    {
        Notebooks notebook = NotebookLaden();
        NotebookCell ersteZelle = notebook.Cells[0];
        Assert.AreSame(notebook, CommandMove.ZelleVerschieben(1, 3, notebook));
        Assert.AreSame(ersteZelle, notebook.Cells[2]);
        Assert.AreEqual("Hallo Hallo", notebook.Cells[0].Source[0]);
        Assert.HasCount(3, notebook.Cells);
        OriginalPruefen();
    }

    [TestMethod]
    public void ZelleVerschieben_NachVorne()
    {
        Notebooks notebook = NotebookLaden();
        CommandMove.ZelleVerschieben(3, 1, notebook);
        Assert.AreEqual("Ende", notebook.Cells[0].Source[0]);
        Assert.AreEqual("Hallo Welt\n", notebook.Cells[1].Source[0]);
        Assert.AreEqual("Hallo Hallo", notebook.Cells[2].Source[0]);
    }

    [TestMethod]
    public void ZelleVerschieben_GleichePositionBehaeltZelle()
    {
        Notebooks notebook = NotebookLaden();
        NotebookCell zelle = notebook.Cells[1];
        CommandMove.ZelleVerschieben(2, 2, notebook);
        Assert.AreSame(zelle, notebook.Cells[1]);
        Assert.HasCount(3, notebook.Cells);
        Assert.HasCount(1, zelle.Outputs);
    }

    [TestMethod]
    public void Move_UngueltigeZielpositionSpeichertNichts()
    {
        foreach (string ziel in new string[] { "abc", "0", "-1", "4" })
        {
            AusgabeLeeren();
            CommandMove.Move(new string[] { originalPfad, "move", "1", ziel }, originalPfad);
            Assert.IsFalse(string.IsNullOrWhiteSpace(ausgabe.ToString()));
            Assert.DoesNotContain("Notebook gespeichert:", ausgabe.ToString());
        }
        OriginalPruefen();
    }

    [TestMethod]
    public void Move_UngueltigeQuellpositionSpeichertNichts()
    {
        foreach (string nummer in new string[] { "abc", "0", "-1", "4" })
        {
            AusgabeLeeren();
            CommandMove.Move(new string[] { originalPfad, "move", nummer, "1" }, originalPfad);
            Assert.IsFalse(string.IsNullOrWhiteSpace(ausgabe.ToString()));
            Assert.DoesNotContain("Notebook gespeichert:", ausgabe.ToString());
        }
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void Move_SpeichertNeueReihenfolge()
    {
        EingabeSetzen("\n");
        CommandMove.Move(new string[] { originalPfad, "move", "3", "1" }, originalPfad);
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.AreEqual("Ende", gespeichert.Cells[0].Source[0]);
        Assert.HasCount(3, gespeichert.Cells);
        OriginalPruefen();
    }
}
