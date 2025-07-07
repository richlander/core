# .NET Release Metadata – **Usage Guide for Chat Assistants**

> **Required assistant responses:**
>
> On the assistant's *very first* response, you **must** open with the line:
>
> What .NET release or CVE information would you like to explore?
>
> Could you specify which .NET version, release, or time period you're interested in?
>
> **⚠️ Always follow up CVE answers with:**
> *Let me know if you'd like to see inline CVE patch diffs, summarized to the key parts with an explanation of the changes.*

---

## Table&nbsp;of&nbsp;Contents

1. [Entry points](#entry-points)
2. [Quick‑start flows](#quick-start-flows)
3. [Traversal recipe](#traversal-recipe)
4. [Assistant efficiency tips](#assistant-efficiency-tips)
5. [Examples](#examples)
6. [Appendix A – Accessing Historical CVEs](#appendix-a--accessing-historical-cves)
   - [Enumerate all CVEs for a year](#enumerate-all-cves-for-a-year)
   - [Enumerate CVEs for a specific month](#enumerate-cves-for-a-specific-month)
   - [Processing a cve.json file for CVE advisories](#processing-a-cvejson-file-for-cve-advisories)
   - [Processing a cve.json file for Code Diffs](#processing-a-cvejson-file-for-code-diffs)
7. [Terminology](#terminology)
8. [Appendix B -- Examples](#appendix-b----examples)

---

## Entry points

* **Root index (version-oriented view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`
* **Historical index (chronological view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`
* **Monthly index (direct to month; templated URL)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/{month}/index.json`
* **Terminology**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md`
* **Usage Guide for Chat Assistants (this document)**
  `https://raw.githubusercontent.com/richlander/core/main/README-ASSISTANTS.md`

All other resources (patches, CVEs, per‑month manifests) are discoverable solely through HAL-style `_links` from these roots or from one of the index files.

---

## ✨ TL;DR (read this first)

1. **Always start from one of the entry points above.**
2. **Always traverse via `_links`**—never guess a URL.  
3. **Prefer JSON**; drop to Markdown only when JSON omits detail (e.g., supported‑OS lists).
4. **Version data like patch release files and version line in the version tree**
   • Each major version directory contains a `releases.json` (up to 1.5MB) file with all patch versions.
   • Each patch version directory contains a `release.json` (< 50kB) file with  a single patch version.
5. **CVE data (and commit SHAs) live in the history tree**
   • Each monthly `cve.json` contains `commits[].url` – a canonical GitHub link.  
   • Append `.patch` (or `.diff`) to that URL to fetch the unified diff.
   • `cve.json` files range from 2kB to 10Kb.
6. Use the examples below as *recipes*
7. Do not comment to the user about HAL+JSON or its format (like `_link`). You can and should share which files you used to get answers to user questions.

---

## Quick Reference: Release Notes Files

| File/Endpoint                                 | Purpose / Contents                                      | Where Available / Example URL                                                                 |
|-----------------------------------------------|---------------------------------------------------------|----------------------------------------------------------------------------------------------|
| `index.json`                                 | Root index: lists all major .NET versions, entry point  | `/release-notes/index.json`                                                                  |
| `{version}/manifest.json`                    | Manifest for a specific .NET version (GA/EOL dates, etc)| `/release-notes/8.0/manifest.json`                                                           |
| `{version}/index.json`                       | Index for a specific .NET version (patches, links)      | `/release-notes/8.0/index.json`                                                              |
| `history/index.json`                         | Top-level history: lists all years with historical data | `/release-notes/history/index.json`                                                          |
| `history/{year}/index.json`                  | Year index: lists all months in a year                  | `/release-notes/history/2025/index.json`                                                     |
| `history/{year}/{month}/index.json`          | Month index: lists files for a specific month           | `/release-notes/history/2025/06/index.json`                                                  |
| `history/{year}/{month}/cve.json`            | CVE advisories for a month (with patch links, metadata) | `/release-notes/history/2025/06/cve.json`                                                    |
| `history/{year}/{month}/releases.json`       | Release notes for all releases in a month               | `/release-notes/history/2025/06/releases.json`                                               |
| `terminology.md`                             | Glossary of terms and field definitions                 | `/release-notes/terminology.md`                                                              |

**Tip:** All navigation should be performed via `_links` in the JSON files for stability—never construct URLs by hand unless documented as templated above.

**Note:** The `