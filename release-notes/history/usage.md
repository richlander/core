# Historical Data Usage Guide

This document provides specialized guidance for using the .NET historical release data and CVE (Common Vulnerabilities and Exposures) information. For general .NET metadata usage, see the [main usage guide](../usage.md).

---

## Quick Start for CVE Research

### Find CVEs by Time Period
1. Start with the historical index: `release-notes/history/index.json`
2. Navigate to the year of interest via `_embedded.years`
3. Fetch the year-specific index (e.g., `2024/index.json`)
4. Examine months with `cve-records` arrays

### Find CVEs Affecting a Specific Version
1. Fetch the year index for the timeframe of interest
2. Look for months containing your target version in `dotnet-releases` arrays
3. Check `cve-records` for those months
4. Follow CVE `href` links for detailed security advisories

---

## Finding Security Vulnerabilities by Time Period

### Recent CVEs (Last 12 Months)
```bash
# Example workflow for finding recent CVEs
# 1. Get current year and previous year data
curl https://raw.githubusercontent.com/richlander/core/main/release-notes/history/2024/index.json
curl https://raw.githubusercontent.com/richlander/core/main/release-notes/history/2025/index.json

# 2. Look for months with cve-records arrays
# 3. Extract CVE IDs and titles from cve-records
```

### CVEs by Severity
- Critical CVEs typically have titles containing "Remote Code Execution"
- High severity often includes "Elevation of Privilege"
- Medium severity commonly involves "Denial of Service"
- Information disclosure vulnerabilities vary in severity

---

## Correlating Releases with Security Disclosures

### Release-to-CVE Mapping
Each monthly entry shows:
- `dotnet-releases`: Which .NET versions had releases that month
- `cve-records`: Which CVEs were disclosed that month
- This correlation helps identify security patches

### Example: Finding Security Patches
```json
{
  "month": "10",
  "dotnet-releases": ["9.0", "8.0", "6.0"],
  "cve-records": [
    {
      "id": "CVE-2024-43483",
      "title": ".NET Denial of Service Vulnerability",
      "href": "https://github.com/dotnet/runtime/security/advisories/GHSA-qj66-m88j-hmgj"
    }
  ]
}
```

This indicates that the October 2024 releases of .NET 9.0, 8.0, and 6.0 likely included patches for CVE-2024-43483.

---

## Compliance and Audit Workflows

### Security Audit Checklist
1. **Identify Target Versions**: List all .NET versions in use
2. **Time Range Analysis**: Determine deployment timeframes
3. **CVE Impact Assessment**: Find CVEs affecting your versions during your deployment period
4. **Patch Verification**: Confirm patches were applied via release notes

### Compliance Reporting
- Use `cve-records` arrays to generate comprehensive vulnerability reports
- Include CVE IDs, titles, and GitHub security advisory links
- Cross-reference with internal deployment records

---

## Advanced Historical Analysis

### Trend Analysis
- Track CVE frequency over time using monthly `cve-records` counts
- Analyze security patterns by examining CVE types and affected components
- Correlate release cycles with security disclosure patterns

### Multi-Year Comparison
```bash
# Get historical data for multiple years
for year in 2022 2023 2024; do
  curl https://raw.githubusercontent.com/richlander/core/main/release-notes/history/${year}/index.json
done
```

### Security Hotspots
- Runtime vulnerabilities: Check titles containing "runtime"
- ASP.NET Core issues: Look for "aspnetcore" in CVE hrefs
- WPF vulnerabilities: Search for "wpf" in advisory links

---

## API Examples and Code Samples

### Python Example: Find All CVEs for a Version
```python
import requests
import json

def find_cves_for_version(version, year):
    """Find all CVEs affecting a specific .NET version in a given year"""
    url = f"https://raw.githubusercontent.com/richlander/core/main/release-notes/history/{year}/index.json"
    response = requests.get(url)
    data = response.json()
    
    cves = []
    for month in data['_embedded']['months']:
        if version in month.get('dotnet-releases', []):
            cves.extend(month.get('cve-records', []))
    
    return cves

# Example usage
net8_cves_2024 = find_cves_for_version("8.0", 2024)
for cve in net8_cves_2024:
    print(f"{cve['id']}: {cve['title']}")
```

### PowerShell Example: CVE Summary Report
```powershell
# Get CVE summary for the last 12 months
$currentYear = (Get-Date).Year
$lastYear = $currentYear - 1

$years = @($currentYear, $lastYear)
$allCves = @()

foreach ($year in $years) {
    $uri = "https://raw.githubusercontent.com/richlander/core/main/release-notes/history/$year/index.json"
    $data = Invoke-RestMethod -Uri $uri
    
    foreach ($month in $data._embedded.months) {
        if ($month.'cve-records') {
            foreach ($cve in $month.'cve-records') {
                $allCves += [PSCustomObject]@{
                    Year = $year
                    Month = $month.month
                    CVE = $cve.id
                    Title = $cve.title
                    Link = $cve.href
                }
            }
        }
    }
}

$allCves | Sort-Object Year, Month | Format-Table
```

---

## Data Formats and Links

### Available Formats
- **JSON**: Structured data for programmatic access
- **Markdown**: Human-readable format with additional context
- **GitHub Links**: Direct links to security advisories

### Link Types
- `_links.self`: Structured JSON endpoint
- `_links.cve-markdown`: GitHub-rendered Markdown view
- `_links.cve-markdown-raw`: Raw Markdown for processing

---

## Best Practices

### Performance
- Cache year-level indexes to reduce API calls
- Use targeted queries rather than scanning all years
- Implement rate limiting for bulk operations

### Data Integrity
- Always use official GitHub security advisory links
- Verify CVE IDs against official CVE databases
- Check for updates to historical data periodically

### Security
- Treat CVE information as sensitive for unreleased products
- Use HTTPS for all API calls
- Validate all external links before following

---

## Troubleshooting

### Common Issues
- **Empty cve-records**: Not all months have CVE disclosures
- **Missing months**: Months without releases may not appear in indexes
- **Link changes**: GitHub URLs may change; use programmatic access when possible

### Debugging
- Check the `kind` field to verify document type
- Use `description` fields for human-readable context
- Verify year/month navigation via `_embedded` structures

---

*For general .NET metadata usage, see the [main usage guide](../usage.md).*
*For terminology and conventions, see [terminology.md](../terminology.md).*