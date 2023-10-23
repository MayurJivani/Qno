using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;

[RequireComponent(typeof(CardDeckGenerator))]
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    AddCardsToDeckObject cardUpdater;
    private List<Card> deck;

    private float playCardLerpDuration = 0.5f;
    private int lightSide = 0;
    private int darkSide = 1;

    private static GameObject cardPrefab;

    private static int cardNumberFromTop = 0;
    private static int cardNumberFromBottom = 0;
    private static int numberOfCardsInDiscardPile = 0;

    private static bool isLightSideUp = true;

    private static GameObject player1HandObject;
    private static GameObject player2HandObject;

    private static Camera player1FrontCamera;
    private static Camera player1BackCamera;

    private static Camera player2FrontCamera;
    private static Camera player2BackCamera;

    private GameObject drawPile;
    private GameObject discardPile;

    private Transform topCardObject;
    private Transform bottomCardObject;

    private Animator drawPileAnimatorController;
    private Animator discardPileAnimatorController;

    private static Animator player1HandObjectAnimatorController;
    private static Animator player2HandObjectAnimatorController;

    private Animator topCardObjectAnimatorController;
    private Animator bottomCardObjectAnimatorController;

    private static Player player1;
    private static Player player2;
    public static Player activePlayer;

    private Transform cursorHoveredObject;

    private void Awake()
    {
        deck = CardDeckGenerator.getDeck();
        InitialiseGameObjects();
        InitialiseAnimatorComponents();
        CreatePlayers();
        StartNewGame();
    }

    private void InitialiseGameObjects()
    {
        cardUpdater = GetComponent<AddCardsToDeckObject>();

        cardPrefab = Resources.Load<GameObject>("Prefabs/Card (2)");

        player1HandObject = GameObject.Find("Player1Hand");
        player2HandObject = GameObject.Find("Player2Hand");

        player1FrontCamera = GameObject.Find("Player1FrontCamera").GetComponent<Camera>();
        player1BackCamera = GameObject.Find("Player1BackCamera").GetComponent<Camera>();

        player2FrontCamera = GameObject.Find("Player2FrontCamera").GetComponent<Camera>();
        player2BackCamera = GameObject.Find("Player2BackCamera").GetComponent<Camera>();

        drawPile = GameObject.Find("DrawPile");
        discardPile = GameObject.Find("DiscardPile");

        topCardObject = drawPile.transform.Find("Card");
        bottomCardObject = drawPile.transform.Find("Card.047");

        player1FrontCamera.enabled = false;
        player1BackCamera.enabled = false;
        player2FrontCamera.enabled = false;
        player2BackCamera.enabled = false;

        CheckAndLogObject("CardPrefab", cardPrefab);
        CheckAndLogObject("Player1HandObject", player1HandObject);
        CheckAndLogObject("Player2HandObject", player2HandObject);
        CheckAndLogObject("Player1FrontCamera", player1FrontCamera?.gameObject);
        CheckAndLogObject("Player1BackCamera", player1BackCamera?.gameObject);
        CheckAndLogObject("Player2FrontCamera", player2FrontCamera?.gameObject);
        CheckAndLogObject("Player2BackCamera", player2BackCamera?.gameObject);
        CheckAndLogObject("DrawPile", drawPile);
        CheckAndLogObject("DiscardPile", discardPile);
        CheckAndLogObject("TopCardObject", topCardObject?.gameObject);
        CheckAndLogObject("BottomCardObject", bottomCardObject?.gameObject);
    }

    private void CheckAndLogObject(string objectName, GameObject obj)
    {
        if (obj != null)
        {
            Debug.Log(objectName + " found: " + obj.name);
        }
        else
        {
            Debug.LogError(objectName + " is null.");
        }
    }

    private void InitialiseAnimatorComponents()
    {
        drawPileAnimatorController = drawPile.GetComponent<Animator>();
        discardPileAnimatorController = discardPile.GetComponent<Animator>();

        player1HandObjectAnimatorController = player1HandObject.GetComponent<Animator>();
        player2HandObjectAnimatorController = player2HandObject.GetComponent<Animator>();

        topCardObjectAnimatorController = topCardObject.GetComponent<Animator>();

        bottomCardObjectAnimatorController = bottomCardObject.GetComponent<Animator>();
    }

    private static void CreatePlayers()
    {
        player1 = new Player("player1", player1HandObject, player1HandObjectAnimatorController, player1FrontCamera, player1BackCamera, cardPrefab);
        player2 = new Player("player2", player2HandObject, player2HandObjectAnimatorController, player2FrontCamera, player2BackCamera, cardPrefab);
        Debug.Log("Players Created");
    }

    private static void StartNewGame()
    {
        activePlayer = player1;

        player1FrontCamera.enabled = true;
        player2BackCamera.enabled = true;

        player1BackCamera.enabled = false;
        player2FrontCamera.enabled = false;

        IterateChildrenRecursivelyAndSetBoxCollider(player1.handObject.transform, true);
        IterateChildrenRecursivelyAndSetBoxCollider(player2.handObject.transform, false);
        //TODO: Draw 7 cards in both player1's and player2's hand 
    }

    private void ChangeActivePlayer()
    {
        if(activePlayer.playerName == player1.playerName)
        {
            activePlayer = player2;
        }
        else
        {
            activePlayer = player1;
        }
        StartCoroutine(ChangePlayerView());
        StartCoroutine(ChangeSelectableHand());
    }

    private static IEnumerator ChangePlayerView()
    {
        yield return new WaitForSeconds(2f);
        if(activePlayer.playerName == player1.playerName)
        {
            player1FrontCamera.enabled = true;
            player2BackCamera.enabled = true;

            player1BackCamera.enabled = false;
            player2FrontCamera.enabled = false;
        }
        else
        {
            player2FrontCamera.enabled = true;
            player1BackCamera.enabled = true;

            player2BackCamera.enabled = false;  
            player1FrontCamera.enabled = false;
        }
    }

    private static IEnumerator ChangeSelectableHand()
    {
        yield return new WaitForSeconds(2f);
        if (activePlayer.playerName == player1.playerName)
        {
            IterateChildrenRecursivelyAndSetBoxCollider(player1.handObject.transform, true);
            IterateChildrenRecursivelyAndSetBoxCollider(player2.handObject.transform, false);
        }
        else
        {
            IterateChildrenRecursivelyAndSetBoxCollider(player1.handObject.transform, false);
            IterateChildrenRecursivelyAndSetBoxCollider(player2.handObject.transform, true);
        }
    }

    private static void IterateChildrenRecursivelyAndSetBoxCollider(Transform parent, bool isColliderEnabled)
    {
        foreach (Transform child in parent)
        {

            if(child.gameObject.GetComponent<BoxCollider>() != null)
            {
                child.gameObject.GetComponent<BoxCollider>().enabled = isColliderEnabled;
            }
            // Recursively iterate through this child's children
            IterateChildrenRecursivelyAndSetBoxCollider(child, isColliderEnabled);
        }
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
            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                cursorHoveredObject = hit.transform;
                if (Input.GetMouseButtonDown(0) && cursorHoveredObject.CompareTag("Selectable") && hit.transform.gameObject != drawPile) //&& !hasCardBeenPlayed)
                {
                    HandlePlayCard(cursorHoveredObject);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            //TODO: assign animator according to the player turn
            activePlayer.handObjectAnimatorController.SetTrigger("MoveUpAnimation");
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

    private void HandlePlayCard(Transform cursorHoveredObject)
    {
        Card cardPlayed = cursorHoveredObject.gameObject.GetComponent<Card>();
        if (isLightSideUp)
            Debug.Log("Card Played: " + cardPlayed.lightSideColour + " " + cardPlayed.lightSideNumber);
        else
            Debug.Log("Card Played: " + cardPlayed.darkSideColour + " " + cardPlayed.darkSideNumber);
        //hasCardBeenPlayed = true;

        cursorHoveredObject.gameObject.tag = "Untagged";
        Destroy(cursorHoveredObject.GetComponent<Animator>());
        activePlayer.handManager.RemoveCardFromHand(cardPlayed);
        activePlayer.handManager.printCardsInHand();
        activePlayer.handManager.RepositionCards(GameManager.activePlayer.handObject);
        numberOfCardsInDiscardPile++;

        cursorHoveredObject.parent.parent = null;

        int desiredLayer = LayerMask.NameToLayer("Discard Pile");
        cursorHoveredObject.gameObject.layer = desiredLayer;
        cursorHoveredObject.parent.gameObject.layer = desiredLayer;

        StartCoroutine(LerpCardPosition(cursorHoveredObject.parent, discardPile.transform.position, discardPile.transform.eulerAngles, playCardLerpDuration));
        cursorHoveredObject.parent.parent = discardPile.transform;

        ChangeActivePlayer();
    }

    private IEnumerator LerpCardPosition(Transform cardTransform, Vector3 targetPosition, Vector3 targetRotation, float duration)
    {
        Debug.Log("Initial Rotation:" + cardTransform.eulerAngles);
        Debug.Log("Target Rotation:" + targetRotation);
        float startTime = Time.time;
        Vector3 startPosition = cardTransform.position;
        Vector3 startRotation = cardTransform.eulerAngles;


        float zOffset = isLightSideUp ? -lightSide * 0.1f : -darkSide * 0.1f;
        targetPosition += new Vector3(0, 0, zOffset);

        if (isLightSideUp)
        {
            lightSide++;
        }
        else
        {
            darkSide++;
        }
        Vector3 randomRotation = new Vector3(0, 0, Random.Range(-30f, 30f));
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            cardTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            cardTransform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, t);
            yield return null;
        }

        cardTransform.position = targetPosition;
        cardTransform.eulerAngles = targetRotation;
        cardTransform.Find("Model").transform.localEulerAngles += randomRotation;
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
        yield return new WaitForSeconds(1.5f);
        if (isLightSideUp)
        {
            Card card = deck[cardNumberFromTop];
            Debug.Log("Deck Card Info:");
            card.PrintCardInfo();
            activePlayer.handManager.DrawCard(card);
            ChangeActivePlayer();
            cardNumberFromTop++;
        }
        else
        {
            Card card = deck[deck.Count - 1 - cardNumberFromBottom];
            card.PrintCardInfo();
            activePlayer.handManager.DrawCard(card);
            ChangeActivePlayer();
            cardNumberFromBottom++;
        }
        cardUpdater.UpdateTopAndBottomCard(cardNumberFromTop, cardNumberFromBottom);
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
