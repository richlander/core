# .NET CVEs for 2025-03-11

The following vulnerabilities have been patched.

| ID                               | Title                                     | Severity | Product | Platforms | CVSS                                                       |
| -------------------------------- | ----------------------------------------- | -------- | ------- | --------- | ---------------------------------------------------------- |
| [CVE-2025-24070][CVE-2025-24070] | .NET Elevation of Privilege Vulnerability | High     |         | All       | CVSS:3.1/AV:L/AC:L/PR:L/UI:N/S:U/C:H/I:H/A:H/E:U/RL:O/RC:C |


## Platform Components

The following table lists version ranges for affected platform components.

| Component | Min Version | Max Version | Fixed Version                                                                         | CVE            | Source fix          |
| --------- | ----------- | ----------- | ------------------------------------------------------------------------------------- | -------------- | ------------------- |
| ASP.NET Runtime | >=8.0.0 | <=8.0.13  | [8.0.14](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.14/8.0.14.md) | CVE-2025-24070 | [67f3b04][67f3b04]  |
|           | >=9.0.0     | <=8.0.2     | [9.0.3](https://github.com/dotnet/core/blob/main/release-notes/9.0/9.0.3/9.0.3.md)    | CVE-2025-24070 | [f71e283][f71e283]  |


## Packages

The following table lists version ranges for affected packages.

| Package                                                        | Min Version | Max Version | Fixed Version                                                               | CVE            | Source fix |
| -------------------------------------------------------------- | ----------- | ----------- | --------------------------------------------------------------------------- | -------------- | ---------- |
| [Microsoft.AspNetCore.Identity][Microsoft.AspNetCore.Identity] | >=2.3.0     | <=2.3.0     | [2.3.1](https://www.nuget.org/packages/Microsoft.AspNetCore.Identity/2.3.1) | CVE-2025-24070 |            |



## Commits

The following table lists commits for affected packages.

| Repo                                   | Branch                     | Commit             |
| -------------------------------------- | -------------------------- | ------------------ |
| [dotnet/aspnetcore][dotnet/aspnetcore] | [release/8.0][release/8.0] | [67f3b04][67f3b04] |
| [dotnet/aspnetcore][dotnet/aspnetcore] | [release/9.0][release/9.0] | [f71e283][f71e283] |



[CVE-2025-24070]: https://github.com/dotnet/announcements/issues/348
[Microsoft.AspNetCore.Identity]: https://www.nuget.org/packages/Microsoft.AspNetCore.Identity
[dotnet/aspnetcore]: https://github.com/dotnet/aspnetcore
[release/8.0]: https://github.com/dotnet/aspnetcore/tree/release/8.0
[67f3b04]: https://github.com/dotnet/aspnetcore/commit/67f3b04274d3acb607fe95796dcb35f4f11149bf
[release/9.0]: https://github.com/dotnet/aspnetcore/tree/release/9.0
[f71e283]: https://github.com/dotnet/aspnetcore/commit/f71e283286d8470639486804053f28391f92fafc
