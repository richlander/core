# Historical Data Usage Guide

This document provides guidance for chat assistants using .NET historical release data and CVE information. For general .NET metadata usage, see the [main usage guide](../usage.md).

---

## Quick Reference for Chat Assistants

### CVE Questions
- **"What CVEs affected .NET 8 in 2024?"** → Check `history/2024/index.json`, find months with "8.0" in `dotnet-releases`, examine `cve-records`
- **"Recent .NET security vulnerabilities?"** → Check current and previous year indexes for months with `cve-records`
- **"When was CVE-2024-43483 disclosed?"** → Search year indexes for the CVE ID in `cve-records`

### Release History Questions
- **"What .NET versions were released in 2023?"** → Check `history/2023/index.json` and collect versions from `dotnet-releases` arrays
- **"Which versions got security patches in October 2024?"** → Check `history/2024/index.json` for October entry

---

## CVE Data Structure

### Monthly CVE Records
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

### CVE Severity Indicators
- **Critical**: "Remote Code Execution" 
- **High**: "Elevation of Privilege"
- **Medium**: "Denial of Service"
- **Variable**: "Information Disclosure"

---

## Assistant Workflow Patterns

### For Security Questions:
1. Start with `history/index.json` to identify relevant years
2. Navigate to year-specific indexes
3. Find months with CVE records
4. Present CVE ID, title, and GitHub advisory link

### For Historical Analysis:
1. Collect data from multiple year indexes
2. Cross-reference release months with CVE disclosures
3. Identify patterns in security patch timing

### For Compliance Queries:
1. Identify target .NET versions and time periods
2. Check historical indexes for CVE records affecting those versions
3. Provide comprehensive CVE list with official advisory links

---

## Best Practices for Assistants

### Data Presentation
- Always include CVE ID, title, and GitHub advisory link
- Use plain dates (not ISO format with timezones)
- Mention which .NET versions were affected
- Provide context about vulnerability severity when possible

### Information Gathering
- Check both current year and previous year for recent CVEs
- Look for correlation between release months and CVE disclosures
- Use GitHub advisory links as authoritative sources

### Error Handling
- Not all months have CVE records (this is normal)
- Some months may not appear in indexes if no releases occurred
- Always verify CVE information against official GitHub advisories

---

*For general .NET metadata usage, see the [main usage guide](../usage.md).*