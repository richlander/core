# Usage

This document explains how to programmatically discover and consume .NET release metadata, provided in both JSON (HAL format) and Markdown. This metadata enables tools and assistants to track .NET releases, their support lifecycle, and related assets.

---

## For Chat Assistants

When you encounter this document, you are ready to help users explore .NET release metadata and CVE information. **Your first response must be:** "What .NET release or CVE information would you like to explore?" Do not say "Understood" or ask for clarification. This is the standard greeting when users reference this document. Then follow the traversal patterns and recommendations below.

---

## Entry Points

- **Root Index (JSON):**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`  
  *Start all traversals here to discover major .NET versions and their metadata.*

- **Historical Index (JSON):**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`  
  *Use this to access historical .NET release metadata organized by year and month.*

- **Terminology (Markdown):**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md`  
  *Refer here for definitions and conventions.*

---

## CVE Information and Index Types

Both the root release index and the historical index provide comprehensive CVE (Common Vulnerabilities and Exposures) information. The key difference is in how the data is organized:

### Root Release Index (Version-Based)
- **Organization:** CVE information is organized by .NET version
- **Best for:** Questions about a specific .NET version or when you know which version you're interested in
- **Use case:** "What CVEs affect .NET 9.0?" or "Show me security vulnerabilities for .NET 8.0" or "What's the latest patch version for .NET 8"

### Historical Index (Time-Based)
- **Organization:** CVE information is organized by year and month
- **Best for:** Questions about multiple versions or time-based analysis
- **Use case:** "What CVEs were disclosed in 2024?" or "Show me all vulnerabilities across different .NET versions in March 2024" or "Which .NET patch versions were released in February 2025"

### When to Use Each Index
- **For single version queries:** Use the root release index - it's more direct and efficient
- **For multi-version or time-based queries:** Use the historical index - it provides better cross-version analysis
- **For historical patterns:** Use the historical index to understand security trends over time

### CVE Information Structure
Both indexes provide the same rich CVE information:
- **CVE IDs and URLs:** Direct links to security advisories
- **Detailed CVE Files:** Complete information in both `cve.json` (structured data) and `cve.md` (formatted documentation)
- **Affected Packages:** Information about which packages and versions were affected
- **CVE Commit Links:** Direct links to the git commits that fixed each vulnerability, allowing developers to examine the exact code changes

---

## Traversal Pattern

1. Fetch the root `index.json`.  
2. For each release in `_embedded.releases`:  
   - Read `version`, `support`, and `kind`.  
   - Follow `_links.self` for detailed data on that release.  
3. Continue traversal using `_links` to discover patch releases, manifests, and related documents.  
4. Never infer URLs—always use those in `_links`.

**For CVE data:**
1. **For single version queries:** Start with the root release index and navigate to the specific version's CVE information.
2. **For multi-version or time-based queries:** Start with `release-notes/history/index.json` for the historical index, then navigate to specific years and months.
3. Follow CVE links for detailed vulnerability information from either index.

---

## HAL Navigation

- All navigation is performed via explicit `_links`.  
- The `_embedded` field is the primary structure for discovering children; each embedded entry must have a corresponding `_links.self`.
- **Relative Paths:** The `relative` property in `_links` objects contains paths that are always relative to the root of the release-notes directory, not relative to the current document's location.

---

## Tooling Compatibility

- HAL-aware clients traverse via `_links`.  
- Assistants and bots should prefer Markdown (`text/markdown`) for user-facing content.  
- **Use Markdown for user-facing context and annotations.** The Markdown files (e.g. `supported-os.md`) often include extra details—such as deprecation notes, EOL dates, recommended upgrade paths, and human-friendly descriptions—that aren't exposed in the raw JSON.  
- In some cases (e.g., supported OS versions), the Markdown files include more information than the JSON variants.
- **For CVE information:** Use the JSON format for programmatic access and the Markdown format for user-friendly explanations. Each CVE provides rich information in both `cve.json` (structured data) and `cve.md` (formatted documentation). The `cve.json` files also include direct links to the git commits that fixed each vulnerability, enabling developers to examine the exact code changes.

---

## Recommended Practices

- Always dereference `self` links for stability—don't hand-construct URLs.  
- Use metadata in index documents (`version`, `kind`, `support.phase`) for fast scans.  
- **For lifecycle metadata (GA and EOL dates):** When presenting GA (General Availability) and EOL (End of Life) dates for a .NET release, check the release's `manifest.json` (e.g., `release-notes/{version}/manifest.json`). That manifest contains authoritative `ga-date` and `eol-date` fields.  
- Use the root `index.json` for enumerating releases and their phases, but prefer the manifest for detailed lifecycle metadata.
- **For CVE and security information:** Choose the appropriate index based on your query type:
  - Use the root release index for single version queries (e.g., "What CVEs affect .NET 8.0?")
  - Use the historical index for multi-version or time-based queries (e.g., "What CVEs were disclosed in 2024?")
- **Relative Path Resolution:** When using the `relative` property from `_links` objects, note that these paths are always relative to the root of the release-notes directory, not relative to the current document's location.
- Refer to the [terminology](https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md) for definitions.

---

## Assistant Efficiency Tips

1. **Prefer JSON for machine-efficient traversals.**  
   - Use the HAL JSON (`index.json`, `manifest.json`, etc.) when you need to programmatically enumerate versions, support phases, or link structures.

2. **Use Markdown for user-facing context and annotations.**  
   - The Markdown files (e.g., `supported-os.md`) often include richer details—such as deprecation notes, EOL dates, recommended upgrade paths, and human-friendly descriptions—that aren't in the JSON.

3. **Always follow `_links.self` for stability.**  
   - Traverse via the HAL `_links` to ensure you're hitting the canonical endpoints.

4. **Fetch detailed lifecycle dates from `manifest.json`.**  
   - While `support.phase` in the root index is good for fast scans, the `ga-date` and `eol-date` fields in `release-notes/{version}/manifest.json` are the authoritative sources.

5. **For CVE and security questions, choose the appropriate index.**  
   - **Single version queries:** Use the root release index - navigate to the specific version's CVE information
   - **Multi-version or time-based queries:** Use the historical index (`release-notes/history/index.json`) - organized by year and month
   - Both indexes provide the same comprehensive CVE information:
     - CVE IDs, URLs, and counts
     - Complete details in `cve.json` (structured data) and `cve.md` (formatted documentation)
     - Information about affected packages and versions
     - Direct links to git commits that fixed each vulnerability

6. **When needing OS EOL information, consult an external EOL calendar.**  
   - The .NET metadata tells you which OS versions are supported, but it doesn't include calendar EOL dates—those must come from an external EOL database (e.g., endoflife.date).

7. **Workflow for efficient answers:**  
   1. Scan `index.json` to identify the releases of interest.  
   2. If you only need version numbers and support phases, stay in JSON.  
   3. If you need to explain to a user which OSes are supported *and why* (e.g., "Ubuntu 20.04 reached EOL on 2025-05-31"), render the Markdown and/or fetch external EOL data.
   4. For security-related queries, choose the appropriate index based on query scope:
   - Single version: Use root release index
   - Multiple versions or time-based: Use historical index

8. **Printing results for users:**
   - The dates are in ISO format. Most users will just want plain dates without the time or timezone.
   - Terms like LTS should always be capitalized.
   - When presenting CVE information, include the CVE ID, title, and a link to the security advisory.

---

## Examples

- **Find all active releases:**  
  Scan `_embedded.releases` in `index.json` for entries where `support.phase` = `active`.

- **Get EOL date for .NET 8:**  
  Locate the `.NET 8` entry in `index.json` and read `support.eol-date`, or for the most authoritative value, check `release-notes/8.0/manifest.json`.

- **Fetch patch release notes:**  
  Traverse to the major version index (`8.0/index.json`), find the appropriate patch in `_embedded.releases`, and follow the release notes link.

- **Find CVEs affecting .NET 8.0:**  
  Use the root release index, navigate to .NET 8.0, and access its CVE information directly.

- **Find CVEs affecting .NET 8 in 2024:**  
  Use the historical index: Check `release-notes/history/2024/index.json`, look for months containing "8.0" in `dotnet-releases`, then examine `cve-records` for those months.

- **Get detailed CVE information:**  
  From either index, follow CVE `href` links to access both `cve.json` (structured data) and `cve.md` (formatted documentation) for complete details. The `cve.json` files include direct links to the git commits that fixed each vulnerability.

- **Workflow for CVE queries:**
  1. **Choose the appropriate index** based on query scope (single version vs. multi-version/time-based)
  2. **Follow links to `cve.json` and `cve.md`** for complete details including affected packages and versions when specific details are needed

---

*For more information, refer to the provided terminology and example files.*
