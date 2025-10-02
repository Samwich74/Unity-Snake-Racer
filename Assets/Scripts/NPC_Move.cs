using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Move : MonoBehaviour
{
    public float speed = 4250;
    public float MaxSpeed = 15000;
    public Rigidbody Rig;

    private bool SlitherDir = false;
    private float SlitherCounter = 0;
    public float SlitherSpeed = 20;
    public float SlitherIntensity = .25f;

    public float DownwardForce = 10f;

    public ManageSnake S_Script;
    public bool SpeedRamp = false;
    public bool OnGround = true;
    private float TempMaxSpeed;


    public GameObject Tail;
    public float tailUpForce;

    public GameObject CurrentPath;
    public GameObject[] Paths;
    public int pathNumber = 0;
    private int path_id;

    void Start()
    {
        CurrentPath = Paths[0];
        TempMaxSpeed = MaxSpeed;
    }

    // for ramp speed changes
    public void SetSpeed(float SpeedPercent, bool OnRamp)
    {
        SpeedRamp = OnRamp;
        MaxSpeed = TempMaxSpeed * SpeedPercent;
    }

    private void Update()
    {
        // calculate the speed for changes in size and maxspeed
        speed = (((float)S_Script.Size / (S_Script.MaxSize * 1.333f)) + .25f) * MaxSpeed;

        // Move the snake towards its current path target
        Rig.AddForce(Vector3.Scale((CurrentPath.transform.position - transform.position).normalized * speed * Time.deltaTime, new Vector3(1, .3f, 1)));
        Rig.AddForce(new Vector3(0, DownwardForce * Time.deltaTime, 0));

        // check if grounded
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ray, 5))
        {
            OnGround = true;
        }
        else
        {
            Tail.GetComponent<Rigidbody>().AddForce(new Vector3(0, tailUpForce * Time.deltaTime, 0));
            OnGround = false;
        }

        // Slither animation
        if (!SpeedRamp && OnGround)
        {
            // gets the forward vector from subtracting the positions
            // then uses Vector.Cross to get the right vector
            Vector3 Direction = Vector3.Cross(Vector3.up, (CurrentPath.transform.position - transform.position).normalized);

            if (SlitherCounter >= SlitherSpeed / 100)
            {
                SlitherCounter = 0;
                SlitherDir = !SlitherDir;
            }
            if (SlitherDir)
            {
                Rig.AddForce(Direction * speed * SlitherIntensity * Time.deltaTime);
                SlitherCounter = SlitherCounter + Time.deltaTime;

            }
            else
            {
                Rig.AddForce(-Direction * speed * SlitherIntensity * Time.deltaTime);
                SlitherCounter = SlitherCounter + Time.deltaTime;
            }

        }
    }

 
    public void ChangePath(int increase, float timeout, GameObject collison)
    {
        // detects if the NPC already collided with this path 
        if (pathNumber != 0)
        {
            if (collison == Paths[pathNumber - 1])
                return;
        }
        else if (collison == Paths[Paths.Length - 1])
        {
            return;
        }


        pathNumber = pathNumber + increase;

        // loops the path
        if (pathNumber + 1 > Paths.Length)
            pathNumber = 0;

        CurrentPath = Paths[pathNumber];

        // id for the backup timeout mechanism
        path_id++;
        if (increase > 0)
        {
            StartCoroutine(PathTimeOut(timeout, path_id));
        }
    }


    // backup mechanism incase NPC gets stuck
    public IEnumerator PathTimeOut(float timeout, int id)
    {
        // checks every .1 secs if the snake moved on to the next path
        for (int i = 0; i <= timeout * 10; i++)
        {
            yield return new WaitForSeconds(.1f);

            if (id != path_id)
            {
                yield break;
            }
        }
        // moves back to the previous path
        ChangePath(-1, 0, gameObject);
        yield break;
    }
}
