using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class Decoration : MonoBehaviour
{
    //This script was intended to create randomized decoration elements from an array of objects set in the inspector
    //I chose to exclude it from the game wither because it didn't work correctly or I didn't find a useful use for it.
    //I might include it in the game in a later update. You can still try to make use of it if you want
    
    public Transform[] DecorObject;
    internal Transform DecorObjectCopy;
    
    public Vector2 RotationRangeX = new Vector2(0,0);
    public Vector2 RotationRangeY = new Vector2(0,360);
    public Vector2 RotationRangeZ = new Vector2(0,0);
    
    public float EmptyChance = 0.8f;
    
    void Awake()
    {
    	if ( Random.value > EmptyChance )
    	{
    		DecorObjectCopy = Instantiate(DecorObject[Random.Range(0, DecorObject.Length)], transform.position, Quaternion.identity) as Transform;
    		
    		DecorObjectCopy.transform.parent = transform;

            DecorObjectCopy.transform.eulerAngles = new Vector3(Random.Range(RotationRangeX.x, RotationRangeX.y), Random.Range(RotationRangeY.x, RotationRangeY.y), Random.Range(RotationRangeZ.x, RotationRangeZ.y));
    	}
    }
}
