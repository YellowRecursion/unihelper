using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ExpressionDrawer : MonoBehaviour
{
    //[SerializeField] private string formula=@"\displaystyle\int_{-\infty}^{\infty}e^{-x^{2}}\;dx=\sqrt{\pi}";
    
    private Texture2D _texture = null;

    /*public IEnumerator Draw(string formula) 
    {
        WWW www=new WWW("http://chart.apis.google.com/chart?cht=tx&chl=" + formula);
        yield return www;
        Texture2D temp = www.texture;
        _texture = new Texture2D(temp.width, temp.height);
        for (int x = 0; x < temp.width; x++)
        {
            for (int y = 0; y < temp.height; y++)
            {
                float r = (1f - temp.GetPixel(x,y).r);
                //texture.SetPixel(x,y, new Color(r * 40f,r * 40f,r * 40f,r * 2f));
                _texture.SetPixel(x,y, new Color(1f,1f,1f,r*1.6f));
            }
        }
        _texture.Apply();
        ApplyTexture(_texture);
    }*/

    /*private void ApplyTexture(Texture2D texture)
    {
        if (texture is null)
        {
            Debug.LogError("Texture is null");
            return;
        } 

        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100, 0, SpriteMeshType.FullRect, Vector4.one, false);

        Image image = GetComponent<Image>();
        image.sprite = sprite;
        image.SetNativeSize();
        image.rectTransform.sizeDelta *= 1.1f;
    }*/

    private void ApplySprite(Sprite sprite)
    {
        Image image = GetComponent<Image>();
        image.sprite = sprite;
        image.SetNativeSize();
        image.rectTransform.sizeDelta *= 1.1f;
    }

    public void TryGetFormulaImageFromFile(string formula)
    {
        /*var path = Application.dataPath + "/ExpressionImages/";
        Texture2D texture = new Texture2D(2,2);
        if (File.Exists(path + Encryption.SimpleEncrypt(formula) + ".png"))
        {
           
            texture.LoadImage(File.ReadAllBytes(path + Encryption.SimpleEncrypt(formula) + ".png"), false);
        }
        */
        var sprite = Resources.Load<Sprite>($"ExpressionImages/{Encryption.SimpleEncrypt(formula)}");

        if (sprite is null) return;

        ApplySprite(sprite);
    }

    /*public IEnumerator Draw(string formula)
    {
        UnityWebRequest unityWebRequest = new UnityWebRequest("http://chart.apis.google.com/chart?cht=tx&chl=" + formula);
        yield return unityWebRequest.SendWebRequest();
        Texture2D temp = DownloadHandlerTexture.GetContent(unityWebRequest);
        texture = new Texture2D(temp.width, temp.height);
        for (int x = 0; x < temp.width; x++)
        {
            for (int y = 0; y < temp.height; y++)
            {
                float r = (1f - temp.GetPixel(x, y).r);
                //texture.SetPixel(x,y, new Color(r * 40f,r * 40f,r * 40f,r * 2f));
                texture.SetPixel(x, y, new Color(1f, 1f, 1f, r * 1.6f));
            }
        }
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 100, 0, SpriteMeshType.FullRect, Vector4.one, false);

        Image image = GetComponent<Image>();
        image.sprite = sprite;
        image.SetNativeSize();
        image.rectTransform.sizeDelta *= 1.1f;
    }*/
}
