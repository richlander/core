# .NET CVEs for 2024-02-13

The following vulnerabilities have been patched.

| ID                | Title             | Severity      | Product       | Platforms     | CVSS                         |
| ----------------- | ----------------- | ------------- | ------------- | ------------- | ---------------------------- |
| [CVE-2024-21386][CVE-2024-21386] | .NET Denial of Service Vulnerability | Critical | ASP.NET Core | All | CVSS:3.1/AV:N/AC:L/PR:N/UI:N/S:U/C:N/I:N/A:H/E:P/RL:O/RC:C |
| [CVE-2024-21404][CVE-2024-21404] | .NET Denial of Service Vulnerability | Critical | .NET | Linux | CVSS:3.1/AV:N/AC:L/PR:N/UI:N/S:U/C:N/I:N/A:H/E:P/RL:O/RC:C |


## Platform Components

The following table lists version ranges for affected platform components.

| Component     | Min Version   | Max Version | Fixed Version | CVE     | Source fix |
| ------------- | ------------- | --------- | --------- | ------------- | -------- |
| ASP.NET Runtime | >=6.0.0     | <=6.0.26  | [6.0.27](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.27/6.0.27.md) | CVE-2024-21386 |  |
|               | >=7.0.0       | <=7.0.15  | [7.0.16](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.16/7.0.16.md) | CVE-2024-21386 |  |
|               | >=8.0.0       | <=8.0.1   | [8.0.2](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.2/8.0.2.md) | CVE-2024-21386 |  |
| .NET Runtime  | >=6.0.0       | <=6.0.26  | [6.0.27](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.27/6.0.27.md) | CVE-2024-21404 |  |
|               | >=7.0.0       | <=7.0.15  | [7.0.16](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.16/7.0.16.md) | CVE-2024-21404 |  |
|               | >=8.0.0       | <=8.0.1   | [8.0.2](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.2/8.0.2.md) | CVE-2024-21404 |  |


## Packages

The following table lists version ranges for affected packages.

No packages with vulnerabilities reported.


[CVE-2024-21386]: https://github.com/dotnet/aspnetcore/security/advisories/GHSA-g74q-5xw3-j7q9
[CVE-2024-21404]: https://github.com/dotnet/runtime/security/advisories/GHSA-qqwf-5c27-4xxx
