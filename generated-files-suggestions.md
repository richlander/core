# Suggestions for Generated Files - SDK Downloads Discoverability

This document contains suggestions to improve discoverability of stable SDK download links in generated files.

## Problem
The `sdk.json` files provide stable download links but are not well advertised in the generated index files and discovery mechanisms.

## Suggested Changes for Generated Files

### 1. Main Index.json Enhancement
**File**: `/release-notes/index.json` (generated)

Add to the `_links` section after the existing help-markdown link:

```json
"stable-downloads": {
  "href": "https://raw.githubusercontent.com/richlander/core/main/release-notes/{version}/sdk/sdk.json",
  "template": true,
  "title": "Stable SDK download links (template: replace {version} with version number)",
  "type": "application/hal+json"
}
```

This provides a templated link that AI assistants and tools can use to construct SDK download URLs.

### 2. llms.txt Enhancement  
**File**: `/llms.txt` (generated)

Add this line to the SDK downloads section:

```
- [Stable SDK downloads](https://raw.githubusercontent.com/richlander/core/main/release-notes/8.0/sdk/sdk.json): Latest SDK download links for all platforms
```

This gives AI assistants a concrete example of how to access SDK downloads.

## Implementation Notes

- The main index.json addition uses a template pattern `{version}` that follows HAL conventions
- The llms.txt addition uses .NET 8.0 as a concrete example since it's the current LTS
- Both changes improve discoverability without breaking existing functionality
- The template approach in index.json allows for programmatic discovery of SDK downloads

## Benefits

- **Discoverability**: Direct links from main discovery files
- **Automation-Friendly**: Template URLs for programmatic access
- **Consistency**: Follows existing HAL patterns in index.json
- **Backward Compatible**: Additive changes only

## Files Affected

1. **index.json**: Add stable-downloads template link
2. **llms.txt**: Add concrete SDK download example

These are both generated files that would need updates in the generation tooling.