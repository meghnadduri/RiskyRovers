using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraPosition : MonoBehaviour
{
    public Transform rover; // The rover to follow
    //public Vector3 miniMapPositionOffset = new Vector3(0f, 0f, -10f); // Where to offset the camera to

    // Start is called before the first frame update
    void Start()
    {
        //rover.transform.position = miniMapPositionOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
