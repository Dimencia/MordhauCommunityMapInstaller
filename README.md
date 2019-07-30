# MordhauCommunityMapInstaller
![Screenshot](https://imgur.com/nZaFaAu.png)

This application interfaces with a server hosting all approved Community maps, and provides a list for users to pick which maps they would like to download

Maps are downloaded and installed with one click, helping users to avoid confusion finding Mordhau paths to put the maps in

If your Mordhau path cannot be found via registry, you will be prompted to set it by browsing to your Mordhau folder

@D.Mentia#0614 on Discord if you have any questions or find any bugs or problems

## Installation Instructions

Full, thorough instructions with screenshots here: https://imgur.com/a/449guZF

Download MCMIx.x.x.x.exe from above, or under the Releases tab, and run it

If you are missing any dependencies it should let you know; if not, an installer is available on Discord to automatically retrieve them.  This will be posted publicly if necessary, otherwise you may need to contact Moderators or @D.Mentia#0614 on Discord

If you want to try compiling it yourself for security, download and extract all files, open MordhauMapInstaller.sln in Visual Studio 2017.  Go to Project -> MordhauMapInstaller Properties, Signing, and disable all checkboxes.  You can then Build -> Build Solution.  The compiled exe will be in MordhauMapInstaller/bin/Release, and you can ignore any other files in there.


## Important information for mapmakers

Preview image minimum size: 698, 146

Note that preview images can, and should be, bigger than this - they will be centered and cropped

**Your zip file name and folder name should exactly match your map's base in-game name, and this is what you enter for FolderName in the info file**

If your map has multiple versions, such as SKM_mapname, you can mention them in your Description, but your folder/zip names should still match the base map

### Map Updates

Players can join a server that has a new version of a map, even if they have an old version.  This can cause problems if the new version has models that have been moved or added, since the client will collide with them but not see them.  

With that in mind, maps receiving minor cosmetic updates can have the same map name, and the version should be increased - the installer will let users know if the map needs to be updated (but only if they actually run it)

Maps receiving major changes such as moving, adding, or removing collision objects should be uploaded under a new name, such as MapNamev2, so that players with an old version can't join them

This is just a suggestion, of course, but your players may otherwise have problems if they have an older version and join a server hosting a newer one, if the names are the same


### Info file format

Check any existing maps for info.txt files to get an example

Note that all lines are **required** and the installer will not show your map if they are not there

If you wish to skip any field you can leave the line blank.  If you want to add a thumbnail but not SuggestedPlayers, for example, just leave a blank line where SuggestedPlayers should be

Filename: FolderName.info.txt

Each of these parameters on newlines, in order




>MapName (Can include spaces, readable name for display purposes only)

>FolderName (Specific name of the zip file and folder and in-game mapname)

>MapDescription (Can include links that will be clickable)

>MapAuthor(s)

>MapVersion

>ReleaseDate (In format: dd/mm/yyyy)

>FileSize (Unzipped size of all files)

>SuggestedPlayers (Set low for small maps)

>Thumbnail URL

