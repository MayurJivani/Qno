
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayCard : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform highlight;
    private RaycastHit raycastHit;
    private bool hasCardBeenPlayed = false;
    private Card cardPlayed;
    public HandManager handManager;
    public GameObject discardPile;
    public GameObject drawPile;
    public float lerpDuration = 0.01f;
    public int numberOfCardsInDiscardPile = 0;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (Input.GetMouseButtonDown(0) && highlight.CompareTag("Selectable") && !hasCardBeenPlayed && highlight && highlight.name != "DrawPile")
            {
                cardPlayed = highlight.gameObject.GetComponent<Card>();
                if (GameManager.IsLightSideUp())
                    Debug.Log("Card Played: " + cardPlayed.lightSideColour +" "+ cardPlayed.lightSideNumber);
                else
                    Debug.Log("Card Played: " + cardPlayed.darkSideColour + " " + cardPlayed.darkSideNumber);
                //hasCardBeenPlayed = true;
                handManager.RemoveCardFromHand(cardPlayed);
                handManager.printCardsInHand(handManager.getCardsInHand());
                handManager.RepositionCards();
                numberOfCardsInDiscardPile++;
                //highlight.parent.transform.position = discardPile.transform.position;

                highlight.parent.parent = null;
                
                StartCoroutine(LerpCardPosition(highlight.parent, discardPile.transform.position, discardPile.transform.eulerAngles, lerpDuration));
                highlight.parent.parent = discardPile.transform;
            }
        }   
    }

    private IEnumerator LerpCardPosition(Transform cardTransform, Vector3 targetPosition, Vector3 targetRotation, float duration)
    {
        float startTime = Time.time;
        Vector3 startPosition = cardTransform.position;
        Vector3 startRotation = cardTransform.eulerAngles;

        targetPosition += new Vector3(Random.Range(0f, 0.5f) , Random.Range(0f, 0.5f), - numberOfCardsInDiscardPile * 0.5f);
        targetRotation += new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f));
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            cardTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            cardTransform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        cardTransform.position = targetPosition;
        cardTransform.eulerAngles = targetRotation;
    }


}
