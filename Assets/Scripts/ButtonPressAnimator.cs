using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class ButtonPressAnimator : MonoBehaviour
{
    public bool interactible = true;
    public const float speed = 12f;
    public float size = 0.9f;
    public UnityEvent onClick;

    private RectTransform rectTransform;
    private Vector3 target;


    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        target = Vector3.one;

        EventTrigger.Entry pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener(delegate { OnDown(); });
        GetComponent<EventTrigger>().triggers.Add(pointerDown);

        EventTrigger.Entry pinterUp = new EventTrigger.Entry();
        pinterUp.eventID = EventTriggerType.PointerUp;
        pinterUp.callback.AddListener(delegate { OnUp(); });
        GetComponent<EventTrigger>().triggers.Add(pinterUp);
    }

    void Update()
    {
        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, target, speed * Time.deltaTime);
    }

    public void OnDown()
    {
        target = new Vector3(size, size, size);
    }

    public void OnUp()
    {
        target = Vector3.one;
        if (interactible) onClick.Invoke();
    }
}
