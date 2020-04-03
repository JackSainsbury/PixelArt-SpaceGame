using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    private static int isOpenBoolHash = Animator.StringToHash("IsOpen");

    private bool isOpen = false;

    public Animator myAnimator;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetOpen(bool open)
    {
        isOpen = open;

        myAnimator.SetBool(Container.isOpenBoolHash, isOpen);
    }
}
