# EveExporter

Yet another tool for extracting files from Eve Online.

<p float="left">
  <img src="https://github.com/BiekerUdan/EveExporter/blob/master/screenshot1.png" width="250" />
  <img src="https://github.com/BiekerUdan/EveExporter/blob/master/screenshot2.png" width="250" /> 
</p>

## How do I use this?
Install Eve Online.
Download and extract a zipfile of this tool from the releases page, or compile it from source.
Run EveExporter.exe
If your Eve installation is not in C:\EVE then use the settings button to find your "ResFiles" directory and your "resfilesindex.txt"
Browse the tree and export your files.

You may find there are lots of files that do not exist in your cache (they are downloaded dynamically by the game when you are playing).  You have 2 options to solve this problem.
1. Download the "old" launcher.  As of last month the "old" version of the EvE Launcher was still available for download and it has an option to allow you to "download full eve client" or something like that.
2. Log into the game, search for the asset with the global search tool, click on the magnifing glass icon or the "information" icon to bring up a 3d model of that asset and zoom in on it.  This should cause the game to download it.

The next version of this tool will be better at identifing which files are missing.

## Current Features

1. Preview ablilty for most image formats including most DDS formats
2. Preview for .webm videos
3. Optional ability to filter out 'lowdetail' files when bulk exporting
4. Optional ability to convert DDS files to uncompressed .png during export
5. Export individual files or entire recursive trees

## Currently in development
I have been working on building a Granny2 exporter in C# to go with this but it is taking longer that I would have liked.  I have been using the Noesis GR2 plugin as a reference because it is the only tool I have found source code for that has the ability to export the face groupings to OBJ in a way that allows for proper assignment of textures without a lot of manual work.  So hopefully someday soon this tool will have the ability to export GR2 to OBJ.

A side effect of that will be the ability to preview the 3d models in this tool.

## Why bother?

I want to be able to use Eve Online assets in UE5 for a variety of personal projects and I found the existing workflows for extracting them were somewhat cumbersome.  If you are interested Eve related UE5 work please get in touch!


# Contributions and feedback are welcome.
This software is released under the MIT software license.
