# Introduction 
This repo serves as a template for setting up repo and build pipeline with dotnet CLI.

## Prerequisites

- Visual Studio 2022 with .NET Core

## What is preconfigured

This template comes with:
- NuGet configuration
- Project traversal targets
- Basic build configurations, such as Release|x64 or Debug|AnyCPU
- Static code analysis using StyleCop.Analyzer and SonarAnalyzer.CSharp and a rule set

## What needs to be updated after creating new project

- Update .sln with your projects
- Update Git location in _RepositoryUrl_ in build\Default.props
- Update the _Pack_ Item in build.proj file with the project file(s) of which nuget package(s) need to be created
- Add _PublishApplicationName_ property to the project files(s) of which the build artifacts are published

## Build targets

To build everything with default settings run this command in repository root:
```
dotnet build build.proj
```

To clean:
```
dotnet clean build.proj
```

To rebuild:
```
dotnet build build.proj --no-incremental
```

To pack (for projects that produce NuGet packages):
```
dotnet pack build.proj
```
