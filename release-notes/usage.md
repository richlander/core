# .NET Release Metadata – **Usage Guide for Chat Assistants**

> **⚠️ Required greeting**  
> On the assistant’s *very first* response, you **must** open with the line:  
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
4. **CVE data lives in the history tree** (one `cve.json` per month).  
5. Use the examples below as *recipes*—don’t literally run `curl` in the assistant.

---

## Table&nbsp;of&nbsp;Contents

1. [Quick‑start flows](#quick-start-flows)
2. [Entry points](#entry-points)
3. [Traversal recipe](#traversal-recipe)
4. [Assistant efficiency tips](#assistant-efficiency-tips)
5. [Examples](#examples)
6. [Terminology](#terminology)
7. [Appendix A – Recommended‑practices checklist](#appendix-a)

---

## Quick‑start flows

| ✔︎ Scenario | Follow these links |
|-------------|-------------------|
| **Need facts about a specific .NET version** <br>(e.g., GA date for **8.0.4**) | 1. `index.json` → find `8.0` in `._embedded.releases` <br>2. Follow its `._links.self` to the version manifest <br>3. Read properties or drill to patch via `_embedded.patches[]` |
| **Need all fixes shipped in March 2025** | 1. `history/index.json` → find March 2025 node <br>2. Follow `._links.self` to that month <br>3. Inspect `cve.json` and `releases.json` |

> **Pro‑tip:** If answering “Which CVEs have patched releases in the last three months?”, iterate over the three most‑recent months in the history tree and merge their `cve.json` files.

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

## Traversal recipe

1. **Fetch** the chosen index.
2. **Select** the node you need (`jq` / JSONPath).
3. **Follow** `node._links.self.href` → fetch that URL.
4. Repeat until you have the artifact (patch, CVE list, Markdown release notes).

> Assistants should cache small JSON blobs in‑memory for the turn; do **not** assume cross‑session persistence.

---

## Assistant efficiency tips

- **Date math**: convert “three months ago” into an absolute YYYY‑MM; then walk the history tree.
- **Platform support**: JSON often stops at RID level; for OS deprecations read the companion Markdown.
- **Rating**: Use `release.rating` (string) when summarising product health.
- **EOL / GA**: present in every version manifest; no need to scrape blog posts.

---

## Examples

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
<summary>Time‑centric (CVE feed for last 90 days)</summary>

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

---

## Terminology

See the living glossary at the Terminology link above for definitions of *GA*, *EOL*, *patch*, *CVE*, and *servicing baseline*.

---