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
    private GameObject deckObject;
    private Transform topCardObject;
    private Transform bottomCardObject;
    public Animator deckObjectAnimatorController;
    public Animator handObjectAnimatorController;

    public readonly static Dictionary<string, Material> cardMaterials;
    public readonly static Dictionary<string, Texture2D> lightTextures;
    public readonly static Dictionary<string, Texture2D> darkTextures;
    public readonly static Dictionary<string, Color[]> materialColours;


    public Animator topCardObjectAnimatorController;
    public Animator bottomCardObjectAnimatorController;

    static GameManager()
    {
        cardMaterials = new Dictionary<string, Material>();
        lightTextures = new Dictionary<string, Texture2D>();
        darkTextures = new Dictionary<string, Texture2D>();
        materialColours = new Dictionary<string, Color[]>();
    }
    private void Awake()
    {
        InitializeCardMaterials();
        deck = CardDeckGenerator.getDeck();
        handManager = GetComponent<HandManager>();
        handObject = GameObject.Find("Plane");
        deckObject = GameObject.Find("DrawPile");

        deckObjectAnimatorController = deckObject.GetComponent<Animator>();
        handObjectAnimatorController = handObject.GetComponent<Animator>();

        topCardObject = deckObject.transform.Find("Card.000"); 
        topCardObjectAnimatorController = topCardObject.GetComponent<Animator>();

        bottomCardObject = deckObject.transform.Find("Card.047");
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
            if (Physics.Raycast(ray, out hit) && hit.transform.gameObject == deckObject)
            {
                HandleDeckClick();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        { 
            handObjectAnimatorController.SetTrigger("MoveUpAnimation");
            if (isLightSideUp)
            {
                deckObjectAnimatorController.SetTrigger("DeckFlipToDarkAnimation");
            }
            else
            {
                deckObjectAnimatorController.SetTrigger("DeckFlipToLightAnimation");
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
            Debug.Log("Deck Card Info:");
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

    private static void InitializeCardMaterials()
    {
        // Associate card colours with materials.
        cardMaterials.Add("FrontFace_Card", Instantiate(Resources.Load<Material>("Materials/FrontFace_Card (1)")));
        cardMaterials.Add("BackFace_Card", Instantiate(Resources.Load<Material>("Materials/BackFace_Card (1)")));
        cardMaterials.Add("Black", Resources.Load<Material>("Materials/Black"));

        //Top Colour         //Bottom Colour    
        materialColours.Add("Red", new Color[] { hexToRGB("FF7474"), hexToRGB("F10000") });
        materialColours.Add("Green", new Color[] { hexToRGB("55FF60"), hexToRGB("00FF00") });
        materialColours.Add("Blue", new Color[] { hexToRGB("55A3FF"), hexToRGB("0028FF") });
        materialColours.Add("Yellow", new Color[] { hexToRGB("F5FF55"), hexToRGB("FFBB0E") });

        materialColours.Add("Orange", new Color[] { hexToRGB("FFAD4C"), hexToRGB("FF3500") });
        materialColours.Add("Teal", new Color[] { hexToRGB("4CFFE3"), hexToRGB("00B2FF") });
        materialColours.Add("Pink", new Color[] { hexToRGB("FF4CBA"), hexToRGB("FF0050") });
        materialColours.Add("Purple", new Color[] { hexToRGB("C84CFF"), hexToRGB("7D00FF") });

        materialColours.Add("Black", new Color[] { hexToRGB("000000"), hexToRGB("000000") });

        lightTextures.Add("0", Resources.Load<Texture2D>("Textures/0_light"));
        lightTextures.Add("1", Resources.Load<Texture2D>("Textures/1_light"));
        lightTextures.Add("2", Resources.Load<Texture2D>("Textures/2_light"));
        lightTextures.Add("3", Resources.Load<Texture2D>("Textures/3_light"));
        lightTextures.Add("4", Resources.Load<Texture2D>("Textures/4_light"));
        lightTextures.Add("5", Resources.Load<Texture2D>("Textures/5_light"));
        lightTextures.Add("6", Resources.Load<Texture2D>("Textures/6_light"));
        lightTextures.Add("7", Resources.Load<Texture2D>("Textures/7_light"));
        lightTextures.Add("8", Resources.Load<Texture2D>("Textures/8_light"));
        lightTextures.Add("9", Resources.Load<Texture2D>("Textures/9_light"));
        lightTextures.Add("Pauli X", Resources.Load<Texture2D>("Textures/pauli_x_light"));
        lightTextures.Add("Pauli Z", Resources.Load<Texture2D>("Textures/pauli_z_light"));

        lightTextures.Add("Entanglement", Resources.Load<Texture2D>("Textures/entanglement_light"));
        lightTextures.Add("Measurement", Resources.Load<Texture2D>("Textures/measurement_light"));
        lightTextures.Add("Colour Superposition", Resources.Load<Texture2D>("Textures/color-superposition_light"));


        darkTextures.Add("0", Resources.Load<Texture2D>("Textures/0_dark"));
        darkTextures.Add("1", Resources.Load<Texture2D>("Textures/1_dark"));
        darkTextures.Add("2", Resources.Load<Texture2D>("Textures/2_dark"));
        darkTextures.Add("3", Resources.Load<Texture2D>("Textures/3_dark"));
        darkTextures.Add("4", Resources.Load<Texture2D>("Textures/4_dark"));
        darkTextures.Add("5", Resources.Load<Texture2D>("Textures/5_dark"));
        darkTextures.Add("6", Resources.Load<Texture2D>("Textures/6_dark"));
        darkTextures.Add("7", Resources.Load<Texture2D>("Textures/7_dark"));
        darkTextures.Add("8", Resources.Load<Texture2D>("Textures/8_dark"));
        darkTextures.Add("9", Resources.Load<Texture2D>("Textures/9_dark"));
        darkTextures.Add("Pauli Y", Resources.Load<Texture2D>("Textures/pauli_y_dark"));
        darkTextures.Add("Teleportation", Resources.Load<Texture2D>("Textures/teleportation_dark"));

        darkTextures.Add("Superposition", Resources.Load<Texture2D>("Textures/superposition_dark"));
        darkTextures.Add("Measurement", Resources.Load<Texture2D>("Textures/measurement_dark"));
        darkTextures.Add("Colour Superposition", Resources.Load<Texture2D>("Textures/color-superposition_dark"));

    }

    private static Color hexToRGB(string hexValue)
    {
        hexValue = "#" + hexValue.ToUpper();
        Color color = new Color(
            int.Parse(hexValue.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) / 255f,
            int.Parse(hexValue.Substring(3, 2), System.Globalization.NumberStyles.HexNumber) / 255f,
            int.Parse(hexValue.Substring(5, 2), System.Globalization.NumberStyles.HexNumber) / 255f);
        return color;
    }

}
