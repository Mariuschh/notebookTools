using System.Text.Json;
using NotebookTools.Notebook;

namespace NotebookTools.Tests.NotebookTests;

[TestClass]
public class NotebookTests
{
    [TestMethod]
    public void Constructor_InitializesEmptyCollectionsAndDefaultVersions()
    {
        var notebook = new Notebooks();

        Assert.IsNotNull(notebook.Cells);
        Assert.HasCount(0, notebook.Cells);
        Assert.IsNotNull(notebook.Metadata);
        Assert.HasCount(0, notebook.Metadata);
        Assert.IsNotNull(notebook.AdditionalData);
        Assert.HasCount(0, notebook.AdditionalData);
        Assert.AreEqual(0, notebook.Format);
        Assert.AreEqual(0, notebook.MinorFormat);
    }

    [TestMethod]
    public void Constructor_CreatesIndependentCollectionsForEachInstance()
    {
        var first = new Notebooks();
        var second = new Notebooks();

        first.Cells.Add(new NotebookCell { Type = "code" });
        first.Metadata.Add("language", "python");
        first.AdditionalData.Add("custom", JsonSerializer.SerializeToElement(true));

        Assert.HasCount(0, second.Cells);
        Assert.HasCount(0, second.Metadata);
        Assert.HasCount(0, second.AdditionalData);
    }

    [TestMethod]
    public void Serialize_WritesNotebookPropertyNamesAndCellOrder()
    {
        var notebook = new Notebooks
        {
            Cells = new List<NotebookCell>
            {
                new() { Type = "markdown", Source = new List<string> { "# Title" } },
                new() { Type = "code", Source = new List<string> { "print(1)" } }
            },
            Metadata = new Dictionary<string, object> { ["language"] = "python" },
            Format = 4,
            MinorFormat = 5
        };

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(notebook));
        var root = document.RootElement;

        Assert.AreEqual(4, root.EnumerateObject().Count());
        Assert.AreEqual(4, root.GetProperty("nbformat").GetInt32());
        Assert.AreEqual(5, root.GetProperty("nbformat_minor").GetInt32());
        Assert.AreEqual("python", root.GetProperty("metadata").GetProperty("language").GetString());
        var cells = root.GetProperty("cells");
        Assert.AreEqual(2, cells.GetArrayLength());
        Assert.AreEqual("markdown", cells[0].GetProperty("cell_type").GetString());
        Assert.AreEqual("# Title", cells[0].GetProperty("source")[0].GetString());
        Assert.AreEqual("code", cells[1].GetProperty("cell_type").GetString());
        Assert.AreEqual("print(1)", cells[1].GetProperty("source")[0].GetString());
    }

    [TestMethod]
    public void Deserialize_ReadsNotebookPropertiesAndCellOrder()
    {
        const string json = """
            {
              "cells": [
                { "cell_type": "markdown", "source": ["# Title"] },
                { "cell_type": "code", "source": ["print(1)"] }
              ],
              "metadata": { "kernelspec": { "name": "python3" } },
              "nbformat": 4,
              "nbformat_minor": 5
            }
            """;

        var notebook = JsonSerializer.Deserialize<Notebooks>(json);

        Assert.IsNotNull(notebook);
        Assert.AreEqual(4, notebook.Format);
        Assert.AreEqual(5, notebook.MinorFormat);
        Assert.HasCount(2, notebook.Cells);
        Assert.AreEqual("markdown", notebook.Cells[0].Type);
        CollectionAssert.AreEqual(new[] { "# Title" }, notebook.Cells[0].Source);
        Assert.AreEqual("code", notebook.Cells[1].Type);
        CollectionAssert.AreEqual(new[] { "print(1)" }, notebook.Cells[1].Source);
        Assert.HasCount(1, notebook.Metadata);
        var kernel = (JsonElement)notebook.Metadata["kernelspec"];
        Assert.AreEqual("python3", kernel.GetProperty("name").GetString());
        Assert.HasCount(0, notebook.AdditionalData);
    }

    [TestMethod]
    public void Deserialize_MissingProperties_PreservesDefaults()
    {
        var notebook = JsonSerializer.Deserialize<Notebooks>("{}");

        Assert.IsNotNull(notebook);
        Assert.IsNotNull(notebook.Cells);
        Assert.HasCount(0, notebook.Cells);
        Assert.IsNotNull(notebook.Metadata);
        Assert.HasCount(0, notebook.Metadata);
        Assert.IsNotNull(notebook.AdditionalData);
        Assert.HasCount(0, notebook.AdditionalData);
        Assert.AreEqual(0, notebook.Format);
        Assert.AreEqual(0, notebook.MinorFormat);
    }

    [TestMethod]
    public void JsonRoundTrip_PreservesUnknownPropertiesAtRootLevel()
    {
        const string json = """
            {
              "cells": [],
              "metadata": {},
              "nbformat": 4,
              "nbformat_minor": 5,
              "custom": { "enabled": true, "tags": ["example", "test"] },
              "revision": 7,
              "description": "Example notebook",
              "optional": null
            }
            """;

        var notebook = JsonSerializer.Deserialize<Notebooks>(json);

        Assert.IsNotNull(notebook);
        Assert.HasCount(4, notebook.AdditionalData);
        Assert.IsTrue(notebook.AdditionalData["custom"].GetProperty("enabled").GetBoolean());
        Assert.AreEqual(7, notebook.AdditionalData["revision"].GetInt32());
        Assert.AreEqual("Example notebook", notebook.AdditionalData["description"].GetString());
        Assert.AreEqual(JsonValueKind.Null, notebook.AdditionalData["optional"].ValueKind);

        using var document = JsonDocument.Parse(JsonSerializer.Serialize(notebook));
        var root = document.RootElement;

        Assert.AreEqual(8, root.EnumerateObject().Count());
        Assert.IsFalse(root.TryGetProperty("AdditionalData", out _));
        var custom = root.GetProperty("custom");
        Assert.IsTrue(custom.GetProperty("enabled").GetBoolean());
        var tags = custom.GetProperty("tags");
        Assert.AreEqual(2, tags.GetArrayLength());
        Assert.AreEqual("example", tags[0].GetString());
        Assert.AreEqual("test", tags[1].GetString());
        Assert.AreEqual(7, root.GetProperty("revision").GetInt32());
        Assert.AreEqual("Example notebook", root.GetProperty("description").GetString());
        Assert.AreEqual(JsonValueKind.Null, root.GetProperty("optional").ValueKind);
    }
}
