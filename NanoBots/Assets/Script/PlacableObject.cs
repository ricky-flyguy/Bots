using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacableObject : MonoBehaviour
{
    private Color invalidCol = Color.red;
    private Color validCol = Color.green;

    public SpriteRenderer sprite;
    public Tilemap tilemap = null;

    public bool snapToTile = true;
    public bool pressActive;
    public bool validPlacement = true;

    public Vector3Int gridPosition;
    protected Vector3Int lastPlacedPos;

    public virtual void Start ()
    {
        gridPosition = tilemap.WorldToCell(transform.position);
        lastPlacedPos = gridPosition;
    }

    public virtual void Update ()
    {
        InputListener();

    }

    public virtual void InputListener()
    {
#if UNITY_EDITOR

        // because Input.GetMouseButtonDown only fires on the first frame.
        if (pressActive)
        {
            HandleTouch(Input.mousePosition);
        }

        if (Input.GetMouseButtonDown(0))
        {
            pressActive = true;
            HandleTouch(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("test");
            pressActive = false;
            SnapToGrid();
        }

       
       
#else
       if (Input.touchCount > 0) {
            touchDown = true;
            HandleTouch(Input.GetTouch(0).position);
       } else if (Input.touchCount <= 0) {
            touchDown = false;
            if (pressActive)
                Fire();
        }
#endif

    }
    void HandleTouch(Vector3 pos)
    {
        Vector3 screen2world = Camera.main.ScreenToWorldPoint(pos);
        transform.position = new Vector3(screen2world.x, screen2world.y, 0f);

        if (validPlacement)
            sprite.color = validCol;
        else
            sprite.color = invalidCol;

        //if (pres)
        //Vector3Int nearesTile = tilemap.WorldToCell(screen2world);
        //Vector3 neareastPos = tilemap.CellToWorld(nearesTile);
    }

    private void SnapToGrid()
    {
        Vector3Int nearesTile = tilemap.WorldToCell(transform.position);
        Vector3 neareastPos = tilemap.CellToWorld(nearesTile);
        transform.position = new Vector3(neareastPos.x, neareastPos.y, 0f);
        sprite.color = Color.white;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //if (collider.IsTouchingLayers(LayerMask.NameToLayer("Level")))
        if (other.tag == "Level")
        {
            //Debug.Log("Enter");
            validPlacement = false;
            sprite.color = invalidCol;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.IsTouchingLayers(LayerMask.NameToLayer("Level")))
        if (other.tag == "Level")
        {
            //Debug.Log("Exit");
            validPlacement = true;
            sprite.color = validCol;
        }
    }

}
