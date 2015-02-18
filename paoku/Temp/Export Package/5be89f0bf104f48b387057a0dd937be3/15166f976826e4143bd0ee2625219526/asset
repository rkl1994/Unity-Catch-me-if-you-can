using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour
{
    //This script handles the movement of a platform based on the player's speed. It moves straight only, and then recreates itself at the start position while destroying itself

    internal GameObject Player; //Used to hold the player object, if it's in the scene
    internal GameObject PlatformCreator; //Used to hold the Platform Creator object, if it's in the scene

    internal float PlatformSpeed; //Current platform speed, set by the player's speed

    public bool CreatedPlatform = false; //Have we created a platform yet? This is used to make sure we create the platform again at the start position only once per platform

    public int SectionLength = 3; //The section length. In my case both middle and edge sections are 3 in length. You have to check your custom made sections and make sure you set them right. 
    //In 3DS max for example 100 = 1 in Unity edit.

    internal int PlatformLength = 0; //The entire platform length with all the sections in it.

    void Start()
    {
        Player = GameObject.FindWithTag("Player"); //Find the player in the scene and put it in a variable, for later use
        PlatformCreator = GameObject.FindWithTag("PlatformCreator"); //Find the Platform Creator in the scene and put it in a variable, for later use
    }

    void Update()
    {
        if (Player)
            PlatformSpeed = -1 * Player.transform.GetComponent<PlayerControls>().Speed; //If a player object exists, set the platform's speed to be the exact opposite of the player's speed

        transform.Translate(Vector3.forward * PlatformSpeed, Space.World); //move the paltforms based their speed

        //create another platform at the start position
        if (transform.position.z < PlatformCreator.transform.position.z - (PlatformCreator.GetComponent<PlatformCreator>().PlatformLength + 2) * SectionLength && CreatedPlatform == false)
        {
            //The platform is created at the end of the last section of the last platform, plus a gap
            PlatformCreator.GetComponent<PlatformCreator>().CreatePlatform((int)(PlatformCreator.GetComponent<PlatformCreator>().NewPlatformCopy.position.z + (PlatformCreator.GetComponent<PlatformCreator>().PlatformLength + 2) * SectionLength));
            CreatedPlatform = true;
        }

        //Remove the current platform after it passed well beyond the player
        if (transform.position.z < PlatformCreator.transform.position.z - (PlatformCreator.GetComponent<PlatformCreator>().PlatformLength + 2) * SectionLength - 100)
            Destroy(gameObject); //remove the platform
    }
}
