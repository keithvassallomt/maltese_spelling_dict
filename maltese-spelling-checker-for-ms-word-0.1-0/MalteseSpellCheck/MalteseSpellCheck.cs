using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Word = Microsoft.Office.Interop.Word;
using NHunspell;
using System.Windows.Forms;
using System.IO;

namespace MalteseSpellCheck
{
    public partial class MalteseSpellCheck
    {
        //The word application
        Word.Application wordApp;
        List<IgnoreWord> ignoreWords = new List<IgnoreWord>();
        List<IgnoreWord> ignoreAllWords = new List<IgnoreWord>();

        // għ and ie are treated as separate characters
        // A B Ċ D E F Ġ G H Ħ I
        // J K L M N O P Q R S T U V
        // W X Ż Z
        // a b ċ d e f ġ g h ħ i  
        // j k l m n o p q r s t u v
        // w x ż z
        char[] MalteseCharacters = new char[] { 
            '\u0041', '\u0042', '\u010A', '\u0044', '\u0045', '\u0046', '\u0120', '\u0047', '\u0048', '\u0126', '\u0049', 
            '\u004A', '\u004B', '\u004C', '\u004D', '\u004E', '\u004F', '\u0050', '\u0051', '\u0052', '\u0053', '\u0054', '\u0055', '\u0056',
            '\u0057', '\u0058', '\u017B', '\u005A',
            '\u0061', '\u0062', '\u010B', '\u0064', '\u0065', '\u0066', '\u0121', '\u0067', '\u0068', '\u0127', '\u0069',
            '\u006A', '\u006B', '\u006C', '\u006D', '\u006E', '\u006F', '\u0070', '\u0071', '\u0072', '\u0073', '\u0074', '\u0075', '\u0076',
            '\u0077', '\u0078', '\u017C', '\u007A'
        };

        // Any Maltese-specific punctuation would go here
        char[] MaltesePunctuations = new char[] {
        };

        char[] CommonEnglishPunctuations = new char[] {
            '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '=', '+', '{', '[', '}', ']', 
            '|', '\\', ':', ';', '"', '\'', '<', ',', '>', '.', '?', '/', '1', '2', '3', '4', '5', '6', '7',
            '8', '9', '0', '•', '‣', '‥', '…', '«', '»', '£', '¥', '©', '®', '¶', '·', '€'};

        private void MalteseSpellCheck_Load(object sender, RibbonUIEventArgs e)
        {
            //Get the word application object
            wordApp = Globals.ThisAddIn.Application;
        }

        private void btnSpellCheck_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                //Exit if there is no active document
                if (wordApp.ActiveDocument == null)
                    return;

                //Get all the words from the active document
                Word.Document doc = wordApp.ActiveDocument;

                #region Locate Dictionary files
                //Get the deployment directory
                System.Reflection.Assembly assemblyInfo = System.Reflection.Assembly.GetExecutingAssembly();

                //Location is where the assembly is run from 
                string assemblyLocation = assemblyInfo.Location;

                //CodeBase is the location of the ClickOnce deployment files
                Uri uriCodeBase = new Uri(assemblyInfo.CodeBase);
                string InstallationLocation = Path.GetDirectoryName(uriCodeBase.LocalPath.ToString());

                //Maltese dictionaries
                string affFile = Path.Combine(InstallationLocation, "mt_MT.aff");
                string dictFile = Path.Combine(InstallationLocation, "mt_MT.dic");
                #endregion

