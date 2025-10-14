using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ManageTornado : MonoBehaviour
{
    public Dictionary<GameObject, float> objectsInTornado = new Dictionary<GameObject, float>();
    public float TornadoForce;
    public float radius = 5;
    public float heightIncrease = .1f;

    public bool Testing = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            objectsInTornado.Add(other.gameObject, 0);
            if (other.GetComponent<NPC_Move>() != null)
                other.GetComponent<NPC_Move>().beingPulled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "NPC")
        {
            objectsInTornado.Remove(other.gameObject);
            if (other.GetComponent<NPC_Move>() != null)
                other.GetComponent<NPC_Move>().beingPulled = false;
        }
    }
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        for (int i = 0; i < objectsInTornado.Count; i++)
        {
            
            GameObject gameobject = objectsInTornado.Keys.ElementAt(i);
            objectsInTornado[gameobject] += heightIncrease;
            float height = objectsInTornado[gameobject];

            float localRadius;
            if (Testing)
                localRadius = radius + Mathf.Sqrt(height);
            else
                localRadius = radius + height;


            float angleIncrement;
            if (Testing)
                angleIncrement = localRadius * 2f;
            else
                angleIncrement = localRadius * 5f;


            angleIncrement = Mathf.Clamp(angleIncrement, 0, 45f);

            Vector3 origDir = gameobject.transform.position - transform.position;
            Vector3 rotatedDir = Quaternion.AngleAxis(angleIncrement, Vector3.up) * origDir;
            Vector3 targetPoint = rotatedDir.normalized * localRadius + transform.position + new Vector3(0, height, 0);
            Vector3 targetDir = targetPoint - gameobject.transform.position;
            gameobject.GetComponent<Rigidbody>().AddForce(TornadoForce * targetDir);
            if (gameobject.GetComponent<NPC_Move>() !=  null)
                gameobject.GetComponent<NPC_Move>().Tail.GetComponent<Rigidbody>().AddForce(TornadoForce * targetDir);

            float torqueForce;
            if (Testing)
                torqueForce = TornadoForce / 4;
            else
                torqueForce = TornadoForce;

            gameobject.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(0, torqueForce), Random.Range(0, torqueForce), Random.Range(0, torqueForce)));
        }


    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
