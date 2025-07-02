# Usage

This document describes how to consume .NET release metadata in JSON and markdown format. The JSON documents follows the [HAL (Hypertext Application Language) format](https://datatracker.ietf.org/doc/html/draft-kelly-json-hal). These HAL-style .NET release documents can be used to determine the General Availability (GA) dates, End-of-life (EOL) dates, and latest available patch releases for .NET major versions, for example.

## Assistant Entry Point

Assistants and automation tools should use the following JSON file as the root index for .NET release and usage data:

**Entry Point URL (JSON):**  
https://raw.githubusercontent.com/dotnet/core/hal-index/main/index.json

_This file contains machine-readable metadata for programmatic access._

**Terminology URL (markdown):**
https://raw.githubusercontent.com/dotnet/core/hal-index/release-notes/terminology.md

## Traversal Pattern

1. Start at the root index.json (`kind: index`).
2. Iterate over `_embedded.releases`.
3. For each entry:
   - Read `version`, `support`, and `kind`.
   - Follow `_links.self` to get the index or release list for that version.
4. Continue traversal using `_links` to discover related documents (`releases.json`, `manifest.json`, etc).

## HAL Interpretation Notes

- All links are explicit in `_links`. Do not infer paths.
- If a field is not found, check the linked document.

While the HAL specification treats _embedded as an optional optimization (embedding linked resources to avoid additional requests), this system uses _embedded to expose critical structured data—such as versioned indexes or manifests—that may not be linked elsewhere.

In this context, _embedded is not merely an optimization; it is the primary discovery mechanism for structured children. Each embedded entry must include a corresponding `_links.self` reference to enable reliable traversal and efficient caching.

## Tooling Compatibility

- HAL-aware clients can use `_links` to traverse.
- Chat assistants should prefer `text/markdown` links where applicable.
- Files are structured such that the most critical data is immutable or very slow to change.
- As a result, it is safe to cache CDN-served files.

## Recommended Practices

- Follow `self` links to ensure stability.
- Rely on metadata within index documents—such as `version`, `kind`, and `support`. These fields are stable and can be relied on for fast scanning without dereferencing links.
- Refer to `terminology.md` for definitions and structural conventions.
