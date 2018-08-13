using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{

    public Tilemap grassTileMap = null;

    bool movingUp, movingDown, movingLeft, movingRight = false;
    Camera controlledCamera = null;

    public float moveSpeed = 1.5f;

    float mapX = 100;
    float mapY = 100;

    float minX, maxX, minY, maxY;

    Vector3Int centerTile, topEdgeTile, botEdgeTile, leftEdgeTile, rightEdgeTile;

    Vector2 camTopEdgeScreenPos;
    Vector3 camTopEdgeWorldPos;

    Vector2 camBotEdgeScreenPos;
    Vector3 camBotEdgeWorldPos;

    Vector2 camLeftEdgeScreenPos;
    Vector3 camLeftEdgeWorldPos;

    Vector2 camRightEdgeScreenPos;
    Vector3 camRightEdgeWorldPos;

    public Tile CenterTile = null;

    // Use this for initialization
    void Start ()
    {
        controlledCamera = GetComponent<Camera>();

        camTopEdgeScreenPos = new Vector2(Screen.width / 2, Screen.height);
        camTopEdgeWorldPos = controlledCamera.ScreenToWorldPoint(camTopEdgeScreenPos);

        //Vector3Int centerTile = bgMap.WorldToCell(cameraPosWorldSpace);
        centerTile = grassTileMap.WorldToCell(controlledCamera.transform.position);
        topEdgeTile = grassTileMap.WorldToCell(camTopEdgeWorldPos);
        botEdgeTile = grassTileMap.WorldToCell(controlledCamera.transform.position);
        leftEdgeTile = grassTileMap.WorldToCell(controlledCamera.transform.position);
        rightEdgeTile = grassTileMap.WorldToCell(controlledCamera.transform.position);

        //grassTileMap.SetTile(centerTile, CenterTile);
        //grassTileMap.SetTile(topEdgeTile, CenterTile);
        

       // Debug.Log("Center Tile: " + centerTile);
       // Debug.Log("Top edge Tile: " + topEdgeTile);


        //Debug.Log("cameraPosScreenSpace: " + cameraPosScreenSpace);
        // Debug.Log("cameraPosWorldSpace: " + cameraPosWorldSpace);

        float vertExtent = controlledCamera.orthographicSize;
        float horzExtent = controlledCamera.orthographicSize * Screen.width / Screen.height;

        minX = horzExtent - mapX / 2f;
        maxX = mapX / 2f - horzExtent;
        minY = vertExtent - mapY / 2f;
        maxY = mapY / 2f - vertExtent;

        //Debug.Log("Screen(Width): " + Screen.width);
        //Debug.Log("Screen(Height): " + Screen.height);

       //Debug.Log("Bounds(Cell): " + grassTileMap.cellBounds);
       //Debug.Log("Bounds(Local): " + grassTileMap.localBounds);
       //Debug.Log("Bounds(Size): " + grassTileMap.size);

       // Debug.Log("Tilemap(Origin): " + grassTileMap.origin);

        //Debug.Log("orthographicSize = vertExtent: " + controlledCamera.orthographicSize);
        //Debug.Log("horzExtent: " + controlledCamera.orthographicSize * Screen.width / Screen.height);

        //Debug.Log("minX: " + minX);
        //Debug.Log("maxX: " + maxX);
        //Debug.Log("minY: " + minY);
        //Debug.Log("maxY: " + maxY);
    }
	
	// Update is called once per frame
	void Update ()
    {
        camTopEdgeScreenPos = new Vector2(Screen.width / 2, Screen.height);
        camTopEdgeWorldPos = controlledCamera.ScreenToWorldPoint(camTopEdgeScreenPos);
        topEdgeTile = grassTileMap.WorldToCell(camTopEdgeWorldPos);

        camBotEdgeScreenPos = new Vector2(Screen.width / 2, 0);
        camBotEdgeWorldPos = controlledCamera.ScreenToWorldPoint(camBotEdgeScreenPos);
        botEdgeTile = grassTileMap.WorldToCell(camBotEdgeWorldPos);

        camLeftEdgeScreenPos = new Vector2(0, 0);
        camLeftEdgeWorldPos = controlledCamera.ScreenToWorldPoint(camLeftEdgeScreenPos);
        leftEdgeTile = grassTileMap.WorldToCell(camLeftEdgeWorldPos);

        camRightEdgeScreenPos = new Vector2(Screen.width, 0);
        camRightEdgeWorldPos = controlledCamera.ScreenToWorldPoint(camRightEdgeScreenPos);
        rightEdgeTile = grassTileMap.WorldToCell(camRightEdgeWorldPos);

        //Debug.Log("rightEdgeTile: " + rightEdgeTile);

        // Up
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            movingUp = true;
        }
        else if(Input.GetKeyUp(KeyCode.UpArrow))
        {
            movingUp = false;
        }

        // down
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            movingDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            movingDown = false;
        }

        // left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            movingLeft = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            movingLeft = false;
        }

        // right
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            movingRight = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            movingRight = false;
        }

        MoveCamera();
        //PrintUserInput();
    }

    private void MoveCamera()
    {
        if (movingUp)
        {
            if ((topEdgeTile.y + 1f) < grassTileMap.size.y / 2)
            {
                transform.position += Vector3.up * moveSpeed;
            }
        }
        if (movingDown)
        {
            if ((botEdgeTile.y - 0.5f) > -grassTileMap.size.y / 2)
            {
                transform.position += Vector3.down * moveSpeed;
            }
        }
        if (movingLeft)
        {
            if ((leftEdgeTile.x - 0.5f) > -grassTileMap.size.x / 2)
            {
                transform.position += Vector3.left * moveSpeed;
            }
        }
        if (movingRight)
        { 
            if ((rightEdgeTile.x +       1) < grassTileMap.size.x / 2)
            {
                transform.position += Vector3.right * moveSpeed;
            }
        }
    }

    private void PrintUserInput()
    {
        if (movingUp)
            Debug.Log("up");
        if (movingDown)
            Debug.Log("down");
        if (movingLeft)
            Debug.Log("left");
        if (movingRight)
            Debug.Log("right");
    }

	
}
