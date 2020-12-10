using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;



public class ReactionManager : MonoBehaviour
{
    public MultiAimConstraint headAimConstraint;
    public GameObject player;
    public Animator animationController;
    public NavigationController navController;
    public AudioSource shoveSound;


    //Make the NPC face towards an NPC or the player when entering the collider sphere
    private void OnTriggerEnter(Collider other)
    {
        //if other is player or NPC
        if (other.gameObject.CompareTag("Player") | other.gameObject.CompareTag("NPC"))
        {

            //Add NPC or Player to target - clear any existing
            var data = headAimConstraint.data.sourceObjects;
            data.Clear();
            data.Add(new WeightedTransform(other.gameObject.transform, 1));
            headAimConstraint.data.sourceObjects = data;
            RigBuilder rigs = GetComponent<RigBuilder>();
            rigs.Build();

            //Make NPC look - set aim constraint weight to one
            if (headAimConstraint != null)
            {
                StartCoroutine(ChangeWeight(0.0f, 1.0f, 0.4f));
            }
        }
    }

    //Reverse of above ^
    private void OnTriggerExit(Collider other)
    {
        //if other is player or NPC
        if (other.gameObject.CompareTag("Player") | other.gameObject.CompareTag("NPC"))
        {
            //Rest Weight
            if (headAimConstraint != null)
            {
                StartCoroutine(ChangeWeight(1.0f, 0.0f, 0.4f));
            }


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
        
        if (v_end == 0.0f) //Check if this is on exit, if so then remove the target after coroutine finishes
        {
            var data = headAimConstraint.data.sourceObjects;
            data.Clear();
            headAimConstraint.data.sourceObjects = data;
            RigBuilder rigs = GetComponent<RigBuilder>();
            rigs.Build();
        }

    }


    //Used to check for distance and call other functions when within NPC range
    private void OnTriggerStay(Collider other)
    {
        //if collison is player
        if (other.gameObject.CompareTag("Player"))
        {

            Vector3 p = transform.InverseTransformPoint(other.gameObject.transform.position); // p is the position of something in local coordinate space.
            ShoveDetection(p);

        }
    }




    //Function to set the "isShoved" animator parameter when the player is very close to the NPC
    private void ShoveDetection(Vector3 pos)
    {
        
        if (Vector3.Distance(transform.position, player.transform.position) < 1.5f)
        {
            if (animationController != null)
            {
                

                if (pos.x < 0.3 & pos.z < 0.8)
                {
                    //Debug.Log("Shoved Left");
                    animationController.SetBool("isShovedLeft", true);
                    shoveSound.Play();
                }
                else if (pos.x > 0.3 & pos.z < 0.8)
                {
                    //Debug.Log("Shoved Right");
                    animationController.SetBool("isShovedRight", true);
                    shoveSound.Play();
                }
                else if (pos.z >= 0.8)
                {
                    //Debug.Log("Shoved Font");
                    animationController.SetBool("isShovedFront", true);
                    shoveSound.Play();
                }

            }
        }
        else
        {
            if (animationController != null)
            {
                animationController.SetBool("isShovedFront", false);
                animationController.SetBool("isShovedLeft", false);
                animationController.SetBool("isShovedRight", false);
            }
        }
    }






}
