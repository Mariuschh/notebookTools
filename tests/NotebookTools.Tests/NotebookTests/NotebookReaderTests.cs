using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookReaderTests
{
    private string dateiPfad = "";

    [TestInitialize]
    public void Vorbereiten()
    {
        // Jeder Test bekommt eine eigene temporäre Datei.
        dateiPfad = Path.GetTempFileName();
    }

    [TestCleanup]
    public void Aufraeumen()
    {
        File.Delete(dateiPfad);
    }

    [TestMethod]
    public void LoadNotebook_LiestGueltigeDatei()
    {
        string json = """
            { "cells": [{ "cell_type": "code", "source": ["print(1)"] }],
              "nbformat": 4, "nbformat_minor": 5 }
            """;
        File.WriteAllText(dateiPfad, json);

        Notebooks notebook = NotebookReader.LoadNotebook(dateiPfad);

        Assert.AreEqual(4, notebook.Format);
        Assert.AreEqual(5, notebook.MinorFormat);
        Assert.HasCount(1, notebook.Cells);
        Assert.AreEqual("code", notebook.Cells[0].Type);
        Assert.AreEqual("print(1)", notebook.Cells[0].Source[0]);
    }

    [TestMethod]
    public void LoadNotebook_LeeresObjekt_HatKeineZellen()
    {
        File.WriteAllText(dateiPfad, "{}");

        Notebooks notebook = NotebookReader.LoadNotebook(dateiPfad);

        Assert.IsEmpty(notebook.Cells);
    }

    [TestMethod]
    public void LoadNotebook_LeererPfad_WirftFehler()
    {
        // Der Aufruf hinter () => wird von Assert ausgeführt und auf den Fehler geprüft.
        Assert.ThrowsExactly<ArgumentException>(() =>
        {
            NotebookReader.LoadNotebook("");
        });
    }

    [TestMethod]
    public void LoadNotebook_FehlendeDatei_WirftFehler()
    {
        File.Delete(dateiPfad);

        Assert.ThrowsExactly<FileNotFoundException>(() =>

        {

            NotebookReader.LoadNotebook(dateiPfad);

        });
    }

    [TestMethod]
    public void LoadNotebook_UngueltigesJson_WirftFehler()
    {
        File.WriteAllText(dateiPfad, "Das ist kein JSON");

        Assert.Throws<JsonException>(() =>

        {

            NotebookReader.LoadNotebook(dateiPfad);

        });
    }

    [TestMethod]
    public void LoadNotebook_JsonNull_WirftFehler()
    {
        File.WriteAllText(dateiPfad, "null");

        Assert.ThrowsExactly<InvalidDataException>(() =>

        {

            NotebookReader.LoadNotebook(dateiPfad);

        });
    }
}
