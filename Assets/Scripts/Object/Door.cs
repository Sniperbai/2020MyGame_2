using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour,Openable
{
    Animator animator;
    public bool isOpen;

    private void Start()
    {
        animator = transform.GetComponent<Animator>();

        if (isOpen)
        {
            Open();
        }
    }

    //开门
    public void Open()
    {
        animator.SetBool("isOpen", true);
    }

    //关门
    public void Close()
    {
        animator.SetBool("isOpen", false);
    }
}
