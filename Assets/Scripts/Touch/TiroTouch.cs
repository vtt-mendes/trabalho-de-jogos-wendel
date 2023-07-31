using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiroTouch : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

    }

    // Update is called once per frame
    void Update()
    {
      

    }

    public void Fire()
    {
        player.touchFire = true;   
    }
}
