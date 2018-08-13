using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    private Vector3Int[] pathList;
    private int numNodesInPath;
    private PathError error;

    public Vector3Int[] PathList { get {  return pathList;  } }
    public PathError Error { get { return error; } }

    public Path(int numNodesInPath, Vector3Int[] path, PathError error)
    {
        pathList = new Vector3Int[numNodesInPath];
        this.error = error;

        for (int i = 0; i < numNodesInPath; ++i)
        {
            this.pathList[i] = path[i];
        }
    }

    public Path(PathError error)
    {
        pathList = new Vector3Int[0];
        this.error = error;
    }
}

public struct PathError
{
    public enum PathErrorType
    {
        None,
        NoPathAvailable,
        StartOutOfBounds,
        DestinationOutOfBounds,
        StartNotWalkable,
        DestinationNotWalkable,
    }

    public PathErrorType errorType;
}
