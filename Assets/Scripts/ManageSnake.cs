using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageSnake : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
    public int Size;
    public Mesh[] SnakeMeshs;   
    public Move Script;
    public NPC_Move NPC_script;
    public float MaxSize = 18;
    public UnityEngine.Color[] colors;
    void Awake()
    {
        if (Script != null)
            ChangeSize(6);
        else
            ChangeSize(15);
        
        //pick random color
        ChangeColor(UnityEngine.Random.Range(0, colors.Length - 1));

        // StartCoroutine(SizeAnim());

    }
    public void ChangeSize(int size)
    {
        Size = size;

        // loops through size for now
        if (Size >= MaxSize)
            Size = 5;

        // changes the mesh for the correct size
        mesh.sharedMesh = SnakeMeshs[Size - 4];


        // for every rigidbody thats not visible lower the mass 
        Transform T = transform.GetChild(0).transform;
        for (int i = 0; i < 20; i++)
        {
            T = T.GetChild(0).transform;
            if (i > Size)
            {
                T.gameObject.GetComponent<Rigidbody>().mass = .1f;
            }
            else
            {
                T.gameObject.GetComponent<Rigidbody>().mass = 1f;
            }
        }

        // checks which script it has and sets speed
        if (Script != null)
        {
            Script.speed = (((float)Size / (MaxSize * 1.333f)) + .25f) * Script.MaxSpeed;
        }
        if (NPC_script != null)
            NPC_script.speed = (((float)Size / (MaxSize * 1.333f)) + .25f) * NPC_script.MaxSpeed;
        
        ChangeTail();
    }
    IEnumerator SizeAnim()
    {
        yield return new WaitForSeconds(4f);
        while (true)
        {
            yield return new WaitForSeconds(.25f);
            Size++;
            if (Size > 17)
                Size = 5;
            ChangeSize(Size);
        }
    }
    public void ChangeColor(int col)
    {
        transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.color = colors[col];
    }
    public void ChangeTail()
    {
        // loops through the child objects until it gets the tail

        if (Script != null)
        {
            GameObject Tail = gameObject;
            for (int i = -1; i < Size; i++)
            {
                Tail = Tail.transform.GetChild(0).gameObject;
            }
            Script.Tail = Tail;
        }
        if (NPC_script != null)
        {
            GameObject Tail = gameObject;
            for (int i = -1; i < Size; i++)
            {
                Tail = Tail.transform.GetChild(0).gameObject;
            }
            NPC_script.Tail = Tail;
        }
    }
}
