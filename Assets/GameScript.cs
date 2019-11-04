using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public static GameScript gs;
    public List<Player> deadPlayers;
    
    public GameScript()
    {
        gs = this;
        deadPlayers = new List<Player>();
    }

    public List<Player> GetDeadTeammates(Player.TEAM team)
    {
        List<Player> p = new List<Player>();
        for(int i = 0; i < deadPlayers.Count; i++)
        {
            if(deadPlayers[i].team == team)
            {
                p.Add(deadPlayers[i]);
            }
        }
        return p;
    }

    public void PlayerDied(Player p)
    {
        deadPlayers.Add(p);
    }

    public void ResPlayer(Player p, Vector2Int pos)
    {
        p.health = Constants.MAX_HEALTH;
        StageGrid.STATUS[,] s = StageGrid.instance.GetSurroundings((Vector2Int)p.gridPosition);
        for (int i = 0; i < s.GetLength(0); i++)
        {
            for (int j = 0; j < s.GetLength(1); j++)
            {
                if (s[i, j] == StageGrid.STATUS.UNOCCUPIED)
                {
                    if(p.shape != Player.SHAPE.CIRCLE)
                       StageGrid.instance.SetPlayerAt(new Vector2Int(pos.x + i - 1, pos.y + j - 1));
                    p.transform.position = pos + new Vector2(i - 1 + 0.5f, j - 1 + 0.5f);
                    p.target = p.transform.position;
                    p.gameObject.SetActive(true);
                    deadPlayers.Remove(p);
                    //Debug.Log("Player is now: " + (p.gameObject.activeSelf ? "Active" : "Not Active"));
                    return;
                }
            }
        }
    }

    public List<Player.TEAM> LivingTeams()
    {
        List<Player.TEAM> teams = new List<Player.TEAM>();
        GameObject[] plrs = GameObject.FindGameObjectsWithTag("Player");
        for(int i = 0; i < plrs.Length; i++)
        {
            if (plrs[i].activeSelf)
            {
                teams.Add(plrs[i].GetComponent<Player>().team);
            }
        }
        return teams;
    }
}
