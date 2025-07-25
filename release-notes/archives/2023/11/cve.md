# .NET CVEs for 2023-11-14

The following vulnerabilities have been patched.

| ID                               | Title                                      | Severity | Product      | Platforms | CVSS                                                       |
| -------------------------------- | ------------------------------------------ | -------- | ------------ | --------- | ---------------------------------------------------------- |
| [CVE-2023-36038][CVE-2023-36038] | .NET Denial of Service Vulnerability       | Critical | ASP.NET Core | Windows   | CVSS:3.1/AV:N/AC:L/PR:N/UI:N/S:U/C:N/I:L/A:H/E:U/RL:O/RC:C |
| [CVE-2023-36049][CVE-2023-36049] | .NET Elevation of Privilege Vulnerability  | Critical | .NET         | All       | CVSS:3.1/AV:N/AC:L/PR:L/UI:N/S:U/C:H/I:H/A:H/E:P/RL:O/RC:C |
| [CVE-2023-36558][CVE-2023-36558] | .NET Security Feature Bypass Vulnerability | Critical | ASP.NET Core | All       | CVSS:3.1/AV:L/AC:L/PR:N/UI:R/S:U/C:H/I:N/A:N/E:P/RL:O/RC:C |


## Platform Components

The following table lists version ranges for affected platform components.

| Component | Min Version | Max Version | Fixed Version                                                                         | CVE            | Source fix |
| --------- | ----------- | ----------- | ------------------------------------------------------------------------------------- | -------------- | ---------- |
| ASP.NET Runtime | >=8.0-rc.2 | <=8.0-rc.2 | [8.0.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.0/8.0.0.md) | CVE-2023-36038 |           |
| .NET Runtime | >=6.0.0  | <=6.0.24    | [6.0.25](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.25/6.0.25.md) | CVE-2023-36049 |            |
|           | >=7.0.0     | <=7.0.13    | [7.0.14](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.14/7.0.14.md) | CVE-2023-36049 |            |
|           | >=8.0-rc.2  | <=8.0-rc.2  | [8.0.0](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.0/8.0.0.md)    | CVE-2023-36049 |            |


## Packages

The following table lists version ranges for affected packages.

| Package | Min Version | Max Version | Fixed Version                                                                   | CVE            | Source fix |
| ------- | ----------- | ----------- | ------------------------------------------------------------------------------- | -------------- | ---------- |
| [Microsoft.AspNetCore.Components][Microsoft.AspNetCore.Components] | >=6.0.0 | <=6.0.24 | [6.0.25](https://www.nuget.org/packages/Microsoft.AspNetCore.Components/6.0.25) | CVE-2023-36558 |  |
|         | >=7.0.0     | <=7.0.13    | [7.0.14](https://www.nuget.org/packages/Microsoft.AspNetCore.Components/7.0.14) | CVE-2023-36558 |            |
|         | >=8.0-rc.2  | <=8.0-rc.2  | [8.0.0](https://www.nuget.org/packages/Microsoft.AspNetCore.Components/8.0.0)   | CVE-2023-36558 |            |



[CVE-2023-36038]: https://github.com/dotnet/announcements/issues/286
[CVE-2023-36049]: https://github.com/dotnet/announcements/issues/287
[CVE-2023-36558]: https://github.com/dotnet/announcements/issues/288
[Microsoft.AspNetCore.Components]: https://www.nuget.org/packages/Microsoft.AspNetCore.Components
