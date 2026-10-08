using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandEditTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void ZelleBearbeiten_AendertNurAusgewaehlteZelle()
    {
        Notebooks notebook = NotebookLaden();
        Notebooks ergebnis = CommandEdit.ZelleBearbeiten(1, notebook, "Neu\nMehrzeilig\n");
        Assert.AreSame(notebook, ergebnis);
        Assert.AreEqual("Neu\nMehrzeilig\n", string.Join("", notebook.Cells[0].Source));
        Assert.AreEqual("Hallo Hallo", notebook.Cells[1].Source[0]);
        Assert.HasCount(3, notebook.Cells);
        OriginalPruefen();
    }

    [TestMethod]
    public void ZelleBearbeiten_CodeEntferntAlteAusgaben()
    {
        Notebooks notebook = NotebookLaden();
        CommandEdit.ZelleBearbeiten(2, notebook, "Neuer Code");
        Assert.IsEmpty(notebook.Cells[1].Outputs);
        Assert.IsNull(notebook.Cells[1].ExecutionCount);
        Assert.AreEqual("code", notebook.Cells[1].Type);
        Assert.AreEqual("behalten", notebook.Cells[1].Metadata["tag"].ToString());
    }

    [TestMethod]
    public void ZelleBearbeiten_LeererInhaltIstErlaubt()
    {
        Notebooks notebook = NotebookLaden();
        CommandEdit.ZelleBearbeiten(3, notebook, "");
        Assert.AreEqual("", string.Join("", notebook.Cells[2].Source));
        Assert.HasCount(3, notebook.Cells);
    }

    [TestMethod]
    public void Edit_FehlendeNummerOeffnetKeinenEditor()
    {
        CommandEdit.Edit(new string[] { originalPfad, "edit" }, originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
        OriginalPruefen();
    }

    [TestMethod]
    public void Edit_UngueltigeNummerOeffnetKeinenEditor()
    {
        CommandEdit.Edit(new string[] { originalPfad, "edit", "abc" }, originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
    }

    [TestMethod]
    public void Edit_ZellnummerAusserhalbBereichOeffnetKeinenEditor()
    {
        foreach (string nummer in new string[] { "0", "-1", "4" })
        {
            AusgabeLeeren();
            CommandEdit.Edit(new string[] { originalPfad, "edit", nummer }, originalPfad);
            StringAssert.Contains(ausgabe.ToString(), "außerhalb");
        }
        OriginalPruefen();
    }
}
