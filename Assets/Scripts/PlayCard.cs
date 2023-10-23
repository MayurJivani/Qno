
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayCard : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform highlight;
    private RaycastHit raycastHit;
    private bool hasCardBeenPlayed = false;
    private Card cardPlayed;

    public GameObject discardPile;
    private float lerpDuration = 0.5f;
    public int numberOfCardsInDiscardPile = 0;
    private Vector3 targetRotationOfPlayedCard;

    private int lightSide = 0;
    private int darkSide = 1;

    private void Awake()
    {
        targetRotationOfPlayedCard = discardPile.transform.eulerAngles;
    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (Input.GetMouseButtonDown(0) && highlight.CompareTag("Selectable") && !hasCardBeenPlayed)
            {
                cardPlayed = highlight.gameObject.GetComponent<Card>();
                if (GameManager.IsLightSideUp())
                    Debug.Log("Card Played: " + cardPlayed.lightSideColour +" "+ cardPlayed.lightSideNumber);
                else
                    Debug.Log("Card Played: " + cardPlayed.darkSideColour + " " + cardPlayed.darkSideNumber);
                //hasCardBeenPlayed = true;
                highlight.gameObject.tag = "Untagged";
                Destroy(highlight.GetComponent<Animator>());
                //handManager.RemoveCardFromHand(cardPlayed);
                //handManager.printCardsInHand();
                //handManager.RepositionCards(GameManager.activePlayer.handObject);
                numberOfCardsInDiscardPile++;

                highlight.parent.parent = null;

                int desiredLayer = LayerMask.NameToLayer("Discard Pile");
                highlight.gameObject.layer = desiredLayer;
                highlight.parent.gameObject.layer = desiredLayer;
                
                StartCoroutine(LerpCardPosition(highlight.parent, discardPile.transform.position, targetRotationOfPlayedCard, lerpDuration));
                highlight.parent.parent = discardPile.transform;

                //GameManager.ChangeActivePlayer();
            }
        }   
    }

    private IEnumerator LerpCardPosition(Transform cardTransform, Vector3 targetPosition, Vector3 targetRotation, float duration)
    {
        Debug.Log("Initial Rotation:" + cardTransform.eulerAngles);
        Debug.Log("Target Rotation:"+ targetRotation);
        float startTime = Time.time;
        Vector3 startPosition = cardTransform.position;
        Vector3 startRotation = cardTransform.eulerAngles;

        

        float zOffset = GameManager.IsLightSideUp() ? -lightSide * 0.1f : -darkSide * 0.1f;
        targetPosition += new Vector3(0, 0, zOffset);

        if (GameManager.IsLightSideUp())
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


}
