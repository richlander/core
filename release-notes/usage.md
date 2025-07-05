# Usage

This document explains how to programmatically discover and consume .NET release metadata, provided in both JSON (HAL format) and Markdown. This metadata enables tools and assistants to track .NET releases, their support lifecycle, and related assets.

---

## Entry Points

- **Root Index (JSON):**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`  
  *Start all traversals here to discover major .NET versions and their metadata.*

- **Historical Index (JSON):**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`  
  *Use this to access historical .NET release information and CVE (Common Vulnerabilities and Exposures) data organized by year and month.*

- **Terminology (Markdown):**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md`  
  *Refer here for definitions and conventions.*

---

## Historical Data and CVE Information

The historical index provides access to .NET release history and security vulnerability information organized by year and month.

### When to Use Historical Index
- **CVE Questions:** Finding security vulnerabilities affecting specific .NET versions
- **Historical Queries:** Understanding past release patterns and security issues
- **Timeline Analysis:** Correlating releases with security disclosures

### Structure
- Years 2016-2025 with monthly breakdowns
- CVE records include ID, title, and GitHub advisory links
- Cross-references between release versions and CVE disclosures

---

## Traversal Pattern

1. Fetch the root `index.json`.  
2. For each release in `_embedded.releases`:  
   - Read `version`, `support`, and `kind`.  
   - Follow `_links.self` for detailed data on that release.  
3. Continue traversal using `_links` to discover patch releases, manifests, and related documents.  
4. Never infer URLs—always use those in `_links`.

**For historical and CVE data:**
1. Start with `release-notes/history/index.json` for the historical index.
2. Navigate to specific years, then examine months with `cve-records` arrays.
3. Follow CVE links for detailed vulnerability information.

---

## HAL Navigation

- All navigation is performed via explicit `_links`.  
- The `_embedded` field is the primary structure for discovering children; each embedded entry must have a corresponding `_links.self`.

---

## Tooling Compatibility

- HAL-aware clients traverse via `_links`.  
- Assistants and bots should prefer Markdown (`text/markdown`) for user-facing content.  
- **Use Markdown for user-facing context and annotations.** The Markdown files (e.g. `supported-os.md`) often include extra details—such as deprecation notes, EOL dates, recommended upgrade paths, and human-friendly descriptions—that aren't exposed in the raw JSON.  
- In some cases (e.g., supported OS versions), the Markdown files include more information than the JSON variants.
- **For CVE information:** Use the JSON format for programmatic access and the Markdown format for user-friendly explanations.

---

## Recommended Practices

- Always dereference `self` links for stability—don't hand-construct URLs.  
- Use metadata in index documents (`version`, `kind`, `support.phase`) for fast scans.  
- **For lifecycle metadata (GA and EOL dates):** When presenting GA (General Availability) and EOL (End of Life) dates for a .NET release, check the release's `manifest.json` (e.g., `release-notes/{version}/manifest.json`). That manifest contains authoritative `ga-date` and `eol-date` fields.  
- Use the root `index.json` for enumerating releases and their phases, but prefer the manifest for detailed lifecycle metadata.
- **For CVE and security information:** Use the historical index to find vulnerabilities affecting specific .NET versions or time periods.
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

5. **For CVE and security questions, use the historical index.**  
   - The historical index (`release-notes/history/index.json`) contains comprehensive CVE information organized by year and month.
   - Use `cve-records` arrays to find vulnerabilities affecting specific .NET versions.
   - Follow CVE `_links` for detailed security advisory information.

6. **When needing OS EOL information, consult an external EOL calendar.**  
   - The .NET metadata tells you which OS versions are supported, but it doesn't include calendar EOL dates—those must come from an external EOL database (e.g., endoflife.date).

7. **Workflow for efficient answers:**  
   1. Scan `index.json` to identify the releases of interest.  
   2. If you only need version numbers and support phases, stay in JSON.  
   3. If you need to explain to a user which OSes are supported *and why* (e.g., "Ubuntu 20.04 reached EOL on 2025-05-31"), render the Markdown and/or fetch external EOL data.
   4. For security-related queries, check the historical index for CVE information.

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

- **Find CVEs affecting .NET 8 in 2024:**  
  Check `release-notes/history/2024/index.json`, look for months containing "8.0" in `dotnet-releases`, then examine `cve-records` for those months.

- **Get detailed CVE information:**  
  From monthly data, follow CVE `href` links to GitHub security advisories for complete details.

---

*For more information, refer to the provided terminology and example files.*
