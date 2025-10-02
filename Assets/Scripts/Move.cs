using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 4250;
    public float MaxSpeed = 15000;
    public Rigidbody Rig;
    public Transform Camera;
    public bool Moving = false;

    private bool SlitherDir = false;
    private float SlitherCounter = 0;
    public float SlitherSpeed = 20;
    public float SlitherIntensity = .25f;

    public float DownwardForce = -3500f;

    public ManageSnake S_Script;
    public bool SpeedRamp = false;
    public bool OnGround = true;

    public GameObject Tail;
    public float tailUpForce;

    private float TempMaxSpeed;
    void Start()
    {
        TempMaxSpeed = MaxSpeed;
    }

    void LateUpdate()
    {
        // the basic player input
        if (Input.GetKey(KeyCode.A) && !SpeedRamp)
        {
            Rig.AddForce(-Camera.transform.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && !SpeedRamp)
        {
            Rig.AddForce(Camera.transform.right * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W))
        {
            Rig.AddForce(Vector3.Scale(Camera.transform.forward * speed * Time.deltaTime, new Vector3(1, .3f, 1)));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Rig.AddForce(-Camera.transform.forward * speed * Time.deltaTime);
        }
        
        // Applying tail and downward force
        Rig.AddForce(new Vector3(0, DownwardForce, 0) * Time.deltaTime);
        Tail.GetComponent<Rigidbody>().AddForce(new Vector3(0, tailUpForce, 0) * Time.deltaTime);

        // Checking for Input and Handling diagonal cases
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))
        {
            if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W))) // If moving diagonally move slower
            {
                if (!SpeedRamp)
                    MaxSpeed = TempMaxSpeed * .48f;
            }
            else if (!SpeedRamp)
                MaxSpeed = TempMaxSpeed;

            Moving = true;
        }
        else
            Moving = false;

        // calculate the speed for changes in size and maxspeed
        speed = (((float)S_Script.Size / (S_Script.MaxSize * 1.333f)) + .25f) * MaxSpeed;

        // ground check
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ray, 5))
        {
            OnGround = true;
        }
        else
            OnGround = false;
    }
    // for ramp speed changes
    public void SetSpeed(float SpeedPercent, bool OnRamp)
    {
        SpeedRamp = OnRamp;
        MaxSpeed = TempMaxSpeed * SpeedPercent;
    }

    private void Update()
    {
        // slither animation

        if (Moving && !SpeedRamp && OnGround)
        {
            if (SlitherCounter >= SlitherSpeed / 100)
            {
                SlitherCounter = 0;
                SlitherDir = !SlitherDir;
            }
            if (SlitherDir)
            {
                Rig.AddForce(-Camera.transform.right * speed * SlitherIntensity * Time.deltaTime);
                SlitherCounter = SlitherCounter + Time.deltaTime;
                
            }
            else
            {
                Rig.AddForce(Camera.transform.right * speed * SlitherIntensity * Time.deltaTime);
                SlitherCounter = SlitherCounter + Time.deltaTime;
            }

        }
    }
}
