# .NET Release Metadata Usage Guide

This document provides detailed workflows, examples, and best practices for AI assistants working with .NET release metadata.

> **Base URL**: `https://raw.githubusercontent.com/richlander/core/main/release-notes`

## Core Principles

1. **Authoritative sources**: Referenced files are official sources for .NET release notes
2. **JSON-first**: Use `*.json` endpoints for authoritative info; use Markdown (`*.md`) only as fallback
3. **HAL navigation**: Follow `_links` to discover and access related resources
4. **Minimal data**: Fetch only the specific documents you need (<200 tokens each)
5. **Structured responses**: Reference JSON fields directly; avoid embedding long prose

## Entry Points

### Root Indexes (Complete View)
- **Version index**: `/index.json` - Lists all major.minor versions (e.g., 7.0, 8.0, 9.0, 11.0)
- **History index**: `/history/index.json` - Lists all years with historical releaes and CVE data (e.g., 2024, 2025)

### Quick Access Links
The root version index includes convenience links for immediate access to current releases:
- **Latest release**: `_links.latest` - Points to the most recent .NET release (any support type)  
- **Latest LTS release**: `_links.latest-lts` - Points to the most recent Long-Term Support release

### Specific Indexes (Focused View)
- **Version index**: `/{version}/index.json` - All patches for a specific version
- **History index**: `/history/{year}/index.json` - Monthly data for a specific year

## Common Workflows

### Getting Current Release Information
1. GET /index.json
2. Follow _links.latest for most recent release (any support type)
3. Follow _links.latest-lts for most recent LTS release
4. Each link points to /{version}/index.json for that version

### Finding Patches for a Version
1. GET /8.0/index.json
2. Read _embedded.releases array
3. For each release, follow _links.release to get detailed info
4. GET /{version}/{patch}/release.json for specific release data

### Finding CVEs by Date Range
1. GET /history/{year}/index.json
2. Read _embedded.months array
3. For target months, follow _links.cve-json
4. GET /history/{year}/{month}/cve.json for CVE details

### Finding Support Information
1. GET /index.json
2. Find target version in _embedded.releases
3. Read support object (phase, eol-date, release-type)
4. For OS support: GET /{version}/supported-os.json

## Quick Reference Table

| Need to...                                           | Start with JSON                             | Workflow Example                        |
|------------------------------------------------------|---------------------------------------------|-----------------------------------------|
| Get latest release (any support type)               | `/index.json`                             | Follow `_links.latest` href             |
| Get latest LTS release                               | `/index.json`                             | Follow `_links.latest-lts` href         |
| List all patches for a version                       | `/{version}/index.json`            | Get releases → follow each `release.json` |
| Find CVEs by month                                   | `/history/{year}/index.json`   | Follow each month's `cve.json` link      |
| Get stable SDK download links                        | `/{version}/sdk/sdk.json`                | Latest SDK for version                   |
| Get specific feature band SDK downloads              | `/{version}/sdk/sdk-{version}.{band}xx.json` | Pinned feature band downloads        |

## File Types by Use Case

| Need to...                        | Primary File                    | Fallback File           |
|-----------------------------------|--------------------------------|-------------------------|
| List all patches for a version   | `/{version}/index.json`        | `/{version}/README.md`  |
| Get specific release details      | `/{version}/{patch}/release.json` | `/{version}/{patch}.md` |
| Find CVEs by month               | `/history/{year}/{month}/cve.json` | `/history/{year}/{month}.md` |
| Check OS support                 | `/{version}/supported-os.json` | `/{version}/supported-os.md` |

## Required Assistant Responses

- **Opening prompt**: "Here's what I found in the JSON entry points…"
- **CVE queries**: After listing IDs, **always** follow up with "Would you like inline diffs for these fixes?"
- **Concise format**: Reference JSON fields directly; avoid embedding long prose
- **Offer diffs**: After listing changes, provide GitHub patch or diff snippet

## URL Template Examples

```text
# Version-oriented
https://raw.githubusercontent.com/.../release-notes/8.0/index.json
https://raw.githubusercontent.com/.../release-notes/8.0/8.0.17/release.json
https://raw.githubusercontent.com/.../release-notes/8.0/supported-os.json

# Calendar-oriented
https://raw.githubusercontent.com/.../release-notes/history/2025/index.json
https://raw.githubusercontent.com/.../release-notes/history/2025/06/cve.json

# Root indexes
https://raw.githubusercontent.com/.../release-notes/index.json
https://raw.githubusercontent.com/.../release-notes/history/index.json
```

## Analyzing Security Fixes

