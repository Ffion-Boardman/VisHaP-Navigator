# Visual Hierarchy & Project Navigator - Tools Development
![Static Badge](https://img.shields.io/badge/Status%3A-Being_Added_To_Portfolio-yellow?style=flat-square&link=https%3A%2F%2Fwww.ffionboardman.com%2Fgame-projects%2Fsimple-scene)
![Static Badge](https://img.shields.io/badge/Unity-grey?style=flat-square&logo=unity)
![Static Badge](https://img.shields.io/badge/Version-6000.17.1f-lightgrey?style=flat-square&logo=unity)
![Static Badge](https://img.shields.io/badge/Language-C%23-lightgrey?style=flat-square&logo=csharp)
![Static Badge](https://img.shields.io/badge/Game_Type-Tool-lightgrey?style=flat-square)
![Static Badge](https://img.shields.io/badge/UI-lightgrey?style=flat-square)

Made for my 'Tools Development' module during my second year at The University of Staffordshire.  
Check out the [project-overview](https://www.ffionboardman.com/game-projects/pennies-for-the-path) of VisHaP Navigator on my portfolio!

## Programming Highlights

### Folder Colouring System
The Player Controller was intentionally designed to avoid using the Update() loop as much as possible, to practice optimization methods in my code.  
Colours: [FolderCustomiser_Colours.cs](Assets/VisHaP-Nav/Folder_Tool/FolderCustomiser_Colours.cs)

### Hierarchy Colouring System
This was my first time using Unity's UI Toolkit instead of the original UI system, with intentions of ensuring the UI is clean and dynamically created when the game is started.  
Colours: [HierarchyCustomiser_Colours.cs](Assets/VisHaP-Nav/Hierarchy_Tool/HierarchyCustomiser_Colours.cs)  
Editor: [HierarchyCustomiser_Editor.cs](Assets/VisHaP-Nav/Hierarchy_Tool/HierarchyCustomiser_Editor.cs)  
Settings: [HierarchyCustomiser_Settings.cs](Assets/VisHaP-Nav/Hierarchy_Tool/HierarchyCustomiser_Settings.cs)  

## Instillation
Simply drag and drop the zip file "VisHaP-Nav.zip" into your project, and extract its contents: it does not matter where in your project you do this.

## How-To: Folder Colouring
Right-click the folder you wish to colour (or select multiple colours if you wish) and select 'Folder Colour' from the context menu; it should be located just above the 'Properties...' option.
The folder tool allows you to use either a default colour from the designated list, or access more variations of those default colours through a standalone pop-up window. You can also create custom colours that will be saved into your 'Recent Colours' within this window.

## How-To: Hierarchy Stylizing
Right-click the game object within the scene view that you wish to customize, and select 'Add Hierarchy Style', it should be located just beneath the Prefab option.
From there, a customization component is automatically added to your game object, and pushed to the top of the components list (right beneath the transform component). You can customize the colour of the background, text, and formatting of that text as you wish.

