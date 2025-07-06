# .NET Release Metadata – **Usage Guide for Chat Assistants**

> **⚠️ Required greeting**  
> On the assistant's *very first* response, you **must** open with the line:  
>
> ```text
> What .NET release or CVE information would you like to explore?
> ```
>
> Keep it exactly as‑is—no additional punctuation or context.

---

## ✨ TL;DR (read this first)

1. **Two entry points**:  
   • *Version‑centric*: `…/release-notes/index.json`  
   • *Time‑centric (history)*: `…/release-notes/history/index.json`
2. **Always traverse via `_links`**—never guess a URL.  
3. **Prefer JSON**; drop to Markdown only when JSON omits detail (e.g., supported‑OS lists).  
4. **CVE data (and commit SHAs) live in the history tree**  
   • Each monthly `cve.json` contains `commits[].url` – a canonical GitHub link.  
   • Append `.patch` (or `.diff`) to that URL to fetch the unified diff.  
5. Use the examples below as *recipes*—don't literally run `curl` in the assistant.

---

## Table&nbsp;of&nbsp;Contents

1. [Quick‑start flows](#quick-start-flows)
2. [Entry points](#entry-points)
3. [Traversal recipe](#traversal-recipe)
4. [Assistant efficiency tips](#assistant-efficiency-tips)
5. [Examples](#examples)
6. [Terminology](#terminology)
7. [Appendix A – Recommended‑practices checklist](#appendix-a)

---

## Quick‑start flows

| ✔︎ Scenario | Follow these links |
|-------------|-------------------|
| **Need facts about a specific .NET version** <br>(e.g., GA date for **8.0.4**) | 1. `index.json` → find `8.0` in `._embedded.releases` <br>2. Follow its `._links.self` to the version manifest <br>3. Read properties or drill to patch via `_embedded.patches[]` |
| **Need all fixes shipped in March 2025** | 1. `history/index.json` → find March 2025 node <br>2. Follow `._links.self` to that month <br>3. Inspect `cve.json` and `releases.json` |
| **Need the code diff for a CVE (e.g., CVE-2025-30399)** | 1. `history/index.json` → pick the month <br>2. `._links.cve` → fetch `cve.json` <br>3. Find the CVE → read its `commits[].url` <br>4. Append `.patch` → download the diff |

> **Pro‑tip:** If answering "Which CVEs have patched releases in the last three months?", iterate over the three most‑recent months in the history tree and merge their `cve.json` files.

---

## Entry points

* **Root index (latest view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/index.json`
* **Historical index (chronological view)**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/history/index.json`
* **Terminology**  
  `https://raw.githubusercontent.com/richlander/core/main/release-notes/terminology.md`

All other resources (patches, CVEs, per‑month manifests) are discoverable solely through `_links` from these roots.

---

## CVE information structure

Each month's **`cve.json`** is a flat list of items. Key fields you will use most:

| Field | Purpose |
|-------|---------|
| `id` | CVE identifier (e.g., **CVE-2025-30399**) |
| `title` / `description` | Human-readable summary |
| `commits[].url` | **Canonical GitHub link to the primary fixing commit** |
| `packages[].affected[].commits[].url` | Branch-specific fix commits (if applicable) |

> **Fast path:** read `commits[].url`, then GET `<url>.diff` (tiny) or `<url>.patch` (full context) for the code change.  
> Assistants must use HTTP fetches—*never* run shell commands.

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