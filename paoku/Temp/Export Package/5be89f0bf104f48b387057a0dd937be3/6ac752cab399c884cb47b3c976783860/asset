using UnityEngine;

public class MenuItem : MonoBehaviour
{
    //This script is used for a button to be displayed through the Menu script. In it you can set one of three types of scripts
    //you can load a level, or Open a URL, or load a component by instantiating the prefab this button it attached to
    
    public string MenuItemName = "More Games"; //The text that appears on the button
    
    public bool LoadLevel = false; //Does it load a level?
    public string LevelName = string.Empty; //The name of the level it loads
    
    public bool LoadURL = true; //Does it open a URL
    public string URLName = "http://activeden.net/category/unity-3d"; //The name of the URL it opens
    
    public bool LoadComponent = false; //Does it create a component
    internal GameObject LoadedComponent; //a copy of the created component
    
    public void RunMenuItem()
    {
    	if ( LoadLevel == true && LevelName != "" )
    	{
    		Application.LoadLevel(LevelName); //load a level
    	}
    	else if ( LoadURL == true && URLName != "" )
    	{
    		Application.OpenURL(URLName); //open a url
    	}
    	else if ( LoadComponent == true )
    	{
    		DestroyObject(LoadedComponent); //destroy the previous instance of this object, if it has already been created
    		
    		LoadedComponent = Instantiate(transform, Vector3.zero, Quaternion.identity) as GameObject; //create an instance of this object	
    	}
    }
}
