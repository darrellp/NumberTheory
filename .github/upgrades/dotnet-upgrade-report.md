# .NET 8.0 Upgrade Report

## Project target framework modifications

| Project name                                   | Old Target Framework | New Target Framework | Commits  |
|:-----------------------------------------------|:--------------------:|:--------------------:|----------|
| NumberTheory\NumberTheory.csproj                | net48                | net8.0               | 22499c4  |
| NumberTheoryTests\NumberTheoryTests.csproj      | net48                | net8.0               | 22499c4  |

## NuGet Packages

| Package Name                 | Old Version | New Version | Commit Id |
|:-----------------------------|:-----------:|:-----------:|-----------|
| Microsoft.NET.Test.Sdk       |             | 18.3.0      | 22499c4   |
| MSTest.TestAdapter           |             | 4.1.0       | 22499c4   |
| MSTest.TestFramework         |             | 4.1.0       | 22499c4   |

## All commits

| Commit ID | Description                                                  |
|:----------|:-------------------------------------------------------------|
| 22499c4   | Upgrade NumberTheory and NumberTheoryTests projects to .NET 8.0 |

## Project feature upgrades

### NumberTheory\NumberTheory.csproj

- Converted project from legacy format to SDK-style project format
- Changed target framework from `net48` to `net8.0`
- Added `Debug` and `Release` configurations with `BIGINTEGER` define constants to support standard build alongside existing custom configurations

### NumberTheoryTests\NumberTheoryTests.csproj

- Converted project from legacy format to SDK-style project format
- Changed target framework from `net48` to `net8.0`
- Added MSTest NuGet packages (MSTest.TestFramework 4.1.0, MSTest.TestAdapter 4.1.0, Microsoft.NET.Test.Sdk 18.3.0) to replace the old assembly reference to Microsoft.VisualStudio.TestTools.UnitTesting
- Removed obsolete System.Core reference and CodeAnalysis dependent assembly paths
- Added `Debug` and `Release` configurations with `BIGINTEGER` define constants
- Added `IsTestProject` property for proper test discovery
- All 28 unit tests passed successfully after upgrade

## Next steps

- Consider addressing the CS8981 warnings about type alias names (`nt`, `euc`) that only contain lower-cased ASCII characters, as such names may become reserved in future C# versions
- Consider removing the duplicate `using Microsoft.VisualStudio.TestTools.UnitTesting` in PellsTest.cs (warning CS0105)
