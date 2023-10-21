using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardDeckGenerator))]
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    HandManager handManager;
    private List<Card> deck;
    static int cardNumberFromTop = 0;
    static int cardNumberFromBottom = 0;
    private static bool isLightSideUp = true;
    private GameObject handObject;
    private GameObject drawPile;
    private GameObject discardPile;
    private Transform topCardObject;
    private Transform bottomCardObject;

    private Animator drawPileAnimatorController;
    private Animator discardPileAnimatorController;
    private Animator handObjectAnimatorController;
    private Animator topCardObjectAnimatorController;
    private Animator bottomCardObjectAnimatorController;

    private void Awake()
    {
        deck = CardDeckGenerator.getDeck();
        handManager = GetComponent<HandManager>();
        handObject = GameObject.Find("Plane");
        drawPile = GameObject.Find("DrawPile");
        discardPile = GameObject.Find("DiscardPile");

        drawPileAnimatorController = drawPile.GetComponent<Animator>();
        handObjectAnimatorController = handObject.GetComponent<Animator>();
        discardPileAnimatorController = discardPile.GetComponent<Animator>();

        topCardObject = drawPile.transform.Find("Card"); 
        topCardObjectAnimatorController = topCardObject.GetComponent<Animator>();

        bottomCardObject = drawPile.transform.Find("Card.047");
        bottomCardObjectAnimatorController = bottomCardObject.GetComponent<Animator>(); 
    }


    // Update is called once per frame
    void Update()
    {
        //TODO: Write code for functionality to be able to click the deck and then get the new card.
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position to see if it hits the deck GameObject.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == drawPile)
            {
                HandleDeckClick();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        { 
            handObjectAnimatorController.SetTrigger("MoveUpAnimation");
            if (isLightSideUp)
            {
                drawPileAnimatorController.SetTrigger("DeckFlipToDarkAnimation");
                discardPileAnimatorController.SetTrigger("FlipDiscardPileToDark");
            }
            else
            {
                drawPileAnimatorController.SetTrigger("DeckFlipToLightAnimation");
                discardPileAnimatorController.SetTrigger("FlipDiscardPileToLight");
            }
            isLightSideUp = !isLightSideUp;
        }
    }

    private void HandleDeckClick()
    {
        if (isLightSideUp)
        {
            topCardObjectAnimatorController.SetTrigger("DrawLightCardAnimation");
            StartCoroutine(AddCardToHand());
        }
        else
        {
            bottomCardObjectAnimatorController.SetTrigger("DrawDarkCardAnimation");
            StartCoroutine(AddCardToHand());
        }
        // TODO: Handle case when cardNumber >= 108
    }


    IEnumerator AddCardToHand()
    {
        yield return new WaitForSeconds(1.6f);
        if (isLightSideUp)
        {
            Card card = deck[cardNumberFromTop];
            Debug.Log("Deck Card Info:");
            card.PrintCardInfo();
            handManager.DrawCard(card);
            cardNumberFromTop++;
        }
        else
        {
            Card card = deck[deck.Count - 1 - cardNumberFromBottom];
            card.PrintCardInfo();
            handManager.DrawCard(card);
            cardNumberFromBottom++;
        }
        
    }

    public static int GetIndexOfCardOnTopOfDeck()
    {
        return cardNumberFromTop;
    }

    public static int GetIndexOfCardAtBottomOfDeck()
    {
        return cardNumberFromBottom;
    }

    public static bool IsLightSideUp()
    {
        return isLightSideUp;
    }
}
