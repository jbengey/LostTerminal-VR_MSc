using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class DialogController_Test : MonoBehaviour
{
    public string characterName = "";
    public string talkToNode = "";
    public YarnProgram scriptToLoad;
    public DialogueRunner dialogueRunner; //refernce to the dialogue control
    public GameObject dialogueCanavas; //refernce to the canvas
    Vector3 PostionSpeachBubble = new Vector3(0f, 0.0f, -0.6f);
    public GameObject player;


    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        if (scriptToLoad == null) Debug.LogError("NPC3D not set up with yarn scriptToLoad ", this);
        if (string.IsNullOrEmpty(characterName)) Debug.LogWarning("NPC3D not set up with characterName", this);
        if (string.IsNullOrEmpty(talkToNode)) Debug.LogError("NPC3D not set up with talkToNode", this);

        if (dialogueRunner == null) Debug.LogError("dialogueRunner not set up", this);
        if (dialogueCanavas == null) Debug.LogError("Dialogue Canvas not set up", this);
        if (scriptToLoad != null && dialogueRunner != null && dialogueRunner != null)
        {
            dialogueRunner.Add(scriptToLoad); //adds the script to the dialogue system
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //if other is player
        if (other.gameObject.CompareTag("Player"))
        {

            if (!string.IsNullOrEmpty(talkToNode))
            {
                if (dialogueCanavas != null)
                {
                    //move the Canvas to the object and off set
                    dialogueCanavas.transform.SetParent(transform.parent.transform); // use the root to prevent scaling
                    dialogueCanavas.GetComponent<RectTransform>().anchoredPosition3D = transform.parent.TransformVector(PostionSpeachBubble);
                }

                if (dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.Stop();
                }
                Debug.Log("start dialogue");
                dialogueRunner.StartDialogue(talkToNode);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("collider exit");
        if (dialogueRunner.IsDialogueRunning)
        {
            dialogueRunner.Stop();
        }
    }



    //Update function to rotate canvas to always face the player?, and enable weight on animation to hae the character to speak facing the player

    private void Update()
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            //Rotate the canvas towards the player

        }
    }




}
