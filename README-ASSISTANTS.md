# .NET Release Metadata – **Usage Guide for Chat Assistants**

Important: Following the instructions in this document will help you save tokens and deliver results to common questions more quickly.

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

**Note:** The `cve.json` file is the authoritative, structured source for all CVE metadata, patch links, and commit information. Chat assistants should always use `cve.json` for programmatic access and to enumerate, summarize, or fetch code diffs for CVEs. The Markdown (`cve.md`) and GitHub advisories are provided for human-friendly reading and additional context, but are not the primary source for automation.

---

## Assistant efficiency tips

- **Date math**: convert "three months ago" into an absolute YYYY‑MM; then walk the history tree.
- **Patch diff retrieval**: Need to surface the exact code change for a CVE?  
  Open the month's `cve.json`, grab `commits[].url`, and fetch `<url>.diff` (or `.patch`) directly.
- **EOL / GA**: present in every version manifest; no need to scrape blog posts.
- **Rating**: Use `release.rating` (string) when summarising product health.

---

## Appendix A – Accessing Historical CVEs

**Note:** The `cve.json` file is the authoritative, structured source for all CVE metadata, patch links, and commit information. Chat assistants should always use `cve.json` for programmatic access and to enumerate, summarize, or fetch code diffs for CVEs. The Markdown (`cve.md`) and GitHub advisories are provided for human-friendly reading and additional context, but are not the primary source for automation.

CVEs can be enumerated using a few patterns, dependening on the needs. Pick the option that is likely to require downloading the fewest files.

### Enumerate all CVEs for a year
1. Fetch the year index:  
   `GET https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/index.json`
2. For each month, follow the `_links.cve` (or similar) to access that month's `cve.json`.
3. Process each `cve.json` as described below.
   - This approach is more efficient than iterating over all possible months, and ensures you only fetch months that actually have CVE data.

### Enumerate CVEs for a specific month
1. Fetch the month index:  
   `GET https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/{month}/index.json`
2. Follow the `_links.cve` (or similar) to access that month's `cve.json`.
3. Process the `cve.json` as described below.

### Processing a cve.json file for CVE advisories

The returned JSON includes:

-  A `records[]` array, where each element is a CVE disclosure that contains:
  - `id` (e.g. "CVE-2025-21171")
  - `title` (e.g. "NET Remote Code Execution Vulnerability")
  - `description`
  - `severity`
  - `references[]` (with `href` URLs CVE announcements)
- A `packages[]` array, where each element is an affected package that contains:
  - `name` (e.g. "System.Text.Json)
  - `affected[]` -- An array of affected package versions, which establishes a join on CVE (by ID) and commit (by SHA), containing:
  - `cve-id` -- Foreign key to `id` in `records[]` array objects
  - `min-vulnerable` -- Minimum affected/vulnerable version
  - `max-vulnerable` -- Maximum affected/vulnerable version
  - `fixed` -- Version with the CVE fix and therefore not vulnerable
  - `family` -- (optional) The major .NET version family for the package
  - `commits[]` -- (optional) The set of SHAs that were published across repos and branches to fix the vulnerability. The SHAs are a foreign key on the root-level `commits[]` property
- A `commits[]` array, where each element is a GitHub repo and branch specific commit that contains:
  - `repo`
  - `branch`
  - `hash`
  - `org`
  - `url` -- Full qualified GitHub URL to commit

After summarizing the CVEs, the assistant **must** offer:

> Let me know if you'd like to see inline CVE patch diffs, summarized to the key parts with an explanation of the changes.

If the user says **Yes**, fetch and present the diffs inline. If **No**, proceed with the next summary step.

### Processing a cve.json file for Code Diffs

Code diffs can be acquired with the following flow:

- Fetch the relevant `cve.json` file for the month.
- Enumerate `packages[]` to create a join between package and commit SHA
- Enumerate `commits[]` array to find the relevant commit SHAs
- For each commit, extract: `url`
- A path-style diff can be acquired by adding `.patch` or `.diff` to the URL

---

## Appendix B -- Examples

The following examples should how to download useful content from the various JSON files.

### Code diff for a CVE (bash recipe)

```bash
# Show the hunk that fixed CVE-2025-30399 in dotnet/runtime
root="https://raw.githubusercontent.com/richlander/core/main/release-notes/history"
month="2025/06"                              # June 2025 contained the fix
cve_json="$(curl -s "$root/$month/cve.json")"
sha=$(echo "$cve_json" | jq -r '.commits[] | select(.repo=="runtime") | .hash')
curl -sL "https://github.com/dotnet/runtime/commit/${sha}.patch" | sed -n '1,120p'
``` 
# -----------------------------------------

<details>
<summary>Version‑centric (8.0 → patches → CVEs)</summary>

```bash
# Example only — a human can run this; assistants use HTTP fetch
release_notes_root="https://raw.githubusercontent.com/richlander/core/main/release-notes"

# 1. Root index → ._embedded.releases[]
curl -s $release_notes_root/index.json | \
  jq -r '._embedded.releases[] | select(.version=="8.0") | ._links.self.href' \
  | while read manifest; do
     # 2. Version manifest → patches
     curl -s "$manifest" | jq -r '._embedded.releases[].version'
   done
```

</details>

<details>
<summary>Time‑centric (CVE feed for last 90 days)</summary>

```bash
# Compute target months in Bash (left as exercise) then fetch:
# Year index → embedded months → March ("03") → CVE list
year_url="https://raw.githubusercontent.com/richlander/core/main/release-notes/history/2025/index.json"

curl -s "$year_url" \
  | jq -r '._embedded.months[] | select(.month=="03") | ._links.cve.href' \
  | xargs curl -s \
  | jq '.items[]'
```

</details>

<details>
<summary>Code-diff for a CVE</summary>

```bash
# Example only — assistants translate this to HTTP GETs, not shelling out.
# Goal: raw diff for CVE-2025-30399
month_root="https://raw.githubusercontent.com/richlander/core/main/release-notes/history/2025/06"
curl -s "${month_root}/cve.json" \
 | jq -r '.commits[].url' \
 | while read url; do curl -s "${url}.diff"; done
```

*(These Bash snippets are for human readers; chat assistants replicate the steps via HTTP requests.)*

</details>

---
