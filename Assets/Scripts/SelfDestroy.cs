using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float waitTime;
    void Start()
    {
        StartCoroutine(SelfDelete());
    }

    public IEnumerator SelfDelete()
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
