Maltese Spelling Dictionary
===========================

| Info        | Value           |
| ------------- |:-------------|
| Word List Author      | Original list by Ramon Casha. Updated list by [Ġabra Project](https://mlrs.research.um.edu.mt/resources/gabra) |
| Repackaged By      | Keith Vassallo (keith@spelling.mt)      |  
| Website | [https://spelling.mt](https://spelling.mt)      |
| Number of Words / Word Forms | 3,686,295      |
| Version | 0.80      |

INSTALLATION
============

LibreOffice/OpenOffice
----------------------

This is probably the easiest way to use the Maltese spelling dictionary, and works on
Linux, macOS and Windows.

Note: Requires LibreOffice/OpenOffice 3.2 or newer.

1) Open LibreOffice/OpenOffice.
2) **Tools** > **Extension Manager**
3) Click **Add**.
4) Browse to this folder/libreoffice-mt and click on **maltese-dictionary.oxt**
5) Restart LibreOffice/OpenOffice when prompted.

To create a document in Maltese

1) **Tools** > **Language** > **For Paragraph**.
2) From the language drop-down list choose **Maltese**.

Microsoft Office on Windows
---------------------------

An Add-In is available for Microsoft Word that allows spell checking capabilities.

To install:

1) Go to the **msoffice-mt** folder, then open **MalteseSpellCheckSetup.msi** or **MalteseSpellCheckSetup64.msi** depending on whether you have a 32-bit or 64-bit Microsoft Office installation.
2) Follow installation instructions. 

Linux System-Wide
-----------------

This enables Maltese system-wide spellchecking on the system.

1) In a terminal, run:
```shell
sudo apt install hunspell  # Debian-based systems
sudo yum install hunspell  # Red Hat-based systems
```
2) Copy the files in the **hunspell-mt** folder of this dictionary to the system library folder:
```shell
sudo cp "/path/to/Maltese_Spellcheck/hunspell-mt/*" "/usr/share/hunspell/"
```
To use Maltese spell checking in an app, please consult that app's documentation.

macOS System-Wide
-----------------

This enables Maltese system-wide spellchecking on the system.

1) Install HomeBrew from [https://brew.sh](https://brew.sh)
2) In a terminal run:
```shell
brew install hunspell
sudo mkdir /Library/Spelling/
```
3) Copy the files in the **hunspell-mt** folder of this dictionary to the system library folder:
```shell
sudo cp "/path/to/Maltese_Spellcheck/hunspell-mt/*" "/Library/Spelling/"
```
To use Maltese spell checking in an app, please consult that app's documentation.


MORE INFORMATION
================

For more information and detailed documentation, see [https://spelling.mt](https://spelling.mt)
