using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Player.TEAM team;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player p = collision.gameObject.GetComponent<Player>();
        if (p != null)
        {
            if(p.team == team)
            {
                
            }
            else
            {
                p.health--;
                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
