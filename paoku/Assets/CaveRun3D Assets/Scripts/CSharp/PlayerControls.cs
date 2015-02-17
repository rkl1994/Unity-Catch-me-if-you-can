using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    //This script controls the player movement. The player follows the mouse position. Holding the left mouse button will make him jump, double clicking will make him double jump.
    //THe script also handles all teh animations and sound for various actions ( running , running fast, jumping, falling, hitting lava etc ).

    internal GameObject PlatformCreator; //Used to hold the Platform Creator object, if it's in the scene
    internal GameObject GameController; //Used to hold the Game Controller object, if it's in the scene

    public float Speed = 0f; //The player's current speed
    public float MaxSpeed = 0.2f; //The players maximum speed
    public float Acceleration = 0.001f; //How quickly the player can get to maximum speed
    public Transform SpeedEffect; //The Speed effect that is displayed when running very fast

    public float JumpPower = 10f; //The player's jump power

    public float TurningSpeed = 0.04f; //The player's turning speed, how quickly he moves left and right
    internal float TurnDamping = 0f; //A slowdown value for the turning speed. At 1, it doesn't affect his speed, lower than that will make him rotate slowly

    internal string HitAnimation = "Lava"; //The player's hit animation, which plays when the palyer hits an obstacle
    public float HitAnimationTime = 0f; //How long the player's hit aniamtion should be player

    public float MovementLimits = 12f; //movement limits for the palyer which prevent him from going to far left or right

    public Transform PuffEffect; //The effect created when the player lands on a platform
    private Transform PuffEffectCopy; //A copy of the PuffEffect

    public Transform TrailEffect; //The effect created when the player flies up from hitting lava
    private Transform TrailEffectCopy; //A copy of the TrailEffect

    internal Vector3 InitPos; //The player's initial position
    internal Vector3 PlayerPos = Vector3.zero; //The player's current position

    internal bool FellOff = false; //Did we fall off? This means the end of the game, you can't jump back after after falling off a platform

    internal int JumpState = 4; //0-on the floor, 1-jump, 2-fall after jump, 3-double jump, 4-fall after double jump

    //private var DisableMouse:boolean = false; //I wanted to add keyboard controls, but so far it isn't working well, maybe next update I'll make it ( along with a head to head mdoe )

    public Transform CustomCursor; //Put your custom cusor here. This is the orange glowing circle that shows where the player is going

    //SOUNDS
    public AudioClip JumpSound;
    public AudioClip FallSound;
    public AudioClip StepSound;
    public AudioClip CrashSound;

    public float CameraFollowSpeed = 0.2f; //How quickly the camera follows the player's position

    internal float JumpChange = 1f; //How much the jump power is affected. 1 means jumping is normal. less than 1 means jumps are much weaker
    internal float JumpChangeTime = 0f; //How long to keep the jump change, in seconds

    void Start()
    {
        PlatformCreator = GameObject.FindWithTag("PlatformCreator"); //Find the Platform Creator in the scene and put it in a variable, for later use
        GameController = GameObject.FindWithTag("GameController"); //Find the Game Controller in the scene and put it in a variable, for later use

        InitPos = transform.position; //Set the initial position of the player

        animation.CrossFade("Fall"); //Start playing the fall animation

        Camera.main.GetComponent<Shake>().shake = GameController.GetComponent<GameController>().LevelUpRumble; //shake the camera
    }

    void Update()
    {
        //Make the camera follow the player
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - (Camera.main.transform.position.x - transform.position.x) * CameraFollowSpeed, Camera.main.transform.position.y, Camera.main.transform.position.z);

        if (FellOff == false) //If we haven't fallen off yet, keep running
        {
            //Increase speed based on acceleration
            if (Speed < MaxSpeed)
            {
                Speed += Acceleration;
            }

            //Add to the distance value in the game controller
            GameController.GetComponent<GameController>().TotalDistance += Speed;

            //If turning speed is being damped by the value of TurnDamping, gradually taking it back to normal ( 1 )
            if (TurnDamping < 1)
                TurnDamping += 0.5f * Time.deltaTime;

            if (JumpChangeTime > 0) //As long as the value of JumpChangeTime is larger than 0, keep the change
                JumpChangeTime -= Time.deltaTime; //reduce the change time
            else if (JumpChange != 1) //Otherwise, reset the jump change back to 1, which means it doesn't affect jump power anymore
                JumpChange = 1;

            if (Input.GetButtonDown("Jump") && JumpState == 0) //If we press the jump button, give the player a velocity up
            {
                JumpState = 1; //Set the jump state to 1, Jumping
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, JumpPower * JumpChange, rigidbody.velocity.z); //Give the player an up velocity
                audio.PlayOneShot(JumpSound); //Play a jump sound
            }
            else if (Input.GetButtonUp("Jump") && rigidbody.velocity.y > 0 && JumpState == 1) //If we release the jump button while jumping, reduce the up velocity to third of its power, making the player fall quickly
            {
                JumpState = 2; //fall after jump
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y * 0.3f, rigidbody.velocity.z); //Reduce the up velocity to third its power
            }

            if (rigidbody.velocity.y < 0 && JumpState == 1) //If we've fallen after jumping, change the animation to falling 
            {
                JumpState = 2; //fall after jump
            }

            if (Input.GetButtonDown("Jump") && JumpState == 2) //If we press the jump button while falling the first time, perform a double jump
            {
                JumpState = 3; //double jump
                rigidbody.velocity = new Vector3(rigidbody.velocity.y, JumpPower * 0.7f * JumpChange, rigidbody.velocity.z); //Give the player an up velocity, which is a little weaker than the first jump
                audio.PlayOneShot(JumpSound); //Play a jump sound
            }
            else if (Input.GetButtonUp("Jump") && rigidbody.velocity.y > -1 && JumpState == 3) //If we release the jump button while double jumping, reduce the up velocity to third of its power, making the player fall quickly
            {
                rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y * 0.3f, rigidbody.velocity.z); //Reduce the up velocity to third its power
                JumpState = 4; //fall after double jump
            }

            if (rigidbody.velocity.y < 0 && JumpState == 3) //If we've fallen after double jumping, change the animation to falling 
            {
                JumpState = 4; //fall after double jump
            }

            // Generate a plane that intersects the transform's position. We use this to make the aiming cursor so the player can run in the direction of the cursor
            Plane playerPlane = new Plane(Vector3.forward, InitPos + new Vector3(0, 0, 6)); //Create an invisible "wall" we can then intersect the mouse with
            var RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);	// Generate a ray from the mouse cursor position
            float HitDist = 0f; // Determine the point where the cursor ray intersects the plane.
            Vector3 TargetPoint = new Vector3(0, 0, 0);

            //Check if we hit the plane
            if (playerPlane.Raycast(RayCast, out HitDist)) //&& DisableMouse == false ) 
            {
                TargetPoint = RayCast.GetPoint(HitDist); // Get the point along the ray that hits the calculated distance.
            }

            //Limit player movement, so he doesn't go too far left or right
            if (TargetPoint.x > MovementLimits) TargetPoint.x = MovementLimits;
            if (TargetPoint.x < -MovementLimits) TargetPoint.x = -MovementLimits;

            transform.LookAt(TargetPoint); //Make the player look in the direction of the cursor

            PlayerPos.x = TargetPoint.x; //Set the desired horizontal player position to be the same as TargetPoint
            PlayerPos.z = InitPos.z; //Keep the player at his initial forward position 

            //Note: The following uncommented code was supposed to be used for keyboard controls. They didn't work as intended
            //so far, but I may get it to work next update. You may still be able to make it work, tell me how it goes :D
            //~ if ( Input.GetAxis("Horizontal") )
            //~ {
            //~ DisableMouse = true;

            //~ TargetPoint.x += Input.GetAxis("Horizontal") * TurningSpeed * 1;

            //~ PlayerPos.x = TargetPoint.x;
            //~ PlayerPos.z = InitPos.z;
            //~ }

            //~ if ( Input.GetAxis("Mouse X") || Input.GetAxis("Mouse X") )
            //~ {
            //~ DisableMouse = false;
            //~ }

            if (CustomCursor) //If we have a custom cursor defined
            {
                CustomCursor.LookAt(transform.position); //make the cursor look at the player

                CustomCursor.position = TargetPoint; //move the cursor to the position of intersection with the plane
                CustomCursor.position = new Vector3(CustomCursor.position.x, transform.position.y, CustomCursor.position.z); //keep the cursor at the vertical position of the player
            }

            //Move the player towards the value of player position based on TurningSpeed, and taking into account TurnDamping which will make turning slower
            transform.position = new Vector3(transform.position.x - (transform.position.x - PlayerPos.x) * TurningSpeed * TurnDamping, transform.position.y, transform.position.z);
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - (transform.position.z - InitPos.z) * 0.1f);

            //Rotate in the direction of movement
            transform.rotation = new Quaternion(transform.rotation.z, transform.rotation.y, 0, transform.rotation.w);

        }
        else //If we've fallen off the platform...
        {
            //Decrease speed based on acceleration
            if (Speed > 0.01f)
            {
                Speed *= 0.9f; //reduce the player's speed
            }
            else if (Speed != 0 && transform.position.y < -3) //if we fell low enough and our speed is not yet 0
            {
                Speed = 0; //set the speed to 0
                Camera.main.GetComponent<Shake>().shake = 200; //shake the camera

                audio.PlayOneShot(CrashSound); //play a crash sound
                
                GameController.GetComponent<GameController>().EndLevel(); //Run the end level function which is inside the GameController script

                Destroy(gameObject); //remove the player
            }
        }

        //Player falls off a platform
        if (transform.position.y < PlatformCreator.GetComponent<PlatformCreator>().PlatformHeightRange.x - 0.5f && FellOff == false)
            FellOff = true;

        //High speed effect
        if (Speed > 0.5f)
            SpeedEffect.particleEmitter.emit = true; //turn on the high speed effect
        else
            SpeedEffect.particleEmitter.emit = false; //turn off the high speed effect

        //Lava, walk, run, jump, and fall animations
        if (HitAnimation != "" && HitAnimationTime > 0) //If we have a hit animation set, play it
        {
            animation.CrossFade(HitAnimation); //play the hti animation

            HitAnimationTime -= Time.deltaTime; //reduce from the hit animation time

            if (HitAnimation == "Lava") //If the hit animation happens to be lava, create a trail effect at the player's position 
            {
                Instantiate(TrailEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            }
        }
        else //all the other animations
        {
            if (JumpState == 0) //if we haven't jumped, play the run animations
            {
                if (Speed > 0.5) //if speed is above 0.5, play the fast run animation 
                {
                    animation.CrossFade("Run");
                    animation["Run"].speed = Speed * 3; //set animation speed be based on the player's actual speed
                }
                else //otherwise, play the normal run animation
                {
                    animation.CrossFade("Walk");
                    animation["Walk"].speed = Speed * 4; //set animation speed be based on the player's actual speed
                }
            }
            else if (JumpState == 1 || JumpState == 3)
            {
                animation.CrossFade("Jump");
                animation["Jump"].speed = Speed * 4; //set animation speed be based on the player's actual speed
            }
            else if (JumpState == 2 || JumpState == 4)
            {
                animation.CrossFade("Fall");
                animation["Fall"].speed = Speed * 4; //set animation speed be based on the player's actual speed
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //If the player falls on a platform, while either jumping or double jumping, se him back to running state
        if (JumpState == 2 || JumpState == 4)
        {
            JumpState = 0;

            audio.PlayOneShot(StepSound); //play a step sound

            if (PuffEffect)
                Instantiate(PuffEffect, transform.position, Quaternion.identity); //if there is a puff effect, create it at the player's feet
        }
    }

    //this gizmo shows the movement limits for the player
    void OnDrawGizmos()
    {
        //Right limit of movement
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(MovementLimits, 0, 0), new Vector3(MovementLimits, 10, 0));

        //Left limit of movement
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(-MovementLimits, 0, 0), new Vector3(-MovementLimits, 10, 0));
    }
}
