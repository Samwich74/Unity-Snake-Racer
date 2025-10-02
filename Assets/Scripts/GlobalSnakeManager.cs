using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalSnakeManager : MonoBehaviour
{
    // Updates information to all Snakes

    public bool UseGravity = true;
    public float tailForce;
    public float downForce;

    public GameObject Player;

    public GameObject ReferenceSnake;
    public GameObject[] paths;
    public Vector3 pathOffset;
    void Start()
    {
        paths = ReferenceSnake.GetComponent<NPC_Move>().Paths;
        StartCoroutine(ChangeBehaviour());
    }
    public IEnumerator ChangeBehaviour()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            foreach (GameObject G in paths)
            {
                G.transform.position = G.GetComponent<NPC_Path>().origPos + pathOffset;
            }
            foreach (GameObject G in GameObject.FindGameObjectsWithTag("NPC"))
            {
                G.GetComponent<Rigidbody>().useGravity = UseGravity;
                G.GetComponent<NPC_Move>().tailUpForce = tailForce;
                G.GetComponent<NPC_Move>().DownwardForce = downForce;
            }
            
        }
    }
}
