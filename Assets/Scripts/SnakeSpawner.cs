using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : MonoBehaviour
{
    public GameObject Prefab;
    public NPC_Move NPC_Referance;
    public float secondsPerSpawn;
    void Start()
    {
        StartCoroutine(Spawn());   
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsPerSpawn);
            GameObject S = Instantiate(Prefab);
            S.transform.GetChild(0).GetChild(0).GetComponent<NPC_Move>().Paths = NPC_Referance.Paths;
        }
    }
}
