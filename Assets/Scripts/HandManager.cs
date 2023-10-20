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
    private static List<Transform> cardsInHandTransform = new List<Transform>();
    private static List<Card> cardsInHand = new List<Card>();

    private void Awake()
    {
        Debug.Log("HandManager Initialized");
    }


    public void DrawCard(Card deckCard)
    {
        float topCardOffset = 1f;
        Debug.Log("Card Drawn");

        // Calculate the card's position based on the previous cards and offset.
        int cardCount = cardsInHandTransform.Count;
        Vector3 cardPosition = handObject.transform.position;

        // Draw the card at the calculated position.
        // Instantiate a new card from the prefab at the specified position.
        GameObject newCard;
        Card cardComponent = cardPrefab.GetComponentInChildren<Card>();
        cardComponent.SetCardProperties(deckCard.lightSideNumber, deckCard.lightSideColour, deckCard.darkSideNumber, deckCard.darkSideColour);

        Debug.Log("card Component Values:");
        cardComponent.PrintCardInfo();
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

        Debug.Log("Card Info After Instantiation:");
        newCard.GetComponentInChildren<Card>().PrintCardInfo();
        // Add the card to the list of cards in the hand.
        cardsInHandTransform.Add(newCard.transform);
        cardsInHand.Add(newCard.GetComponentInChildren<Card>());

        // Check for available space and reposition cards if necessary.
        RepositionCards();
    }

    public void RemoveCardFromHand(Card cardToRemove)
    {
        Transform cardToRemoveFromListTransform = null;
        Card cardToRemoveFromList = null;
        foreach (Transform cardObject in cardsInHandTransform)
        {
            Card card = cardObject.GetComponentInChildren<Card>();
            if(compareCards(card, cardToRemove))
            {
                cardToRemoveFromListTransform = cardObject;
                cardToRemoveFromList = card;
                break;
            }
        }

        cardsInHand.Remove(cardToRemoveFromList);
        cardsInHandTransform.Remove(cardToRemoveFromListTransform);
    }

    private bool compareCards(Card card1, Card card2)
    {
        if(card1 != null && card2 != null) 
        {
            if (card1.lightSideColour != card2.lightSideColour
                || card1.lightSideNumber != card2.lightSideNumber
                || card1.darkSideColour != card2.darkSideColour
                || card1.darkSideNumber != card2.darkSideNumber)
            {
                return false;
            }
            else
                return true;
        }
        return false;
    }

    public void RepositionCards()
    {
        int cardCount = cardsInHandTransform.Count;

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
            StartCoroutine(LerpCardPosition(cardsInHandTransform[i], newPositionCard, lerpDuration));
        }
            //cardsInHandTransform[i].localPosition = newPositionCard;
           // cardsInHandTransform[i].localEulerAngles = newAngleCard;
       
    }

    private IEnumerator LerpCardPosition(Transform cardTransform, Vector3 targetPosition, float duration)
    {
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

    public List<Transform> getCardsInHand()
    {
        return cardsInHandTransform;
    }
    public void printCardsInHand(List<Transform> cardsInHandTransform)
    {
        Debug.Log("Cards In Hand:");
        for(int i=0; i<cardsInHandTransform.Count; i++)
        {
            Card card = cardsInHandTransform[i].GetComponentInChildren<Card>();
            Debug.Log("Card " + i + ": ");
            card.PrintCardInfo();
        }
    }
}
