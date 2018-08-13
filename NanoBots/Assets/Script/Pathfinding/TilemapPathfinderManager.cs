using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapPathfinderManager : MonoBehaviour
{
    public Vector3Int GridBounds;

    List<Vector3Int> walkableTiles;
    List<Vector3Int> unWalkableTiles;

    public Tilemap baseTilemap;
    public List<Tilemap> otherTilemaps;

    private BoundsInt mapBounds;

    public Tile CenterTile;

    public BoundsInt MapBounds { get {  return mapBounds;  } }

    void Start ()
    {
	    if (baseTilemap != null)
        {
            GridBounds = baseTilemap.size;
            
            walkableTiles = new List<Vector3Int>();
            unWalkableTiles = new List<Vector3Int>();

            ProcessTileMaps();
        }
        else
        {
            Debug.Log("A kinda important thing is NULL");
        }

    }

    /// <summary>
    /// Process the tile map to determine walkable tiles
    /// </summary>
    private void ProcessTileMaps()
    {
        Debug.Log(baseTilemap.name + " Size: " + baseTilemap.size);

        mapBounds = baseTilemap.cellBounds;

        foreach (Tilemap map in otherTilemaps)
        {
            for (int x = map.cellBounds.xMin; x < map.cellBounds.xMax; ++x)
            {
                for(int y = map.cellBounds.yMin;  y < map.cellBounds.yMax; ++y)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);

                    if (map.HasTile(tilePos))
                    {
                        //Debug.Log(string.Format("Tile ({0}, {1}): is Occupied", x, y));

                        if (map != baseTilemap)
                        {
                            unWalkableTiles.Add(tilePos);
                            baseTilemap.SetTile(tilePos, CenterTile);
                        }
                        else
                        {
                            walkableTiles.Add(tilePos);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Refreshes the Walkable tiles
    /// </summary>
    public void UpdateWalkableTiles()
    {
        unWalkableTiles = new List<Vector3Int>();
        walkableTiles = new List<Vector3Int>();

        ProcessTileMaps();
    }

    /// <summary>
    /// Checks if a tile exist in the Unwalkable list.
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    public bool IsCellWalkable(Vector3Int tilePos)
    {
        if (!IsInBounds(tilePos))
        {
            return false;
        }

        if (unWalkableTiles != null && unWalkableTiles.Count > 0)
        {
            if (unWalkableTiles.Contains(tilePos))
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public bool IsInBounds(Vector3Int pos)
    {
        if ((pos.x > mapBounds.xMin && pos.x < mapBounds.xMax) && (pos.y > mapBounds.yMin && pos.y < mapBounds.yMax))
        {
            return true;
        }

        return false;
    }

    void Update ()
    {
		
	}
}
