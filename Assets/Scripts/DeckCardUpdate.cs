using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CardUpdateAnimationEvents : MonoBehaviour
{
    // This method is called by the animation event when you want to update the top card for the light side.
    public GameObject gameManager;
    public void UpdateTopCardForLightSide()
    {
        int currentIndexFromTop = GameManager.GetIndexOfCardOnTopOfDeck();
        AddCardsToDeckObject cardUpdater = gameManager.GetComponent<AddCardsToDeckObject>();
        cardUpdater.AddPropertiesToCardPrefab(cardUpdater.cardBelowTopCardComponent, cardUpdater.deck[currentIndexFromTop + 1]);
    }

    // This method is called by the animation event when you want to update the top card for the dark side.
    public void UpdateTopCardForDarkSide()
    {
        int currentIndexFromBottom = GameManager.GetIndexOfCardAtBottomOfDeck();
        AddCardsToDeckObject cardUpdater = gameManager.GetComponent<AddCardsToDeckObject>();
        int deckCount = cardUpdater.deck.Count;
        cardUpdater.AddPropertiesToCardPrefab(cardUpdater.cardAboveBottomCardComponent, cardUpdater.deck[deckCount - 2 - currentIndexFromBottom]);
    }
}

