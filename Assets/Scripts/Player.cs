using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  
   public float speed;
   public float jumpForce;
   private bool isJumping;
   private bool doubleJump;
   private bool isFire;

   public GameObject tiro;
   public Transform firePoint;

   private Rigidbody2D rig;
   private Animator anim;

   private float movement;

    // Start is called before the first frame update
    void Start()
    {
       rig = GetComponent<Rigidbody2D>();
       anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
       if(!isFire)
       {
            Move();
            Jump();
       }

       Ataque();
    }

    void Move()
    {
       //se nao pressionar nada valor é 0. Pressinar direita, valor maximo 1. Esquerda valor maximo -1 
        movement = Input.GetAxis("Horizontal");

        //adiciona velocidade ao corpo do personagem no eixo x e y
        rig.velocity = new Vector2(movement * speed, rig.velocity.y);

        //andando pra direita
        if(movement > 0)
        {
            if(!isJumping)
            {
               anim.SetInteger("transition", 1); 
            }
           
           transform.eulerAngles = new Vector3(0, 0, 0);
        }

        //andando pra esquerda
        if(movement < 0)
        {
            if(!isJumping)
            {
               anim.SetInteger("transition", 1); 
            }
            
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if(movement == 0 && !isJumping)
        {
            anim.SetInteger("transition", 0);
        }
    }

    void Jump()
    {
       if(Input.GetButtonDown("Jump"))
       {
        if(!isJumping)
        {
            anim.SetInteger("transition", 2);
            rig.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            doubleJump = true;
            isJumping = true;
            
        }
        else
        {
            if(doubleJump)
            {
                anim.SetInteger("transition", 2);
                rig.AddForce(new Vector2(0, jumpForce / 2), ForceMode2D.Impulse);
                doubleJump = false;
            }
        }
       }
    }

    void Ataque()
    {
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            isFire = true;
            anim.SetInteger("transition", 3);
            GameObject Tiro = Instantiate(tiro, firePoint.position, firePoint.rotation);
            

            if(transform.rotation.y == 0)
            {
                Tiro.GetComponent<Tiro>().isRight = true;
            }

            if(transform.rotation.y == 180)
            {
                Tiro.GetComponent<Tiro>().isRight = false;
            }

            yield return new WaitForSeconds(0.3f);
            anim.SetInteger("transition", 0);
            isFire = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 6 )
        {
            isJumping = false;
        }
    }
}
