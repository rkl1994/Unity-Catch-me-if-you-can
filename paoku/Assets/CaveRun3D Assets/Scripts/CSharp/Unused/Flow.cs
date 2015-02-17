using UnityEngine;
using System.Collections;

public class Flow : MonoBehaviour
{
    //This script was intended to create a flowing effect for mud and lava by offsetting their texture at a constant speed
    //I chose to exclude it from the game wither because it didn't work correctly or I didn't find a useful use for it.
    //I might include it in the game in a later update. You can still try to make use of it if you want

    public float Offset = 0.1f;

    internal float CurrentOffset = 0;

    void Update()
    {
        transform.renderer.material.mainTextureScale = new Vector2(CurrentOffset, CurrentOffset);
        //transform.renderer.material.mainTextureOffset = Vector2(0,CurrentOffset);
        CurrentOffset += Offset;

        if (CurrentOffset > 1) CurrentOffset = 0;
    }
}
