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

---

# LLM Documentation Testing Framework Development

## Project Context
We developed a comprehensive C# framework for testing how effectively LLMs can navigate and use structured documentation. The framework was originally designed for .NET release documentation but built to be reusable for any documentation scheme.

## What We Built
Created **`llm-doc-tester`** - a complete C# project at `/Users/rich/git/llm-doc-tester/` containing:

### Project Structure
```
src/
├── Program.cs                    # Main CLI interface with --all-models, --output options
├── DotNetReleaseDocTester.csproj # .NET 10.0 project file
├── Models/
│   ├── TestModels.cs            # TestCase, TestCriteria, TestResult, TestResults, ScoreResult
│   └── ChatModels.cs            # ChatMessage, ChatResponse for OpenRouter API
└── Services/
    ├── OpenRouterClient.cs      # HTTP client for OpenRouter API integration
    ├── TestCaseLoader.cs        # 15 comprehensive test cases across 5 categories
    ├── TestFramework.cs         # Main test execution engine with system prompt
    ├── ResponseScorer.cs        # 5-criteria scoring: Accuracy, Completeness, HAL Usage, Source Citation, Format
    └── ReportGenerator.cs       # Detailed markdown report generation
```

### Key Features
- **Multi-Model Testing**: OpenRouter API integration (Claude, GPT-4, Gemini)
- **15 Test Cases** across 5 categories:
  - Basic Info (3 tests): LTS versions, EOL dates, supported versions
  - CVE Analysis (3 tests): Date-based CVE retrieval, version-specific analysis, cross-version comparison
  - HAL Navigation (3 tests): Patch release navigation, platform-specific downloads, version indexes
  - Cross-Reference (3 tests): CVE impact analysis, commit history, version relationships
  - Error Handling (3 tests): Non-existent versions, future dates, unsupported platforms
- **5-Criteria Scoring System**: Each response scored 0-1.0 on accuracy, completeness, HAL usage, source citation, format
- **System Prompt**: Directs LLMs to use structured JSON data from GitHub raw URLs with HAL navigation

### Configuration
- **Environment**: `OPENROUTER_API_KEY` required
- **Usage**: `dotnet run [--all-models] [--output file.md]`
- **Target Framework**: .NET 10.0

## Current Status
✅ Complete framework implemented
✅ Moved to separate repository `/Users/rich/git/llm-doc-tester/`
✅ Updated to .NET 10.0
⏳ **Next Step**: Validate build in new directory (requires restarting Claude Code in `/Users/rich/git/llm-doc-tester/`)

## Design Philosophy
- **Reusable**: Easy to adapt for different documentation schemes
- **Comprehensive**: Tests navigation, accuracy, source citation, error handling
- **Measurable**: Quantitative scoring with detailed reports
- **API-First**: Uses production LLM APIs rather than web interfaces

## Customization Points
To adapt for new documentation:
1. Update test cases in `TestCaseLoader.cs`
2. Modify system prompt in `TestFramework.cs`
3. Adjust scoring criteria in `ResponseScorer.cs`
4. Update expected keywords/URLs for new domain

## Technical Notes
- Uses OpenAI-compatible API pattern
- HAL+JSON navigation testing built-in
- Async/await throughout for performance
- Comprehensive error handling and rate limiting
- Markdown report generation with recommendations

## Related Files in core-rich
- `llms.txt`: AI assistant discovery file with .NET release metadata entry points
- `CVE_PROJECT_SUMMARY.md`: Background on CVE JSON property ordering work
- Previous CVE ordering scripts: `fix-cve-order-final.cs`