using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandAddCellTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void AddCell_LiestTextdateiAlsNeueZelle()
    {
        string textPfad = Path.Combine(testOrdner, "aufgabe.txt");
        File.WriteAllText(textPfad, "Überschrift\nLanger Text");
        EingabeSetzen("markdown\n");
        Notebooks notebook = NotebookLaden();
        CommandAddCell.AddCell(notebook, textPfad);
        Assert.HasCount(4, notebook.Cells);
        Assert.AreEqual("markdown", notebook.Cells[3].Type);
        Assert.AreEqual("Überschrift\nLanger Text", notebook.Cells[3].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void AddCell_FehlendeTextdateiFuegtKeineZelleHinzu()
    {
        EingabeSetzen("code\n");
        Notebooks notebook = NotebookLaden();
        CommandAddCell.AddCell(notebook, Path.Combine(testOrdner, "fehlt.txt"));
        Assert.HasCount(3, notebook.Cells);
        OriginalPruefen();
    }

    // Gültige Eingaben öffnen den Windows-Editor. Dieser Ablauf bleibt eine manuelle Prüfung.
    [TestMethod]
    public void AddCell_EingabeEndeFuegtKeineZelleHinzu()
    {
        Notebooks notebook = NotebookLaden();
        Assert.AreSame(notebook, CommandAddCell.AddCell(notebook));
        Assert.HasCount(3, notebook.Cells);
        OriginalPruefen();
    }

    [TestMethod]
    public void AddCell_UngueltigerTypFragtErneutUndKannAbbrechen()
    {
        EingabeSetzen("ungueltig\n");
        Notebooks notebook = NotebookLaden();
        CommandAddCell.AddCell(notebook);
        StringAssert.Contains(ausgabe.ToString(), "Ungültiger Zelltyp");
        Assert.HasCount(3, notebook.Cells);
        OriginalPruefen();
    }
}
