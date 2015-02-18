using UnityEngine;

public class ScoreScreen : MonoBehaviour
{
    //This script is used to dispaly the score screen at the end of the game, where two values ( distance and gems collected ) are added to teh total score
    public GUISkin GUIskin; //The skin gui we'll use

    public int GemValue = 100; //The value of a single gem in points
    public int DistanceValue = 10; //The value of a single meter of distance in points

    internal int TotalDistance = 0; //The total distance passed
    internal int TotalGems = 0; //The total gems collected

    internal int TotalScore = 0; //The total score calculated from both distance and gems collected
    internal int TotalScoreCurrent = 0; //The current total score, used to animate the score rising from 0 to TotalScore

    void Start()
    {
        TotalDistance = PlayerPrefs.GetInt("TotalDistance"); //Get the distance value from PlayerPrefs, which is used to hold values on your local machine even after you shutdown the game
        TotalGems = PlayerPrefs.GetInt("TotalGems"); //Get the number of gems from PlayerPrefs, which is used to hold values on your local machine even after you shutdown the game

        TotalScore = TotalDistance * DistanceValue + TotalGems * GemValue; //Calculate the total score from the gems and distance multiplied by their respective values
    }

    void OnGUI()
    {
        GUI.skin = GUIskin; //The skin gui we'll use

        if (TotalScoreCurrent < TotalScore) //If we haven't reached the TotalScore, keep counting up to it
            TotalScoreCurrent -= (int)((TotalScoreCurrent + TotalScore) * 0.05); //Count up from 0 to the value of TotalScore smoothly

        //Display 3 boxes, the first showing total distance passed and multiplied by the value of each meter, the second showing total gems collected and multiplied by the value of a gem, and finally a bigger box showing the
        //total score.	
        GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.1f, Screen.width * 0.8f, Screen.height * 0.15f), "Total Distance: " + TotalDistance.ToString() + "M" + " X " + DistanceValue.ToString());
        GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.25f, Screen.width * 0.8f, Screen.height * 0.15f), "Total Gems: " + TotalGems.ToString() + " X " + GemValue.ToString());
        GUI.Box(new Rect(Screen.width * 0.1f, Screen.height * 0.4f, Screen.width * 0.8f, Screen.height * 0.35f), "Total Score \n" + TotalScoreCurrent.ToString(), "TotalScore");
    }


}
