#!/usr/bin/env dotnet

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Encodings.Web;

// Product name mappings based on the official schema
var productNameMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
{
    { "dotnet-runtime-libraries", "dotnet-runtime" },
    { "microsoft.netcore.sdk", "dotnet-sdk" },
    { "dotnet-runtime-aspnetcore", "dotnet-aspnetcore" },
    { "microsoft.aspnetcore.app.runtime", "dotnet-aspnetcore" }
};

// Find all cve.json files
var cveFiles = Directory.GetFiles(
    "/Users/rich/git/core-rich/release-notes",
    "cve.json",
    SearchOption.AllDirectories
);

Console.WriteLine($"Found {cveFiles.Length} cve.json files");

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

int totalFilesChanged = 0;
int totalProductsFixed = 0;

foreach (var filePath in cveFiles)
{
    try
    {
        var jsonText = File.ReadAllText(filePath);
        var doc = JsonNode.Parse(jsonText);
        
        if (doc == null)
        {
            Console.WriteLine($"⚠️  Skipping {filePath} - couldn't parse JSON");
            continue;
        }

        var products = doc["products"]?.AsArray();
        if (products == null || products.Count == 0)
        {
            continue;
        }

        bool fileChanged = false;
        int productsFixedInFile = 0;

        foreach (var product in products)
        {
            if (product == null) continue;
            
            var nameNode = product["name"];
            if (nameNode == null) continue;
            
            var currentName = nameNode.GetValue<string>();
            
            if (productNameMappings.TryGetValue(currentName, out var correctName))
            {
                product["name"] = correctName;
                fileChanged = true;
                productsFixedInFile++;
                Console.WriteLine($"  {Path.GetFileName(Path.GetDirectoryName(filePath))}/{Path.GetFileName(filePath)}: {currentName} → {correctName}");
            }
        }

        if (fileChanged)
        {
            var updatedJson = doc.ToJsonString(jsonOptions);
            File.WriteAllText(filePath, updatedJson + "\n");
            totalFilesChanged++;
            totalProductsFixed += productsFixedInFile;
            Console.WriteLine($"✅ Fixed {productsFixedInFile} product(s) in {filePath}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error processing {filePath}: {ex.Message}");
    }
}

Console.WriteLine();
Console.WriteLine($"Summary:");
Console.WriteLine($"  Files changed: {totalFilesChanged}");
Console.WriteLine($"  Product names fixed: {totalProductsFixed}");
