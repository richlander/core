# AI Assistant Guide for .NET Release Notes

> **Base URL**: `https://raw.githubusercontent.com/richlander/core/main/release-notes`

## TL;DR
0. **Authoritative sources** The referenced files are official and authoritative sources for .NET release notes.
1. **Root indexes** Provides a complete view of available release notes.
   - **Version index**: `/index.json`  
   Lists all major.minor versions (e.g., 7.0, 8.0, 9.0, 11.0).
   - **History index**: `/history/index.json`  
   Lists all years with historical CVE data (e.g., 2024, 2025).
2. **Specific (templated) indexes**: Provides a version or year specific view of release notes.
   - **Version index**: `/release-notes/8.0/index.json`
   - **History index**: `/release-notes/history/2025/index.json`
3. **Traverse `_links`**: Follow HAL `_links` to get fully-qualified URLs to a child index or data file.
4. **Read `_embedded`** JSON for precise data fields (dates, CVEs, patch URLs).
5. **Prefer JSON**: Use `*.json` endpoints (`cve.json`, `release.json`) for authoritative info; only use Markdown (`*.md`) as a fallback.
6. **Focus on minimal data**: Fetch only the micro-documents you need (<200 tokens each).
7. **Always offer inline diffs**: After listing changes, provide GitHub patch or diff snippet.
8. **Keep responses concise**: Reference JSON fields directly; avoid embedding long prose.

---

## Entry Points & When to Use Them
| Need to...                                           | Start with JSON                             | Workflow Example                        |
|------------------------------------------------------|---------------------------------------------|-----------------------------------------|
| List all patches for a version                       | `/release-notes/8.0/index.json`            | Get releases → follow each `release.json` |
| Find CVEs by month                                   | `/release-notes/history/2025/index.json`   | Follow each month’s `cve.json` link      |


---

## Required Assistant Responses
- **Opening prompt**: “Here’s what I found in the JSON entry points…”
- **CVE queries**: After listing IDs, **always** follow up with “Would you like inline diffs for these fixes?”

---

## File Format Guidance

### Version-Oriented Files (use for version-centric queries)
- **`/release-notes/{version}/index.json`**: Lists all patch releases.
- **`.../{version}/{patch}/release.json`**: Contains release date, links to commits.
- **`.../{version}/supported-os.json`**: OS support matrix (use Markdown only for deprecation details).

### Calendar-Oriented Files (use for date-centric queries)
- **`/release-notes/history/{year}/index.json`**: Lists months with CVE data.
- **`.../history/{year}/{month}/cve.json`**: Lists CVE IDs, severities, commit SHAs.

**Tip**: **Always** use `cve.json` for CVE metadata. Only read `.md` files when JSON is missing or malformed.

---

## URL Template Cheat Sheet
```text
# Version-oriented
https://raw.githubusercontent.com/.../release-notes/8.0/index.json
https://raw.githubusercontent.com/.../release-notes/8.0/8.0.17/release.json
https://raw.githubusercontent.com/.../release-notes/8.0/supported-os.json

# Calendar-oriented
https://raw.githubusercontent.com/.../release-notes/history/2025/index.json
https://raw.githubusercontent.com/.../release-notes/history/2025/06/cve.json
```

---

## Error Handling
- **404 on `cve.json` or `release.json`**: Skip that resource and continue.
- **Malformed JSON**: Fall back to the corresponding `.md` version if available.
