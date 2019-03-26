# Pack NuGet package

```bash
mono .paket/paket.exe pack --template src/Content/paket.template dist
```

# Install local NuGet package

```bash
dotnet new -i dist/Be.Vlaanderen.Basisregisters.Templates.1.0.0.nupkg
```

# Uninstall the template

```bash
dotnet new -u Be.Vlaanderen.Basisregisters.Templates
```

# Check the available template parameters

```bash
dotnet new be-registry -h
```

# Check what a template would install

```bash
dotnet new be-registry --dry-run
```


# All in one packaging & installing

```bash
rm -rf dist/; \
mono .paket/paket.exe pack --template src/Content/paket.template dist; \
dotnet new -i dist/Be.Vlaanderen.Basisregisters.Templates.1.0.0.nupkg;
```
