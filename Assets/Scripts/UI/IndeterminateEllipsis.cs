using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndeterminateEllipsis : MonoBehaviour
{
    //Declare an animatedEllipisis instance  
    private AnimatedElipsis animatedEllipsis;

    [SerializeField] private TMP_Text ellipsisTxt;

    void Start()
    {
        /*Initialize the animatedEllipsis. Or just make it public  
         * and initialize it at the Inspector. */
        this.animatedEllipsis = this.GetComponent<AnimatedElipsis>();

        //Initialize the delegate  
        this.animatedEllipsis.onCompletion = DisplayMessageOnCompletion;

        //Start the animation of the indeterminate animated ellipsis  
        this.animatedEllipsis.Animate(1f);
    }

    void OnGUI()
    {
        //Render the animated ellipsis  
        ellipsisTxt.text = this.animatedEllipsis.text;
    }

    //This method will be referenced by the 'animatedEllipsis' delegate  
    public void DisplayMessageOnCompletion()
    {
        this.animatedEllipsis.text = "Completed!";
    }
}
