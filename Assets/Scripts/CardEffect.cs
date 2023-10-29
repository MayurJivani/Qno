using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardEffect : MonoBehaviour
{
    static string[] normalNumbers = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
    static bool teleportationPlayed = false;

    private void Update()
    {
        if (teleportationPlayed)
            ApplyTeleportation();
    }
    public static void ApplyCardEffect(Card cardPlayed)
    {
        string numberPlayed = GameManager.IsLightSideUp() ? cardPlayed.lightSideNumber : cardPlayed.darkSideNumber;

        if (!checkIfNormalCard(numberPlayed))
        {
            ApplySpecialCardEffect(numberPlayed);
        }
    }

    public static bool checkIfNormalCard(string numberPlayed)
    {
        return normalNumbers.Contains(numberPlayed);
    }

    public static void ApplySpecialCardEffect(string typeOfSpecialCard)
    {
        switch(typeOfSpecialCard)
        {
            case "Pauli X":
                ApplyPauliX();
                break;
            case "Pauli Z":
                ApplyPauliZ();
                break;
            case "Pauli Y":
                ApplyPauliY();
                break;
            case "Teleportation":
                teleportationPlayed = true;
                break;
            case "Entanglement":
                ApplyEntanglement();
                break;
            case "Superposition":
                ApplySuperposition();
                break;
            case "Measurement":
                ApplyMeasurement();
                break;
            case "Colour Superposition":
                ApplyColourSuperposition();
                break;
        }
    }

    private static void ApplyColourSuperposition()
    {
        GameObject cardOnTopOfDiscardPileGameObject;
        Card cardOnTopOfDiscardPile;

        float randomNumber = Random.Range(0f, 4f);

        string[] lightSideColors = { "Red", "Green", "Blue", "Yellow" };
        string[] darkSideColors = { "Orange", "Teal", "Pink", "Purple" };

        int colorIndex = Mathf.FloorToInt(randomNumber);

        if (GameManager.IsLightSideUp())
        {
            cardOnTopOfDiscardPileGameObject = GameManager.discardPileListGameObject.ElementAt(GameManager.numberOfCardsInDiscardPile);
            cardOnTopOfDiscardPile = cardOnTopOfDiscardPileGameObject.GetComponentInChildren<Card>();


            cardOnTopOfDiscardPile.SetLightSideColourTo(lightSideColors[colorIndex]);
            GameManager.discardPileList[GameManager.numberOfCardsInDiscardPile] = cardOnTopOfDiscardPile;
            GameManager.discardPileListGameObject[GameManager.numberOfCardsInDiscardPile] = cardOnTopOfDiscardPileGameObject;
        }
        else
        {
            cardOnTopOfDiscardPileGameObject = GameManager.discardPileListGameObject.ElementAt(0);
            cardOnTopOfDiscardPile = cardOnTopOfDiscardPileGameObject.GetComponentInChildren<Card>();

            cardOnTopOfDiscardPile.SetDarkSideColourTo(darkSideColors[colorIndex]);
            GameManager.discardPileList[0] = cardOnTopOfDiscardPile;
            GameManager.discardPileListGameObject[0] = cardOnTopOfDiscardPileGameObject;
        }
    }

    private static void ApplyMeasurement()
    {
        GameObject cardOnTopOfDiscardPileGameObject;
        GameObject cardBelowTopCardGameObject;
        Card cardOnTopOfDiscardPile;
        Card cardBelowTopCard;

        if(GameManager.IsLightSideUp())
        {
            cardOnTopOfDiscardPileGameObject = GameManager.discardPileListGameObject.ElementAt(GameManager.numberOfCardsInDiscardPile);
            cardBelowTopCardGameObject = GameManager.discardPileListGameObject.ElementAt(GameManager.numberOfCardsInDiscardPile - 1);

            cardOnTopOfDiscardPile = cardOnTopOfDiscardPileGameObject.GetComponentInChildren<Card>();
            cardBelowTopCard = cardBelowTopCardGameObject.GetComponentInChildren<Card>();

            //If the card below the measurement card is a normal card, then measurement card will become the same card as the card below it
            if (!cardBelowTopCard.lightSideColour.Equals("Black"))
            {
                cardOnTopOfDiscardPile.SetLightSideColourAndNumberTo(cardBelowTopCard.lightSideColour, cardBelowTopCard.lightSideNumber);
                GameManager.discardPileList[GameManager.numberOfCardsInDiscardPile] = cardOnTopOfDiscardPile;
                GameManager.discardPileListGameObject[GameManager.numberOfCardsInDiscardPile] = cardOnTopOfDiscardPileGameObject;
            }
            //TODO: Define what the behaviour should be when a measurement card is played on an entanglement card
        }
        else
        {
            cardOnTopOfDiscardPileGameObject = GameManager.discardPileListGameObject.ElementAt(0);
            cardBelowTopCardGameObject = GameManager.discardPileListGameObject.ElementAt(1);

            cardOnTopOfDiscardPile = cardOnTopOfDiscardPileGameObject.GetComponentInChildren<Card>();
            cardBelowTopCard = cardBelowTopCardGameObject.GetComponentInChildren<Card>();

            if (!cardBelowTopCard.darkSideColour.Equals("Black"))
            {
                cardOnTopOfDiscardPile.SetDarkSideColourAndNumberTo(cardBelowTopCard.darkSideColour, cardBelowTopCard.darkSideNumber);
                GameManager.discardPileList[0] = cardOnTopOfDiscardPile;
                GameManager.discardPileListGameObject[0] = cardOnTopOfDiscardPileGameObject;
            }
            else if (cardOnTopOfDiscardPile.darkSideNumber.Equals("Superposition"))
            {
                RandomlyAssignCardNumberAndColour(cardOnTopOfDiscardPile);
                GameManager.discardPileList[0] = cardOnTopOfDiscardPile;
                GameManager.discardPileListGameObject[0] = cardOnTopOfDiscardPileGameObject;
            }
        }
    }

    private static void RandomlyAssignCardNumberAndColour(Card cardOnTopOfDiscardPile)
    {
        float randomNumberForColour = Random.Range(0f, 4f);
        float randomNumberForCardNumber = Random.Range(0f, 10f);

        string[] numbers = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
        string[] lightSideColors = { "Red", "Green", "Blue", "Yellow" };
        string[] darkSideColors = { "Orange", "Teal", "Pink", "Purple" };

        int colorIndex = Mathf.FloorToInt(randomNumberForColour);
        int numberIndex = Mathf.FloorToInt(randomNumberForCardNumber);

        if (GameManager.IsLightSideUp())
        {
            cardOnTopOfDiscardPile.SetLightSideColourAndNumberTo(lightSideColors[colorIndex], numbers[numberIndex]);
        }
        else
        {
            cardOnTopOfDiscardPile.SetDarkSideColourAndNumberTo(darkSideColors[colorIndex], numbers[numberIndex]);
        }
    }

    private static void ApplySuperposition()
    {

        //If card on top of discard pile is a normal card then the top card will now be in a superposition state. Unless the next player plays a superposition card or a measurement card
        //They will have to continue drawing cards from the draw pile.

        GameObject cardOnTopOfDiscardPileGameObject = GameManager.discardPileListGameObject.ElementAt(0);
        GameObject cardBelowTopCardGameObject = GameManager.discardPileListGameObject.ElementAt(1);

        Card cardOnTopOfDiscardPile = cardOnTopOfDiscardPileGameObject.GetComponentInChildren<Card>();
        Card cardBelowTopCard = cardBelowTopCardGameObject.GetComponentInChildren<Card>();

        if(!GameManager.IsLightSideUp()) //Superposition card is only available on the dark side.
        {
            //If card on top of discard pile is a superposition card then playing another superposition card will cause the superposition to collapse
            //The card will collapse to the card underneath the initial superposition card
            //After superposition card is played, the card on top of discard pile will be the superposition card. Hence we check for the card below the card that was just played
            if (cardBelowTopCard.darkSideNumber.Equals("Superposition")) 
            {
                GameObject TwoCardsBelowTopCardGameObject = GameManager.discardPileListGameObject.ElementAt(2);
                Card TwoCardsBelowTopCard = TwoCardsBelowTopCardGameObject.GetComponentInChildren<Card>();

                cardOnTopOfDiscardPile.SetDarkSideColourAndNumberTo(TwoCardsBelowTopCard.darkSideColour, TwoCardsBelowTopCard.darkSideNumber);

                GameManager.discardPileList[0] = cardOnTopOfDiscardPile;
                GameManager.discardPileListGameObject[0] = cardOnTopOfDiscardPileGameObject;
            }
        }
    }

    private static void ApplyEntanglement()
    {
        return;
    }

    //TODO: Add animation trigger
    //TODO: Highlight opponents cards when pointer is hovered over them
    private static void ApplyTeleportation()
    {
        if (GameManager.activePlayer.playerName.Equals(GameManager.player1.playerName)) //If active player is player1
        {
            //Turn of box colliders of the cards in the hand of player1 and turn on the box colliders of the cards in the hands of player2
            //Cast a ray from player2backcam to find out which card was clicked
            //Remove the selected card from the opponents hand and also from the list that contains info about the cards in their hand
            //Add the card in the hand of player1

            GameManager.IterateChildrenRecursivelyAndSetBoxCollider(GameManager.player1.handObject.transform, false);
            GameManager.IterateChildrenRecursivelyAndSetBoxCollider(GameManager.player2.handObject.transform, true);

            Camera rayCastCamera = GameManager.player2BackCamera;
            Card cardClicked;
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
            {
                Ray ray = rayCastCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform a raycast to see what the mouse clicked
                if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<Card>() != null)
                {
                    cardClicked = hit.transform.GetComponent<Card>();
                    cardClicked.PrintCardInfo();
                    GameManager.player2.handManager.RemoveCardFromHand(cardClicked);
                    GameManager.player2.handManager.DestroyCard(cardClicked);
                    GameManager.player1.handManager.DrawCard(cardClicked);
                    teleportationPlayed = !teleportationPlayed;
                }
                GameManager.IterateChildrenRecursivelyAndSetBoxCollider(GameManager.player1.handObject.transform, false);
            }
        }
        else if(GameManager.activePlayer.playerName.Equals(GameManager.player2.playerName))
        {
            GameManager.IterateChildrenRecursivelyAndSetBoxCollider(GameManager.player1.handObject.transform, true);
            GameManager.IterateChildrenRecursivelyAndSetBoxCollider(GameManager.player2.handObject.transform, false);

            Camera rayCastCamera = GameManager.player1BackCamera;
            Card cardClicked;
            if (Input.GetMouseButtonDown(0)) // Check for left mouse button click
            {
                Ray ray = rayCastCamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Perform a raycast to see what the mouse clicked
                if (Physics.Raycast(ray, out hit) && hit.transform.GetComponent<Card>() != null)
                {
                    cardClicked = hit.transform.GetComponent<Card>();
                    cardClicked.PrintCardInfo();
                    GameManager.player1.handManager.RemoveCardFromHand(cardClicked);
                    GameManager.player1.handManager.DestroyCard(cardClicked);
                    GameManager.player2.handManager.DrawCard(cardClicked);
                    teleportationPlayed = !teleportationPlayed;
                }
                GameManager.IterateChildrenRecursivelyAndSetBoxCollider(GameManager.player2.handObject.transform, false);
            }
        }
    }

    private static void ApplyPauliY()
    {
        ApplyPauliX();
        ApplyPauliZ();
    }

    private static void ApplyPauliZ()
    {
        //TODO: Reverse the animation on the rotating arrows showing direction of play
        //In a 2 player game, this gate will have no effect on the game
        return;
    }

    private static void ApplyPauliX()
    {
        GameManager.activePlayer.handObjectAnimatorController.SetTrigger("MoveUpAnimation");
        ApplyFlipOnAllCards();

        if (GameManager.IsLightSideUp())
        {
            GameManager.drawPileAnimatorController.SetTrigger("DeckFlipToDarkAnimation");
            GameManager.discardPileAnimatorController.SetTrigger("FlipDiscardPileToDark");
        }
        else
        {
            GameManager.drawPileAnimatorController.SetTrigger("DeckFlipToLightAnimation");
            GameManager.discardPileAnimatorController.SetTrigger("FlipDiscardPileToLight");
        }
        GameManager.FlipValueOfIsLightSideUp();
    }

    private static void ApplyFlipOnAllCards()
    {
        Transform player1HandObject = GameManager.player1.handObject.transform;
        Transform player2HandObject = GameManager.player1.handObject.transform;

        IterateChildrenRecursivelyAndApplyFlip(player1HandObject);
        IterateChildrenRecursivelyAndApplyFlip(player2HandObject);

    }

    public static void IterateChildrenRecursivelyAndApplyFlip(Transform parent)
    {
        foreach (Transform child in parent)
        {

            if (child.gameObject.GetComponent<Card>() != null)
            {
                Card card = child.gameObject.GetComponent<Card>();
                card.FlipCard();
            }
            // Recursively iterate through this child's children
            IterateChildrenRecursivelyAndApplyFlip(child);
        }
    }
}
