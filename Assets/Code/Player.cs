using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiJack;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public static List<Player> players = new List<Player>();
    public enum SHAPE { CIRCLE, TRIANGLE, SQUARE };
    public enum TEAM { ONE, TWO, THREE, FOUR, FIVE };
    public enum KEY { LEFT, RIGHT, FORWARD, ACTION};

    const float BULLET_SPEED = 20;

    public int cooldown = 0;

    public SHAPE shape;
    public TEAM team;
    public Vector3Int gridPosition;

    private float speed = 12.0f;
    private float rotationSpeed = 1400.0f;
    public Vector2 target; 
    private GameObject rotationTarget;

    public int health;

    private GameObject bullet;
    public GameObject mySquare;
    public GameObject shield;
    private Quaternion shieldRot;

    // Start is called before the first frame update
    void Start()
    {
        players.Add(this);
        health = Constants.MAX_HEALTH;
        bullet = Resources.Load<GameObject>("Prefabs/BulletPrefab");
        gridPosition = StageGrid.instance.GetCellFromWorld(transform.position);
        StageGrid.instance.SetPlayerAt((Vector2Int)gridPosition);
        Vector2 startPos = (Vector2)StageGrid.instance.GetWorldFromCell(gridPosition);
        startPos.x += 0.5f;
        startPos.y += 0.5f;
        transform.position = startPos;
        target = transform.position;
        rotationTarget = new GameObject();
        rotationTarget.transform.rotation = transform.rotation;
        shieldRot = Quaternion.identity;
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
            if(rotation < 22.5) { direction = new Vector2Int(0, 1); }
            else if (rotation < 67.5) { direction = new Vector2Int(-1, 1); }
            else if (rotation < 112.5) { direction = new Vector2Int(-1, 0); }
            else if (rotation < 157.5) { direction = new Vector2Int(-1, -1); }
            else if (rotation < 202.5) { direction = new Vector2Int(0, -1); }
            else if (rotation < 247.5) { direction = new Vector2Int(1, -1); }
            else if (rotation < 292.5) { direction = new Vector2Int(1, 0); }
            else if (rotation < 337.5) { direction = new Vector2Int(1, 1); }
            else if (rotation < 360.5) { direction = new Vector2Int(0, 1); }
            TryMove(direction);
        }
        if (PressedKey(KEY.LEFT))
        {
            rotationTarget.transform.Rotate(new Vector3(0, 0, 90));
        }
        if (PressedKey(KEY.RIGHT))
        {
            rotationTarget.transform.Rotate(new Vector3(0, 0, -90));
        }
        if (PressedKey(KEY.ACTION))
        {
            if (cooldown <= 0)
            {
                switch (shape)
            {
                case (SHAPE.TRIANGLE):
                        FireBullet();
                        cooldown = 120;
                        SoundManager.instance.Fire();
                break;
                case (SHAPE.CIRCLE):
                        TeleportToSquare();
                        cooldown = 300;
                        SoundManager.instance.Teleport();
                break;
                case (SHAPE.SQUARE):
                        cooldown = 0;
                        MoveShield();
                break;
                }
            }
            //Debug.Log("Someone tried to use an ability!");
        }
        if (health <= 0)
        {
            CameraScript.instance.Shake(45, 0.55f);
            SoundManager.instance.Die();
            StageGrid.instance.PlayerDied((Vector2Int)gridPosition);
            GameScript.gs.PlayerDied(this);
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        cooldown--;
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target, step);
        step = rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationTarget.transform.rotation, step);
        if(shape == SHAPE.SQUARE)
        {
            float step1 = rotationSpeed * Time.deltaTime;
            shield.transform.rotation = Quaternion.RotateTowards(shield.transform.rotation, shieldRot, step1);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        if (shape == SHAPE.CIRCLE)
        {
            int val = StageGrid.instance.CircleIsMoveValid((Vector2Int)gridPosition, direction);
            if ( val!= 0)
            {
                Move(direction);
                if(val == 1)
                {
                    for ( int i = 0; i < players.Count; i++)
                    {
                        if(gridPosition.x == players[i].gridPosition.x && gridPosition.y == players[i].gridPosition.y)
                        {
                            Debug.Log("Stunned " + players[i].shape + " on team " + players[i].team + "!");
                            SoundManager.instance.Stun();
                        }
                    }
                }
            }
        }
        else
        {
            if (StageGrid.instance.PlayerIsMoveValid((Vector2Int)gridPosition, direction))
            {
                Move(direction);
            }
        }
    }

    public void Move(Vector2Int direction)
    {
        StageGrid.instance.PlayerMove((Vector2Int)gridPosition, direction);
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
        if(mySquare == null)
        {
            return;
        }
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
        shieldRot = rotationTarget.transform.rotation;
    }

    public void TakeDamageFromDirection(int damage, Vector2 direction, Vector2 bullet_pos)
    {
        if(shape == SHAPE.SQUARE)
        {
            if (Vector2.Dot((shield.transform.rotation * Vector2.up).normalized, direction.normalized) >= -0.7)
            {
                health -= damage;
            }
            else
            {
                //Debug.Log("Blocked " + damage + " damage");
                Debug.Log("Tink!");
                SoundManager.instance.Tink();
            }
        }
        else
        {
            health -= damage;
        }
    }
}
