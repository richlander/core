# Claude Development Notes

## System.Text.Json with NAOT (Native AOT) Compilation

When working with System.Text.Json in .NET projects with Native AOT compilation, use **source generation** for optimal performance and compatibility.

### Preferred Approach: Source Generation with JsonSerializerContext

Define a partial class that inherits from `JsonSerializerContext` and use attributes to configure serialization:

```csharp
using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(
    WriteIndented = true,
    PropertyNamingPolicy = JsonKnownNamingPolicy.KebabCaseLower,
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping)]
[JsonSerializable(typeof(JsonNode))]
[JsonSerializable(typeof(JsonObject))]
[JsonSerializable(typeof(JsonArray))]
[JsonSerializable(typeof(CveRecords))]
public partial class CveJsonSerializerContext : JsonSerializerContext
{
}
```

### Usage

```csharp
// Serialize
var jsonString = JsonSerializer.Serialize(jsonDoc, CveJsonSerializerContext.Default.JsonNode);

// Deserialize 
var cveRecords = JsonSerializer.Deserialize(jsonString, CveJsonSerializerContext.Default.CveRecords);
```

### Benefits
- ✅ Full NAOT compatibility
- ✅ No reflection at runtime
- ✅ Better performance
- ✅ Smaller binary size
- ✅ No runtime warnings

### Reference Implementation
See `/Users/rich/git/distroessed/src/DotnetRelease/DataModel/Other/CveInfoSerializationContext.cs` for the canonical example used in the .NET release tooling.

## CVE JSON Property Ordering

CVE JSON files must follow this exact property order per the C# schema:

1. `id`
2. `title`  
3. `severity`
4. `cvss`
5. `description` (optional)
6. `mitigation` (optional)
7. `product` (optional)
8. `platforms` (optional)
9. `references`

Use the `fix-cve-order-naot.cs` script to automatically reorder properties in all CVE JSON files using NAOT-compatible source generation.

## Git Commands for CVE Project

```bash
# Check current status
git status

# View recent commits
git log --oneline -5

# View changes in a specific commit
git show <commit-hash>
```

## File Locations

- **CVE JSON files**: `/release-notes/history/YYYY/MM/cve.json`
- **CVE Markdown files**: `/release-notes/{6.0,7.0,8.0,9.0}/cve.md`
- **Schema definition**: `/Users/rich/git/distroessed/src/DotnetRelease/DataModel/Other/CveRecords.cs`
- **Serialization context**: `/Users/rich/git/distroessed/src/DotnetRelease/DataModel/Other/CveInfoSerializationContext.cs`