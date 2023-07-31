using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   public int health = 3;
   public float speed;
   public float jumpForce;

   public GameObject tiro;
   public Transform firePoint;

   private bool isJumping;
   private bool doubleJump;
   private bool isFire;

   private Rigidbody2D rig;
   private Animator anim;

   public float movement;
   public bool isMobile;

   public bool touchJump;
   public bool touchFire;

    // posição inicial
   public Vector3 posInicial;


    // Start is called before the first frame update
    void Start()
    {
       rig = GetComponent<Rigidbody2D>();
       anim = GetComponent<Animator>();

       GameController.instance.UpdateLives(health);

       posInicial = new Vector3(-23.81f, -0.6f, 0);
       transform.position = posInicial;
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

        if(!isMobile) 
        {
//se nao pressionar nada valor é 0. Pressinar direita, valor maximo 1. Esquerda valor maximo -1 
        movement = Input.GetAxis("Horizontal");
        }
       

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
       if(Input.GetButtonDown("Jump") || touchJump)
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

        touchJump = false;
       }
    }

    void Ataque()
    {
        StartCoroutine("Fire");
    }

    IEnumerator Fire()
    {
        if(Input.GetKeyDown(KeyCode.E) || touchFire)
        {
            touchFire = false;

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

    public void Damege(int dmg)
    {
        health -= dmg;
        GameController.instance.UpdateLives(health);
        
            if(transform.rotation.y == 0)
            {
                transform.position += new Vector3(-0.5f,0,0);
            }

            if(transform.rotation.y == 180)
            {
                transform.position += new Vector3(0.5f,0,0);
            }

        if(health <= 0)
        {
            //chamar game over
            GameController.instance.GameOver();

        }

    }

    public void IncreaseLife(int value)
    {
        health += value;
        GameController.instance.UpdateLives(health);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if(coll.gameObject.layer == 6 )
        {
            isJumping = false;
        }

       
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == "checkpoint")
        {
            posInicial = coll.gameObject.transform.position;
            Destroy(coll.gameObject);
        }

        if(coll.gameObject.tag == "FallDetector")
        {
            transform.position = posInicial;
        }
    }
}
