using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Structure : PlacableObject
{
    public Vector3Int structureSize;
    private BoundsInt area;
    public Tile CenterTile = null;

    public override void Start ()
    {
        base.Start();

        area = new BoundsInt(gridPosition, structureSize);

        foreach (Vector3Int pos in area.allPositionsWithin)
        {
            //tilemap.SetTile(pos, CenterTile);
        }
    }

    public override void Update()
    {
        base.Update();
    }

    
}
