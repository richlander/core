# .NET CVEs for 2024-05-14

The following vulnerabilities have been patched.

| ID                | Title             | Severity      | Product       | Platforms     | CVSS                         |
| ----------------- | ----------------- | ------------- | ------------- | ------------- | ---------------------------- |
| [CVE-2024-30045][CVE-2024-30045] | .NET Remote Code Execution Vulnerability | Critical | .NET | All | CVSS:3.1/AV:N/AC:L/PR:N/UI:R/S:U/C:L/I:L/A:L/E:U/RL:O/RC:C |
| [CVE-2024-30046][CVE-2024-30046] | .NET Denial of Service Vulnerability | Critical | ASP.NET Core | All | CVSS:3.1/AV:N/AC:H/PR:N/UI:N/S:U/C:N/I:N/A:H/E:U/RL:O/RC:C |

## Platform Components

The following table lists version ranges for affected platform components.

| Component     | Min Version   | Max Version | Fixed Version | CVE     | Source fix |
| ------------- | ------------- | --------- | --------- | ------------- | -------- |
| ASP.NET Runtime | >=7.0.0     | <=7.0.18  | [7.0.19](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.19/7.0.19.md) | CVE-2024-30046 |  |
|               | >=8.0.0       | <=8.0.4   | [8.0.5](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.5/8.0.5.md) | CVE-2024-30046 |  |
| .NET Runtime  | >=7.0.0       | <=7.0.18  | [7.0.19](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.19/7.0.19.md) | CVE-2024-30045 |  |
|               | >=8.0.0       | <=8.0.4   | [8.0.5](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.5/8.0.5.md) | CVE-2024-30045 |  |

## Packages

The following table lists version ranges for affected packages.

No packages with vulnerabilities reported.

[CVE-2024-30045]: https://github.com/dotnet/runtime/security/advisories/GHSA-7fcr-8qw6-92fr
[CVE-2024-30046]: https://github.com/dotnet/aspnetcore/security/advisories/GHSA-hhc7-x9w4-cw47
