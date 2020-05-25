#r "paket:
version 5.245.1
framework: netstandard20
source https://api.nuget.org/v3/index.json
nuget Be.Vlaanderen.Basisregisters.Build.Pipeline 4.1.0 //"

#load "packages/Be.Vlaanderen.Basisregisters.Build.Pipeline/Content/build-generic.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.IO.FileSystemOperators
open ``Build-generic``

let packTemplate _ =
  let nugetVersion = sprintf "%s" buildNumber

  Paket.pack(fun p ->
    { p with
        ToolType = ToolType.CreateLocalTool()
        BuildConfig = "Release"
        OutputPath = buildDir
        Version = nugetVersion
        TemplateFile = "src" @@ "Be.Vlaanderen.Basisregisters.Templates" @@ "paket.template"
    }
  )

Target.create "Pack_Template" (fun _ -> packTemplate ())

Target.create "Pack" ignore

"Clean"
  ==> "Pack_Template"
  ==> "Pack"

Target.runOrDefault "Pack"
