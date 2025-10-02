using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRamp : MonoBehaviour
{
    public float SpeedBoost = 3f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Move>() != null)
        {
            collision.gameObject.GetComponent<Move>().SetSpeed(SpeedBoost, true);
        }
        else if (collision.gameObject.GetComponent<NPC_Move>() != null)
            collision.gameObject.GetComponent<NPC_Move>().SetSpeed(SpeedBoost, true);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Move>() != null)
        {
            collision.gameObject.GetComponent<Move>().SetSpeed(1, false);
        }
        else if (collision.gameObject.GetComponent<NPC_Move>() != null)
            collision.gameObject.GetComponent<NPC_Move>().SetSpeed(1, false);
    }
}
