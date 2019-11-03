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

    private float speed = 5.0f;
    private Vector2 target;
<<<<<<< HEAD

=======
    private Vector2 position;
>>>>>>> d10dfd52d5f7df69a6e3048ebded3a389530f0bc

    public int health;

    private GameObject bullet;
    public GameObject mySquare;

    // Start is called before the first frame update
    void Start()
    {
        health = Constants.MAX_HEALTH;
        bullet = Resources.Load<GameObject>("Prefabs/BulletPrefab");
        gridPosition = StageGrid.instance.GetCellFromWorld(transform.position);
        StageGrid.instance.SetPlayerAt((Vector2Int)gridPosition);
        Vector2 startPos = (Vector2)StageGrid.instance.GetWorldFromCell(gridPosition);
        startPos.x += 0.5f;
        startPos.y += 0.5f;
        transform.position = startPos;
        target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gridPosition = StageGrid.instance.GetCellFromWorld(transform.position);
        gridPosition.z = -1;

        if (PressedKey(KEY.FORWARD))
        {
            Vector2Int direction = new Vector2Int(0,0);
            float rotation = transform.eulerAngles.z;
            if(rotation < 30) { direction = new Vector2Int(0, 1); }
            else if (rotation < 120) { direction = new Vector2Int(-1, 0); }
            else if (rotation < 210) { direction = new Vector2Int(0, -1); }
            else if (rotation < 300) { direction = new Vector2Int(1, 0); }
            TryMove(direction);
        }
        if (PressedKey(KEY.LEFT))
        {
            transform.Rotate(new Vector3(0,0,90));
        }
        if (PressedKey(KEY.RIGHT))
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }
        if (PressedKey(KEY.ACTION))
        {
<<<<<<< HEAD
            switch (shape)
            {
                case (SHAPE.TRIANGLE):
                    FireBullet();
                    break;
                case (SHAPE.CIRCLE):
                    TeleportToSquare();
                    Debug.Log("CIRCLE!!!!!");
                    break;
                case (SHAPE.SQUARE):
                    break;
            }
            Debug.Log("Someone tried to use an ability!");
=======
            FireBullet();
>>>>>>> d10dfd52d5f7df69a6e3048ebded3a389530f0bc
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, direction))
        {
            Move(direction);
        }
    }

<<<<<<< HEAD
    public void Move(Vector2Int direction)
    {
        StageGrid.instance.PlayerMove((Vector2Int)gridPosition, direction);
=======
    private void FixedUpdate()
    {
        
    }

    private void TryMove(Vector2Int direction)
    {
        if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, direction))
        {
            Move(direction);
        }
    }

    public void Move(Vector2Int direction)
    {
        StageGrid.instance.PlayerMove((Vector2Int)gridPosition, direction);
        position = gameObject.transform.position;
>>>>>>> d10dfd52d5f7df69a6e3048ebded3a389530f0bc
        gridPosition.y += direction.y;
        gridPosition.x += direction.x;
        target = (Vector2)StageGrid.instance.GetWorldFromCell(gridPosition);
        target.x += 0.5f;
        target.y += 0.5f;
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

    private void FireBullet()
    {
        GameObject b = Instantiate(bullet, transform.position, transform.rotation);
        b.GetComponent<BulletBehavior>().team = team;
        b.GetComponent<Rigidbody2D>().velocity = (transform.rotation * Vector3.up) * BULLET_SPEED;
    }

    private void TeleportToSquare()
    {
        Player sq = mySquare.GetComponent<Player>();
        StageGrid.STATUS[,] s = StageGrid.instance.GetSurroundings((Vector2Int)sq.gridPosition);
        for(int i = 0; i < s.GetLength(0); i++)
        {
            for (int j = 0; j < s.GetLength(1); j++)
            {
                Debug.Log(i + ", " + j + " is " + s[i, j]);
                if(s[i, j] == StageGrid.STATUS.UNOCCUPIED)
                {
                    StageGrid.instance.PlayerMove((Vector2Int)gridPosition, new Vector2Int(sq.gridPosition.x + i - 1 - gridPosition.x, sq.gridPosition.y + j - 1 - gridPosition.y));
                    transform.position = mySquare.transform.position + new Vector3(i - 1, j - 1, 0);
                    target = transform.position;
                    return;
                }
            }
        }
    }

    private void MoveShield()
    {

    }

    private void OnDestroy()
    {
        StageGrid.instance.PlayerDied((Vector2Int)gridPosition);
    }
}
