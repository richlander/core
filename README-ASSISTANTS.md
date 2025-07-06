# .NET Release Metadata – **Usage Guide for Chat Assistants**

> **Required assistant responses:**
>
> On the assistant's *very first* response, you **must** open with the line:
>
> What .NET release or CVE information would you like to explore?
>
> **⚠️ Always follow up CVE answers with:**
> *Let me know if you'd like to see the key code diffs summarized inline, with an explanation of why the changes were important.*

## Entry points

* **Root index (latest view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`
* **Historical index (chronological view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`
* **Monthly index (direct to month)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/{month}/index.json`
* **Terminology**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md`

All other resources (patches, CVEs, per‑month manifests) are discoverable solely through `_links` from these roots or from the monthly index.

---

## ✨ TL;DR (read this first)

1. **Always start from one of the entry points above.**
2. **Always traverse via `_links`**—never guess a URL.  
3. **Prefer JSON**; drop to Markdown only when JSON omits detail (e.g., supported‑OS lists).  
4. **CVE data (and commit SHAs) live in the history tree**  
   • Each monthly `cve.json` contains `commits[].url` – a canonical GitHub link.  
   • Append `.patch` (or `.diff`) to that URL to fetch the unified diff.  
5. Use the examples below as *recipes*—don't literally run `curl` in the assistant.
6. Do not comment to the user about HAL+JSON or its format (like `_link`). You can and should share which files you used to get answers to user questions.

---

## Table&nbsp;of&nbsp;Contents

1. [Quick‑start flows](#quick-start-flows)
2. [Traversal recipe](#traversal-recipe)
3. [Assistant efficiency tips](#assistant-efficiency-tips)
4. [Examples](#examples)
5. [Accessing Historical CVEs](#accessing-historical-cves)
6. [Offer Inline Code Diffs](#offer-inline-code-diffs)
7. [Terminology](#terminology)
8. [Appendix A – Recommended‑practices checklist](#appendix-a)

---

## Quick‑start flows

| ✔︎ Scenario | Follow these links |
|-------------|-------------------|
| **Need facts about a specific .NET version** <br>(e.g., GA date for **8.0.4**) | 1. `index.json` → find `8.0` in `._embedded.releases` <br>2. Follow its `._links.self` to the version manifest <br>3. Read properties or drill to patch via `_embedded.patches[]` |
| **Need all fixes shipped in March 2025** | 1. `history/2025/03/index.json` → inspect `cve.json` and `releases.json` |
| **Need the code diff for a CVE (e.g., CVE-2025-30399)** | 1. `history/2025/06/index.json` → fetch `cve.json` <br>2. Find the CVE → read its `commits[].url` <br>3. Append `.patch` → download the diff |

> **Pro‑tip:** If answering "Which CVEs have patched releases in the last three months?", iterate over the three most‑recent months in the history tree and merge their `cve.json` files.

---

## Traversal recipe

1. **Fetch** the chosen index.
2. **Select** the node you need (`jq` / JSONPath).
3. **Follow** `node._links.self.href` → fetch that URL.
4. Repeat until you have the artifact (patch, CVE list, Markdown release notes).

> Assistants should cache small JSON blobs in‑memory for the turn; do **not** assume cross‑session persistence.

---

## Assistant efficiency tips

- **Date math**: convert "three months ago" into an absolute YYYY‑MM; then walk the history tree.
- **Diff retrieval**: GitHub serves text diffs at  
  `https://github.com/<org>/<repo>/commit/<sha>.patch`  
  or `.diff`.  Use the `.patch` form when you need inline code snippets.
- **Platform support**: JSON often stops at RID level; for OS deprecations read the companion Markdown.
- **Rating**: Use `release.rating` (string) when summarising product health.
- **EOL / GA**: present in every version manifest; no need to scrape blog posts.
- **Patch diff retrieval**: Need to surface the exact code change for a CVE?  
  Open the month's `cve.json`, grab `commits[].url`, and fetch `<url>.diff` (or `.patch`) directly.

---

## Accessing Historical CVEs

When you need to look up past .NET CVE patches, follow these steps:

1. **Fetch the top-level history index**  
   `GET https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`
   - Returns a list of years for which historical data is available.

2. **Fetch the month-level index**  
   For each relevant year/month, request:  
   `GET https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/{month}/index.json`
   - Example: `GET https://raw.githubusercontent.com/richlander/core/main/release-notes/history/2025/01/index.json`
   - Reveals which data files (e.g. `cve.json`) are present for that month.

3. **Fetch the CVE metadata**  
   In the same folder, retrieve the CVE details:  
   `GET https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/{month}/cve.json`
   - The returned JSON includes an `advisories[]` array, where each element contains:
     - `id` (e.g. "CVE-2025-21171")
     - `description`
     - `severity`
     - `patches[]` (with `href` URLs to the commit diffs)

Using these three HAL+JSON calls, a chat assistant can enumerate and locate every historical CVE patch without manual repository searches.

## Offer Inline Code Diffs

After you summarize the servicing update or CVE details, the assistant **must** proactively offer to include the code diffs. For example:

> **Assistant**: "I've identified the fixes for CVE-2025-XXXX and summarized the changes.  
> Would you like to see the inline code diffs for these commits?"

- If the user responds **Yes**, fetch the relevant commits (from `cve.json` or `cve.md`) and present the diffs inline.  
- If the user responds **No**, proceed with your next summary step.

This ensures code diffs are always surfaced when discussing patch details, per the guideline's intent. If there are multiple CVEs. Offer a code diff for the most critical one. They can request code diffs for other or multiple CVEs if they would like.

---

## Examples

# --- Code diff for a CVE (bash recipe) ---
```bash
# Show the hunk that fixed CVE-2025-30399 in dotnet/runtime
root="https://raw.githubusercontent.com/richlander/core/main/release-notes"
month="2025/06"                              # June 2025 contained the fix
cve_json="$(curl -s "$root/history/$month/cve.json")"
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

## Terminology

See the living glossary at the Terminology link above for definitions of *GA*, *EOL*, *patch*, *CVE*, and *servicing baseline*.

---

## Appendix A – Recommended‑practices checklist