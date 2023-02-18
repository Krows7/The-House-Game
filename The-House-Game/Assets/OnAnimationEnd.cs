using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnAnimationEnd : MonoBehaviour
{
    public void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }
}
