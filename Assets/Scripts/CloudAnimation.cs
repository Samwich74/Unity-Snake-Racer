using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using VertexAnimationTools_30;
using static UnityEngine.Rendering.DebugUI.Table;

public class CloudAnimation : MonoBehaviour
{
    public GameObject Particle;
    public float growthSpeed = 1;
    public float lifeSpan = 10f;
    public float radius;
    public float angleIncrement;
    public float currentAngle;
    public Vector2 randRotRange;
    public GameObject parentObject;


    public bool isDebugging;

    private List<Vector3> gizmoPoints = new List<Vector3>();
    public enum radiusType
    {
        normal,
        star,
        pulse,
        shuriken,
        vertical,
        spiral,
        all
    }
    public radiusType radEnum;

    public float radEffectStrength;
    public float radAngleEffect = 45f;
    void Start()
    {
        StartCoroutine(CloudAnim());
    }

    IEnumerator CloudAnim()
    {
        while (true)
        {
            Vector3 rot = new Vector3(-90, 0, currentAngle);

            GameObject cloudPart = Instantiate(Particle);

            float[] output = applyRadiusEffect(radEnum);

            float rad = output[0];
            float heightEffect = output[1];

            if (radEnum == radiusType.vertical)
            {
                rot = Vector3.zero;
            }

            Vector3 origDir = Vector3.forward;
            currentAngle += angleIncrement;
            Vector3 rotDir = Quaternion.AngleAxis(currentAngle, Vector3.up) * origDir;

            Vector3 pos = rotDir.normalized * rad + transform.position + new Vector3(0, heightEffect, 0);
            cloudPart.transform.position = transform.rotation * pos;

            if (isDebugging)
                gizmoPoints.Add(transform.rotation * pos);


            cloudPart.transform.rotation = Quaternion.Euler(rot);
            rot = Quaternion.AngleAxis(Random.Range(randRotRange.x, randRotRange.y), cloudPart.transform.right) * rot;
            cloudPart.transform.rotation = Quaternion.Euler(rot) * transform.rotation;
            cloudPart.transform.localScale = transform.localScale;
            cloudPart.transform.parent = parentObject.transform;
            cloudPart.GetComponent<MeshSequencePlayer>().PlaybackMode = AutoPlaybackTypeEnum.Repeat;
            cloudPart.GetComponent<SelfDestroy>().waitTime = lifeSpan;

            if (isDebugging)
                cloudPart.GetComponent<MeshRenderer>().enabled = false;

            yield return new WaitForSeconds(growthSpeed / 100f);
        }
    }
    public float[] applyRadiusEffect(radiusType RT)
    {
        float heightEffect = 0;
        float rad = radius;
        switch (RT)
        {
            case radiusType.star:
                float remainder = currentAngle % radAngleEffect;
                remainder = Mathf.Min(remainder, radAngleEffect - remainder);
                rad = rad + (remainder * radEffectStrength);
                break;
            case radiusType.shuriken:
                remainder = currentAngle % radAngleEffect;
                remainder = Mathf.Min(remainder, radAngleEffect - remainder) / (radAngleEffect / 2);

                float offset = Mathf.Sin(currentAngle / (radAngleEffect / 3)) * radEffectStrength * 6;
                rad = rad + Mathf.Lerp(0, offset * 3, remainder);
                break;
            case radiusType.pulse:
                float tempAngle = radAngleEffect * 5f;
                remainder = currentAngle % tempAngle;
                remainder = Mathf.Min(remainder, tempAngle - remainder) / (tempAngle / 2);

                offset = Mathf.Sin(currentAngle / (radAngleEffect / 3)) * radEffectStrength * 6;
                offset = offset + (Mathf.Lerp(0, rad / 2, remainder));
                rad = rad + 2 * Mathf.Lerp(-offset / 2, offset, remainder);
                break;
            case radiusType.vertical:
                heightEffect = Mathf.Sin(currentAngle / (radAngleEffect / 3)) * radEffectStrength * 15;
                break;
            case radiusType.spiral:
                remainder = currentAngle % 720 / 720;
                rad = Mathf.Lerp(rad, 0, remainder);
                break;
            case radiusType.all:
                float[] output1 = applyRadiusEffect(radiusType.pulse);
                float[] output2 = applyRadiusEffect(radiusType.shuriken);
                float[] output3 = applyRadiusEffect(radiusType.spiral);
                float[] output4 = applyRadiusEffect(radiusType.star);
                rad = (output1[0] + output2[0] + output3[0] + output4[0]) * .3f;

                float[] output5 = applyRadiusEffect(radiusType.vertical);
                heightEffect = output5[1] * .3f;
                break;
        }
        return new float[] { rad, heightEffect };
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomizeEffect();
        }
    }
    public void RandomizeEffect()
    {
        radEnum = (radiusType)Random.Range(2, 7);
        lifeSpan = Random.Range(.5f, 5);
        radius = Random.Range(10, 40);
        angleIncrement = Random.Range(1, 10);
        radEffectStrength = Random.Range(.5f, 2f);
        radAngleEffect = Random.Range(10, 100);

        float rand = Random.Range(.5f, 3f);
        transform.localScale = new Vector3(rand, rand, rand);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (isDebugging)
        {
            foreach (Vector3 point in gizmoPoints)
            {
                Gizmos.DrawSphere(point, 1f);
            }
        }
    }
}
