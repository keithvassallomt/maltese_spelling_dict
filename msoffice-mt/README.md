Maltese Spelling Checker for Microsoft Word 2010-2019
=====================================================

A Maltese spelling checker Addin for Microsoft Word 2010-2019 using NHunspell (https://sourceforge.net/projects/nhunspell/).

All add-in code is based on the SBBIC Khmer spelling checker for Microsoft Word, from http://www.sbbic.org/2014/08/08/sbbic-khmer-spelling-checker-microsoft-word/

Download
========
To download the completed Addin, visit: https://spelling.mt

How to Contribute and Build
==============
With the Open Source Visual Studio Community Edition 2019, you can now contribute and add to the existing features of Maltese Spell Checker for Microsoft Word.

To start contributing, please install the following tools:

1. Visual Studio Community 2019: Download and Install Visual Studio Community 2019 from the below link. https://www.visualstudio.com/products/visual-studio-community-vs

2. Visual Studio Installer Project Extension: Download and Install the Visual Studio Installer Project Extension from the below link. https://marketplace.visualstudio.com/items?itemName=visualstudioclient.MicrosoftVisualStudio2017InstallerProjects

3. Office Developer Tools: From the below link, download and install the Office Developer Tools by clicking on “2 Get Office Developer Tools” button. https://www.visualstudio.com/en-us/features/office-tools-vs.aspx

Then once these are installed, download the source by cloning in Desktop or downloading as a ZIP.

4. Once you have the source go to “MalteseSpellCheck\bin\Debug” directory and replace the “mt_MT.aff” and “mt_MT.dic” files. Please make sure you do not change the file names

5. Double-Click “MalteseSpellCheck.sln” file to open the project in Visual Studio.

6. The solution explorer looks similar to what is shown below. We now would need to rebuild each project in the solution explorer.

7. To rebuild a project, right-click on “MalteseSpellCheck” and click Rebuild. Repeat the steps for “MalteseSpellCheckSetup” and “MalteseSpellCheckSetup64”.

8. Finally, for 32-bit office, go to “MalteseSpellCheckSetup\Debug” folder to get the setup files. And for 64-bit office, go to “MalteseSpellCheckSetup64\Debug” folder to get new setup files.

License
=======
GNU GENERAL PUBLIC LICENSE v2