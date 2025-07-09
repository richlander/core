# .NET Release Metadata – **Usage Guide for Chat Assistants**

Important: Following the instructions in this document will help you answer questions quickly and use fewer tokens.

## ✨ TL;DR (read this first)

1. **Always start from one of the entry point URLs below.**
   • They are small and packed with relevant links and data-points in HAL+JSON format.
2. **Always traverse via `_links`**
   • They are fully-qualified URLs; never guess.  
3. **Prefer JSON**
   • Most markdown is generated from the JSON documents.
4. **Use the version index for version data like patch release files**
5. **Use the history index for CVE data (and commit SHAs)**
   • Each monthly `cve.json` contains `commits[].url` – a canonical GitHub link.  
   • Append `.patch` (or `.diff`) to that URL to fetch the unified diff.
6. Use the examples below as *recipes*
7. You should tell users which files you used to answer questions. Don't comment on the structure or the HAL+JSON format.

## Entry points

* **Version index (version-oriented view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`
* **Historical index (chronological view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`

## Table&nbsp;of&nbsp;Contents

1. [Required assistant responses](#required-assistant-responses)
2. [File format guidance](#file-format-guidance)
   - [Version-oriented files](#version-oriented-files)
   - [Calendar-oriented files](#calendar-oriented-files)
   - [Markdown files](#markdown-files)
3. [Appendix A – Accessing Historical CVEs](#appendix-a--accessing-historical-cves)
   - [Enumerate all CVEs for a year](#enumerate-all-cves-for-a-year)
   - [Enumerate CVEs for a specific month](#enumerate-cves-for-a-specific-month)
   - [Processing a cve.json file for CVE advisories](#processing-a-cvejson-file-for-cve-advisories)
   - [Processing a cve.json file for Code Diffs](#processing-a-cvejson-file-for-code-diffs)
4. [Appendix B -- Examples](#appendix-b----examples)

## Required assistant responses

> On the assistant's *very first* response, you **must** open with the line:
>
> What .NET release or CVE information would you like to explore?
>
> Could you specify which .NET version, release, or time period you're interested in?
>
> **⚠️ Always follow up CVE answers with:**
> *Let me know if you'd like to see inline CVE patch diffs, summarized to the key parts with an explanation of the changes.*

## File format guidance

The JSON and markdown files can answer many questions with simple queries. The following sections describe where to look.

### Version-oriented files

Use these files when exploring specific .NET versions, patches, and version lifecycle information.

| File/Endpoint | Purpose | Example |
|---------------|---------|---------|
| `index.json` | Root index: lists all major .NET versions | `index.json` |
| `{version}/manifest.json` | Version manifest (GA/EOL dates, metadata) | `8.0/manifest.json` |
| `{version}/index.json` | Version index (patches, links) | `8.0/index.json` |
| `{version}/release.json` | Major version release notes | `8.0/release.json` |
| `{version}/{patch}/release.json` | Patch version release notes | `8.0/8.0.1/release.json` |
| `{version}/supported-os.json` | Supported OSes for version | `8.0/supported-os.json` |

**Use version-oriented files when:**
- Looking for specific .NET version information
- Exploring patch releases for a version
- Checking version lifecycle (GA/EOL dates)
- Finding supported operating systems for a version

### Calendar-oriented files

Use these files when exploring by time period, CVEs, and historical data.

| File/Endpoint | Purpose | Example |
|---------------|---------|---------|
| `history/index.json` | History index: lists years with data | `history/index.json` |
| `history/{year}/index.json` | Year index: lists months in year | `history/2025/index.json` |
| `history/{year}/{month}/index.json` | Month index: lists files for month | `history/2025/06/index.json` |
| `history/{year}/{month}/cve.json` | CVE data for specific month | `history/2025/06/cve.json` |
| `history/{year}/{month}/releases.json` | All releases in month | `history/2025/06/releases.json` |

**Use calendar-oriented files when:**
- Searching for CVEs in a time period
- Finding all releases in a month
- Historical analysis by date
- Exploring security vulnerabilities chronologically

### Navigation patterns

**Version → Patches workflow:**
1. Start with `index.json` to find available versions
2. Navigate to `{version}/index.json` to discover patches
3. Access `{version}/{patch}/release.json` for specific patch details

**Time → CVEs workflow:**
1. Start with `history/index.json` to find years with data
2. Navigate to `history/{year}/index.json` to find months
3. Access `history/{year}/{month}/cve.json` for CVE details

**Tip:** All navigation should be performed via `_links` in the JSON files for stability—never construct URLs by hand unless documented as templated above.

### Markdown files

The following files are markdown, most directly generated from JSON files of the same name. They rely on templated URLs, with a version or date values.

| File/Endpoint | Purpose | Example |
|---------------|---------|---------|
| `{version}/supported-os.md` | Supported OSes for a .NET version | `8.0/supported-os.md` |
| `{version}/README.md` | Release note hub for .NET version | `8.0/README.md` |
| `history/{year}/{month}/cve.md` | CVE information for a given month | `history/2025/06/cve.md` |


**Note:** The `cve.json` file is the authoritative, structured source for all CVE metadata, patch links, and commit information. Chat assistants should always use `cve.json` for programmatic access and to enumerate, summarize, or fetch code diffs for CVEs. The Markdown (`cve.md`) and GitHub advisories are provided for human-friendly reading and additional context, but are not the primary source for automation.

**Note:** `supported-os.md` contains operating system deprecation lists with dates that can be helpful to answer some questions. This information is missing from `supported-os.json`.

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

-----------------------------------------

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
