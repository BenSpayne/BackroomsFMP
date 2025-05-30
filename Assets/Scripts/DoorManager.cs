using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject CursorHover;
    public Animation doorOpen;
    public AudioSource doorOpenAudio;

    private IEnumerator OnMouseOver()
    {
        if (PlayerCasting.DistanceFromTarget < 2.5)
        {
            CursorHover.SetActive(true);

            if(Input.GetKeyDown(KeyCode.E))
            {
                GetComponent<BoxCollider>().enabled = false;
                doorOpen.Play();

                yield return new WaitForSeconds(0.5f);
                doorOpenAudio.Play();
            }
        }

        else
        {
            CursorHover.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        CursorHover.SetActive(false);   
    }  
}
