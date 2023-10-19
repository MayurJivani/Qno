using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class HandManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform handObject; // Reference to the parent hand object.
    public float zOffset = 0.05f;
    public float angleDifference = 1f;
    public float lerpDuration = 1f;

    // List to keep track of all the cards in the hand.
    private List<Transform> cardsInHand = new List<Transform>();

    private void Awake()
    {
        Debug.Log("HandManager Initialized");
    }


    public void DrawCard(Card deckCard)
    {
        float topCardOffset = 1f;
        Debug.Log("Card Drawn");

        // Calculate the card's position based on the previous cards and offset.
        int cardCount = cardsInHand.Count;
        Vector3 cardPosition = handObject.transform.position;
        Debug.Log("HandObject position: " + cardPosition);

        // Draw the card at the calculated position.
        // Instantiate a new card from the prefab at the specified position.
        GameObject newCard;
        Card cardComponent = cardPrefab.GetComponentInChildren<Card>();
        cardComponent.lightSideNumber = deckCard.lightSideNumber;
        cardComponent.lightSideColour = deckCard.lightSideColour;
        cardComponent.darkSideNumber = deckCard.darkSideNumber;
        cardComponent.darkSideColour = deckCard.darkSideColour;
        Debug.Log("Is Light Side Up:" + GameManager.IsLightSideUp());
        
        if (GameManager.IsLightSideUp())
        {
            Transform Model = cardPrefab.transform.Find("Model");
            Model.localEulerAngles = new Vector3(180f, 0f, 0f);
            newCard = Instantiate(cardPrefab, cardPosition, Quaternion.Euler(0f, 180f, 180f), handObject.transform);
        }
        else
        {
            Transform Model = cardPrefab.transform.Find("Model");
            Model.localEulerAngles = new Vector3(0f, 0f, -180f);
            newCard = Instantiate(cardPrefab, cardPosition, Quaternion.Euler(0f, 180f, 180f), handObject.transform);
            
        }
        // Add the card to the list of cards in the hand.
        cardsInHand.Add(newCard.transform);

        // Check for available space and reposition cards if necessary.
        RepositionCards(cardPosition);
    }

    public void RemoveCardFromHand(Card cardToRemove)
    {
        Transform cardToRemoveFromList = null;
        foreach (Transform cardObject in cardsInHand)
        {
            Card cardComponent = cardObject.GetComponentInChildren<Card>();
            if(compareCards(cardComponent, cardToRemove))
            {
                cardToRemoveFromList = cardObject;
            }
        }

        cardsInHand.Remove(cardToRemoveFromList);
    }

    private bool compareCards(Card card1, Card card2)
    {
        if(card1 != null && card2 != null) 
        { 
            card1.lightSideColour = card2.lightSideColour;
            card1.lightSideNumber = card2.lightSideNumber;
            card1.darkSideColour = card2.darkSideColour;
            card1.darkSideNumber = card2.darkSideNumber;
            return true;
        }
        return false;
    }

    private void RepositionCards(Vector3 newCardInitialPosition)
    {
        int cardCount = cardsInHand.Count;

        Vector3 cardSize = cardPrefab.GetComponentInChildren<Renderer>().bounds.size;

        // Center position of the hand object
        Vector3 handCenter = handObject.transform.localPosition;

        int factor = cardCount - 1;

        for (int i = 0; i < cardCount; i++)
        {
            float offset = cardSize.x / 4;
            Vector3 newPositionCard;
            //Vector3 newAngleCard = new Vector3(0, 180 - factor * angleDifference, 0);
            newPositionCard = new Vector3(handCenter.x - factor * offset, i*(0.075f), i*(0.05f));
            //Debug.Log("New Card Position for card " + i + " is :" + newPositionCard);


            factor = factor - 2;
            StartCoroutine(LerpCardPosition(cardsInHand[i], newPositionCard, lerpDuration));
        }
            //cardsInHand[i].localPosition = newPositionCard;
           // cardsInHand[i].localEulerAngles = newAngleCard;
       
    }

    private IEnumerator LerpCardPosition(Transform cardTransform, Vector3 targetPosition, float duration)
    {
        //Debug.Log("Lerp Started");
        float startTime = Time.time;
        Vector3 startPosition = cardTransform.localPosition;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            cardTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        cardTransform.localPosition = targetPosition;
    }
    public void printCardsInHand()
    {
        for(int i=0; i<cardsInHand.Count; i++)
        {
            Debug.Log("cardsInHand["+i+"] Position: " + cardsInHand[i].transform.position);
            Debug.Log("cardsInHand[" + i + "] Angles: " + cardsInHand[i].transform.localEulerAngles);
        }
    }
}
