module Program.fs
open System
open System.IO
open System.Windows.Forms

// Function to count words
let countWords (text: string) =
    text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; '!'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.length
// Function to count sentences
let countSentences (text: string) =
    text.Split([|'.'; '!'; '?'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.length
