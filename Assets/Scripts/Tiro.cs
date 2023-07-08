using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : MonoBehaviour
{
    private Rigidbody2D rig;
    public bool isRight;

    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>(); 
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if(isRight)
        {
            rig.velocity = Vector2.right * speed;
        }
        else
        {
            rig.velocity = Vector2.left * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D colision)
    {
        if(colision.gameObject.tag == "Enemy")
        {
            colision.gameObject.GetComponent<Enemy>().Death();
            Destroy(gameObject);
        }
    }
}
