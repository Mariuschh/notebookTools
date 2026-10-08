using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandAddCellTests : ConsoleCommandTestBase
{
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
