using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
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
    private List<Vector3> gizmoPoints = new List<Vector3>();
    private int leapState;
    public bool beingPulled;

    public float leapForce;
    public float leapHeightOffset;
    public float leapForwardOffset;

    private float CartwheelForce = 150f;
    public GameObject particleEffect1;

    void Start()
    {
        TempMaxSpeed = MaxSpeed;
    }

    void LateUpdate()
    {
        // the basic player input
        if (Input.GetKey(KeyCode.A) && !SpeedRamp && !beingPulled)
        {
            Rig.AddForce(-Camera.transform.right * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && !SpeedRamp && !beingPulled)
        {
            Rig.AddForce(Camera.transform.right * speed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.W) && !beingPulled)
        {
            Rig.AddForce(Vector3.Scale(Camera.transform.forward * speed * Time.deltaTime, new Vector3(1, .3f, 1)));
        }
        else if (Input.GetKey(KeyCode.S) && !beingPulled)
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
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit ray, 2))
        {
            if (ray.collider.gameObject.tag != "Player")
            {
                OnGround = true;
            }
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
        if (leapState == 0 && Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            if (S_Script.Size > 5)
            {
                leapState = 1;
                StartCoroutine(Leap(S_Script.Size));
            }        }
    }

    IEnumerator Leap(int size)
    {
        S_Script.ChangeSize(size-1);
        float sizeFactor = size / 18 + .5f;
        float forwardOffset = sizeFactor * leapForwardOffset * (size / 10 + 1);

        leapState = 2;
        beingPulled = true;

        Vector3 topPos = transform.position + new Vector3(0, leapHeightOffset * 2 * sizeFactor, 0);
        Vector3 midPos = transform.position + new Vector3(0, leapHeightOffset * sizeFactor, 0);
        Vector3 axis = Camera.right;
        Vector3 origDir = Tail.transform.position - midPos;



        int angleIncrement = 3;

        // inital swing
        while (angleIncrement < 150)
        {
            if (angleIncrement % 18 == 0)
                Instantiate(particleEffect1, Tail.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));

            angleIncrement += 3;

            Vector3 rotatedDir = Quaternion.AngleAxis(angleIncrement, axis) * origDir;
            Vector3 targetPos = rotatedDir.normalized * leapHeightOffset + midPos;
            Vector3 targetDir = targetPos - Tail.transform.position;

            gizmoPoints.Add(targetPos);

            Tail.GetComponent<Rigidbody>().AddForce(targetDir * leapForce);
            Tail.GetComponent<Rigidbody>().AddForce(
                new Vector3(Camera.transform.forward.x * forwardOffset, forwardOffset / 6, Camera.transform.forward.z * forwardOffset));

            Debug.DrawRay(targetPos, new Vector3(Camera.transform.forward.x, 0, Camera.transform.forward.z), Color.green, 10);

            yield return new WaitForSeconds(.01f);
        }

        float counter = 0;

        // upward propulsion 
        while (counter < size * 5)
        {
            if (counter % 6 == 0)
                Instantiate(particleEffect1, Tail.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            counter++;

            Tail.GetComponent<Rigidbody>().AddForce(Vector3.up * leapForce);
            Tail.GetComponent<Rigidbody>().AddForce(
                new Vector3(Camera.transform.forward.x * forwardOffset, forwardOffset / 6, Camera.transform.forward.z * forwardOffset));

            yield return new WaitForSeconds(.01f);
        }

        counter = 0;

        // forward propulsion 
        while (counter < 100)
        {
            if (counter % 20 == 0)
            {
                GameObject Particle = Instantiate(particleEffect1, Tail.transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
                Particle.transform.localScale *= .5f;
            }

            counter++;
            Tail.GetComponent<Rigidbody>().AddForce(
                new Vector3(Camera.transform.forward.x * forwardOffset, forwardOffset / 6, Camera.transform.forward.z * forwardOffset));
            yield return new WaitForSeconds(.01f);
        }

        counter = 0;

        // cartwheel mid air
        while (!OnGround)
        {
            counter += .02f;
            if (counter == 1)
                counter = 0;

            Rig.AddForce(Vector3.Lerp(-Camera.up, Camera.up, counter) * CartwheelForce);
            Rig.AddForce(Vector3.Lerp(-Camera.forward, Camera.forward, counter) * CartwheelForce);

            Tail.GetComponent<Rigidbody>().AddForce(Vector3.Lerp(Camera.up, -Camera.up, counter) * CartwheelForce);
            Tail.GetComponent<Rigidbody>().AddForce(Vector3.Lerp(Camera.forward, -Camera.forward, counter) * CartwheelForce);



           yield return new WaitForSeconds(.01f);
        }

        leapState = 0;
        beingPulled = false;
        yield break;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        foreach (Vector3 point in gizmoPoints)
        {
            Gizmos.DrawSphere(point, .15f);
        }
    }
}
