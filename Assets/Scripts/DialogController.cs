using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.UI;
using UnityEngine.Animations.Rigging;


public class DialogController : MonoBehaviour
{
    public string characterName = "";
    public string talkToNode = "";
    public YarnProgram scriptToLoad;
    public DialogueRunner dialogueRunner; //refernce to the dialogue control
    public GameObject dialogueCanavas; //refernce to the canvas
    Vector3 PostionSpeachBubble = new Vector3(0f, 0.7f, 0.01f);
    public GameObject player,emptyDialogueHolder,dialogueName; //reference to the player and to an empty game object
    public MultiAimConstraint headAimConstraint;


    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        if (scriptToLoad == null) Debug.LogError("Diaogue Controller not set up with yarn scriptToLoad ", this);
        if (string.IsNullOrEmpty(characterName)) Debug.LogWarning("Diaogue Controller not set up with characterName", this);
        if (string.IsNullOrEmpty(talkToNode)) Debug.LogError("Diaogue Controller not set up with talkToNode", this);

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
                    dialogueCanavas.SetActive(true);
                    dialogueCanavas.transform.SetParent(transform.parent.transform); // use the root to prevent scaling
                    dialogueCanavas.GetComponent<RectTransform>().anchoredPosition3D = transform.parent.TransformVector(PostionSpeachBubble);
                    dialogueName.GetComponent<Text>().text = characterName;

                }

                //Make NPC look at player - set aim constraint weight to one
                if (headAimConstraint != null)
                {
                StartCoroutine(ChangeWeight(0.0f, 1.0f, 0.6f));
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
        if (other.gameObject.CompareTag("Player"))
        {
            //move the Canvas to a empty holding gameobject, out of view on exit. Conversations therefore dont remain when walking away.
            dialogueCanavas.SetActive(false);
            dialogueCanavas.transform.SetParent(emptyDialogueHolder.transform); // use the root to prevent scaling
            dialogueCanavas.GetComponent<RectTransform>().anchoredPosition3D = transform.parent.TransformVector(PostionSpeachBubble);

            if (dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.Stop();
            }

            //Stop NPC from looking at player - set aim constraint weight to zero
            if (headAimConstraint != null)
            {
               StartCoroutine(ChangeWeight(1.0f, 0.0f, 0.6f));
            }

        }
    }



    //Update function to rotate canvas to always face the player

    private void Update()
    {
        if (dialogueRunner.IsDialogueRunning & dialogueCanavas.activeInHierarchy)
        {
            //Rotate the canvas towards the player
            //dialogueCanavas.transform.rotation =  player.transform.rotation;
            Vector3 playerlock = new Vector3 (player.transform.position.x, dialogueCanavas.transform.position.y, player.transform.position.z);
            dialogueCanavas.transform.LookAt(playerlock);


        }
    }


    //Function to smooth the change in multi-aim weight over time, so head doesnt jar to the player
    private IEnumerator ChangeWeight(float v_start, float v_end, float duration)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            headAimConstraint.weight = Mathf.Lerp(v_start, v_end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        headAimConstraint.weight = v_end;
    }



}
