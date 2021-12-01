using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOpener : MonoBehaviour
{
    public Animator animator;
    public ButtonPressAnimator button;

    private bool opened;

    public void OnClick()
    {
        button.interactible = false;
        Invoke("EnableButton", 0.5f);
        opened = !opened;
        animator.SetBool("Open", opened);
        animator.SetBool("Close", !opened);
    }

    private void EnableButton()
    {
        button.interactible = true;
    }
}
