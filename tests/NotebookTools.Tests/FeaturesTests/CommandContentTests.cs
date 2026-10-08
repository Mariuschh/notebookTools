using NotebookTools.Features;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class CommandContentTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void Content_ZeigtNummerTypUndEinzeiligenInhalt()
    {
        CommandContent.Content(originalPfad, NotebookLaden());
        StringAssert.Contains(ausgabe.ToString(), "1 | markdown | Hallo Welt Noch eine Zeile");
        StringAssert.Contains(ausgabe.ToString(), "2 | code     | Hallo Hallo");
        StringAssert.Contains(ausgabe.ToString(), "3 | markdown | Ende");
        OriginalPruefen();
    }

    [TestMethod]
    public void Content_KuerztNurInhalteUeberAchtzigZeichen()
    {
        Notebooks notebook = NotebookLaden();
        notebook.Cells[0].Source = new List<string> { new string('a', 80) };
        notebook.Cells[1].Source = new List<string> { new string('b', 81) };
        notebook.Cells[2].Source.Clear();
        CommandContent.Content(originalPfad, notebook);
        StringAssert.Contains(ausgabe.ToString(), "1 | markdown | " + new string('a', 80) + Environment.NewLine);
        StringAssert.Contains(ausgabe.ToString(), "2 | code     | " + new string('b', 80) + "...");
        StringAssert.Contains(ausgabe.ToString(), "3 | markdown | ");
    }

    [TestMethod]
    public void Content_LeeresNotebookHatKeineAusgabe()
    {
        CommandContent.Content(originalPfad, new Notebooks());
        Assert.AreEqual("", ausgabe.ToString());
    }
}
