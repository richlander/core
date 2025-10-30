#!/usr/bin/env dotnet

using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Encodings.Web;

// Find all cve.json files
var cveFiles = Directory.GetFiles(".", "cve.json", SearchOption.AllDirectories)
    .Where(f => f.Contains("release-notes/archives/"))
    .ToList();

Console.WriteLine($"Found {cveFiles.Count} CVE files to check");

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
};

int totalFixed = 0;

foreach (var file in cveFiles)
{
    var jsonText = File.ReadAllText(file);
    var doc = JsonNode.Parse(jsonText) as JsonObject;
    
    if (doc == null)
    {
        Console.WriteLine($"  ERROR: Could not parse {file}");
        continue;
    }
    
    int fileFixed = 0;
    
    // Fix products
    if (doc.ContainsKey("products") && doc["products"] is JsonArray products)
    {
        foreach (var product in products.Cast<JsonObject>())
        {
            if (product["commits"] == null)
            {
                product["commits"] = new JsonArray();
                fileFixed++;
            }
        }
    }
    
    // Fix packages
    if (doc.ContainsKey("packages") && doc["packages"] is JsonArray packages)
    {
        foreach (var package in packages.Cast<JsonObject>())
        {
            if (package["commits"] == null)
            {
                package["commits"] = new JsonArray();
                fileFixed++;
            }
        }
    }
    
    if (fileFixed > 0)
    {
        Console.WriteLine($"  {file}: Fixed {fileFixed} null commits");
        var newJson = doc.ToJsonString(options);
        File.WriteAllText(file, newJson + "\n");
        totalFixed += fileFixed;
    }
}

Console.WriteLine($"\nCompleted: Fixed {totalFixed} null commits across {cveFiles.Count} files");
