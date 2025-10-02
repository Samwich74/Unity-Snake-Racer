using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Path : MonoBehaviour
{
    public float PathTimeLength;
    public Vector3 origPos;
    public void Start()
    {
        origPos = transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            other.GetComponent<NPC_Move>().ChangePath(1, PathTimeLength, gameObject);
        }
    }
}
