using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardDeckGenerator))]
public class AddCardsToDeckObject : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Card> deck;
    public GameObject deckObject;
    public int currentIndexFromTop = 0; 
    public int currentIndexFromBottom = 0; 
    public Transform topCard;
    public Transform cardBelowTopCard;
    public Transform cardAboveBottomCard;
    public Transform bottomCard;
    public Card topCardComponent;
    public Card cardBelowTopCardComponent;
    public Card cardAboveBottomCardComponent;
    public Card bottomCardComponent;
    private bool onlyOnce = true;

    private void Awake()
    {
        Debug.Log("AddCardToDeckObject Initialized");
        deck = CardDeckGenerator.getDeck();
        deckObject = GameObject.Find("DrawPile");

        // Initialize references to the card objects and their components.
        topCard = deckObject.transform.Find("Card.000");
        cardBelowTopCard = deckObject.transform.Find("Card.001");
        cardAboveBottomCard = deckObject.transform.Find("Card.046");
        bottomCard = deckObject.transform.Find("Card.047");
        topCardComponent = topCard.GetComponent<Card>();
        cardBelowTopCardComponent = cardBelowTopCard.GetComponent<Card>();
        cardAboveBottomCardComponent = cardAboveBottomCard.GetComponent<Card>();
        bottomCardComponent = bottomCard.GetComponent<Card>();
    }
    private void Update()
    {
        currentIndexFromTop = GameManager.GetIndexOfCardOnTopOfDeck();
        currentIndexFromBottom = GameManager.GetIndexOfCardAtBottomOfDeck();

        if(onlyOnce)
        {
            AddPropertiesToCardPrefab(cardBelowTopCardComponent, deck[1]);
            AddPropertiesToCardPrefab(cardAboveBottomCardComponent, deck[deck.Count - 2]);
            onlyOnce = false;
        }

        AddPropertiesToCardPrefab(topCardComponent, deck[currentIndexFromTop]);
        AddPropertiesToCardPrefab(bottomCardComponent, deck[deck.Count - 1 - currentIndexFromBottom]);
    }


    public void AddPropertiesToCardPrefab(Card cardComponent, Card deckCard)
    {
        cardComponent.SetCardProperties(deckCard.lightSideNumber, deckCard.lightSideColour, deckCard.darkSideNumber, deckCard.darkSideColour);
        cardComponent.SetDarkSideMaterial();
        cardComponent.SetLightSideMaterial();
    }


}
