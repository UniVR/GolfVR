/*
*	Splatmap Helper v1.0 by Matt Gadient
*	http://mattgadient.com/2014/09/28/unity3d-a-free-script-to-convert-a-splatmap-to-a-png/
*
*	Use at your own risk. No warranty (expressed or implied). You're on your own if this results in the loss of data, the death of all mankind, or anything in between.
*
*	Quick Instructions:
*
*		MAKE SURE YOU HAVE A RECENT BACKUP!!!!
*
*		This must be placed in an editor folder (like your Assets/Editor folder)!
*		A few menu options will appear in the Unity3D Editor under "Terrain/SplatmapHelper".
*
*		TOOL #1 - Converts splatmap->PNG
*					-Select the Unity Terrain Splatmap from your asset folder (often a child of an asset called something like "New Terrain 1.asset")
*					-Run the tool. Read the warnings.
*					-It will ask you for a location for the new PNG. If overwriting an old PNG, you may need the read/write flag enabled int it's texture first.
*					-Note that your new PNG will lack the alpha channel.
*
*		TOOL #2 - Converts a PNG to splatmap "format"
*					-This is required to restore the alpha channel.
*					-Select the PNG you've worked on/adjusted.
*					-Run the tool. Read the warnings.
*					-It will ask you for a location for the splatmap-ready PNG. If overwriting an old one (or the current one), you may need to set read/write.
*
*		TOOL #3 - Puts a "splatmap ready" PNG into a Unity Terrain's splatmap (overwriting it).
*					-Make sure you've run tool#2 first if you've done any editing (otherwise it will probably show up all white).
*					-Select the splatmap (just like in #1).
*					-Run the tool. Read the warnings.
*					-It will ask you for the location of the splatmap-ready PNG you created in #2.
*					-NOTE THAT THE FILE BROWSER MAY SHOW IT AS TRANSPARANT. DON'T PANIC. You can verify it looks "right" from within Unity if you want
*					 (browsing to it from the assets folder in Unity, it will usually show the Red/Green/Blue correct).
*
*	Credit/thanks to:
*		-Kragh of the Unity3D forum for posting some C# code that handles the transparancy issue in a nifty way ( http://forum.unity3d.com/threads/encode-to-png.214663/#post-1439634 ).
*		-VivienS for a UnityAnswers comment that aided in putting this together as well ( http://answers.unity3d.com/questions/13479/exporting-terrain-splatmap.html ).
*		-The many others who ask/answer questions on the Unity forums on a regular basis.
*
*
*/

#pragma strict
import System.IO;

@MenuItem("Terrain/Splatmap Helper/0 - Instructions - CLICK FIRST TO READ")
static function Info () {
 
	EditorUtility.DisplayDialog("The gist... (1/2)", "This tool is designed to help with basic conversion from a Splatmap<->PNG and back again. Note that it can easily destroy your terrain's splatmap if something goes wrong, so BACK UP your project first.\n\nTool #1 - Select your splatmap from your assets folder (a child under what usually looks like 'New Terrain 1.asset'), and it will ask you for a location in which to make a new PNG.\n\nTool #2 - Select a PNG you have worked on, and this will convert it to a splatmap-friendly format.\n\nTool #3 - Select your splatmap (similar to #1), and it will ask you for a PNG (that you probably ran through #2) to copy over your splatmap.\n\nCaveats are shown on the next page, but more info can be found at http://mattgadient.com/2014/09/28/unity3d-a-free-script-to-convert-a-splatmap-to-a-png/", "Next");
	EditorUtility.DisplayDialog("The gist... (2/2)", "There are some caveats to watch for, because a splatmap puts the 4 texture strengths in the 4 color channels (1 each in R,G,B,A).\n\n1) Most image editors don't like this, so this tool (TOOL #1) sets that 'A' channel to full. While this basically destroys any splat info about your 4th texture, it makes it possible to work on and edit the first 3 channels of the PNG or otherwise cut/rotate/edit/modify the thing. When you're done editing the PNG, TOOL #2 will try to recalculate the 'A' channel based on what R,G,B are (normally they should add up to 1.0).\n\n2) Due to the above, after you've finished with the PNG and run #2 to get the transparancy added again, the image MAY LOOK TRANSPARANT in your file browser (so don't panic on step #3 when you browse for the file and see it's transparant).\n\n3) During TOOL #3, you may need to set the READ/WRITE flag for the PNG texture so that it can read from it. This is done by selecting the PNG, choosing 'Advanced' for the texture type, and enabling the flag. If you choose to overwrite any previous files in #1 or #2, you may need to do so as well.\n\nMore info at http://mattgadient.com/2014/09/28/unity3d-a-free-script-to-convert-a-splatmap-to-a-png/", "OK");
}

