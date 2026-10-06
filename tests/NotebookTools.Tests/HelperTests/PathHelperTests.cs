using NotebookTools.Help;

namespace NotebookTools.Tests.HelperTests;

[TestClass]
public class PathHelperTests
{
    private string dateiPfad = "";

    [TestInitialize]
    public void Vorbereiten()
    {
        // Jeder Test bekommt einen eigenen Dateinamen mit der Notebook-Endung.
        dateiPfad = Path.GetTempFileName();
        File.Delete(dateiPfad);
        dateiPfad = dateiPfad + ".ipynb";
    }

    [TestCleanup]
    public void Aufraeumen()
    {
        File.Delete(dateiPfad);
    }

    [TestMethod]
    public void NormalizeInput_OhneEndung_ErgaenztIpynb()
    {
        string ergebnis = PathHelper.NormalizeInput("notebook");

        Assert.AreEqual("notebook.ipynb", ergebnis);
    }

    [TestMethod]
    public void NormalizeInput_MitEndung_BleibtUnveraendert()
    {
        string ergebnis = PathHelper.NormalizeInput("notebook.ipynb");

        Assert.AreEqual("notebook.ipynb", ergebnis);
    }

    [TestMethod]
    public void NormalizeInput_GrosseEndung_BleibtUnveraendert()
    {
        string ergebnis = PathHelper.NormalizeInput("notebook.IPYNB");

        Assert.AreEqual("notebook.IPYNB", ergebnis);
    }

    [TestMethod]
    public void NormalizeInput_AndereEndung_HaengtIpynbAn()
    {
        string ergebnis = PathHelper.NormalizeInput("notebook.txt");

        Assert.AreEqual("notebook.txt.ipynb", ergebnis);
    }

    [TestMethod]
    public void NormalizeInput_LeererText_WirftFehler()
    {
        Assert.ThrowsExactly<ArgumentException>(() => PathHelper.NormalizeInput(""));
    }

    [TestMethod]
    public void NormalizeInput_NurLeerzeichen_WirftFehler()
    {
        Assert.ThrowsExactly<ArgumentException>(() => PathHelper.NormalizeInput("   "));
    }

    [TestMethod]
    public void GetLocalPath_VorhandeneDatei_GibtPfadZurueck()
    {
        File.WriteAllText(dateiPfad, "{}");

        string? ergebnis = PathHelper.GetLocalPath(dateiPfad);

        Assert.AreEqual(dateiPfad, ergebnis);
    }

    [TestMethod]
    public void GetLocalPath_OhneEndung_FindetNotebook()
    {
        File.WriteAllText(dateiPfad, "{}");
        string pfadOhneEndung = Path.ChangeExtension(dateiPfad, null);

        string? ergebnis = PathHelper.GetLocalPath(pfadOhneEndung);

        Assert.AreEqual(dateiPfad, ergebnis);
    }

    [TestMethod]
    public void GetLocalPath_RelativerPfad_GibtAbsolutenPfadZurueck()
    {
        File.WriteAllText(dateiPfad, "{}");
        string relativerPfad = Path.GetRelativePath(Directory.GetCurrentDirectory(), dateiPfad);

        string? ergebnis = PathHelper.GetLocalPath(relativerPfad);

        Assert.AreEqual(dateiPfad, ergebnis);
    }

    [TestMethod]
    public void GetLocalPath_FehlendeDatei_GibtNullZurueck()
    {
        string? ergebnis = PathHelper.GetLocalPath(dateiPfad);

        Assert.IsNull(ergebnis);
    }

    [TestMethod]
    public void GetLocalPath_LeererText_WirftFehler()
    {
        Assert.ThrowsExactly<ArgumentException>(() => PathHelper.GetLocalPath(""));
    }
}
