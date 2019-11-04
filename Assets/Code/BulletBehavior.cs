using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public Player.TEAM team;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            if(p.team == team) { }
            else if(collision.tag == "Shield")
            {
                //Debug.Log("Tink!");
                SoundManager.instance.Tink();
            }
            else
            {
                p.TakeDamageFromDirection(1, rb.velocity, transform.position);
                Destroy(gameObject);
            }
        }
        else if (collision.tag == "Shield")
        {
            //Debug.Log("Tink!");
            SoundManager.instance.Tink();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
}
