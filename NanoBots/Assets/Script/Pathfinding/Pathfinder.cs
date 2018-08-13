using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    public TilemapPathfinderManager pathManagerInstance; // still have to determin how each pathfinder will gain acces to this 
    public PathSearch currentSearch;

    public Vector3Int tStart, tFinish;

    private void Start()
    {
        Path p = CalculatePath(tStart, tFinish);

        Debug.Log("path length: " + p.PathList.Length);
        Debug.Log("path error: " + p.Error.errorType.ToString());

        for (int i = 0; i < p.PathList.Length; ++i)
        {
            pathManagerInstance.baseTilemap.SetTile(p.PathList[i], pathManagerInstance.CenterTile);
        }

        pathManagerInstance.baseTilemap.SetTile(tStart, pathManagerInstance.CenterTile);
        pathManagerInstance.baseTilemap.SetTile(tFinish, pathManagerInstance.CenterTile);
    }


    public Path CalculatePath(Vector3Int startCell, Vector3Int finCell)
    {
        currentSearch = new PathSearch(startCell, finCell, this);
        return currentSearch.FindPath();
    }
}
