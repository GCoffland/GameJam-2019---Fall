using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConditionChecker : MonoBehaviour
{
    public static Player.TEAM winner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("uH:");
        if (Player.players.Count > 0)
        {
            Player.TEAM t = Player.players[0].team;
            for (int i = 1; i < Player.players.Count; i++)
            {
                if (Player.players[i].team != t)
                {
                    return;
                }
            }
            winner = t;
            SceneManager.LoadScene("GameOver");
            return;
        }
        else
        {
            SceneManager.LoadScene("GameOver");
            Debug.Log("NO ONE WINS!");
            return;
        }
    }
}
