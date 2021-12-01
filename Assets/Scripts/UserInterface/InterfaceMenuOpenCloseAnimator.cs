using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class InterfaceMenuOpenCloseAnimator : MonoBehaviour
{
    [Header("Animation parameters")]
    public bool isOpened;
    public float animationTime = 0.5f;
    public AnimationCurve animationCurve;

    [Header("Animate objects")]
    public GameObject box;
    public Transform[] transforms;
    public Vector2[] positionsDelta;

    private CanvasGroup canvasGroup;
    private Vector3[] startPositions;
    private Vector3[] endPositions;
    private float timer;
    private float lastRealtimeSinceStartup;

    private void Start()
    {
        if (transforms.Length != positionsDelta.Length) Debug.LogError("Transforms length and positionsDelta length not equal");

        canvasGroup = GetComponent<CanvasGroup>();
        startPositions = new Vector3[transforms.Length];
        endPositions = new Vector3[transforms.Length];
        for (int i = 0; i < transforms.Length; i++)
        {
            startPositions[i] = transforms[i].localPosition + (Vector3)positionsDelta[i];
            endPositions[i] = transforms[i].localPosition;
        }

        timer = -1f;
        if (isOpened)
        {
            SetFadeAmount(1f);
            if (box) box.SetActive(true);
        }
        else
        {
            SetFadeAmount(0f);
            if (box) box.SetActive(false);
        }

        
    }

    void Update()
    {
        float deltaTime = Time.realtimeSinceStartup - lastRealtimeSinceStartup;

        if (IsTimerInRange())
        {
            if (!isOpened)
                timer -= deltaTime;
            else
                timer += deltaTime;

            SetFadeAmount(timer / animationTime);
        }
        else if (box && !isOpened && box.activeSelf) box.SetActive(false);

        lastRealtimeSinceStartup = Time.realtimeSinceStartup;
    }

    private void SetFadeAmount(float value)
    {
        value = FadeAmountSettingObjectiveFunction(Mathf.Clamp01(value));

        canvasGroup.alpha = value;

        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].localPosition = Vector3.Lerp(startPositions[i], endPositions[i], value);
        }
    }
    private float FadeAmountSettingObjectiveFunction(float value)
    {
        return animationCurve.Evaluate(value);
    }
    private bool IsTimerInRange()
    {
        return timer >= 0 && timer <= animationTime;
    }


    public void Open()
    {
        isOpened = true;
        timer = Mathf.Clamp(timer, 0f, animationTime);
        if (box) box.SetActive(true);
        lastRealtimeSinceStartup = Time.realtimeSinceStartup;
    }
    public void Close()
    {
        isOpened = false;
        timer = Mathf.Clamp(timer, 0f, animationTime);
        lastRealtimeSinceStartup = Time.realtimeSinceStartup;
    }
    [ContextMenu("OpenOrClose")]
    public void OpenOrClose()
    {
        if (isOpened) Close();
        else Open();
    }
}
