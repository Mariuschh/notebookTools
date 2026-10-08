using System.Text.Json;
using System.Text.Json.Nodes;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookMergerTests
{
    private string ersteDatei = "";
    private string zweiteDatei = "";

    [TestInitialize]
    public void Vorbereiten()
    {
        ersteDatei = Path.GetTempFileName();
        zweiteDatei = Path.GetTempFileName();
    }

    [TestCleanup]
    public void Aufraeumen()
    {
        File.Delete(ersteDatei);
        File.Delete(zweiteDatei);
    }

    [TestMethod]
    public void Merge_VerbindetZellenAusZweiDateien()
    {
        File.WriteAllText(ersteDatei, """
            { "nbformat": 4, "cells": [{ "source": ["Hallo"] }] }
            """);
        File.WriteAllText(zweiteDatei, """
            { "nbformat": 4, "cells": [{ "source": ["Welt"] }] }
            """);
        List<string> dateien = new List<string> { ersteDatei, zweiteDatei };
        // Für jede Datei gibt es eine Liste mit den gewünschten Zellnummern.
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int> { 1 });
        zellnummern.Add(new List<int> { 1 });

        JsonObject ergebnis = NotebookMerger.Merge(dateien, zellnummern);

        using JsonDocument dokument = JsonDocument.Parse(ergebnis.ToJsonString());
        JsonElement zellen = dokument.RootElement.GetProperty("cells");
        Assert.AreEqual(2, zellen.GetArrayLength());
        Assert.AreEqual("Hallo", zellen[0].GetProperty("source")[0].GetString());
        Assert.AreEqual("Welt", zellen[1].GetProperty("source")[0].GetString());
    }

    [TestMethod]
    public void Merge_BeachtetReihenfolgeDerAuswahl()
    {
        File.WriteAllText(ersteDatei, """
            { "nbformat": 4, "cells": [{ "source": ["A"] }, { "source": ["B"] }] }
            """);
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int> { 2, 1 });

        JsonObject ergebnis = NotebookMerger.Merge(dateien, zellnummern);

        using JsonDocument dokument = JsonDocument.Parse(ergebnis.ToJsonString());
        JsonElement zellen = dokument.RootElement.GetProperty("cells");
        Assert.AreEqual("B", zellen[0].GetProperty("source")[0].GetString());
        Assert.AreEqual("A", zellen[1].GetProperty("source")[0].GetString());
    }

    [TestMethod]
    public void Merge_DoppelteAuswahl_BekommtVerschiedeneIds()
    {
        string json = """
            { "nbformat": 4, "cells": [{ "id": "alte-id", "source": ["Hallo"] }] }
            """;
        File.WriteAllText(ersteDatei, json);
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int> { 1, 1 });

        JsonObject ergebnis = NotebookMerger.Merge(dateien, zellnummern);

        using JsonDocument dokument = JsonDocument.Parse(ergebnis.ToJsonString());
        JsonElement zellen = dokument.RootElement.GetProperty("cells");
        Assert.AreEqual(2, zellen.GetArrayLength());
        string? ersteId = zellen[0].GetProperty("id").GetString();
        string? zweiteId = zellen[1].GetProperty("id").GetString();
        Assert.IsFalse(string.IsNullOrEmpty(ersteId));
        Assert.IsFalse(string.IsNullOrEmpty(zweiteId));
        Assert.AreNotEqual(ersteId, zweiteId);
        Assert.AreNotEqual("alte-id", ersteId);
        Assert.AreNotEqual("alte-id", zweiteId);
        Assert.AreEqual(json, File.ReadAllText(ersteDatei));
    }

    [TestMethod]
    public void Merge_UebernimmtMetadatenDerErstenDatei()
    {
        File.WriteAllText(ersteDatei, """
            { "nbformat": 4, "cells": [], "metadata": { "name": "Erstes Notebook" } }
            """);
        File.WriteAllText(zweiteDatei, """
            { "nbformat": 4, "cells": [], "metadata": { "name": "Zweites Notebook" } }
            """);
        List<string> dateien = new List<string> { ersteDatei, zweiteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());
        zellnummern.Add(new List<int>());

        JsonObject ergebnis = NotebookMerger.Merge(dateien, zellnummern);

        using JsonDocument dokument = JsonDocument.Parse(ergebnis.ToJsonString());
        Assert.AreEqual("Erstes Notebook", dokument.RootElement.GetProperty("metadata").GetProperty("name").GetString());
    }

    [TestMethod]
    public void Merge_NimmtHoechsteUnterversion()
    {
        File.WriteAllText(ersteDatei, """{ "nbformat": 4, "nbformat_minor": 5, "cells": [] }""");
        File.WriteAllText(zweiteDatei, """{ "nbformat": 4, "nbformat_minor": 7, "cells": [] }""");
        List<string> dateien = new List<string> { ersteDatei, zweiteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());
        zellnummern.Add(new List<int>());

        JsonObject ergebnis = NotebookMerger.Merge(dateien, zellnummern);

        using JsonDocument dokument = JsonDocument.Parse(ergebnis.ToJsonString());
        Assert.AreEqual(7, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
    }

    [TestMethod]
    public void Merge_LeereAuswahl_HatKeineZellenUndMindestensVersionFuenf()
    {
        File.WriteAllText(ersteDatei, """{ "nbformat": 4, "nbformat_minor": 2, "cells": [{}] }""");
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());

        JsonObject ergebnis = NotebookMerger.Merge(dateien, zellnummern);

        using JsonDocument dokument = JsonDocument.Parse(ergebnis.ToJsonString());
        Assert.AreEqual(0, dokument.RootElement.GetProperty("cells").GetArrayLength());
        Assert.AreEqual(5, dokument.RootElement.GetProperty("nbformat_minor").GetInt32());
    }

    [TestMethod]
    public void Merge_ZellnummerNull_WirftFehler()
    {
        File.WriteAllText(ersteDatei, """{ "nbformat": 4, "cells": [{}] }""");
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int> { 0 });

        Assert.ThrowsExactly<ArgumentException>(() =>

        {

            NotebookMerger.Merge(dateien, zellnummern);

        });
    }

    [TestMethod]
    public void Merge_ZellnummerZuGross_WirftFehler()
    {
        File.WriteAllText(ersteDatei, """{ "nbformat": 4, "cells": [{}] }""");
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int> { 2 });

        Assert.ThrowsExactly<ArgumentException>(() =>

        {

            NotebookMerger.Merge(dateien, zellnummern);

        });
    }

    [TestMethod]
    public void Merge_FalschesFormat_WirftFehler()
    {
        File.WriteAllText(ersteDatei, """{ "nbformat": 3, "cells": [] }""");
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());

        Assert.ThrowsExactly<ArgumentException>(() =>

        {

            NotebookMerger.Merge(dateien, zellnummern);

        });
    }

    [TestMethod]
    public void Merge_FehlendeZellliste_WirftFehler()
    {
        File.WriteAllText(ersteDatei, """{ "nbformat": 4 }""");
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());

        Assert.ThrowsExactly<ArgumentException>(() =>

        {

            NotebookMerger.Merge(dateien, zellnummern);

        });
    }

    [TestMethod]
    public void Merge_FehlendeDatei_WirftFehler()
    {
        File.Delete(ersteDatei);
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());

        Assert.ThrowsExactly<FileNotFoundException>(() =>

        {

            NotebookMerger.Merge(dateien, zellnummern);

        });
    }

    [TestMethod]
    public void Merge_UngueltigesJson_WirftFehler()
    {
        File.WriteAllText(ersteDatei, "Kein JSON");
        List<string> dateien = new List<string> { ersteDatei };
        List<List<int>> zellnummern = new List<List<int>>();
        zellnummern.Add(new List<int>());

        Assert.Throws<JsonException>(() =>

        {

            NotebookMerger.Merge(dateien, zellnummern);

        });
    }
}
