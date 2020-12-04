using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;

public class YarnAnimator : MonoBehaviour
{
    public Animator animationController;

    //Command to allow yarn to trigger a certain animation state
    [YarnCommand("playAnimatorState")]
    public void playAnimatorState(string animatorState)
    {
        if (animatorState != null)
        {
            animationController.CrossFade(animatorState,0.5f);
        }
    }
   
}
