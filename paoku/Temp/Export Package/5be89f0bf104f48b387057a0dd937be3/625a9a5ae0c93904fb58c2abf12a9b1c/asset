using UnityEngine;

public class Instructions : MonoBehaviour
{
    //This script just displays a box with some instructions in it, set from the inspector
    public GUISkin GUIskin; //The skin gui we'll use
    
    //You can set the text yourself from the inpspector
    public string InstructionsText = "Move with the mouse \n Hold the mouse button to jump \n Hold longer to jump higher \n Click Twice to Double Jump \n Collect Gems, avoid obstacles \n and don’t fall!";
    
    void OnGUI()
    {
    	GUI.skin = GUIskin; //The skin gui we'll use
    	
    	//Create a box with the instructions text in it
    	GUI.Box(new Rect(Screen.width * 0.04f, Screen.height * 0.05f, Screen.width * 0.92f, Screen.height * 0.7f), InstructionsText);
    }
}
