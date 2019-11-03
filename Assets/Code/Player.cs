using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public enum SHAPE { CIRCLE, TRIANGLE, SQUARE };
    public enum TEAM { ONE, TWO, THREE, FOUR, FIVE };
    public enum KEY { LEFT, RIGHT, FORWARD, ACTION};

    const float BULLET_SPEED = 20;

    public SHAPE shape;
    public TEAM team;
    public Vector3Int gridPosition;

    public int health;

    private GameObject bullet;

    // Start is called before the first frame update
    void Start()
    {
        health = Constants.MAX_HEALTH;
        bullet = Resources.Load<GameObject>("Prefabs/BulletPrefab");
        //gridPosition = StageGrid.instance.GetCellFromWorld(transform.position);
        //StageGrid.instance.SetPlayerAt((Vector2Int)gridPosition);
    }

    // Update is called once per frame
    void Update()
    {
        gridPosition = StageGrid.instance.GetCellFromWorld(transform.position);
        gridPosition.z = -1;
        //Debug.Log(StageGrid.instance.GetTileAt(StageGrid.instance.tilemap.WorldToCell(transform.position).x, StageGrid.instance.tilemap.WorldToCell(transform.position).y).name);
        if (PressedKey(KEY.FORWARD))
        {
            Debug.Log("Player pos is: " + gridPosition);
            Vector2Int temp = new Vector2Int(gridPosition.x, gridPosition.y);
            temp.x -= StageGrid.instance.tilemaps[0].cellBounds.xMin;
            temp.y -= StageGrid.instance.tilemaps[0].cellBounds.yMin;
            //Debug.Log("Temp is: " + temp);
            float rotation = transform.eulerAngles.z;
            if(rotation < 30)
            {
                if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, new Vector2Int(0, 1)))
                {
                    StageGrid.instance.PlayerMove((Vector2Int)gridPosition, new Vector2Int(0, 1));
                    gridPosition.y++;
                }
            }
            /*else if(rotation < 60)
            {
                gridPosition.y++;
                gridPosition.x--;
            }*/
            else if (rotation < 120)
            {
                if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, new Vector2Int(-1,0)))
                {
                    StageGrid.instance.PlayerMove((Vector2Int)gridPosition, new Vector2Int(-1, 0));
                    gridPosition.x--;
                }
            }
            else if (rotation < 210)
            {
                if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, new Vector2Int(0, -1)))
                {
                    StageGrid.instance.PlayerMove((Vector2Int)gridPosition, new Vector2Int(0, -1));
                    gridPosition.y--;
                }
            }
            else if (rotation < 300)
            {
                if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, new Vector2Int(1, 0)))
                {
                    StageGrid.instance.PlayerMove((Vector2Int)gridPosition, new Vector2Int(1, 0));
                    gridPosition.x++;
                }
            }
            //Debug.Log("The " + shape + " on team " + team + " wants to move forward!");
        }
        if (PressedKey(KEY.LEFT))
        {
            transform.Rotate(new Vector3(0,0,90));
            //Debug.Log("The " + shape + " on team " + team + " wants to turn left!");
        }
        if (PressedKey(KEY.RIGHT))
        {
            transform.Rotate(new Vector3(0, 0, -90));
            //Debug.Log("The " + shape + " on team " + team + " wants to turn right!");
        }
        if (PressedKey(KEY.ACTION))
        {
            FireBullet();
            //Debug.Log("The " + shape + " on team " + team + " wants to use their action!");
        }
        Vector3 pos = StageGrid.instance.GetWorldFromCell(gridPosition);
        pos.z = -1;
        pos.x += 0.5f;
        pos.y += 0.5f;
        transform.position = pos;
        if (health <= 0)
        {
            Destroy(this);
        }
    }

    // Was a key pressed?
    private bool PressedKey(KEY key)
    {
        return MidiMaster.GetKeyDown(36 + ((int)shape) * 4 + (int)team * 12 + GetKeyOffset(key));
    }

    // How hard was a key pressed?
    private float KeyForce(KEY key)
    {
        return 0;
    }

    public int GetKeyOffset (KEY key)
    {
        if (shape == SHAPE.CIRCLE)
        {
            if (key == KEY.LEFT)
                return 0;
            else if (key == KEY.RIGHT)
                return 2;
            else if (key == KEY.FORWARD)
                return 1;
            else if (key == KEY.ACTION)
                return 3;
        }
        else if (shape == SHAPE.TRIANGLE)
        {
            if (key == KEY.LEFT)
                return 0;
            else if (key == KEY.RIGHT)
                return 3;
            else if (key == KEY.FORWARD)
                return 1;
            else if (key == KEY.ACTION)
                return 2;
        }
        else if (shape == SHAPE.SQUARE)
        {
            if (key == KEY.LEFT)
                return 1;
            else if (key == KEY.RIGHT)
                return 3;
            else if (key == KEY.FORWARD)
                return 2;
            else if (key == KEY.ACTION)
                return 0;
        }
        Debug.Log("Player is formless!!!! (shape field not set)");
        return -1;
    }

    private bool TryMoveForward()
    {
        return true;
    }

    private void FireBullet()
    {
        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
        b.GetComponent<BulletBehavior>().team = team;
        b.GetComponent<Rigidbody2D>().velocity = (transform.rotation * Vector3.up) * BULLET_SPEED;
    }
}
