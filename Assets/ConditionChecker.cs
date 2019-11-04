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
        //Debug.Log("uH:");
        if (GameScript.gs.deadPlayers.Count >= 12)
        {
            List<Player.TEAM> teams = GameScript.gs.LivingTeams();
            Player.TEAM tem1 = teams[0];
            for (int i = 0; i < teams.Count; i++)
            {
                if(tem1 != teams[i])
                {
                    return;
                }
            }
            winner = teams[0];
            SceneManager.LoadScene("GameOver");
            return;
        }
    }
}
