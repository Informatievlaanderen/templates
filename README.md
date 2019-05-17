# agentschap Informatie Vlaanderen .NET Core templates

## Goal

> Provide `dotnet new` templates to increase development speed.

## Getting started

Install [Be.Vlaanderen.Basisregisters.Templates](https://www.nuget.org/packages/Be.Vlaanderen.Basisregisters.Templates/) and have a look at the `be-registry` template:

```console
$ dotnet new -i Be.Vlaanderen.Basisregisters.Templates
> Installing Be.Vlaanderen.Basisregisters.Templates 1.3.2.

$ dotnet new be-registry -h
> Basisregisters Vlaanderen Registry (C#)
> Author: Basisregisters Vlaanderen
> Description: A professional .NET Core project setup including build scripts, documentation and unit tests for an event-sourced project.
```

In case you want to verify if the correct version is installed, you can run:

```console
$ ls ~/.templateengine/dotnetcli/v*/packages/be.vlaanderen.basisregisters.templates.*.nupkg
> /home/cumpsd/.templateengine/dotnetcli/v2.2.203/packages/be.vlaanderen.basisregisters.templates.1.3.2.nupkg
```

To create a new registry, in this case a registry containing public services, run:

```console
$ dotnet new be-registry \
       -n PublicServiceRegistry \
       -aggregate PublicService \
       -desc "Authentic base registry containing public services of Flanders." \
       -company "agentschap Informatie Vlaanderen" \
       -email informatie.vlaanderen@vlaanderen.be \
       -site https://vlaanderen.be/informatie-vlaanderen
> The template "Basisregisters Vlaanderen Registry" was created successfully.
```

This can also be used to bootstrap any event-sourced applications accessible via an API:

```console
$ dotnet new be-registry \
       -n SandwichStore \
       -aggregate Sandwich \
       -desc "Backend for the exira.com sandwich store." \
       -company exira.com \
       -email info@exira.com \
       -site https://exira.com \
       -gh-org exira
> The template "Basisregisters Vlaanderen Registry" was created successfully.
```

Afterwards, you can run `build.sh` to compile everything and have a ready to go project. It is possible you will have to make `build.sh` executable first.

```console
$ chmod +x build.sh
$ ./build.sh
> Paket version 5.198.0
>
> ... a lot of build output ...
>
> ---------------------------------------------------------------------
> Build Time Report
> ---------------------------------------------------------------------
> Target             Duration
> ------             --------
> NpmInstall         00:00:22.1432580
> DotNetCli          00:00:00.3928220
> Clean              00:00:00.0154320
> Restore_Solution   00:00:15.3776050
> Build_Solution     00:00:23.8466700
> Build              00:00:00.0001190
> Test_Solution      00:00:12.4589250
> Test               00:00:00.0002850
> Total:             00:01:14.3630510
> ---------------------------------------------------------------------
> Status:            Ok
> ---------------------------------------------------------------------
```

To remove everything, run:

```console
$ dotnet new -u Be.Vlaanderen.Basisregisters.Templates
```

## Features

Our Base Registry template provides quite a bit out of the box:

* FAKE build script + Semantic versioning
* Command Handling (SQL Stream Store, Aggregate Source)
* Event Handling (Projac, Projector)
* Pagination, Sorting, Filtering
* Validation (Fluent Validation, Problem Details)
* Localization
* API Versioning
* Logging (Serilog)
* Health Checks (Alive, Database)
* CORS configuration
* Response Compression (GZip, Brotli)
* xUnit Tests (Domain, API, Projections)
* Swagger/OpenAPI documentation
* ReDoc API documentation
* Structurizr documentation
* Docker deployment

## License

[European Union Public Licence (EUPL)](https://joinup.ec.europa.eu/news/understanding-eupl-v12)

The new version 1.2 of the European Union Public Licence (EUPL) is published in the 23 EU languages in the EU Official Journal: [Commission Implementing Decision (EU) 2017/863 of 18 May 2017 updating the open source software licence EUPL to further facilitate the sharing and reuse of software developed by public administrations](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=uriserv:OJ.L_.2017.128.01.0059.01.ENG&toc=OJ:L:2017:128:FULL) ([OJ 19/05/2017 L128 p. 59–64](https://eur-lex.europa.eu/legal-content/EN/TXT/?uri=uriserv:OJ.L_.2017.128.01.0059.01.ENG&toc=OJ:L:2017:128:FULL)).

## Credits

### Languages & Frameworks

* [.NET Core](https://github.com/Microsoft/dotnet/blob/master/LICENSE) - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Core Runtime](https://github.com/dotnet/coreclr/blob/master/LICENSE.TXT) - _CoreCLR is the runtime for .NET Core. It includes the garbage collector, JIT compiler, primitive data types and low-level classes._ - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Core SDK](https://github.com/dotnet/sdk/blob/master/LICENSE.TXT) - _Core functionality needed to create .NET Core projects, that is shared between Visual Studio and CLI._ - [MIT](https://choosealicense.com/licenses/mit/)
* [.NET Standard definition](https://github.com/dotnet/standard/blob/master/LICENSE.TXT) - _The principles and definition of the .NET Standard._ - [MIT](https://choosealicense.com/licenses/mit/)
* [F#](https://github.com/fsharp/fsharp/blob/master/LICENSE) - _The F# Compiler, Core Library & Tools_ - [MIT](https://choosealicense.com/licenses/mit/)
* [F# and .NET Core](https://github.com/dotnet/netcorecli-fsc/blob/master/LICENSE) - _F# and .NET Core SDK working together._ - [MIT](https://choosealicense.com/licenses/mit/)

### Libraries

* [Paket](https://fsprojects.github.io/Paket/license.html) - _A dependency manager for .NET with support for NuGet packages and Git repositories._ - [MIT](https://choosealicense.com/licenses/mit/)
* [FAKE](https://github.com/fsharp/FAKE/blob/release/next/License.txt) - _"FAKE - F# Make" is a cross platform build automation system._ - [MIT](https://choosealicense.com/licenses/mit/)

### Tooling

* [npm](https://github.com/npm/cli/blob/latest/LICENSE) - _A package manager for JavaScript._ - [Artistic License 2.0](https://choosealicense.com/licenses/artistic-2.0/)
* [semantic-release](https://github.com/semantic-release/semantic-release/blob/master/LICENSE) - _Fully automated version management and package publishing._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/changelog](https://github.com/semantic-release/changelog/blob/master/LICENSE) - _Semantic-release plugin to create or update a changelog file._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/commit-analyzer](https://github.com/semantic-release/commit-analyzer/blob/master/LICENSE) - _Semantic-release plugin to analyze commits with conventional-changelog._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/exec](https://github.com/semantic-release/exec/blob/master/LICENSE) - _Semantic-release plugin to execute custom shell commands._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/git](https://github.com/semantic-release/git/blob/master/LICENSE) - _Semantic-release plugin to commit release assets to the project's git repository._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/npm](https://github.com/semantic-release/npm/blob/master/LICENSE) - _Semantic-release plugin to publish a npm package._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/github](https://github.com/semantic-release/github/blob/master/LICENSE) - _Semantic-release plugin to publish a GitHub release._ - [MIT](https://choosealicense.com/licenses/mit/)
* [semantic-release/release-notes-generator](https://github.com/semantic-release/release-notes-generator/blob/master/LICENSE) - _Semantic-release plugin to generate changelog content with conventional-changelog._ - [MIT](https://choosealicense.com/licenses/mit/)
* [commitlint](https://github.com/marionebl/commitlint/blob/master/license.md) - _Lint commit messages._ - [MIT](https://choosealicense.com/licenses/mit/)
* [commitizen/cz-cli](https://github.com/commitizen/cz-cli/blob/master/LICENSE) - _The commitizen command line utility._ - [MIT](https://choosealicense.com/licenses/mit/)
* [commitizen/cz-conventional-changelog](https://github.com/commitizen/cz-conventional-changelog/blob/master/LICENSE) _A commitizen adapter for the angular preset of conventional-changelog._ - [MIT](https://choosealicense.com/licenses/mit/)

### Flemish Government Libraries

* [Be.Vlaanderen.Basisregisters.Build.Pipeline](https://github.com/informatievlaanderen/build-pipeline/blob/master/LICENSE) - _Contains generic files for all Basisregisters pipelines._ - [MIT](https://choosealicense.com/licenses/mit/)