@MenuItem("Terrain/Splatmap Helper/1 - Save Selected Splatmap to PNG")
static function CreatePNG () {
 
	if (!EditorUtility.DisplayDialog("Saving a Splatmap to PNG - How this works...", "BACK UP YOUR PROJECT FIRST (at the very least, your terrain...). This will let you choose a file to save a PNG of your selected splatmap. Note that the 4th texture in your splatmap (if it exists) is normally stored in the transparant layer. This will convert that layer to opaque (1.0) so it can be edited in Photoshop/etc.\n\nThis means that the 4th texture's splatmap position basically will NOT EXIST in the PNG.\n\nWhen you are finished editing, one of the other helper options can recalculate the 4th channel by subtracting the strengths of the first 3.\n\nIf you wish to proceed, make sure you have selected the splatmap (usually a child object of your terrain in the assets folder).", "Continue", "Cancel")) {
		return;
	}
	
	var texture1 : Texture2D = Selection.activeObject as Texture2D; 
	if (texture1 == null) { 
	   EditorUtility.DisplayDialog("Select A Splatmap", "You need to select a texture first (the splatmap).", "OK"); 
	   return; 
	} 
  
	var path = EditorUtility.SaveFilePanelInProject("Save Splatmap as PNG", "splatmap-to-png", "png", "Select the location to save the new PNG splatmap.");
	if (path.Length != 0) {
	
		var texture = Instantiate(texture1) as Texture2D;
		var textureColors = texture.GetPixels();
		for (var i : int = 0; i < textureColors.Length; i++) {
			textureColors[i].a = 1;
		}
			 
		texture.SetPixels(textureColors);
		texture.Apply();

		if(texture.format != TextureFormat.ARGB32 && texture.format != TextureFormat.RGB24) {
			var newTexture = Texture2D(texture.width, texture.height);
			newTexture.SetPixels(texture.GetPixels(0),0);
			texture = newTexture;
		}

		var bytes = texture.EncodeToPNG(); 
		File.WriteAllBytes(path, bytes); 
		AssetDatabase.Refresh();
   }
}

