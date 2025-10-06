using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachSaw : MonoBehaviour
{
    public Transform snakeHead;
    public Vector3 offset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = snakeHead.position + offset;
    }
}
