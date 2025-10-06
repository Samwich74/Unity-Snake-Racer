using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MagneticPull : MonoBehaviour
{
    public List<Rigidbody> List = new List<Rigidbody>();
    public float PullForce = 50f;
    public float SelfForce = 25f;
    public void Start()
    {
        StartCoroutine(Delete());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.name == "SnakeHead")
        {
            other.GetComponent<NPC_Move>().beingPulled = true;
            List.Add(other.gameObject.GetComponent<Rigidbody>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player" && other.name == "SnakeHead")
        {
            other.GetComponent<NPC_Move>().beingPulled = false;
            List.Remove(other.gameObject.GetComponent<Rigidbody>());
        }
    }
    public void FixedUpdate()
    {
        if (List.Count > 0 && List[0] != null)
        {
            GetComponent<Rigidbody>().AddForce((List[0].gameObject.transform.position - transform.position) * PullForce);
        }
        foreach (Rigidbody R in List)
        {
            R.AddForce((transform.position - R.gameObject.transform.position) * PullForce);
            R.gameObject.GetComponent<NPC_Move>().Tail.GetComponent<Rigidbody>().AddForce((transform.position - R.gameObject.transform.position) * PullForce);
        }
    }

    public IEnumerator Delete()
    {

        yield return new WaitForSeconds(10f);
        foreach (Rigidbody R in List)
        {
            R.gameObject.GetComponent<NPC_Move>().beingPulled = false;
        }
        Destroy(gameObject);

    }
}
