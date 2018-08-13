using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private Vector3Int cellPosition;
    private PathNode originNode;
    private Vector3Int destinationCell;

    public bool isStartingNode = false;
    public bool isDestinationNode = false;

    private int moves;
    private int heuristic;
    private int score;

    public int Moves {  get { return moves; } }
    public int Heuristic { get {  return heuristic; } }
    public int Score { get { return score; } }

    public Vector3Int CellPosition { get {  return cellPosition; } }
    public PathNode OriginNode { get {  return originNode; } }

    /// <summary>
    /// Create Instance
    /// </summary>
    /// <param name="numMoves">The number of moves made to get to this cell</param>
    /// <param name="cellPosition">the position of the cell in the grid</param>
    /// <param name="originNode"> The cell that generated this cell</param>
    /// <param name="destination">the cell we are trying to reach</param>
    public PathNode(int numMoves, Vector3Int cellPosition, PathNode originNode, Vector3Int destination)
    {
        this.cellPosition = cellPosition;
        this.originNode = originNode;
        this.destinationCell = destination;
        moves = numMoves;

        if (originNode == null)
        {
            isStartingNode = true;
        }

        if(cellPosition == destination)
        {
            isDestinationNode = true;
        }

        CalculateScore();
    }

    /// <summary>
    /// Calculate the score of Node given
    /// </summary>
    private void CalculateScore()
    {
        heuristic = CalculateManhattanDistance();
        score = moves + heuristic;
    }

    /// <summary>
    /// Calculate distance to destinaion using the Manhattan Block method.
    /// </summary>
    /// <returns></returns>
    private int CalculateManhattanDistance()
    {
        int xDistance = Mathf.Abs(CellPosition.x - destinationCell.x);
        int yDistance = Mathf.Abs(CellPosition.y - destinationCell.y);

        return xDistance + yDistance;
    }
}
