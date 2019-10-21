Maltese Spelling Dictionary
===========================

Word list by:		  Ramon Casha (late)
Repackaged by:		Keith Vassallo (keithv@me.com)
Website:        	https://spelling.mt
Number of words:	~530,000
Version:		      0.50-1

INSTALLATION
============

LibreOffice/OpenOffice
----------------------

This is probably the easiest way to use the Maltese spelling dictionary, and works on
Linux, macOS and Windows.

Note: Requires LibreOffice/OpenOffice 3.2 or newer.

1) Open LibreOffice/OpenOffice.
2) Tools > Extension Manager
3) Click 'Add'.
4) Browse to this folder/libreoffice and click on maltese-dictionary.oxt
5) Restart LibreOffice/OpenOffice when prompted.

To create a document in Maltese

1) Tools > Language > For Paragraph.
2) From the language drop-down list choose Maltese.

Linux System-Wide
-----------------

This enables Maltese system-wide spellchecking on the system.

1) In a terminal, run:

sudo apt install hunspell (on Debian-based systems)
sudo yum install hunspell (on Red Hat-based systems)

2) Copy the files in the hunspell folder of this dictionary to the system library folder:

sudo cp /path/to/Maltese_Spellcheck/hunspell*/* /usr/share/hunspell/

To use Maltese spell checking in an app, please consult that app's documentation.

macOS System-Wide
-----------------

This enables Maltese system-wide spellchecking on the system.

1) Install HomeBrew from https://brew.sh
2) In a terminal run:

brew install hunspell
sudo mkdir /Library/Spelling/

3) Copy the files in the hunspell folder of this dictionary to the system library folder:

sudo cp /path/to/Maltese_Spellcheck/hunspell*/* /Library/Spelling/

To use Maltese spell checking in an app, please consult that app's documentation.


MORE INFORMATION
================

For more information and detailed documentation, see https://spelling.mt
