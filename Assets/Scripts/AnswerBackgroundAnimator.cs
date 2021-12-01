using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerBackgroundAnimator : MonoBehaviour
{
    private Color color;
    public float speed = 6f;

    private Color empty;
    private Image image;
    private bool animate;
    private float timer;

    private void Start()
    {
        image = GetComponent<Image>();
        empty = new Color(color.r, color.g, color.b, 0f);
        End();
    }

    private void Update()
    {
        if (animate)
        {
            timer += Time.deltaTime * speed;
            image.color = Color.Lerp(empty, color, (Mathf.Sin(timer) + 1f) / 6f);
        }
    }

    public void Begin(Color color)
    {
        animate = true;
        empty = new Color(color.r, color.g, color.b, 0f);
        this.color = color;
        timer = 0f;
    }

    public void End()
    {
        animate = false;
        image.color = empty;
    }
}
