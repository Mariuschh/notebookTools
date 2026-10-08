using NotebookTools.Help;

namespace NotebookTools.Tests.HelperTests;

[TestClass]
public class FileHelperTests
{
    // Die Dateisuche selbst benötigt den Windows-Suchindex.
    // Diese Tests prüfen die Eingaben, bevor die Suche gestartet wird.

    [TestMethod]
    public void FindByFileName_Null_WirftFehler()
    {
        // null! übergibt absichtlich einen ungültigen Wert für diesen Test.
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            FileHelper.FindByFileName(null!);
        });
    }

    [TestMethod]
    public void FindByFileName_LeererText_WirftFehler()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            FileHelper.FindByFileName("");
        });
    }

    [TestMethod]
    public void FindByFileName_NurLeerzeichen_WirftFehler()
    {
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            FileHelper.FindByFileName("   ");
        });
    }

}
