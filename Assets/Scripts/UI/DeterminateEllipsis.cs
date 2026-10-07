using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
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

        this.StartCoroutine(BackgroundOperation());
    }

    void OnGUI()
    {
        //Render the animated ellipsis  
        ellipsisTxt.text = this.animatedEllipsis.text;
    }

    private IEnumerator BackgroundOperation()
    {
        yield return new WaitForSeconds(10); //An object to wait for, a 'new WaitForSeconds()' call, etc  
                     //Update progress  
        this.animatedEllipsis.Animate(10);
        //Do whatever needs to be done and update progress again  
        this.animatedEllipsis.Animate(25);

        //.... More lines of code with the appropriate Animate() method calls...  

        //And so on, until Animate() receives the value defined for 'maxProgress'  
        this.animatedEllipsis.Animate(100);
    }

    //This method will be referenced by the 'animatedEllipsis' delegate  
    public void DisplayMessageOnCompletion()
    {
        this.animatedEllipsis.text = "Done!";
    }
}
