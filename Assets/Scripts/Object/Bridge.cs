using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, Openable
{
    Animator animator;

    public void Close()
    {
        animator.SetBool("isOpen", false);
    }

    public void Open()
    {
        animator.SetBool("isOpen", true);
    }

    private void Start()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }
}
