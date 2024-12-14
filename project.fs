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
    Regex.Matches(text, @"[.!?](\s|$)").Count

// Function to count paragraphs
let countParagraphs (text: string) =
    text.Replace("\r\n", "\n")
        .Replace("\r", "\n")
        .Trim() // Remove leading/trailing newlines or spaces
        .Split([|"\n\n"|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.filter (fun paragraph -> not (String.IsNullOrWhiteSpace(paragraph)))
        |> Array.length
