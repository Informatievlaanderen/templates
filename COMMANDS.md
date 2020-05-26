# Pack NuGet package

```bash
dotnet paket pack --template src/Be.Vlaanderen.Basisregisters.Templates/paket.template dist
```

# Install local NuGet package

```bash
dotnet new -i dist/Be.Vlaanderen.Basisregisters.Templates.1.0.0.nupkg
```

# Install a beta version from nuget.org

```bash
dotnet new -i Be.Vlaanderen.Basisregisters.Templates::0.0.2-beta
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
dotnet new be-registry --dry-run \
       -n PublicServiceRegistry \
       -aggregate PublicService \
       -desc "Authentic base registry containing public services of Flanders." \
       -company "agentschap Informatie Vlaanderen" \
       -email informatie.vlaanderen@vlaanderen.be \
       -site https://vlaanderen.be/informatie-vlaanderen
```

# Create a new registry

```bash
dotnet new be-registry \
       -n SandwichStore \
       -aggregate Sandwich \
       -desc "Backend for the exira.com sandwich store." \
       -company exira.com \
       -email info@exira.com \
       -site https://exira.com \
       -gh-org exira

The template "Basisregisters Vlaanderen Registry" was created successfully.
```

```bash
dotnet new be-registry \
       -n PublicServiceRegistry \
       -aggregate PublicService \
       -desc "Authentic base registry containing public services of Flanders." \
       -company "agentschap Informatie Vlaanderen" \
       -email informatie.vlaanderen@vlaanderen.be \
       -site https://vlaanderen.be/informatie-vlaanderen

The template "Basisregisters Vlaanderen Registry" was created successfully.
```

```bash
dotnet new be-registry -n MunicipalityRegistry -aggregate Municipalit -desc "Authentic base registry containing public services of Flanders." -company "agentschap Informatie Vlaanderen" -email informatie.vlaanderen@vlaanderen.be -site https://vlaanderen.be/informatie-vlaanderen

dotnet new be-registry \
       -n MunicipalityRegistry \
       -aggregate Municipality \
       -desc "Authentic base registry containing public services of Flanders." \
       -company "agentschap Informatie Vlaanderen" \
       -email informatie.vlaanderen@vlaanderen.be \
       -site https://vlaanderen.be/informatie-vlaanderen

The template "Basisregisters Vlaanderen Registry" was created successfully.
```

# All in one packaging & installing

```bash
rm -rf dist/; \
dotnet paket pack --template src/Be.Vlaanderen.Basisregisters.Templates/paket.template dist; \
dotnet new -u Be.Vlaanderen.Basisregisters.Templates; \
dotnet new -i dist/Be.Vlaanderen.Basisregisters.Templates.0.0.1-beta.nupkg;
```

# Example appsettings.xxx.json

```json
{
  "ConnectionStrings": {
    "Events": "Server=.;Database=example-registry-dev;Trusted_Connection=True;",
    "ApiProjections": "Server=.;Database=example-registry-dev;Trusted_Connection=True;",
    "ApiProjectionsAdmin": "Server=.;Database=example-registry-dev;Trusted_Connection=True;"
  },

  "DataDog": {
    "Enabled": false
  }
}
```
