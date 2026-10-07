using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShot : MonoBehaviour
{
    public int shotDamage;

    public float shotSpeed;

    public Vector2 moveDirection;

    private Rigidbody2D shotRDBY;

    private Animator anim;

    private SpriteRenderer shotSprite;

    // Start is called before the first frame update
    void Start()
    {
        shotRDBY = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        shotSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        shotRDBY.linearVelocity = moveDirection * shotSpeed;

        if(shotRDBY.linearVelocity.x < 0)
        {
            shotSprite.flipX = true;
        }
    }

    
    private void OnTriggerEnter2D(Collider2D shotHit)
    {
        //Shot Damage
        if(shotHit.tag == "Enemy")
        {
            shotHit.GetComponent<Enemy>().TakeDamage(shotDamage);
            //Shot Effect
            anim.SetBool("Hit", true);
            shotSpeed = 0;
            Destroy(gameObject, 0.7f);
            AudioManager.instance.PlayPlayerSFX(1);
        }
        else if (shotHit.tag == "Boss")
        {
            shotHit.GetComponent<BossHealth>().TakeDamage(shotDamage);
            //Shot Effect
            //anim.SetBool("Hit", true);
            shotSpeed = 0;
            Destroy(gameObject, 0.7f);
            AudioManager.instance.PlayPlayerSFX(1);
        }
        else
        {
            anim.SetBool("Hit", true);
            shotSpeed = 0;
            Destroy(gameObject, 0.7f);
            AudioManager.instance.PlayPlayerSFX(1);
        }
    }
    
    

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
