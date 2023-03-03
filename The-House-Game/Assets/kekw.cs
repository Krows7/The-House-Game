using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kekw : MonoBehaviour
{

    Animator animator;
    InputController movement;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        movement = GameObject.Find("MovementController").GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetInteger("AnimState", 1);
        }
    }
}
