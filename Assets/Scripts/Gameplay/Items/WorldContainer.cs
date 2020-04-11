using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldContainer : Container
{
    public Animator myAnimator;

    public override void SetOpen(bool open)
    {
        base.SetOpen(open);

        myAnimator.SetBool(Container.isOpenBoolHash, isOpen);
    }
}