                //Loop through all the words to find the first mis-spell
                using (Hunspell hunspell = new Hunspell(affFile, dictFile))
                {
                    //Process all Paragraphs in the documents
                    Object oMissing = System.Reflection.Missing.Value;
                    object WdLine = Microsoft.Office.Interop.Word.WdUnits.wdLine; // change a line; 
                    object moveExtend = Microsoft.Office.Interop.Word.WdMovementType.wdExtend;

                    doc.ActiveWindow.Selection.HomeKey(Word.WdUnits.wdStory, ref oMissing);

                    foreach (Word.Paragraph para in doc.Paragraphs)
                    {
                        para.Range.Select();

                        //Set the repeatcheck and stopcheck
                        bool repeatcheck, stopcheck;

                        //Check Maltese Check
                        do
                        {
                            string selectedText = doc.ActiveWindow.Selection.Text;
                            string[] maltesewords = GetMalteseWords(selectedText);

                            CheckWords(doc, hunspell, selectedText, maltesewords, out stopcheck, out repeatcheck);

                            //Break if user selects to exit
                            if (stopcheck) return;
                        }
                        while (repeatcheck);
                    }

                    var shapes = doc.Shapes;
                    //Finds text within textboxes, then changes them
                    foreach (Microsoft.Office.Interop.Word.Shape shape in shapes)
                    {
                        shape.Select();

                        //Set the repeatcheck and stopcheck
                        bool repeatcheck, stopcheck;

                        //Check Maltese Check
                        do
                        {
                            //Get the selected text and Maltese words
                            string selectedText = shape.TextFrame.TextRange.Text;
                            string[] maltesewords = GetMalteseWords(selectedText);

                            CheckWords(doc, hunspell, selectedText, maltesewords, out stopcheck, out repeatcheck, "shape", shape);

                            //Break if user selects to exit
                            if (stopcheck) return;
                        }
                        while (repeatcheck);
                    }


                    MessageBox.Show("Maltese Spelling Check is complete", "Maltese Spell Check", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckWords(Word.Document doc, Hunspell hunspell, string selectedText, string[] maltesewords, out bool stopcheck, out bool repeatcheck, string objecttype = "", object wordobject = null)
        {
            int startposition = 0;
            Object oMissing = System.Reflection.Missing.Value;
            stopcheck = repeatcheck = false;

            //Check all the Maltese words from the selected line
            foreach (string malteseword in maltesewords)
            {
                DialogResult dialogResult = DialogResult.None;
                frmMaltese frmMaltese = null;
                String newMalteseWord = String.Empty;

                if (!hunspell.Spell(malteseword))
                {
                    if (!ignoreAllWords.Any(ignoreAllWord => ignoreAllWord.malteseword == malteseword))
                    {
                        if (!ignoreWords.Contains(new IgnoreWord { document = doc.Name, malteseword = malteseword, selectedText = selectedText, startposition = startposition, ignoreAll = false }))
                        {
                            Word.Range start = null;
                            Word.WdColorIndex highlightcolorindex = Word.WdColorIndex.wdNoHighlight;
                            Word.WdUnderline fontunderline = Word.WdUnderline.wdUnderlineNone;
                            Word.WdColor fontcolor = Word.WdColor.wdColorBlack;
                            Word.Range selectionRange = null;

                            //Select the erroneous word on the main document
                            if (String.IsNullOrWhiteSpace(objecttype))
                            {
                                //Set the initial selection
                                start = doc.ActiveWindow.Selection.Range;

                                //Set the search area
                                doc.ActiveWindow.Selection.Start += startposition;
                                Word.Selection searchArea = doc.ActiveWindow.Selection;

                                //Set the find object
                                Word.Find findObject = searchArea.Find;
                                findObject.ClearFormatting();
                                findObject.Text = malteseword;


                                //Find the mis-spelled word
                                findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                                //Temp store the current formatting
                                highlightcolorindex = doc.ActiveWindow.Selection.Range.HighlightColorIndex;
                                fontunderline = doc.ActiveWindow.Selection.Range.Font.Underline;
                                fontcolor = doc.ActiveWindow.Selection.Range.Font.UnderlineColor;

                                //Highlight the selection
                                doc.ActiveWindow.Selection.Range.HighlightColorIndex = Word.WdColorIndex.wdYellow;
                                doc.ActiveWindow.Selection.Range.Font.Underline = Word.WdUnderline.wdUnderlineWavy;
                                doc.ActiveWindow.Selection.Range.Font.UnderlineColor = Word.WdColor.wdColorRed;
                                selectionRange = doc.ActiveWindow.Selection.Range;
                                doc.ActiveWindow.Selection.Collapse();
                            }
                            else
                            {
                                if (objecttype == "table")
                                {
                                    start = ((Word.Cell)wordobject).Range;
                                }
                                else if (objecttype == "shape")
                                {
                                    start = ((Word.Shape)wordobject).TextFrame.TextRange;
                                    start.Start += startposition;
                                }

                                //Set the find object
                                Word.Find findObject = start.Find;
                                findObject.ClearFormatting();
                                findObject.Text = malteseword;

                                //Temp store the current formatting
                                highlightcolorindex = start.HighlightColorIndex;
                                fontunderline = start.Font.Underline;
                                fontcolor = start.Font.UnderlineColor;

                                //Find the mis-spelled word
                                findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                                //Highlight the selection
                                start.HighlightColorIndex = Word.WdColorIndex.wdYellow;
                                start.Font.Underline = Word.WdUnderline.wdUnderlineWavy;
                                start.Font.UnderlineColor = Word.WdColor.wdColorRed;
                                start.Select();
                            }

                            bool isObject = !String.IsNullOrWhiteSpace(objecttype);
                            frmMaltese = new frmMaltese(selectedText, malteseword, startposition, hunspell.Suggest(malteseword), isObject);
                            dialogResult = frmMaltese.ShowDialog();

                            //Select the line again
                            if (String.IsNullOrWhiteSpace(objecttype))
                            {
                                //Revert the highlights
                                selectionRange.Select();
                                doc.ActiveWindow.Selection.Range.HighlightColorIndex = highlightcolorindex;
                                doc.ActiveWindow.Selection.Range.Font.Underline = fontunderline;
                                doc.ActiveWindow.Selection.Range.Font.UnderlineColor = fontcolor;

                                if (dialogResult != DialogResult.Abort) start.Select();
                            }
                            else
                            {
                                start.HighlightColorIndex = highlightcolorindex;
                                start.Font.Underline = fontunderline;
                                start.Font.UnderlineColor = fontcolor;

                                if (dialogResult != DialogResult.Abort)
                                {
                                    if (objecttype == "table")
                                    {
                                        ((Word.Cell)wordobject).Select();
                                    }
                                    else if (objecttype == "shape")
                                    {
                                        ((Word.Shape)wordobject).Select();
                                    }
                                }
                            }
                        }
                    }
                }

                #region Cancel Button Clicked
                //Return if the user hits Cancel Button
                if (dialogResult == DialogResult.Cancel || dialogResult == DialogResult.Abort)
                {
                    stopcheck = true;
                    repeatcheck = false;
                    return;
                }
                #endregion

                #region Ignore or Ignore All Clicked
                //Ignore the word
                if (dialogResult == DialogResult.Ignore)
                {
                    if (frmMaltese.ignoreAll)
                    {
                        ignoreAllWords.Add(new IgnoreWord { malteseword = malteseword, ignoreAll = frmMaltese.ignoreAll });
                    }
                    else
                    {
                        ignoreWords.Add(new IgnoreWord { document = doc.Name, malteseword = malteseword, selectedText = selectedText, startposition = startposition });
                    }
                }
                #endregion

                #region Change or Change All Clicked
                if (dialogResult == DialogResult.Yes)
                {
                    if (String.IsNullOrWhiteSpace(objecttype))
                    {
                        //Set the initial selection
                        Word.Range start = doc.ActiveWindow.Selection.Range;

                        //Set the searcharea
                        if (frmMaltese.changeAll)
                        {
                            doc.Content.Select();
                        }
                        Word.Selection searchArea = doc.ActiveWindow.Selection;

                        //Set the find object
                        Word.Find findObject = searchArea.Find;
                        findObject.ClearFormatting();
                        findObject.Text = malteseword;
                        findObject.Replacement.ClearFormatting();
                        findObject.Replacement.Text = frmMaltese.selectedSuggestion;

                        object replaceAll = frmMaltese.changeAll ? Word.WdReplace.wdReplaceAll : Word.WdReplace.wdReplaceOne;

                        findObject.Execute(ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                            ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                            ref replaceAll, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                        newMalteseWord = frmMaltese.selectedSuggestion;

                        //Set back the selection
                        start.Select();

                        //Set repeatcheck to true
                        if (frmMaltese.changeAll)
                        {
                            stopcheck = false;
                            repeatcheck = true;
                            return;
                        }
                    }
                    else
                    {
                        var resultingText = selectedText.Replace(malteseword, frmMaltese.selectedSuggestion);

                        if (objecttype == "table")
                        {
                            Word.Range range = ((Word.Cell)wordobject).Range;
                            range.Text = resultingText;
                        }
                        else if (objecttype == "shape")
                        {
                            Word.Shape shape = (Word.Shape)wordobject;
                            shape.TextFrame.TextRange.Text = resultingText;
                        }

                        stopcheck = false;
                        repeatcheck = true;
                        return;
                    }
                }
                #endregion

                startposition += String.IsNullOrWhiteSpace(newMalteseWord) ? malteseword.Length : newMalteseWord.Length;
            }
        }

        private string[] GetMalteseWords(string selectedText)
        {
            string[] maltesewords =
                selectedText
                    .Split(new char[] { '\u200B', '\u200C', ' ', '\r', '\a', '.', '\t', '\v' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(malteseword => malteseword.Trim(MaltesePunctuations))
                    .Select(malteseword => malteseword.Trim(CommonEnglishPunctuations))
                    .Where(malteseword => String.IsNullOrWhiteSpace(malteseword) == false)
                    .Where(malteseword => MalteseCharacters.Contains(malteseword[0]))
                    .ToArray();
            return maltesewords;
        }
    }
}
