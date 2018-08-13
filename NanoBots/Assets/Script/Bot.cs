using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float speed = 1.0f;
    public float damp = 0f;

    Vector3 vectorToMouse;
    bool mouseBtnDown = false;

    Rigidbody2D rigidBody2D;
    Vector3 moveDirNormalized;

    // Use this for initialization
    void Start ()
    {
        rigidBody2D = gameObject.GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 mousePosWorldSpace = new Vector3();
        Camera mainCamera = Camera.main;

        if (Input.GetMouseButtonDown(0))
            mouseBtnDown = true;
        if (Input.GetMouseButtonUp(0))
            mouseBtnDown = false;

        if (mouseBtnDown)
        {
            mousePosWorldSpace = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
            MoveTo(mousePosWorldSpace);
        }
        else
        {
            Debug.Log(string.Format("vel.Mag: {0}", rigidBody2D.velocity.magnitude));

            if (rigidBody2D.velocity.magnitude < 0.1f)
            {
                rigidBody2D.velocity = Vector3.zero;
                return;
            }

            Vector3 stopVector = Negate(rigidBody2D.velocity);
            rigidBody2D.AddForce(stopVector.normalized * damp);
        }

        //Debug.Log(string.Format("Move Dir: {0}", moveDirNormalized));
        //Debug.Log(string.Format("Mouse World Space Pos: {0}", mousePosWorldSpace));


    }

    void MoveTo(Vector3 finPos)
    {

        Vector3 initPos = transform.position;
        Vector3 dirVector = finPos - initPos;

        moveDirNormalized = dirVector.normalized;
        rigidBody2D.AddForce(moveDirNormalized * speed);
        //rigidBody2D.AddTorque(0.5f);

        // Pointer angle calculation.
        float _o, _a = 0;
        // could use finPos instead of pullV here. They are essentially create the same triangle, insted finPos extents ends within radius
        _o = dirVector.x;
        _a = dirVector.y;

        // Conversion from radians to degrees.
        float angle = Mathf.Rad2Deg * Mathf.Atan(_o / _a);

        // Calculate a full 360deg angle
        // compensates for math sometimes negating and flipping angles depending on position. 
        if (_a < 0) angle += 180;
        if (angle < 0) angle += 360;

        float fireAngle = angle - 180;
        if (fireAngle < 0) fireAngle += 360;

        transform.localRotation = Quaternion.AngleAxis(fireAngle + 90, Vector3.back);

        //Debug.Log(string.Format("V = ({0}, {1})    Ang = {2}deg", pullV.x, pullV.y, pullAngle));

        //Debug.Log(string.Format(
        //        //"finPos: {6}\n" +
        //        //"intiPos: {7}\n" +
        //        //"PullV: {8}\n" +
        //        //"Rad2Deg: {0}\n" +
        //        //"opp: {1}\n" +
        //        //"adj: {2}\n" +
        //        //"tan({1}/{2}) = {3}\n" +
        //        //"Atan({1}/{2}) = {4}\n" +
        //        "pullAngle: {5}\n" +
        //        "fireAngle: {10}\n" +
        //        "touchPos: {9}\n",
        //        Mathf.Rad2Deg, _o, _a, Mathf.Tan(_o / _a), Mathf.Atan(_o / _a), angle, finPos, initPos, dirVector, finPos, fireAngle));
    }

    Vector3 Negate(Vector3 vec)
    {
        return vec * -1;
    }
}
