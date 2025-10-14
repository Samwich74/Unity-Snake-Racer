using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject Prefab;
    public float secondsPerSpawn;
    public bool isEnabled = true;
    void Start()
    {
        StartCoroutine(Spawn());
    }

    public IEnumerator Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(secondsPerSpawn);
            if (isEnabled)
                Instantiate(Prefab, transform.position, transform.rotation);
        }
    }
}
