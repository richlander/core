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

### GitHub Access Failure
```
"I cannot fetch this diff URL directly. One of the following approaches may work:
- Copy/paste the URL: {diff-url}  
- Copy/paste the contents of the URL"
```

## URL Patterns Cheat Sheet

| Task | Pattern | Example |
|------|---------|---------|
| **All versions** | `/index.json` | Get latest, LTS info |
| **Version patches** | `/{version}/index.json` | `/8.0/index.json` |
| **Specific release** | `/{version}/{patch}/release.json` | `/8.0/8.0.17/release.json` |
| **CVEs by month** | `/archives/{year}/{month}/cve.json` | `/archives/2024/07/cve.json` |
| **OS support** | `/{version}/supported-os.json` | `/8.0/supported-os.json` |
| **SDK downloads** | `/{version}/sdk/sdk.json` | `/8.0/sdk/sdk.json` |

**Base URL**: `https://raw.githubusercontent.com/richlander/core/main/release-notes`

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

1. **GET** `/archives/{year}/{month}/cve.json`
2. **Extract** CVE IDs from `cves[]` array
3. **List** CVEs with `id`, `problem`, `severity`
4. **ALWAYS ask**: "Would you like inline diffs for these fixes?"
5. **If yes**: Get commit URLs from `commits{}` section
6. **Append** `.diff` (preferred) or `.patch` to GitHub URLs

### CVE JSON Quick Reference

```bash
# Get all CVE IDs
jq -r '.cves[].id' cve.json

# Get CVEs for .NET 8.0
jq -r '.["release-cves"]["8.0"][]' cve.json  

# Get commits for specific CVE
jq -r '. as $root | .["cve-commits"]["CVE-2024-38095"][] | $root.commits[.].url' cve.json

# Get critical CVEs
jq -r '.cves[] | select(.severity == "Critical") | .id' cve.json
```

## GitHub URL Formats

From commit hash `abc123...`:
- **Code analysis**: `https://github.com/dotnet/runtime/commit/abc123.diff` ✅ 
- **With context**: `https://github.com/dotnet/runtime/commit/abc123.patch`
- **Web view**: `https://github.com/dotnet/runtime/commit/abc123`

## Common Response Patterns

### Latest Release Query
```
1. GET /index.json
2. Follow _links.latest or _links.latest-lts  
3. Present version, release date, support phase
```

### Version Comparison
```  
1. GET /{version1}/index.json and /{version2}/index.json
2. Compare support phases, EOL dates, patch counts
3. Reference latest patches from _embedded.releases
```

### SDK Downloads
```
1. GET /{version}/sdk/sdk.json
2. Present files[] array with url, rid, type
3. Emphasize these are stable aka.ms URLs
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
- `core[]`: Affected runtime components  
- `extensions[]`: Affected packages
- `commits{}`: Commit details keyed by hash
- `cve-commits`: CVE ID → commit hashes
- `cve-releases`: CVE ID → release versions
- `release-cves`: Release version → CVE IDs

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