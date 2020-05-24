#r "paket:
version 5.241.6
framework: netstandard20
source https://api.nuget.org/v3/index.json
nuget Be.Vlaanderen.Basisregisters.Build.Pipeline 4.1.0 //"

#load "packages/Be.Vlaanderen.Basisregisters.Build.Pipeline/Content/build-generic.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open ``Build-generic``

let product = "Basisregisters Vlaanderen"
let copyright = "Copyright (c) Vlaamse overheid"
let company = "Vlaamse overheid"

let dockerRepository = "example-registry"
let assemblyVersionNumber = (sprintf "2.%s")
let nugetVersionNumber = (sprintf "%s")

let build = buildSolution assemblyVersionNumber
let setVersions = (setSolutionVersions assemblyVersionNumber product copyright company)
let test = testSolution
let publish = publish assemblyVersionNumber
let pack = pack nugetVersionNumber
let containerize = containerize dockerRepository
let push = push dockerRepository

supportedRuntimeIdentifiers <- [ "linux-x64" ]

// Solution -----------------------------------------------------------------------

Target.create "Restore_Solution" (fun _ -> restore "ExampleRegistry")

Target.create "Build_Solution" (fun _ ->
  setVersions "SolutionInfo.cs"
  build "ExampleRegistry")

Target.create "Test_Solution" (fun _ -> test "ExampleRegistry")

Target.create "Publish_Solution" (fun _ ->
  [
    "ExampleRegistry.Api"
  ] |> List.iter publish)

Target.create "Pack_Solution" (fun _ ->
  [
    "ExampleRegistry.Api"
  ] |> List.iter pack)

Target.create "Containerize_Api" (fun _ -> containerize "ExampleRegistry.Api" "api")
Target.create "PushContainer_Api" (fun _ -> push "api")

Target.create "Containerize_Projector" (fun _ -> containerize "ExampleRegistry.Projector" "projector")
Target.create "PushContainer_Projector" (fun _ -> push "projector")

// --------------------------------------------------------------------------------

Target.create "Build" ignore
Target.create "Test" ignore
Target.create "Publish" ignore
Target.create "Pack" ignore
Target.create "Containerize" ignore
Target.create "Push" ignore

"NpmInstall"
  ==> "DotNetCli"
  ==> "Clean"
  ==> "Restore_Solution"
  ==> "Build_Solution"
  ==> "Build"

"Build"
  ==> "Test_Solution"
  ==> "Test"

"Test"
  ==> "Publish_Solution"
  ==> "Publish"

"Publish"
  ==> "Pack_Solution"
  ==> "Pack"

"Pack"
  ==> "Containerize_Api"
  ==> "Containerize_Projector"
  ==> "Containerize"
// Possibly add more projects to containerize here

"Containerize"
  ==> "DockerLogin"
  ==> "PushContainer_Api"
  ==> "PushContainer_Projector"
  ==> "Push"
// Possibly add more projects to push here

// By default we build & test
Target.runOrDefault "Test"
