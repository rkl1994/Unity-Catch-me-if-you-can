using UnityEngine;

public class Menu : MonoBehaviour
{
    //This script displays a horizontal menu, with buttons that can be set by the user. Each button is actually
    //a prefab with a MenuItem script in it. take a look at the MENU folder to see those prefabs
    
    public GUISkin GUIskin; //The skin gui we'll use
    
    public int ButtonHeight = 100; //The height of each button, the width is set automatically to fill the screen with a margin set by ButtonMargin
    internal int ButtonHeightOffset = 0; //Used to offset the buttons in the beginning so that they later move into the scene
    
    public int ButtonMargin = 10; //The space between each two buttons
    
    public float MenuDelay = 2; //How much to wait before moving the menu into the scene
    internal float MenuDelayCount = 0; //The counter for MenuDelay
    
    public Transform[] MenuItems; //All the buttons, these are prefabs with MenuItem script in them. Take a look inside MENU folder to see them
    internal int MenuIndex = 0; //The current index of the menu items array
    
    void Start()
    {
    	ButtonHeightOffset = -ButtonMargin; //Move the buttons a little up, by the value of the margin
    }
    
    void OnGUI()
    {
    	//Animate the entry of the menu items, first by waiting a few seconds and then moving them in to the scene
    	if ( MenuDelayCount < MenuDelay )
    	{
    	   MenuDelayCount += Time.deltaTime; //Count up to the value of MenuDelay
    	}
    	else if ( ButtonHeightOffset < ButtonHeight ) 
    	{
    		ButtonHeightOffset -= (int)((ButtonHeightOffset - ButtonHeight) * 0.1f); //Gradually change the value of ButtonHeightOffset to ButtonHeight
     	}
    	
    	GUI.skin = GUIskin; //The skin gui we'll use
    	
    	//Draw all the buttons from the Menu item array
    	for ( MenuIndex = 0 ; MenuIndex < MenuItems.Length ; MenuIndex++ )
    	{
    		//draw a button for each item
    		if ( GUI.Button (new Rect(ButtonMargin + ((Screen.width - ButtonMargin)/MenuItems.Length) * MenuIndex ,Screen.height - ButtonHeightOffset - ButtonMargin,(Screen.width - ButtonMargin * 2)/MenuItems.Length - ButtonMargin,ButtonHeight ), MenuItems[MenuIndex].GetComponent<MenuItem>().MenuItemName ) )
    		{
    			if ( MenuItems[MenuIndex] ) //if the item exists
    			{			
    				MenuItems[MenuIndex].GetComponent<MenuItem>().RunMenuItem(); //Run the menu item function which is inside a MenuItem script component attached to a prefab
    			}
    		}
    	}
    }
}
