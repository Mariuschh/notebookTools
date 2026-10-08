using NotebookTools.Features;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class RunFileSearchTests : ConsoleCommandTestBase
{
    [TestMethod]
    public void Search_ExpliziterPfadOhneEndungWirdGefunden()
    {
        string pfadOhneEndung = Path.Combine(testOrdner, "original");
        string gefunden;
        Assert.AreEqual(0, RunFileSearch.Search(new string[] { pfadOhneEndung }, out gefunden));
        Assert.AreEqual(originalPfad, gefunden);
        OriginalPruefen();
    }

    [TestMethod]
    public void Search_ExpliziterPfadMitEndungWirdGefunden()
    {
        string gefunden;
        Assert.AreEqual(0, RunFileSearch.Search(new string[] { originalPfad }, out gefunden));
        Assert.AreEqual(originalPfad, gefunden);
    }

    [TestMethod]
    public void Search_OhneArgumenteZeigtVerwendung()
    {
        string gefunden;
        Assert.AreEqual(1, RunFileSearch.Search(new string[0], out gefunden));
        Assert.AreEqual("", gefunden);
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
    }

    [TestMethod]
    public void Search_LeererDateinameZeigtFehler()
    {
        string gefunden;
        Assert.AreEqual(1, RunFileSearch.Search(new string[] { "" }, out gefunden));
        Assert.AreEqual("", gefunden);
    }
}
