using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour
{
    //This script handles the shaking effect of the camera. It can be used to shake any object though.

    internal GameObject Player; //Used to hold the player object, if it's in the scene  
    internal Vector3    InitPos; //Original position of the object
    internal int        shake   = 0; //How long and how shaky the object gets
    internal bool       Shaking = false; //Is the object shaking now?

    public Transform    DebrisEffect; //The falling debris effect played while the object shakes ( particleEmitter only )
    public AudioClip    RumbleSound; //The sound played while the object shakes
    public bool         KeepInitialPosition = false; //If this is set to true, the object will always retun to its original position after finishing the shake effect
    public int          ShakeDecay          = 2; //How quickly the shaking stops

    void Start()
    {
        Player = GameObject.FindWithTag("Player"); //Find the player in the scene and put it in a variable, for later use	
        InitPos = transform.position; //set the original position of the object so we can return to it after shaking ends
    }

    void Update()
    {
        if (shake > 0) //If the value of shake is alrger than 0, SHAKE!
        {
            shake -= ShakeDecay; //Decrease the shake value based on ShakeDecay

            //If there's no need to keep the initial position of hte shaken object, update teh calue of InitPos based on the current position of the object
            if (KeepInitialPosition == false) InitPos = transform.position;

            //Shake the object by moving it in a random offset from InitPos, multiplying it by the value of Shake so that at the start the shake is stronger and it gets weaker towards the end, and then stops
            transform.position = InitPos + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f)) * shake * 0.002f;

            if (Shaking == false) //If the object is not shaking, start shaking it
            {
                Shaking = true; //Used to make this shake check happen just once

                if (DebrisEffect)
                    DebrisEffect.particleEmitter.emit = true; //If there is a debris effect ( particleEmitter only ), play it
                
                if (RumbleSound)
                    audio.PlayOneShot(RumbleSound); //If there is a debris sound, play it
            }
        }
        else //If the value of shake reaches 0, stop shaking
        {
            if (Shaking == true) //If the object is still shaking, stop shaking it
            {
                Shaking = false; //Used to make this shake check happen just once

                if (DebrisEffect) 
                    DebrisEffect.particleEmitter.emit = false; //If there is a debris effect ( particleEmitter only ), stop it
            }
        }
    }
}
