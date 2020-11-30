using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class BoardingPassLogic : MonoBehaviour
{
    public GameObject boardingPass;
    public DialogueRunner dialogueRunner;

    //Command to check if player has boarding pass, returns bool
    [YarnCommand("hasBoardingPass")]
    public void hasBoardingPass()
    {
        if(boardingPass.activeInHierarchy is true)
        {
            dialogueRunner.variableStorage.SetValue("$RetVal", true);
            return ; //player has boarding pass
        }
        else
        {
            dialogueRunner.variableStorage.SetValue("$RetVal", false);
            return ; //player does not have boarding pass
        }
    }




}
