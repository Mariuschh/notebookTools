using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandDeleteTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void ZelleLoeschen_EntferntMittlereZelle()
    {
        Notebooks notebook = NotebookLaden();
        Assert.AreSame(notebook, CommandDelete.ZelleLoeschen(2, notebook));
        Assert.HasCount(2, notebook.Cells);
        Assert.AreEqual("Hallo Welt\n", notebook.Cells[0].Source[0]);
        Assert.AreEqual("Ende", notebook.Cells[1].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void ZelleLoeschen_LetzteVerbleibendeZelle()
    {
        Notebooks notebook = NotebookLaden();
        CommandDelete.ZelleLoeschen(3, notebook);
        CommandDelete.ZelleLoeschen(1, notebook);
        CommandDelete.ZelleLoeschen(1, notebook);
        Assert.IsEmpty(notebook.Cells);
    }

    [TestMethod]
    public void Delete_UngueltigeEingabenSpeichernNichts()
    {
        foreach (string nummer in new string[] { "abc", "0", "-1", "4" })
        {
            AusgabeLeeren();
            CommandDelete.Delete(new string[] { originalPfad, "delete", nummer }, originalPfad);
            Assert.IsFalse(string.IsNullOrWhiteSpace(ausgabe.ToString()));
            Assert.DoesNotContain("Notebook gespeichert:", ausgabe.ToString());
        }
        OriginalPruefen();
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
    }

    [TestMethod]
    public void Delete_FehlendeNummerZeigtVerwendung()
    {
        CommandDelete.Delete(new string[] { originalPfad, "delete" }, originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
        OriginalPruefen();
    }

    [TestMethod]
    public void Delete_SpeichertGeaendertesNotebookInNeueDatei()
    {
        EingabeSetzen("\n");
        CommandDelete.Delete(new string[] { originalPfad, "delete", "2" }, originalPfad);
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.HasCount(2, gespeichert.Cells);
        Assert.AreEqual("Ende", gespeichert.Cells[1].Source[0]);
        OriginalPruefen();
    }
}