@MenuItem("Terrain/Splatmap Helper/2 - Turn PNG Back Into Splatmap Format")
static function Fix() {

	if (!EditorUtility.DisplayDialog("Turning a PNG into a splatmap - How this works...", "BACK UP YOUR PROJECT FIRST (at the very least, your terrain...).\n\nIf you have edited a PNG, you might get lucky and it may plunk into whatever terrain system you're using and look correct.\n\nIf not, this utility will try to recalculate the 4th texture channel of the splatmap (based on the strengths of the first 3), and re-save the file.\n\nNote that it will OVERWRITE your exiting PNG with the new data so make sure you have an extra copy of it somewhere!!!\n\nBefore this works, you wil have to select your PNG's texture file, choose 'Advanced', and set Read/Write to 'enabled'.\n\nIf you already selected the PNG you want to modify (and have a backup!), you can continue. Otherwise, hit cancel.", "Continue", "Cancel")) {
		return;
	}

	var texture1 : Texture2D = Selection.activeObject as Texture2D;
  	if (texture1 == null) { 
	      EditorUtility.DisplayDialog("Select the PNG First!", "You need to select the PNG from your assets first! Do so, and then try again.", "OK"); 
	      return; 
	   } 
	var texture2 = Instantiate(texture1) as Texture2D;

	if(texture2.format != TextureFormat.ARGB32 && texture1.format != TextureFormat.RGB24) {
			var newTexture = Texture2D(texture2.width, texture2.height);
			newTexture.SetPixels(texture2.GetPixels(0),0);
			texture2 = newTexture;
			texture2.Apply();
	}
		
	var textureColors : Color[] = texture2.GetPixels();

	for (var i : int = 0; i < textureColors.Length; i++) {
		textureColors[i].a = 1.0-(textureColors[i].r+textureColors[i].g+textureColors[i].b); //Your [B]a[/B] components will be set to whatever is leftover from the other components, when summed)
	}
	 
	texture2.SetPixels(textureColors );
	texture2.Apply();

	var path = EditorUtility.SaveFilePanelInProject("Location for new Splatmap", "new-splatmap", "png", "Select the location to save the new PNG splatmap.");
	if (path.Length != 0) {
			var bytes = texture2.EncodeToPNG();
			File.WriteAllBytes(path, bytes); 
			AssetDatabase.Refresh();
			EditorUtility.DisplayDialog("Done!", "If it worked, the location of the PNG should show up in your debug log, and there should be no errors.\n\nIf you tried overwriting the original and didn't have read/write flags set, it may not have worked. Either save to a new location, or enable those flags.", "OK");
	} else {
		
	}

//  Original version - not used, as it requires the original to be marked read/write and people might not expect the original to be overwritten anyway.
//	If you want to modify this to save into your original file by default instead of asking, you'll have to pull some stuff from lines #121-124 and modify a litttle.
//	Debug.Log("Writing to " + AssetDatabase.GetAssetPath(texture1));
// 	File.WriteAllBytes(AssetDatabase.GetAssetPath(texture1), bytes); 

}

@MenuItem("Terrain/Splatmap Helper/3 - Replace Unity Terrain Splatmap With New PNG")

static function Replace() {

	if (!EditorUtility.DisplayDialog("Replacing your existing Splatmap with a PNG - How this works...", "BACK UP YOUR PROJECT FIRST (at the very least, your terrain...).\n\nThis will attempt to replace the Unity Terrain Splatmap (that you hopefully selected already) with a new PNG.\n\nThe new PNG must have the read/write flag turned on (select the PNG, then change 'Texture Type' to 'Advanced' to set it).\n\nYou should have already backed up your existing terrain data (both the gameobject, and the terrain ASSET in your assets folder - you really SHOULD back up your whole project just in case...) - I can not stress how important that is, because if something goes wrong, YOUR TERRAIN MIGHT BE TOAST.\n\nIf you're ready and already selected the Terrain's splatmap (from your ASSET folder - a child of your terrain asset), press continue and it will ask you for the location of the PNG.", "Continue", "Cancel")) {
		return;
	}
	if (!EditorUtility.DisplayDialog("REMINDER!", "You should have selected the terrain's splatmap already from your assets folder. It will ask you for the NEW PNG right away. The order is important!.", "OK", "Cancel")) {
		return;
	}

	var texture : Texture2D = Selection.activeObject as Texture2D; 
		if (texture == null) { 
			EditorUtility.DisplayDialog("Select The Splatmap", "You need to select the terrain's splatmap from your asset folder first (a child of your terrain asset). Try again once you have done so.", "OK"); 
		    return; 
	 	} 
  
	var path = EditorUtility.OpenFilePanel("Select the PNG to copy over your splatmap.", "", "png");
	if (path.Length != 0) {
	
		var textureColors = texture.GetPixels();

		Debug.Log ("path was " + path);
		var pngtexture = WWW("file:///" + path);

		pngtexture.LoadImageIntoTexture(texture);
		texture.Apply();
	} else {
		EditorUtility.DisplayDialog("Cancel or Error", "Either you hit cancel, or there was an error attempting to read the file (check the debug log - you may need to set your PNG's read/write flag by choosing 'Texture Type - Advanced' so that it can be read.)", "OK");
		return;
	}

	AssetDatabase.Refresh();
	EditorUtility.DisplayDialog("Done!", "If it worked, there should be no errors in your debug log, and hopefully the terrain will look correct after you hit OK.\n\nIf the splatmap took the texturebut it is ALL WHITE, you may need to run the 2nd step on the texture first, and do this again after.\n\nAsssuming all went well, you may want to select the PNG and disable the read/write flags now.", "OK");


}
