using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectApple : MonoBehaviour
{
    public Transform Apple;
    public float AnimMoveSpeed = .3f;
    public float AnimScaleSpeed = 1f;
    public float Scale = 40f;
    public bool Started = true;
    public void Start()
    {
        Started = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "SnakeHead" && Started)
        {
            Started = false;
            StartCoroutine(AppleAnim(other.gameObject));
        }
    }
    
    IEnumerator AppleAnim(GameObject SnakeHead)
    {
        GetComponent<SphereCollider>().enabled = false;
        Apple.GetComponent<Animator>().enabled = false;

        while (true)
        {
            Apple.position = Vector3.MoveTowards(Apple.position, SnakeHead.transform.position, AnimMoveSpeed);

            Scale = Scale - AnimScaleSpeed;
            Apple.localScale = new Vector3(Scale, Scale, Scale);
            if (Scale < .1f)
            {
                ManageSnake MS = SnakeHead.transform.parent.parent.GetComponent<ManageSnake>();
                MS.ChangeSize(MS.Size + 1);

                // Big Apple 
                if (Apple.name == "BigApple")
                {
                    SnakeHead.transform.localScale = new Vector3(3, 3, 3);
                }
                 
                Destroy(Apple.gameObject);
                break;
            }
            yield return new WaitForSeconds(0.008f);
        }
    }
}
