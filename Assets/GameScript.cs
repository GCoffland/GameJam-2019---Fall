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
                    StageGrid.instance.SetPlayerAt(new Vector2Int(pos.x + i - 1, pos.y + j - 1));
                    p.transform.position = pos + new Vector2(i - 1 + 0.5f, j - 1 + 0.5f);
                    p.target = p.transform.position;
                    Debug.Log("Resing player at: " + new Vector2Int(pos.x + i - 1, pos.y + j - 1));
                    Debug.Log("Activating Player");
                    p.gameObject.SetActive(true);
                    Debug.Log("Player is now: " + (p.gameObject.activeSelf ? "Active" : "Not Active"));
                    return;
                }
            }
        }
    }
}
