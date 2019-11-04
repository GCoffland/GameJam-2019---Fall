using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRespawners : MonoBehaviour
{

    GameObject respawner;
    float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        respawner = Resources.Load<GameObject>("Prefabs/ResSprite");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cooldown < 0 && GameScript.gs.deadPlayers.Count > 0)
        {
            bool flag = true;
            while (flag)
            {
                Vector3Int n = new Vector3Int(Random.Range(0, StageGrid.instance.worldStatusArray.GetLength(0)), Random.Range(0, StageGrid.instance.worldStatusArray.GetLength(1)), 0);

                if (StageGrid.instance.worldStatusArray[n.x, n.y] == StageGrid.STATUS.UNOCCUPIED)
                {
                    Vector3 y = StageGrid.instance.GetWorldFromCell(n);
                    y.x += StageGrid.instance.tilemaps[0].cellBounds.xMin + 0.5f;
                    y.y += StageGrid.instance.tilemaps[0].cellBounds.yMin + 0.5f;
                    Instantiate(respawner, y, Quaternion.identity);
                    flag = false;
                }
            }
            cooldown = 45;
        }
        if(GameScript.gs.deadPlayers.Count > 0)
        {
            cooldown -= Time.fixedDeltaTime;
        }
    }
}
