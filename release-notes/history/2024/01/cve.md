# .NET CVEs for 2024-01-09

The following vulnerabilities have been patched.

| ID                | Title             | Severity      | Product       | Platforms     | CVSS                         |
| ----------------- | ----------------- | ------------- | ------------- | ------------- | ---------------------------- |
| [CVE-2024-0056][CVE-2024-0056] | Microsoft.Data.SqlClient and System.Data.SqlClient SQL Data provider Information Disclosure Vulnerability | High | .NET | All | CVSS:3.1/AV:N/AC:L/PR:N/UI:N/S:U/C:H/I:N/A:N/E:U/RL:O/RC:C |
| [CVE-2024-0057][CVE-2024-0057] | .NET Security Feature Bypass Vulnerability | Critical | .NET | All | CVSS:3.1/AV:N/AC:L/PR:L/UI:N/S:U/C:H/I:H/A:H/E:P/RL:O/RC:C |
| [CVE-2024-21319][CVE-2024-21319] | .NET Denial of Service Vulnerability | Critical | ASP.NET Core | All | CVSS:3.1/AV:N/AC:L/PR:N/UI:N/S:C/C:N/I:N/A:H/E:U/RL:O/RC:C |

## Platform Components

The following table lists version ranges for affected platform components.

| Component     | Min Version   | Max Version | Fixed Version | CVE     | Source fix |
| ------------- | ------------- | --------- | --------- | ------------- | -------- |
| ASP.NET Runtime | >=6.0.0     | <=6.0.25  | [6.0.26](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.26/6.0.26.md) | CVE-2024-21319 |  |
|               | >=8.0.0       | <=8.0.0   | [8.0.1](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.1/8.0.1.md) | CVE-2024-21319 |  |
|               | >=7.0.0       | <=7.0.14  | [7.0.15](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.15/7.0.15.md) | CVE-2024-21319 |  |
| .NET Runtime  | >=6.0.0       | <=6.0.25  | [6.0.26](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.26/6.0.26.md) | CVE-2024-0057 |  |
|               | >=8.0.0       | <=8.0.0   | [8.0.1](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.1/8.0.1.md) | CVE-2024-0057 |  |
|               | >=7.0.0       | <=7.0.14  | [7.0.15](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.15/7.0.15.md) | CVE-2024-0057 |  |
| Windows Desktop Runtime | >=6.0.0 | <=6.0.25 | [6.0.26](https://github.com/dotnet/core/blob/main/release-notes/6.0/6.0.26/6.0.26.md) | CVE-2024-0057 |  |
|               | >=8.0.0       | <=8.0.0   | [8.0.1](https://github.com/dotnet/core/blob/main/release-notes/8.0/8.0.1/8.0.1.md) | CVE-2024-0057 |  |
|               | >=7.0.0       | <=7.0.14  | [7.0.15](https://github.com/dotnet/core/blob/main/release-notes/7.0/7.0.15/7.0.15.md) | CVE-2024-0057 |  |

## Packages

The following table lists version ranges for affected packages.

| Package       | Min Version   | Max Version | Fixed Version | CVE     | Source fix |
| ------------- | ------------- | --------- | --------- | ------------- | -------- |
| [Microsoft.Data.SqlClient][Microsoft.Data.SqlClient] | >=1.0.19123.2-Preview | <=2.1.6 | [2.17](https://www.nuget.org/packages/Microsoft.Data.SqlClient/2.17) | CVE-2024-0056 |  |
|               | >=3.0.0       | <=3.1.4   | [3.1.5](https://www.nuget.org/packages/Microsoft.Data.SqlClient/3.1.5) | CVE-2024-0056 |  |
|               | >=4.0.0       | <=4.0.4   | [4.0.5](https://www.nuget.org/packages/Microsoft.Data.SqlClient/4.0.5) | CVE-2024-0056 |  |
|               | >=5.0.0       | <=5.1.2   | [5.1.3](https://www.nuget.org/packages/Microsoft.Data.SqlClient/5.1.3) | CVE-2024-0056 |  |
| [Microsoft.IdentityModel.JsonWebTokens][Microsoft.IdentityModel.JsonWebTokens] | >=5.2.4 | <=5.6.0 | [5.7.0](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/5.7.0) | CVE-2024-21319 |  |
|               | >=6.5.0       | <=6.33.0  | [6.34.0](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/6.34.0) | CVE-2024-21319 |  |
|               | >=7.0.0       | <=7.0.3   | [7.1.2](https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens/7.1.2) | CVE-2024-21319 |  |
| [System.Data.SqlClient][System.Data.SqlClient] | >=1.0.0-beta1 | <=4.8.5 | [4.8.6](https://www.nuget.org/packages/System.Data.SqlClient/4.8.6) | CVE-2024-0056 |  |
| [System.IdentityModel.Tokens.Jwt][System.IdentityModel.Tokens.Jwt] | >=5.2.4 | <=5.6.0 | [5.7.0](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/5.7.0) | CVE-2024-21319 |  |
|               | >=6.5.0       | <=6.33.0  | [6.34.0](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/6.34.0) | CVE-2024-21319 |  |
|               | >=7.0.0       | <=7.0.3   | [7.1.2](https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt/7.1.2) | CVE-2024-21319 |  |


[CVE-2024-0056]: https://github.com/advisories/GHSA-98g6-xh36-x2p7
[CVE-2024-0057]: https://github.com/dotnet/runtime/security/advisories/GHSA-56gp-vpf5-xmqc
[CVE-2024-21319]: https://github.com/dotnet/aspnetcore/security/advisories/GHSA-59j7-ghrg-fj52
[Microsoft.Data.SqlClient]: https://www.nuget.org/packages/Microsoft.Data.SqlClient
[Microsoft.IdentityModel.JsonWebTokens]: https://www.nuget.org/packages/Microsoft.IdentityModel.JsonWebTokens
[System.Data.SqlClient]: https://www.nuget.org/packages/System.Data.SqlClient
[System.IdentityModel.Tokens.Jwt]: https://www.nuget.org/packages/System.IdentityModel.Tokens.Jwt
