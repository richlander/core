# .NET Release Metadata Terminology

This document defines terms and fields used in .NET release metadata HAL+JSON documents.

## Abbreviations

- **HAL**: Hypertext Application Language - JSON format for self-describing APIs
- **LTS**: Long-Term Support - Extended support lifecycle (typically 3+ years)
- **STS**: Standard-Term Support - Shorter support lifecycle (typically 18 months)
- **EOL**: End of Life - No longer supported
- **GA**: General Availability - Production-ready release
- **SDK**: Software Development Kit
- **API**: Application Programming Interface

## Document Types (`kind`)

- **`index`**: Main version index document
- **`release-history-index`**: Historical release overview 
- **`history-year-index`**: Releases organized by year
- **`releases`**: Collection of releases for a version
- **`manifest`**: High-level version metadata with contextual links

### Top-Level Fields
- **`description`**: Human-readable summary of document contents
- **`version`**: .NET version the document applies to (e.g., `8.0`, `6.0.1xx`)
- **`component`**: SDK/runtime focus specifier (e.g., `sdk`, `runtime`)
- **`title`**: Display name for the document or resource
- **`year`**: Calendar year for historical documents (e.g., `2025`)
- **`month`**: Calendar month for monthly history documents (e.g., `06`)

### Support Object
- **`release-type`**: Either `lts` or `sts`
- **`phase`**: Current support phase (see Support Phases above)
- **`eol-date`**: ISO 8601 timestamp when support ends
- **`ga-date`**: ISO 8601 timestamp when version became generally available

### Date Formats
All dates use ISO 8601 format with UTC timezone: `YYYY-MM-DDTHH:MM:SS+00:00`

## Support Phases

- **`preview`**: Pre-release version, not production-ready
- **`active`**: Fully supported with regular updates and fixes
- **`maintenance`**: Limited support, critical fixes only
- **`eol`**: End of life, no longer supported

## Release Types

- **`lts`**: Long-Term Support release with extended lifecycle
- **`sts`**: Standard-Term Support release with shorter lifecycle

## HAL Structure

### Links (`_links`)
- **`self`**: Current document URL
- **`index`**: Root index document
- **`releases`**: Releases list for major version
- **`release`**: Specific release document
- **`manifest`**: High-level metadata with contextual links
- **`download`**: Download page or direct download link
- **`release-notes`**: Release notes document
- **`whats-new`**: What's new documentation
- **`supported-os`**: Supported operating systems information
- **`cve-json`**: CVE (security vulnerability) information
- **`terminology-markdown`**: GitHub-rendered terminology documentation
- **`terminology-markdown-raw`**: Raw Markdown terminology documentation

### Link Properties
- **`href`**: Fully qualified URL
- **`relative`**: Path relative to root index
- **`title`**: Human-readable label (optional)
- **`type`**: Media type (e.g., `application/json`, `text/markdown`)

### Embeddings (`_embedded`)
- **`releases`**: Array of version descriptors with navigation links
- **`years`**: Array of year descriptors for historical organization (history documents only)
- **`months`**: Array of month descriptors within a year (year history documents only)

## Content Flavors

Different representations of the same resource:

- **Base name** (e.g., `supported-os`): JSON format, machine-readable
- **`-markdown` suffix**: GitHub-rendered Markdown, human-readable  
- **`-raw-markdown` suffix**: Raw Markdown source, for processing tools

Flavors are differentiated by link `type`: `application/json`, `text/markdown`, `text/html`

## Version Patterns

- **Major versions**: `8.0`, `9.0` (no patch number)
- **Patch versions**: `8.0.1`, `9.0.0` (includes patch number)  
- **SDK versions**: `8.0.4xx`, `9.0.1xx` (feature band notation)
- **Preview versions**: `9.0.0-preview.1` (pre-release identifier)

## Related Documentation

- **[Release Policies](https://raw.githubusercontent.com/dotnet/core/main/release-policies.md)**: Comprehensive .NET release and support policies, including detailed explanations of LTS/STS definitions, support phases, and timelines
