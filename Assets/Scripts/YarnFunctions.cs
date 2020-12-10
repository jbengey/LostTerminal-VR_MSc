using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using System;
using UnityEngine.SceneManagement;

public class YarnFunctions : MonoBehaviour
{
    public Animator animationController;
    public AudioSource[] dialogAudio;

    //Command to allow yarn to trigger a certain animation state
    [YarnCommand("playAnimatorState")]
    public void PlayAnimatorState(string animatorState)
    {
        if (animatorState != null)
        {
            animationController.CrossFadeInFixedTime(animatorState,0.3f);

        }
    }

    //Command to allow yarn to trigger a certain sound for dialog
    [YarnCommand("playCharacterDialog")]
    public void PlayCharacterDialog(string dialogLineRef)
    {
        foreach (var audioLine in dialogAudio) //Loop through provided audio sources until a match is found with the line yarn requested
        {
            if (audioLine.name == dialogLineRef)
            {
                audioLine.Play();
            }
        }

    }


    //Command to allow yarn to end the game
    [YarnCommand("gameOver")]
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");

    }


}
