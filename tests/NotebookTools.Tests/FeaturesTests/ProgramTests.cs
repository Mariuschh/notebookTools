using NotebookTools.Features;
using NotebookTools.Notebook;
using System.Diagnostics;

namespace NotebookTools.Tests.FeaturesTests;

[TestClass]
[DoNotParallelize]
public class ProgramTests : ConsoleCommandTestBase
{
    private void ProgrammAufrufen(string[] args)
    {
        // Das Programm wie im Terminal starten, ohne private Methoden aufzurufen.
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = Path.Combine(AppContext.BaseDirectory, "NotebookTools.exe");
        startInfo.UseShellExecute = false;
        startInfo.CreateNoWindow = true;
        startInfo.RedirectStandardInput = true;
        startInfo.RedirectStandardOutput = true;
        foreach (string argument in args)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using (Process programm = new Process())
        {
            programm.StartInfo = startInfo;
            programm.Start();
            programm.StandardInput.Write(Console.In.ReadToEnd());
            programm.StandardInput.Close();
            string programmAusgabe = programm.StandardOutput.ReadToEnd();
            programm.WaitForExit();
            ausgabe.Write(programmAusgabe);
            Assert.AreEqual(0, programm.ExitCode);
        }
    }

    [TestMethod]
    public void Main_OhneArgumenteZeigtHilfe()
    {
        ProgrammAufrufen(new string[0]);
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
        Assert.DoesNotContain("Notebook gefunden:", ausgabe.ToString());
    }

    [TestMethod]
    public void Main_HelpIstNichtCaseSensitive()
    {
        foreach (string command in new string[] { "help", "HELP", "HeLp" })
        {
            AusgabeLeeren();
            ProgrammAufrufen(new string[] { command });
            StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
            Assert.DoesNotContain("Notebook gefunden:", ausgabe.ToString());
        }
    }

    [TestMethod]
    public void Main_ContentIstAngebunden()
    {
        ProgrammAufrufen(new string[] { originalPfad, "CONTENT" });
        StringAssert.Contains(ausgabe.ToString(), "1 | markdown | Hallo Welt Noch eine Zeile");
        OriginalPruefen();
    }

    [TestMethod]
    public void Main_ShowOhneNummerZeigtVerwendung()
    {
        ProgrammAufrufen(new string[] { originalPfad, "show" });
        StringAssert.Contains(ausgabe.ToString(), "Verwendung:");
    }

    [TestMethod]
    public void Main_ShowUngueltigeNummerZeigtFehler()
    {
        ProgrammAufrufen(new string[] { originalPfad, "show", "abc" });
        StringAssert.Contains(ausgabe.ToString(), "Bitte gib eine Zahl");
    }

    [TestMethod]
    public void Main_DeleteIstAngebundenUndKannSpeichern()
    {
        EingabeSetzen("\n");
        ProgrammAufrufen(new string[] { originalPfad, "delete", "1" });
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.HasCount(2, gespeichert.Cells);
        OriginalPruefen();
    }

    [TestMethod]
    public void Main_EditUngueltigeNummerOeffnetKeinenEditor()
    {
        ProgrammAufrufen(new string[] { originalPfad, "edit", "0" });
        StringAssert.Contains(ausgabe.ToString(), "außerhalb");
        OriginalPruefen();
    }

    [TestMethod]
    public void Main_AddBeiEingabeEndeSpeichertNichts()
    {
        ProgrammAufrufen(new string[] { originalPfad, "add" });
        Assert.HasCount(1, Directory.GetFiles(testOrdner));
        OriginalPruefen();
    }

    [TestMethod]
    public void Main_MoveIstAngebunden()
    {
        EingabeSetzen("\n");
        ProgrammAufrufen(new string[] { originalPfad, "move", "3", "1" });
        Notebooks gespeichert = NotebookReader.LoadNotebook(Path.Combine(testOrdner, "original_bearbeitet.ipynb"));
        Assert.AreEqual("Ende", gespeichert.Cells[0].Source[0]);
        OriginalPruefen();
    }

    [TestMethod]
    public void Main_MergeSuchtNichtNachZieldatei()
    {
        string ziel = Path.Combine(testOrdner, "zusammen");
        ProgrammAufrufen(new string[] { ziel, "merge", originalPfad, "1", originalPfad, "3" });
        Assert.IsTrue(File.Exists(ziel + ".ipynb"));
        OriginalPruefen();
    }

    [TestMethod]
    public void Main_FindAndReplaceIstAngebunden()
    {
        EingabeSetzen("Hallo\nNeu\nabbrechen\n");
        ProgrammAufrufen(new string[] { originalPfad, "findandreplace" });
        StringAssert.Contains(ausgabe.ToString(), "Zeile(n) wurden geändert.");
        OriginalPruefen();
    }
}
