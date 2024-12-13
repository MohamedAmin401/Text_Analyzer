module project.fs

open System
open System.IO
open System.Text.RegularExpressions

// load the text from file
let loadText (filePath: string) : string =
    try
        File.ReadAllText(filePath)
    with
    | :? FileNotFoundException -> 
        printfn "File not found: %s" filePath
        ""
    | :? UnauthorizedAccessException ->
        printfn "Access denied to file: %s" filePath
        ""
    | ex ->
        printfn "An error occurred: %s" ex.Message
        ""

// compute the number of words 
let countWords (text: string) : int =
    text.Split([|' '; '\n'; '\t'; ','; '.'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.length