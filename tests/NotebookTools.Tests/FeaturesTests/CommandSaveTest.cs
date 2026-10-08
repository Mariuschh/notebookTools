using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandSaveTest : ConsoleCommandTestBase
{
    [TestMethod]
    public void Save_AbbrechenErstelltKeineDatei()
    {
        EingabeSetzen(" AbBrEcHeN \n");
        CommandSave.Save(NotebookLaden(), originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "nicht gespeichert");
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void Save_EingabeEndeErstelltKeineDatei()
    {
        CommandSave.Save(NotebookLaden(), originalPfad);
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
    }

    [TestMethod]
    public void Save_EnterNutztStandardnamen()
    {
        EingabeSetzen("\n");
        CommandSave.Save(NotebookLaden(), originalPfad);
        Assert.IsTrue(File.Exists(Path.Combine(testOrdner, "original_bearbeitet.ipynb")));
        OriginalPruefen();
    }

    [TestMethod]
    public void Save_EigenerPfadErgaenztEndungUndSpeichertInhalt()
    {
        string ziel = Path.Combine(testOrdner, "Neuer Name");
        EingabeSetzen(ziel + "\n");
        Notebooks notebook = NotebookLaden();
        CommandEdit.ZelleBearbeiten(1, notebook, "Geändert");
        CommandSave.Save(notebook, originalPfad);
        Notebooks gespeichert = NotebookReader.LoadNotebook(ziel + ".ipynb");
        Assert.AreEqual("Geändert", gespeichert.Cells[0].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void Save_VorhandenerPfadWirdNichtUeberschrieben()
    {
        string ziel = Path.Combine(testOrdner, "vorhanden.ipynb");
        File.WriteAllText(ziel, "Behalten");
        EingabeSetzen(ziel + "\n");
        CommandSave.Save(NotebookLaden(), originalPfad);
        Assert.AreEqual("Behalten", File.ReadAllText(ziel));
        StringAssert.Contains(ausgabe.ToString(), "existiert bereits");
        Assert.IsFalse(File.Exists(ziel + ".ipynb"));
        OriginalPruefen();
    }

    [TestMethod]
    public void Save_OriginalAlsZielWirdAbgelehnt()
    {
        EingabeSetzen(originalPfad + "\n");
        CommandSave.Save(NotebookLaden(), originalPfad);
        StringAssert.Contains(ausgabe.ToString(), "existiert bereits");
        OriginalPruefen();
    }

    [TestMethod]
    public void Save_FehlenderZielordnerZeigtFehler()
    {
        EingabeSetzen(Path.Combine(testOrdner, "fehlt", "neu") + "\n");
        CommandSave.Save(NotebookLaden(), originalPfad);
        Assert.DoesNotContain("Notebook gespeichert:", ausgabe.ToString());
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }
}
