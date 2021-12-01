using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceElementAnimator : MonoBehaviour
{
    [Header("Rotation")]
    public float rotateSpeed = 0f;

    [Header("Scaling")]
    public float scaleSpeed = 0f;
    public float scaleDelta = 0.1f;

    private float scalingTimer;

    void FixedUpdate()
    {
        if (rotateSpeed != 0f)
        {
            transform.eulerAngles += Vector3.forward * rotateSpeed * Time.fixedDeltaTime;
        }

        if (scaleSpeed != 0f)
        {
            scalingTimer += Time.fixedDeltaTime;
            transform.localScale = Vector3.one + Vector3.one * scaleDelta * (Mathf.Sin(scalingTimer * scaleSpeed) + 1f) * 0.5f;
        }
    }
}
