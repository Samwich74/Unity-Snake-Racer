using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CollectMagnet : MonoBehaviour
{
    public Transform Magnet;
    public Transform Camera;
    public float AnimMoveSpeed = .3f;
    public float AnimRotSpeed = 1f;
    public bool Started = false;

    public float ThrowOffset;
    public float ThrowForce;
    public float ThrowRotForce;

    public Vector3 Offset;
    public void Start()
    {
        Started = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !Started)
        {
            Started = true;
            StartCoroutine(MagnetAnim(other.gameObject));
        }
    }



    IEnumerator MagnetAnim(GameObject SnakeHead)
    {
        GetComponent<SphereCollider>().enabled = false;
        Magnet.GetComponent<Animator>().enabled = false;

        while (true && Started)
        {
            Magnet.position = Vector3.MoveTowards(Magnet.position, SnakeHead.transform.position + new Vector3(0, 1.2f, 0), AnimMoveSpeed);
             
            Quaternion TargetDir = Quaternion.LookRotation(new Vector3(-Camera.transform.forward.x, 0, -Camera.transform.forward.z));
            Quaternion offset = Quaternion.Euler(Offset);

            Magnet.transform.rotation = Quaternion.RotateTowards(Magnet.rotation, TargetDir * offset, AnimRotSpeed);

            // ManageSnake MS = SnakeHead.transform.parent.parent.GetComponent<ManageSnake>();
                


            // Destroy(Magnet.gameObject);
            
               
            
            yield return new WaitForSeconds(0.008f);
        }
        yield break;
    }

    public void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Started)
        {
            Magnet.AddComponent<SphereCollider>();
            Magnet.GetComponent<SphereCollider>().radius = 10f;
            Magnet.GetComponent<SphereCollider>().isTrigger = true;
            Magnet.AddComponent<SphereCollider>();
            Magnet.AddComponent<MagneticPull>();
            Magnet.AddComponent<Rigidbody>();
            Magnet.GetComponent<Rigidbody>().AddForce((Camera.transform.forward + new Vector3(0, ThrowOffset, 0)) * ThrowForce);
            Magnet.GetComponent<Rigidbody>().AddTorque(Camera.transform.forward * ThrowRotForce);
            Started = false;
        }

    }
}
