using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card: MonoBehaviour
{
    Transform parent;
    public string lightSideNumber;
    public string lightSideColour;
    public string darkSideNumber;
    public string darkSideColour;
    public Animator animatorController;

    public Card(string lightSideNumber, string lightSideColour, string darkSideNumber, string darkSideColour)
    {
        this.lightSideNumber = lightSideNumber;
        this.lightSideColour = lightSideColour;
        this.darkSideNumber = darkSideNumber;
        this.darkSideColour = darkSideColour;
    }

    public MeshRenderer cardRenderer;
    //public Dictionary<string, Material> cardMaterials;
    //public Dictionary<string, Texture2D> lightTextures;
    //public Dictionary<string, Texture2D> darkTextures;
    //public Dictionary<string, Color[]> materialColours;

    private void Start()
    {
        cardRenderer = GetComponent<MeshRenderer>();
        animatorController = GetComponent<Animator>();

        //cardMaterials = new Dictionary<string, Material>();
        //ightTextures = new Dictionary<string, Texture2D>();
        //darkTextures = new Dictionary<string, Texture2D>();
        //materialColours = new Dictionary<string, Color[]>();

        // Initialize the dictionary by associating card types with materials.
        //InitializeCardMaterials();

        SetLightSideMaterial();
        SetDarkSideMaterial();
    }

    private void Update()
    {
        // Check for user input (e.g., mouse click) to flip the card.
        if (Input.GetKeyDown(KeyCode.F))
        {
           FlipCard();
        }
    }

    private void FlipCard()
    {
        Debug.Log("Light Side Up After Flip: "+ GameManager.IsLightSideUp());

        if (GameManager.IsLightSideUp())
        {
            animatorController.SetTrigger("FlipCardToDark");
        }
        else
        {
            animatorController.SetTrigger("FlipCardToLight");
        }
    }


    public void SetLightSideMaterial()
    {
        // Determine the card type based on properties like lightSideNumber and lightSideColour.
        string cardColour = $"{lightSideColour}";
        string cardNumber = $"{lightSideNumber}";

        Material[] mats = cardRenderer.materials;
        // Apply the material from the dictionary based on the card type.
        if (GameManager.materialColours.ContainsKey(cardColour))
        {
            Color[] colours = GameManager.materialColours[cardColour];
            mats[1] = GameManager.cardMaterials["FrontFace_Card"];
            mats[1].SetColor("_TopColor", colours[0]);
            mats[1].SetColor("_BottomColor", colours[1]);
            mats[1].SetColor("_BorderColor", Color.white);
            mats[1].SetTexture("_Texture", GameManager.lightTextures[cardNumber]);
            cardRenderer.materials = mats;
        }
        else
        {
            // Handle the case where the card type is not found in the dictionary.
            Debug.LogError($"Material for card type '{cardColour}' not found.");
        }
    }

    public void SetDarkSideMaterial()
    {
        /// Determine the card type based on properties like lightSideNumber and lightSideColour.
        string cardColour = $"{darkSideColour}";
        string cardNumber = $"{darkSideNumber}";

        Material[] mats = cardRenderer.materials;
        // Apply the material from the dictionary based on the card type.
        if (GameManager.materialColours.ContainsKey(cardColour))
        {
            Color[] colours = GameManager.materialColours[cardColour];
            mats[0] = GameManager.cardMaterials["BackFace_Card"];
            mats[0].SetColor("_TopColor", colours[0]);
            mats[0].SetColor("_BottomColor", colours[1]);
            mats[0].SetColor("_BorderColor", Color.black);
            mats[0].SetTexture("_Texture", GameManager.darkTextures[cardNumber]);
            cardRenderer.materials = mats;
        }
        else
        {
            // Handle the case where the card type is not found in the dictionary.
            Debug.LogError($"Material for card type '{cardColour}' not found.");
        }
    }

    public void SetCardProperties(string lightSideNumber, string lightSideColour, string darkSideNumber, string darkSideColour)
    {
        this.lightSideNumber = lightSideNumber;
        this.lightSideColour = lightSideColour;
        this.darkSideNumber = darkSideNumber;
        this.darkSideColour = darkSideColour;
    }

    public void PrintCardInfo()
    {
        Debug.Log($"Light Side: {this.lightSideNumber} {this.lightSideColour}, Dark Side: {this.darkSideNumber} {this.darkSideColour}");
    }
}
