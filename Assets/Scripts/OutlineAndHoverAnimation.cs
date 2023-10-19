using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutlineSelection : MonoBehaviour
{
    private Transform highlight;
    private RaycastHit raycastHit;
    private Animator highlightAnimator;
    private bool isAnimationPlaying = false;
    void Update()
    {
        // Highlight
        if (highlight != null)
        {
            highlight.gameObject.GetComponent<Outline>().enabled = false;
            if(GameManager.IsLightSideUp())
            {
                highlight.gameObject.GetComponent<Animator>().SetTrigger("HoverDownLight");
                Debug.Log("Hover Down Light");
            }
            else
            {
                highlight.gameObject.GetComponent<Animator>().SetTrigger("HoverDownDark");
                Debug.Log("Hover Down Dark");
            }
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit)) //Make sure you have EventSystem in the hierarchy before using EventSystem
        {
            highlight = raycastHit.transform;
            highlightAnimator = highlight.gameObject.GetComponent<Animator>();
            if (highlight.CompareTag("Selectable"))
            {
                if (highlight.gameObject.GetComponent<Outline>() != null)
                {
                    highlight.gameObject.GetComponent<Outline>().enabled = true;
                    if(!isAnimationPlaying)
                    {
                        if(GameManager.IsLightSideUp())
                        {
                            highlightAnimator.SetTrigger("HoverUpLight");
                            Debug.Log("Hover Up Light");
                        }
                        else
                        {
                            highlightAnimator.SetTrigger("HoverUpDark");
                            Debug.Log("Hover Up Dark");
                        }
                        isAnimationPlaying = true;
                    }
                    isAnimationPlaying = false;
                }
                else
                {
                    Outline outline = highlight.gameObject.AddComponent<Outline>();
                    outline.enabled = true;
                    highlight.gameObject.GetComponent<Outline>().OutlineColor = Color.magenta;
                    highlight.gameObject.GetComponent<Outline>().OutlineWidth = 7.0f;
                }
            }
            else
            {
                highlight = null;
            }
        }

    }
}