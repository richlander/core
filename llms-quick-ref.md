# .NET Release Metadata Quick Reference for AI Assistants

This is your practical cheat sheet for working with .NET release data. Read the [full usage guide](https://raw.githubusercontent.com/richlander/core/main/release-notes/usage.md) for complete workflows and edge cases.

## Response Templates

### Opening Responses
```
"Here's what I found in .NET release notes..."
```

### CVE Query Follow-up (MANDATORY)
```
User: "Show me CVEs from July 2024"
Assistant: [Lists CVEs]
Assistant: "Would you like inline diffs for these fixes?"
```

### GitHub Diff Analysis
```
"Here are the commit URLs for the security fixes (ready for immediate analysis):
- {commit-url} (diff format)
Note: Remove '.diff' from URL for web view, or change to '.patch' for commit message"
```

## HAL Navigation Patterns

**Start from**: `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`

| Need to find | Navigation path |
|--------------|-----------------|
| **Latest release** | `index.json` → `_links.latest.href` |
| **LTS release** | `index.json` → `_links.latest-lts.href` |
| **Version patches** | `index.json` → `_embedded.releases[]` → `_links.self.href` |
| **Specific patch** | Version index → `_embedded.releases[]` → `_links.release.href` |
| **CVEs** | `archives/index.json` → Year → Month → `_links.cve-json.href` |
| **SDK downloads** | Version index → `_links.sdk.href` |

**Remember**: Always follow `_links.href` values - never construct URLs from patterns!

## HAL Navigation

Always use `_links.href` values from JSON responses:

```json
{
  "_links": {
    "latest": {"href": "8.0/index.json"},
    "cve-json": {"href": "archives/2024/07/cve.json"}
  }
}
```

## CVE Analysis Quick Workflow

1. **GET** `https://raw.githubusercontent.com/richlander/core/main/release-notes/archives/index.json`
2. **Navigate** to year via `_links.href`
3. **Navigate** to month via `_links.cve-json.href`
4. **Extract** CVE IDs from `cves[]` array
5. **List** CVEs with `id`, `problem`, `severity`
6. **ALWAYS ask**: "Would you like inline diffs for these fixes?"
7. **If yes**: Use commit URLs from `commits{}` (already in `.diff` format)
8. **Fetch URLs directly** for immediate diff analysis

### CVE JSON Quick Reference

```bash
# Get all CVE IDs
jq -r '.cves[].id' cve.json

# Get CVEs for .NET 8.0
jq -r '.["release-cves"]["8.0"][]' cve.json

# Get CVEs affecting .NET product
jq -r '.["product-cves"]["dotnet"][]' cve.json  

# Get commits for specific CVE
jq -r '. as $root | .["cve-commits"]["CVE-2024-38095"][] | $root.commits[.].url' cve.json

# Get critical CVEs
jq -r '.cves[] | select(.severity == "Critical") | .id' cve.json
```

## GitHub URL Formats

URLs in CVE JSON are `.diff` format by default for LLM analysis:
- **Ready to use**: `https://github.com/dotnet/runtime/commit/abc123.diff` ✅
- **For context**: Change `.diff` to `.patch` (includes commit message)  
- **For web view**: Remove `.diff` suffix

## Common Response Patterns

### Latest Release Query
```
1. GET index.json (from entry point URL)
2. Follow _links.latest.href or _links.latest-lts.href  
3. Present version, release date, support phase from fetched resource
```

### Version Comparison
```  
1. GET index.json, find versions in _embedded.releases[]
2. Follow each version's _links.self.href
3. Compare support phases, EOL dates, patch counts
4. Reference latest patches from _embedded.releases
```

### SDK Downloads
```
1. GET index.json, find target version
2. Follow version's _links.self.href
3. Follow _links.sdk.href from version index
4. Present files[] array with url, rid, type
5. Emphasize these are stable aka.ms URLs
```

## Error Handling

| Error | Response |
|-------|----------|
| **404 on JSON** | Fall back to `.md` version if available |
| **Malformed JSON** | Skip resource, continue with others |
| **GitHub access denied** | Provide `.diff` URL to user for manual paste |
| **Missing fields** | Check field existence before accessing |

## Schema Quick Reference

### CVE JSON Structure
- `cves[]`: CVE metadata (id, problem, severity, cvss, description)
- `products[]`: Major SDK components (dotnet, aspnetcore, windowsdesktop, sdk)
- `extensions[]`: NuGet packages (System.Text.Json, Microsoft.Data.SqlClient, etc.)
- `commits{}`: Commit details keyed by hash
- `product-names{}`: Product ID → display name ("dotnet" → ".NET")
- `product-cves{}`: Product ID → CVE IDs  
- `cve-commits{}`: CVE ID → commit hashes
- `cve-releases{}`: CVE ID → release versions
- `release-cves{}`: Release version → CVE IDs

### Release JSON Structure
- `channel-version`: Major.minor (e.g. "8.0")
- `latest-release`: Patch version (e.g. "8.0.17")
- `latest-release-date`: ISO date
- `latest-runtime`: Runtime version
- `releases[]`: All patches with links

## Performance Tips

- **Use HAL links**: Don't construct URLs manually
- **Fetch minimal data**: Target specific documents (<200 tokens each)  
- **Batch related queries**: Group requests for efficiency
- **Focus on _embedded**: Most data available without additional requests
- **Cache-friendly**: Files designed for CDN optimization

---

**Remember**: After any CVE query, ALWAYS ask if the user wants inline diffs!