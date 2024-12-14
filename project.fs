module Program.fs
open System
open System.IO
open System.Windows.Forms

// Function to count words
let countWords (text: string) =
    text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; '!'; '-'; ':'; ';'|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map (fun word -> word.Trim([|'.'; ','; '!'|]))
    |> Array.filter (fun word -> not (String.IsNullOrWhiteSpace(word)))
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
        
// Function to calculate word frequency
let wordFrequency (text: string) =
    text.Split([|' '; '\t'; '\n'; '\r'; '.'; ','; '!'; '-'|], StringSplitOptions.RemoveEmptyEntries)
    |> Seq.map (fun word -> word.Trim([|'.'; ','; '!'|]).ToLower())
    |> Seq.filter (fun word -> not (String.IsNullOrWhiteSpace(word)))
    |> Seq.groupBy id
    |> Seq.map (fun (word, instances) -> word, Seq.length instances)
    |> Seq.sortByDescending snd
// Function to calculate average sentence length
let averageSentenceLength (text: string) =
    let totalWords = countWords text
    let totalSentences = countSentences text
    if totalSentences > 0 then totalWords / totalSentences else 0

  // GUI Setup
let form = new Form(Text = "Text Analyzer", Width = 600, Height = 400)
let inputTextBox = new TextBox(Multiline = true, Dock = DockStyle.Top, Height = 200)
let analyzeButton = new Button(Text = "Analyze", Dock = DockStyle.Top)
let loadFileButton = new Button(Text = "Load File", Dock = DockStyle.Top)
let resultsTextBox = new TextBox(Multiline = true, ReadOnly = true, Dock = DockStyle.Fill)

// Add controls to form
form.Controls.Add(resultsTextBox)
form.Controls.Add(analyzeButton)
form.Controls.Add(loadFileButton)
form.Controls.Add(inputTextBox)

// Event Handlers
loadFileButton.Click.Add(fun _ ->
    let openFileDialog = new OpenFileDialog(Filter = "Text Files|*.txt")
    if openFileDialog.ShowDialog() = DialogResult.OK then
        inputTextBox.Text <- File.ReadAllText(openFileDialog.FileName))

analyzeButton.Click.Add(fun _ ->
    let text = inputTextBox.Text
    let wordCount = countWords text
    let sentenceCount = countSentences text
    let paragraphCount = countParagraphs text
    let avgSentenceLength = averageSentenceLength text
    let wordFreq = 
        wordFrequency text 
        |> Seq.map (fun (w, c) -> sprintf "%s: %d" w c) 
        |> String.concat Environment.NewLine

    resultsTextBox.Text <- 
        String.Join(Environment.NewLine, 
            [|
                sprintf "Words: %d" wordCount
                sprintf "Sentences: %d" sentenceCount
                sprintf "Paragraphs: %d" paragraphCount
                sprintf "Average Sentence Length: %.2f" avgSentenceLength
                "Word Frequency:"
                wordFreq
            |])
)

// Run Application
[<STAThread>]
do Application.Run(form)
