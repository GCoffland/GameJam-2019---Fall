using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResScript : MonoBehaviour
{
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
        Debug.Log("Collided!");
        if(collision.tag == "Player")
        {
            Debug.Log("With Player!");
            Player p = collision.gameObject.GetComponent<Player>();
            List<Player> dtm = GameScript.gs.GetDeadTeammates(p.team);
            if(dtm.Count > 0)
            {
                Debug.Log("Resing!");
                GameScript.gs.ResPlayer(dtm[0], (Vector2Int)StageGrid.instance.GetCellFromWorld((Vector2)transform.position));
                Destroy(gameObject);
            }
        }
    }
}
