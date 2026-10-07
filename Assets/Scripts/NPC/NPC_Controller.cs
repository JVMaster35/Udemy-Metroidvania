using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour
{
    private Animator _anim;
    bool playAnim = true; // var for boolean

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (playAnim)
            StartCoroutine(WaitAnim()); //wait random seconds for animation
    }

    public IEnumerator WaitAnim()
    {
        playAnim = false;
        int randomWait = Random.Range(0, 10);
        yield return new WaitForSeconds(randomWait);
        _anim.Play("NPC_Idle");  //Put your animation string
        playAnim = true;
    }
}
