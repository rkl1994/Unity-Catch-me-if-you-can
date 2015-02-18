using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    //This script handles the leveling up in the game and the score and distance records. In each level up the value ranges for platforms are changed ( platform length/height/width/rotate etc )

    internal GameObject Player; //Used to hold the player object, if it's in the scene
    internal GameObject PlatformCreator; //Used to hold the Platform Creator object, if it's in the scene

    public int CurrentLevel = 1; //The current level we are at
    public int DistanceToLevelUp = 100; //How much more distance do we have to pass to get to the next level. This value is added up to the previous value in each level, meaning that to pass level 1 you need
    //to get to 100m, and to pass level 2 you have to get to 300 ( Distance To Level Up multiplied by the Current Level plus the distance passed so far ), to pass level 3 you need to get to 600 ( 100 * 3 + 300 ), and so on

    public string[] LevelUpText; //An array holding several texts that will be randomly displayed when you pass a level
    internal string CurrentLevelUpText; //The current level up text
    internal bool LevelUp = false; //Are we leveling up? Used to know when to animate the level up text passing across the screen

    public int LevelUpRumble = 200; //How much to shake the screen when you level up

    public float MaxSpeedChange = 0.1f; //How much to change the value of MaxSpeed, which is in the PlayerControls script
    public float PlatformWidthChange = 0.05f; //How much to change the value of PlatformWidthRange, which is in the PlatformCreator script
    public float PlatformLengthChange = 0.05f; //How much to change the value of PlatformLengthRange, which is in the PlatformCreator script
    public float PlatformHeightChange = 0.05f; //How much to change the value of PlatformHeightRange, which is in the PlatformCreator script
    public float PlatformGapChange = 0.05f; //How much to change the value of PlatformGapRange, which is in the PlatformCreator script
    public float PlatformRotateChange = 0.05f; //How much to change the value of PlatformRotateRange, which is in the PlatformCreator script
    public float PlatformShiftChange = 0.05f; //How much to change the value of PlatformShiftRange, which is in the PlatformCreator script
    public float ObstacleRateChange = 0.05f; //How much to change the value of ObstacleRate, which is in the PlatformCreator script
    public float GemRateChange = 0.05f; //How much to change the value of GemRate, which is in the PlatformCreator script

    public GUISkin GUIskin; //The skin gui we'll use
    public Texture2D Gems; //a 2D image that is palced beside the gems score count

    internal float LevelUpPosX = -Screen.width; //The horizontal position of the level up text, currently placed beyond teh screen to the left

    internal float TotalDistance = 0; //The total distance passed by the player
    internal float LastLevelDistance = 0; //The total distance passed at the start of the current level. For example the value of LastLevelDistance when getting to level 2 is 100, and on level 3 is 300
    internal float TotalGems = 0; //total gems collected so far

    void Start()
    {
        Player = GameObject.FindWithTag("Player"); //Find the player in the scene and put it in a variable, for later use
        PlatformCreator = GameObject.FindWithTag("PlatformCreator"); //Find the Platform Creator in the scene and put it in a variable, for later use
    }

    void Update()
    {
        //If we haven't already leveled up and we passed the target distance for the next level, LEVEL UP!
        if (LevelUp == false && TotalDistance >= DistanceToLevelUp * CurrentLevel + LastLevelDistance)
        {
            LevelUp = true; //we are leveling up right now!

            LastLevelDistance = TotalDistance; //Set the value of LastLevelDistance to the total distance

            CurrentLevel++; //Increase the cuttent level
            LevelUpPosX = Screen.width; //Put the level up text off the screen to the right
            CurrentLevelUpText = LevelUpText[Random.Range(0, LevelUpText.Length)]; //Choose a random text from the level up text array to be displayed

            Camera.main.GetComponent<Shake>().shake = LevelUpRumble; //Shake the camera by teh value of LevelUpRumble

            //Here we are changing level values when leveling up, like the platform length/width etc
            Player.GetComponent<PlayerControls>().MaxSpeed += MaxSpeedChange; //Change the player's maximum speed

            PlatformCreator.GetComponent<PlatformCreator>().PlatformWidthRange.x += PlatformWidthChange; //change the minimum value of PlatformWidthRange
            PlatformCreator.GetComponent<PlatformCreator>().PlatformWidthRange.y += PlatformWidthChange; //change the maximum value of PlatformWidthRange

            PlatformCreator.GetComponent<PlatformCreator>().PlatformLengthRange.x += PlatformLengthChange; //change the minimum value of PlatformLengthRange
            PlatformCreator.GetComponent<PlatformCreator>().PlatformLengthRange.y += PlatformLengthChange; //change the maximum value of PlatformLengthRange

            PlatformCreator.GetComponent<PlatformCreator>().PlatformHeightRange.x -= PlatformHeightChange; //change the minimum value of PlatformHeightRange
            PlatformCreator.GetComponent<PlatformCreator>().PlatformHeightRange.y += PlatformHeightChange; //change the maximum value of PlatformHeightRange

            PlatformCreator.GetComponent<PlatformCreator>().PlatformGapRange.x += PlatformGapChange; //change the minimum value of PlatformGapRange
            PlatformCreator.GetComponent<PlatformCreator>().PlatformGapRange.y += PlatformGapChange; //change the maximum value of PlatformGapRange

            PlatformCreator.GetComponent<PlatformCreator>().PlatformRotateRange.x -= PlatformRotateChange; //change the minimum value of PlatformRotateRange
            PlatformCreator.GetComponent<PlatformCreator>().PlatformRotateRange.y += PlatformRotateChange; //change the maximum value of PlatformRotateRange

            PlatformCreator.GetComponent<PlatformCreator>().PlatformShiftRange.x -= PlatformShiftChange; //change the minimum value of PlatformShiftRange
            PlatformCreator.GetComponent<PlatformCreator>().PlatformShiftRange.y += PlatformShiftChange; //change the maximum value of PlatformShiftRange

            PlatformCreator.GetComponent<PlatformCreator>().ObstacleRate += ObstacleRateChange; //Change the value of the ObstacleRate
            PlatformCreator.GetComponent<PlatformCreator>().GemRate += GemRateChange; //Change the value of the GemRate
        }
    }

    //This function runs at the end of the game right after you fall off the platform. It's called from inside the player script when he falls
    public void EndLevel()
    {
        PlayerPrefs.SetInt("TotalDistance", (int)TotalDistance); //Put the value of TotalDistance in a playerPref record which will keep this value even after going to a different level or even quitting the game
        PlayerPrefs.SetInt("TotalGems", (int)TotalGems); //Put the value of TotalGems in a playerPref record which will keep this value even after going to a different level or even quitting the game
        
        StartCoroutine(WaitAndDelay(2.0f));

        Application.LoadLevel("end"); //Load the end screenm, which has total score displayed and a menu
    }

    IEnumerator WaitAndDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    void OnGUI()
    {
        GUI.skin = GUIskin; //The skin gui we'll use

        GUI.Label(new Rect(Screen.width * 0.98f, Screen.height * 0.01f, 0, 0), Mathf.Round(TotalDistance).ToString() + " M"); //Place the distance count on the top right of the screen

        GUI.Label(new Rect(Screen.width * 0.93f, Screen.height * 0.1f, 0f, 0f), TotalGems.ToString()); //Place the gems count on the top right of the screen

        GUI.DrawTexture(new Rect(Screen.width * 0.94f, Screen.height * 0.1f, 32f, 32f), Gems); //Place the gem image beside the gems count on the top right of the screen

        //Animate the level up text by passing it from the right side of the screen to the left side
        if (LevelUp == false && LevelUpPosX > -Screen.width)
        {
            GUI.Label(new Rect(LevelUpPosX, Screen.height * 0.85f, 200f, 50f), CurrentLevelUpText, "LevelUp"); //Display the text
            LevelUpPosX -= 10; //move the text to the left
        }
        else if (LevelUp == true)
        {
            LevelUp = false; //We ended the level up animation
        }
    }
}
