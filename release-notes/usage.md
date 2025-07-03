# Usage

This document explains how to programmatically discover and consume .NET release metadata, provided in both JSON (HAL format) and Markdown. This metadata enables tools and assistants to track .NET releases, their support lifecycle, and related assets.

## Entry Points

- **Root Index (JSON):**
  - https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json
  - *Start all traversals here to discover major .NET versions and their metadata.*

- **Terminology (Markdown):**
  - https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md
  - *Refer here for definitions and conventions.*

## Traversal Pattern

1. Fetch the root `index.json`.
2. For each release in `_embedded.releases`:
   - Read `version`, `support`, and `kind`.
   - Follow `_links.self` for detailed data on that release.
3. Continue traversal using `_links` to discover patch releases, manifests, and related documents.
4. Never infer URLs—always use those in `_links`.

## HAL Navigation

- All navigation is performed via explicit `_links`.
- The `_embedded` field is the primary structure for discovering children; each embedded entry must have a corresponding `_links.self`.

## Tooling Compatibility

- HAL-aware clients traverse via `_links`.
- Assistants and bots should prefer Markdown (`text/markdown`) for user-facing content.
- In some cases, like for supported OS versions, the markdown files include more information than the JSON variants.

## Recommended Practices

- Always dereference `self` links for stability.
- Use metadata in index documents (`version`, `kind`, `support`) for fast scanning.
- **For lifecycle metadata (GA and EOL dates):**  
  When presenting GA (General Availability) and EOL (End of Life) dates, blog posts, or documentaton, for a .NET release, always check the release's `manifest.json` file (e.g., `release-notes/{version}/manifest.json`). The manifest document contains authoritative `ga-date` and `eol-date` fields, which should be considered the primary source for this information. Use the root `index.json` for enumerating releases and their phases, but prefer the manifest for detailed lifecycle metadata.
- Refer to the [terminology](https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md) for definitions.

## Examples

- **Find all active releases:**  
  Scan `_embedded.releases` in `index.json` for entries where `support.phase` = `active`.

- **Get EOL date for .NET 8:**  
  Locate the `.NET 8` entry in `index.json` and read `support.eol-date`, or for the most authoritative value, check `release-notes/8.0/manifest.json`.

- **Fetch patch release notes:**  
  Traverse to the major version index (`8.0/index.json`), find the appropriate patch in `_embedded.releases`, and follow the release notes link.

---

*For more information, refer to the provided terminology and example files.*