using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StageGrid : MonoBehaviour
{
    public enum STATUS { UNOCCUPIED, OCCUPIED, UNTRAVERSABLE}
    public static StageGrid instance;
    public List<Tilemap> tilemaps;
    public Vector3Int origin;
    public STATUS[,] worldStatusArray;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        tilemaps = new List<Tilemap>();
        for(int i = 0; i < gameObject.transform.childCount; i++)
        {
            tilemaps.Add(gameObject.transform.GetChild(i).GetComponent<Tilemap>());
        }
        worldStatusArray = new STATUS[tilemaps[0].cellBounds.xMax - tilemaps[0].cellBounds.xMin, tilemaps[0].cellBounds.yMax - tilemaps[0].cellBounds.yMin];
        Debug.Log(worldStatusArray.GetLength(0) + ", " + worldStatusArray.GetLength(1));
        origin.x = tilemaps[0].cellBounds.xMin;
        origin.y = tilemaps[0].cellBounds.yMin;
        origin.z = tilemaps[0].cellBounds.zMin;
        for (int i = 0; i < worldStatusArray.GetLength(0); i++)
        {
            for (int j = 0; j < worldStatusArray.GetLength(1); j++)
            {
                worldStatusArray[i, j] = STATUS.UNOCCUPIED;
                List<TileBase> tiles = GetTilesAt(i , j);
                for(int k = 0; k < tiles.Count; k++)
                {
                    if(tiles[k].name == "Wall_1")
                    {
                        worldStatusArray[i, j] = STATUS.UNTRAVERSABLE;
                    }
                }
            }
        }
    }   

    public Vector3Int GetCellFromWorld(Vector2 position)
    {
        Vector3Int pos = tilemaps[0].WorldToCell(position);
        return pos;
    }

    public Vector3 GetWorldFromCell(Vector3Int position)
    {
        return tilemaps[0].CellToWorld(position);
    }

    public List<TileBase> GetTilesAt(int x, int y)
    {
        List<TileBase> tiles = new List<TileBase>();
        if (x < 0 || y < 0)
            return null;
        if (x > tilemaps[0].cellBounds.xMax - tilemaps[0].cellBounds.xMin)
            return null;
        if(y > tilemaps[0].cellBounds.yMax - tilemaps[0].cellBounds.yMin)
            return null;
        for (int i = 0; i < tilemaps.Count; i++)
        {
            TileBase temp = tilemaps[i].GetTile(new Vector3Int(origin.x + x, origin.y + y, origin.z));
            if (temp != null)
            {
                tiles.Add(temp);
            }
        }
        return tiles;
    }

    public STATUS[,] GetSurroundings(Vector2Int position)
    {
        Vector2Int pos = new Vector2Int(position.x - tilemaps[0].cellBounds.xMin, position.y - tilemaps[0].cellBounds.yMin);
        STATUS[,] surroundings = new STATUS[3,3];
        for(int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                surroundings[i, j] = STATUS.UNTRAVERSABLE;
                if(pos.x - 1 + i >= 0 && pos.x - 1 + i <= worldStatusArray.GetLength(0) && pos.y - 1 + j >= 0 && pos.y - 1 + j <= worldStatusArray.GetLength(1))
                {
                    surroundings[i, j] = worldStatusArray[pos.x - 1 + i, pos.y - 1 + i];
                }
            }
        }
        return surroundings;
    }

    public void SetPlayerAt(Vector2Int position)
    {
        Vector2Int pos = new Vector2Int(position.x - tilemaps[0].cellBounds.xMin, position.y - tilemaps[0].cellBounds.yMin);
        if (worldStatusArray[pos.x, pos.y] == STATUS.UNTRAVERSABLE || worldStatusArray[pos.x, pos.y] == STATUS.OCCUPIED)
            Debug.Log("Bad player start position");
        worldStatusArray[pos.x, pos .y] = STATUS.OCCUPIED;
    }

    public void PlayerMove(Vector2Int current_pos, Vector2Int displacement)
    {

        Vector3Int pos = new Vector3Int(current_pos.x - origin.x, current_pos.y - origin.y, origin.z);
        Debug.Log("Position is: " + pos + ", Destination is " + (pos + (Vector3Int)displacement));
        worldStatusArray[pos.x, pos.y] = STATUS.UNOCCUPIED;
        worldStatusArray[pos.x + displacement.x, pos.y + displacement.y] = STATUS.OCCUPIED;
    }

    public bool PlayerIsMoveValid(Vector2Int current_pos, Vector2Int displacement)
    {
        Vector2Int pos = new Vector2Int(current_pos.x - tilemaps[0].cellBounds.xMin, current_pos.y - tilemaps[0].cellBounds.yMin);
        //Debug.Log("Cell at " + (pos.x + displacement.x) + ", " + (pos.y + displacement.y) + " is " + worldStatusArray[(pos.x + displacement.x), (pos.y + displacement.y)]);

        if (worldStatusArray[(pos + displacement).x, (pos + displacement).y] == STATUS.UNOCCUPIED)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CircleIsMoveValid(Vector2Int current_pos, Vector2Int displacement)
    {
        Vector2Int pos = new Vector2Int(current_pos.x - tilemaps[0].cellBounds.xMin, current_pos.y - tilemaps[0].cellBounds.yMin);
        //Debug.Log("Cell at " + (pos.x + displacement.x) + ", " + (pos.y + displacement.y) + " is " + worldStatusArray[(pos.x + displacement.x), (pos.y + displacement.y)]);

        if (worldStatusArray[(pos + displacement).x, (pos + displacement).y] == STATUS.UNTRAVERSABLE)
        {
            return 0;
        }
        else if (worldStatusArray[(pos + displacement).x, (pos + displacement).y] == STATUS.OCCUPIED)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    public void PlayerDied(Vector2Int current_pos)
    {
        Vector3Int pos = new Vector3Int(current_pos.x - origin.x, current_pos.y - origin.y, origin.z);
        worldStatusArray[pos.x, pos.y] = STATUS.UNOCCUPIED;
    }
}