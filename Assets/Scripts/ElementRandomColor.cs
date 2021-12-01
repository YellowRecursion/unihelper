using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ElementRandomColor : MonoBehaviour
{
    public Color color = Color.green;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        float h = 0f, s = 0f, v = 0f;
        Color.RGBToHSV(color, out h, out s, out v);
        h = Random.value;
        image.color = Color.HSVToRGB(h, s, v);
        Destroy(this);
    }
}