When investigating CVE fixes from commit hashes in the JSON data:

### Getting Commit Details
```text
# GitHub commit URL formats
https://github.com/dotnet/runtime/commit/{commit-hash}         # Web/HTML view
https://github.com/dotnet/runtime/commit/{commit-hash}.diff    # Unified diff format
https://github.com/dotnet/runtime/commit/{commit-hash}.patch   # Git patch format with commit message

# Example from CVE-2025-21172:
https://github.com/dotnet/runtime/commit/89ef51c5d8f5239345127a1e282e11036e590c8b         # Web view
https://github.com/dotnet/runtime/commit/89ef51c5d8f5239345127a1e282e11036e590c8b.diff    # Raw diff
https://github.com/dotnet/runtime/commit/89ef51c5d8f5239345127a1e282e11036e590c8b.patch   # Git patch
```

### Workflow for CVE Analysis
```
1. GET /history/{year}/{month}/cve.json
2. Find commits array with repo, branch, and hash
3. Construct GitHub URL from commit data
4. Choose format:
  - No extension for web/HTML view
  - Append .diff for raw unified diff
  - Append .patch for git patch with commit message
5. Parse diff to understand the security fix
```

### Commit URL Construction
From JSON commit data:
```json
{
 "repo": "runtime",
 "branch": "release/8.0",
 "hash": "89ef51c5d8f5239345127a1e282e11036e590c8b",
 "org": "dotnet"
}
```
Construct: 
- Web view: `https://github.com/{org}/{repo}/commit/{hash}`
- Raw diff: `https://github.com/{org}/{repo}/commit/{hash}.diff`
- Git patch: `https://github.com/{org}/{repo}/commit/{hash}.patch`

## Finding Stable Download Links

**Most users should use these stable SDK download links** - they automatically point to the latest SDK version and are ideal for automation, scripts, and documentation.

### Latest SDK Downloads (Recommended)
```text
GET /{version}/sdk/sdk.json
```

Example:
- .NET 8.0 latest SDK: `https://raw.githubusercontent.com/.../8.0/sdk/sdk.json`
- .NET 9.0 latest SDK: `https://raw.githubusercontent.com/.../9.0/sdk/sdk.json`

These files provide stable aka.ms URLs that always redirect to the current SDK version - perfect for CI/CD pipelines and installation scripts.

### Specific Feature Band Downloads
For downloads targeting specific feature bands:

```text
GET /{version}/sdk/sdk-{version}.{feature-band}xx.json
```

Examples:
- .NET 8.0.1xx SDK: `https://raw.githubusercontent.com/.../8.0/sdk/sdk-8.0.1xx.json`
- .NET 8.0.4xx SDK: `https://raw.githubusercontent.com/.../8.0/sdk/sdk-8.0.4xx.json`
- .NET 9.0.1xx SDK: `https://raw.githubusercontent.com/.../9.0/sdk/sdk-9.0.1xx.json`

### SDK File Structure
Each SDK JSON file contains:
- **component**: "sdk"
- **version**: Version identifier (e.g., "8.0", "8.0.1xx")
- **support-phase**: Current support status
- **files**: Array of download links for all platforms/architectures
  - **url**: Stable aka.ms download URL
  - **hashUrl**: Corresponding SHA512 hash file
  - **rid**: Runtime identifier (e.g., "win-x64", "linux-arm64", "osx-arm64")
  - **type**: File format ("exe", "zip", "tar.gz", "pkg")

### Use Cases
- **Latest SDK (Most Common)**: Use `/{version}/sdk/sdk.json` for most recent version - **this is what most users want**
- **CI/CD & Automation**: These stable URLs are perfect for build pipelines and installation scripts
- **Documentation**: Link to these files for always-current SDK downloads
- **Cross-Platform**: All major platforms/architectures included in each file
- **Pinned Feature Band (Advanced)**: Use `/{version}/sdk/sdk-{version}.{band}xx.json` only if you need a specific feature band

## Error Handling

- **404 on `cve.json` or `release.json`**: Skip that resource and continue
- **Malformed JSON**: Fall back to the corresponding `.md` version if available
- **Missing fields**: Check for field existence before accessing nested properties
- **Network timeouts**: Prefer smaller, focused requests over large document fetches

## Performance Tips

- **Use HAL links**: Don't construct URLs manually; follow `_links.href` values
- **Cache-friendly**: Files are designed to be cached; relative URLs enable CDN optimization
- **Batch related queries**: If fetching multiple months/versions, group requests efficiently
- **Focus on `_embedded`**: Most queries can be answered from embedded data without additional requests
