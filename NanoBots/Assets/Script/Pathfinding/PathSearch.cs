using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathSearch
{
    public List<PathNode> nodeList = new List<PathNode>();
    public List<PathNode> openList = new List<PathNode>();
    public List<PathNode> closedList = new List<PathNode>();

    public Vector3Int finalDestination;
    public Vector3Int startingCell;

    public Pathfinder pathfinder;


    public int moves = 0;

    public PathSearch(Vector3Int start, Vector3Int finish, Pathfinder pathfinder)
    {
        this.pathfinder = pathfinder;

        nodeList = new List<PathNode>();
        openList = new List<PathNode>();
        closedList = new List<PathNode>();

        startingCell = start;
        finalDestination = finish;
    }

    public Path FindPath()
    {
        PathError error = new PathError();

        PreProcess(error);

        if (error.errorType != PathError.PathErrorType.None)
        {
            return new Path(error);
        }

        // 1.
        moves = 0;
        PathNode currentNode = new PathNode(moves, startingCell, null, finalDestination);
        closedList.Add(currentNode);
        GenerateNeighbourNodes(currentNode.Moves, currentNode);

        do
        {
            //pathfinder.pathManagerInstance.baseTilemap.SetTile(currentNode.CellPosition, pathfinder.pathManagerInstance.CenterTile);
            //System.Threading.Thread.Sleep(750);

            if (openList.Count == 0)
            {
                error.errorType = PathError.PathErrorType.NoPathAvailable;
                break;
            }

            // Check openlist for best option, best option gets added to the closed list.
            currentNode = ChooseBestOption();
            closedList.Add(currentNode);

            if (currentNode.isDestinationNode)
            { break; }

            // if this node is being search it must be on the openlist, remove it so we can selcet it as an option again for this search.
            openList.Remove(currentNode);

            // Generate Neighbours which get added to openlist.
            GenerateNeighbourNodes(currentNode.Moves, currentNode);
        } while (true);

        // 2.
        // if a full path can't be found, we will retur a path to the tile with best heuristic from the destination
        // setting currentNode to that tile will cause the loop to start retracing its path from that tile.
        if (error.errorType == PathError.PathErrorType.NoPathAvailable)
        {
            currentNode = GetNodeWithBestHeuristicInList(closedList);
        }

        // 3.
        // We must retrace our steps fronm destination, returniung to each tiles origin until we are back at the startnode
        List<Vector3Int> path = new List<Vector3Int>();

        do
        {
            path.Add(currentNode.CellPosition);
            currentNode = currentNode.OriginNode;

            if (currentNode.isStartingNode)
            { break; }

        } while (true);

        return new Path(path.Count, path.ToArray(), error);
    }

    private PathNode GetNodeWithBestHeuristicInList(List<PathNode> list)
    {
        int lowest = int.MaxValue;
        PathNode bestOption = null;

        foreach (PathNode node in list)
        {
            if (node.Heuristic < lowest)
            {
                lowest = node.Heuristic;
                bestOption = node;
            }
        }

        return bestOption;
    }

    private void PreProcess(PathError error)
    {
        openList = new List<PathNode>();
        closedList = new List<PathNode>();

        // Start out of bounds
        if (!pathfinder.pathManagerInstance.IsInBounds(startingCell))
        {
            error.errorType = PathError.PathErrorType.StartOutOfBounds;
            return;
        }
        // Destination out of bounds
        if (!pathfinder.pathManagerInstance.IsInBounds(finalDestination))
        {
            error.errorType = PathError.PathErrorType.DestinationOutOfBounds;
            return;
        }
        // Start not Walkable
        if (!pathfinder.pathManagerInstance.IsCellWalkable(startingCell))
        {
            error.errorType = PathError.PathErrorType.StartNotWalkable;
            return;
        }
        // Destination not Walkable
        if (!pathfinder.pathManagerInstance.IsCellWalkable(startingCell))
        {
            error.errorType = PathError.PathErrorType.DestinationNotWalkable;
            return;
        }
    }

    public PathNode ChooseBestOption()
    {
        if (openList.Count <= 0)
        {
            return null;
        }

        PathNode bestOption = null;
        int bestScore = int.MaxValue;
        List<PathNode> bestOptSameScore = new List<PathNode>();

        // 1. Determine if we have one clear best option or multiple
        foreach (PathNode node in openList)
        {
            if (node.Score == bestScore)
            {
                bestOptSameScore.Add(node);
                continue;
            }

            if (node.Score < bestScore)
            {
                bestOption = node;
                bestScore = node.Score;
                bestOptSameScore.Clear();
                bestOptSameScore.Add(node);
            }
        }

        //2 Determine the best of best options
        if (bestOptSameScore.Count > 1)
        {
            int lowestMoveCost = int.MaxValue;

            foreach (PathNode node in bestOptSameScore)
            {
                if (node.Moves < lowestMoveCost)
                {
                    bestOption = node;
                    lowestMoveCost = node.Moves;
                }

                // if another tie, use the first bestoption
            }
        }
        else
        {
            return bestOption;
        }

        return bestOption;
    }

    private void GenerateNeighbourNodes(int moves, PathNode currentNode)
    {
        PathNode up = null;
        PathNode right = null;
        PathNode down = null;
        PathNode left = null;
        moves++;

        Vector3Int neighbour;

        neighbour = Vector3Int.up + currentNode.CellPosition;
        if (CanCreate(neighbour))
        {
            up = new PathNode(moves, neighbour, currentNode, finalDestination);
            openList.Add(up);
        }

        neighbour = Vector3Int.right + currentNode.CellPosition;
        if (CanCreate(neighbour))
        {
            right = new PathNode(moves, neighbour, currentNode, finalDestination);
            openList.Add(right);
        }

        neighbour = Vector3Int.down + currentNode.CellPosition;
        if (CanCreate(neighbour))
        {
            down = new PathNode(moves, neighbour, currentNode, finalDestination);
            openList.Add(down);
        }

        neighbour = Vector3Int.left + currentNode.CellPosition;
        if (CanCreate(neighbour))
        {
            left = new PathNode(moves, neighbour, currentNode, finalDestination);
            openList.Add(left);
        }

    }

    private bool CanCreate(Vector3Int neighbour)
    {
        // False if neighbour pos is already in closed or openlist, or if it is not traversable
        for (int i = 0; i < openList.Count; ++i)
        {
            if (openList[i].CellPosition == neighbour)
            {
                return false;
            }
        }

        for (int i = 0; i < closedList.Count; ++i)
        {
            if (closedList[i].CellPosition == neighbour)
            {
                return false;
            }
        }

        return pathfinder.pathManagerInstance.IsCellWalkable(neighbour);
    }
}