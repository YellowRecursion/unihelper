using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public bool playEndAnimation = true;
    public float animateTime = 0.5f;
    public Canvas canvas;
    public Animator animator;

    int index;
    private void Start()
    {
        if (playEndAnimation)
        {
            canvas.enabled =true;
            Invoke("CloseEndAnimation", animateTime);
        }
    }

    void CloseEndAnimation()
    {
        canvas.enabled = false;
    }

    public void LoadLevel(int levelIndex)
    {
        canvas.enabled = true;
        animator.SetTrigger("Start");
        index = levelIndex;
        Invoke("Load", animateTime + 0.1f);
    }

    void Load()
    {
        SceneManager.LoadScene(index);
        //Application.LoadLevel(index);
    }
}
