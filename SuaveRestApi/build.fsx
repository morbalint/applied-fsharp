#r "paket:
nuget Fake.Core.Target //"
// include Fake modules, see Fake modules section
#load "./.fake/build.fsx/intellisense.fsx"

open Fake.Core

// *** Define Targets ***

// Default target
Target.create "Default" (fun _ -> Trace.trace "Hello World from FAKE")

// *** Start Build ***
Target.runOrDefault "Default"
// // *** Define Targets ***
// Target.create "Clean" (fun _ -> Trace.log " --- Cleaning stuff --- ")
// Target.create "Build" (fun _ -> Trace.log " --- Building the app --- ")
// Target.create "Deploy" (fun _ -> Trace.log " --- Deploying app --- ")
// open Fake.Core.TargetOperators
// // *** Define Dependencies ***
// "Clean" ==> "Build" ==> "Deploy"
// // *** Start Build ***
// Target.runOrDefault "Deploy"







