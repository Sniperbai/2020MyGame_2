using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderSetting : MonoBehaviour
{

    MeshRenderer meshRenderer;
    public int orderInLayer;

    public void Start()
    {
        meshRenderer = transform.GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = orderInLayer;
    }
}

