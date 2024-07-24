using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockexperiment : MonoBehaviour
{
    public int rockNum;
    private ExperimentImage experimentButton;


    void Start() {
        experimentButton = GameObject.Find("ExperimentImage").GetComponent<ExperimentImage>(); // finds the experiment image in heirarchy 
    }
    private void OnTriggerEnter2D(Collider2D x) {

        
        experimentButton.ChangeImage(rockNum); // when the rover enters the trigger child from the rock, the experiment image should change 
    }

    private void OnTriggerExit2D(Collider2D y) {
        experimentButton.ResetImage(); // when exiting the trigger it resets image. 
    }

    
}
