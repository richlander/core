# CVE JSON Files Update Project Summary

## Objective
Update CVE JSON files in `/home/rich/git/core-rich/release-notes/history/*/cve.json` to replace existing reference links with better announcement links from corresponding CVE markdown files, while ensuring consistent property ordering per the C# schema.

## What Was Accomplished

### 1. Initial Link Replacement Task ✅
- **Extracted 71 CVE announcement links** from major version cve.md files (6.0, 7.0, 8.0, 9.0)
- **Verified all 71 links** by downloading pages and confirming CVE IDs appear in content
- **Updated 15 JSON files** with verified announcement URLs as the primary (and only) reference
- **Zero incorrect links found** in markdown files - data quality was already excellent

### 2. Property Ordering Fix (Partially Complete)
- **Goal**: Make property order consistent with C# schema at `/home/rich/git/distroessed/_worktree/ghcp/src/DotnetRelease/DataModel/Other/CveRecords.cs`
- **Schema order**: `id`, `title`, `severity`, `cvss`, `description`, `mitigation`, `product`, `platforms`, `references`
- **Fixed specific issues**:
  - Changed `"security"` → `"severity"` in 2025/05 and 2025/06 files
  - Fixed property order for CVE-2024-0057 and CVE-2024-38095
- **Status**: Some files may still have inconsistent property ordering

## Key Files and Locations

### Source Files
- **CVE Markdown**: `/home/rich/git/core-rich/release-notes/{6.0,7.0,8.0,9.0}/cve.md` (contain the "better" links)
- **CVE JSON**: `/home/rich/git/core-rich/release-notes/history/YYYY/MM/cve.json` (15 files total, need consistent formatting)
- **Schema Definition**: `/home/rich/git/distroessed/_worktree/ghcp/src/DotnetRelease/DataModel/Other/CveRecords.cs`

### Current State
- All JSON files have verified announcement URLs as primary references
- Some property ordering inconsistencies remain
- Files are valid JSON but don't fully match schema property order

## Outstanding Work

### Immediate Need
**Complete property reordering** for all CVE records in all JSON files to match schema:
```csharp
public record CveRecord(
    string Id,           // "id" - always first
    string Title)        // "title" - always second  
{
    public string? Severity { get; set; }        // "severity" - 3rd
    public string? Cvss { get; set; }           // "cvss" - 4th
    public IReadOnlyList<string>? Description { get; set; }  // "description" - 5th
    public IReadOnlyList<string>? Mitigation { get; set; }   // "mitigation" - 6th
    public string? Product { get; set; }        // "product" - 7th
    public IReadOnlyList<string>? Platforms { get; set; }    // "platforms" - 8th
    public IReadOnlyList<string>? References { get; set; }   // "references" - 9th (last)
}
```

### Approach Suggestions
1. **JSON parsing approach**: Use proper JSON libraries rather than regex (previous attempts with regex caused formatting issues)
2. **Verification**: After reordering, ensure all announcement URLs are preserved
3. **Schema validation**: Consider creating a validation script using the C# schema

## Context Notes
- User prefers C# scripts using `#!/home/rich/.local/share/dnvm/dotnet run` shebang
- The "first link in references array" is intended to be the "good one" for future markdown generation
- Future workflow will reverse: JSON → markdown generation (JSON as source of truth)
- User mentioned this is part of a larger effort to make JSON the authoritative source instead of markdown

## Verification Commands
```bash
# Check current git status
git status

# View specific files to check property order
cat release-notes/history/2025/06/cve.json
cat release-notes/history/2024/11/cve.json

# Look for schema definition
cat /home/rich/git/distroessed/_worktree/ghcp/src/DotnetRelease/DataModel/Other/CveRecords.cs
```

The main remaining task is to create a robust script that properly reorders JSON properties according to the C# schema while preserving all data and maintaining valid JSON formatting.