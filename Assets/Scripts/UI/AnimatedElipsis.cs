using System.Collections;
using UnityEngine;

public class AnimatedElipsis : MonoBehaviour
{
    //The string that will be displayed alongside with the ellipsis  
    public string text;

    //Variables for both animation methods  
    private int ellipsisAnimator = 0;
    public int numberOfDots = 3;
    public bool isCompleted = false;

    //Just a single variable for the indeterminate method  
    private int initTextLength;

    //Variables specifically created for the determinate method  
    private int oldEllipsisAnimator = 0;
    private int progressPerDot = 0;
    public int minProgress = 0;
    public int maxProgress = 100;
    private int progressRange = 0;

    /*This boolean flags whether to animate the ellipsis using the determinate or  
     * the indeterminate method. */
    public bool indeterminate = false;

    //Define the delegate that points to the method that runs when the process is completed  
    public delegate void OnCompletionDel();

    //Create an instance of the above delegate  
    public OnCompletionDel onCompletion;

    void Start()
    {
        //Check if 'maxProgress' is bigger than 'minProgress'   
        if (this.maxProgress > this.minProgress)
        {
            //The range defined by the maximum and minimum values  
            this.progressRange = this.maxProgress - this.minProgress;
            //Calculate the value that adds a single dot the ellipsis (determinate mode only)    
            this.progressPerDot = this.progressRange / this.numberOfDots;

            //Set 'ellipsisAnimator' and 'oldEllipsisAnimator' to zero,  
            this.ellipsisAnimator = 0;
            this.oldEllipsisAnimator = 0;
        }
        else //'maxProgress' is smaller than 'minProgress' or the other way around.  
        {
            Debug.LogError("Either 'minProgress' is bigger or equal to 'maxProgress' " +
                "or 'maxProgress' is smaller or equal to 'minProgress'. Double check these " +
                "values at the Inspector.");

            //Disable this script  
            this.enabled = false;
        }
    }

    //This IEnumerator will animate the ellipsis (both in the indeterminate and determinate modes)  
    private IEnumerator AnimateEllipsis(float rateOrProgress)
    {
        //If indeterminate is true  
        if (indeterminate)
        {
            //Save the initial text length at the 'initTextLenght' variable  
            this.initTextLength = this.text.Length;

            while (!this.isCompleted)
            {
                /*Increase the value of 'ellipsisAnimator' and get the rest of the division 
                 * by 'numberOfDots'. Save the results back at 'ellipsisAnimator' */
                this.ellipsisAnimator = (++this.ellipsisAnimator) % this.numberOfDots;

                /*Test if the current 'text' string length is bigger the maximum expected 
                 * length */
                if (this.text.Length >= this.initTextLength + this.numberOfDots)
                {
                    //Remove the dots that are in excess  
                    this.text = this.text.Remove(this.initTextLength);
                }
                else //The 'text' string hasn't yet reached its maximum length  
                {
                    //Add one dot to it  
                    this.text += ".";
                }

                /*Make the code execution wait for the time defined at 'rateOrProgress'  
                 * (in seconds)*/
                yield return new WaitForSeconds(rateOrProgress);
            }

            /*If the code has reached this line, the boolean 'isCompleted has been  
             * set to 'true'. */

            //Check if the 'onCompletion' has been assigned  
            if (this.onCompletion != null)
            {
                //Run the method that this delegate points to   
                this.onCompletion();
            }
        }
        else //Indeterminate is false  
        {
            /*Whether the progress passed as a parameter is is smaller than 'maxProgress' 
             * Also, check if progress is completed. */
            if (rateOrProgress + this.minProgress < this.progressRange && !this.isCompleted)
            {
                /*Divide the current progress by 'progressPerDot' and convert  
                /* it to an integer */
                this.ellipsisAnimator = (int)(rateOrProgress / this.progressPerDot);

                //If 'ellipsisAnimator' is bigger than its previous value  
                if (this.ellipsisAnimator > this.oldEllipsisAnimator)
                {
                    //Add a dot the the ellipsis  
                    this.text += ".";
                    //Assign the value of 'ellipsisAnimator' to 'oldEllipsisAnimator'  
                    this.oldEllipsisAnimator = this.ellipsisAnimator;
                }
            }
            else
            {
                //Display a message at the console  
                Debug.Log("Completed");

                //Set 'is_Completed' to true  
                this.isCompleted = true;

                //Check if the 'onCompletion' has been assigned  
                if (this.onCompletion != null)
                {
                    //Run the method that this delegate points to   
                    this.onCompletion();
                }
            }
        }
    }

    //Encapsulate the coroutine invocation   
    public void Animate(float rateOrProgress)
    {
        StartCoroutine(AnimateEllipsis(rateOrProgress));
    }

    //When this script is disabled, stop all pending coroutines  
    void OnDisable()
    {
        this.StopAllCoroutines();
    }
}
