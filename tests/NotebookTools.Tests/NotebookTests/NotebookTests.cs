using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookTests
{
    [TestMethod]
    public void NeuesNotebook_IstLeer()
    {
        Notebooks notebook = new Notebooks();

        Assert.IsEmpty(notebook.Cells);
        Assert.IsEmpty(notebook.Metadata);
        Assert.IsEmpty(notebook.AdditionalData);
        Assert.AreEqual(0, notebook.Format);
        Assert.AreEqual(0, notebook.MinorFormat);
    }

    [TestMethod]
    public void ZelleHinzufuegen_AendertNurEinNotebook()
    {
        Notebooks erstesNotebook = new Notebooks();
        Notebooks zweitesNotebook = new Notebooks();

        erstesNotebook.Cells.Add(new NotebookCell());

        Assert.HasCount(1, erstesNotebook.Cells);
        Assert.IsEmpty(zweitesNotebook.Cells);
    }

    [TestMethod]
    public void NotebookInJsonUmwandeln_SpeichertFormat()
    {
        Notebooks notebook = new Notebooks();
        notebook.Format = 4;
        notebook.MinorFormat = 5;

        string json = JsonSerializer.Serialize(notebook);
        using JsonDocument dokument = JsonDocument.Parse(json);

        Assert.AreEqual(4, dokument.RootElement.GetProperty("nbformat").GetInt32());
        Assert.AreEqual(5, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
    }

    [TestMethod]
    public void JsonEinlesen_LiestZelleUndFormat()
    {
        string json = """
            { "cells": [{ "cell_type": "code" }], "nbformat": 4, "nbformat_minor": 5 }
            """;

        Notebooks? notebook = JsonSerializer.Deserialize<Notebooks>(json);

        Assert.IsNotNull(notebook);
        Assert.HasCount(1, notebook.Cells);
        Assert.AreEqual("code", notebook.Cells[0].Type);
        Assert.AreEqual(4, notebook.Format);
        Assert.AreEqual(5, notebook.MinorFormat);
    }

    [TestMethod]
    public void JsonEinlesen_OhneFelder_ErzeugtLeeresNotebook()
    {
        Notebooks? notebook = JsonSerializer.Deserialize<Notebooks>("{}");

        Assert.IsNotNull(notebook);
        Assert.IsEmpty(notebook.Cells);
        Assert.IsEmpty(notebook.Metadata);
    }

    [TestMethod]
    public void ZusaetzlichesFeld_BleibtBeimUmwandelnErhalten()
    {
        Notebooks? notebook = JsonSerializer.Deserialize<Notebooks>("""{ "titel": "Test" }""");
        Assert.IsNotNull(notebook);
        Assert.AreEqual("Test", notebook.AdditionalData["titel"].GetString());

        string json = JsonSerializer.Serialize(notebook);
        using JsonDocument dokument = JsonDocument.Parse(json);

        Assert.AreEqual("Test", dokument.RootElement.GetProperty("titel").GetString());
    }
}
