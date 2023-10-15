using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    Transform parent;
    public string lightSideNumber;
    public string lightSideColour;
    public string darkSideNumber;
    public string darkSideColour;
    public Material FrontFace_Card;
    public Material BackFace_Card;
    public Animator animatorController;

    public Card(string lightSideNumber, string lightSideColour, string darkSideNumber, string darkSideColour)
    {
        this.lightSideNumber = lightSideNumber;
        this.lightSideColour = lightSideColour;
        this.darkSideNumber = darkSideNumber;
        this.darkSideColour = darkSideColour;
    }

    public MeshRenderer cardRenderer;
    public Dictionary<string, Material> cardMaterials;

    private void Start()
    {
        Debug.Log("Initialized CardMaterials" + FrontFace_Card);
        parent = transform.parent;
        cardRenderer = GetComponent<MeshRenderer>();
        animatorController = parent.GetComponent<Animator>();

        Debug.Log("MeshRenderer Initialised: " + cardRenderer.gameObject.name);

        cardMaterials = new Dictionary<string, Material>();

        // Initialize the dictionary by associating card types with materials.
        InitializeCardMaterials();
        
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
        Debug.Log("Light Side Up After Flip: " + GameManager.IsLightSideUp());

        if (GameManager.IsLightSideUp())
        {
            animatorController.SetTrigger("FlipCardToDark");
        }
        else
        {
            animatorController.SetTrigger("FlipCardToLight");
        }

        /*
        if (transform.localRotation == Quaternion.Euler(90f, 90f, -270f))
        {
            animatorController.SetTrigger("FlipToLight");
        }
        else if(transform.localRotation == Quaternion.Euler(90f, 90f, -90f))
        {
            animatorController.SetTrigger("FlipToDark");
        }
        */

    }

    private void InitializeCardMaterials()
    {
        // Associate card colours with materials.
        cardMaterials.Add("Red", Resources.Load<Material>("Materials/LightColour/Red"));
        cardMaterials.Add("Blue", Resources.Load<Material>("Materials/LightColour/Blue"));
        cardMaterials.Add("Green", Resources.Load<Material>("Materials/LightColour/Green"));
        cardMaterials.Add("Yellow", Resources.Load<Material>("Materials/LightColour/Yellow"));

        cardMaterials.Add("Pink", Resources.Load<Material>("Materials/DarkColour/Pink"));
        cardMaterials.Add("Purple", Resources.Load<Material>("Materials/DarkColour/Purple"));
        cardMaterials.Add("Orange", Resources.Load<Material>("Materials/DarkColour/Orange"));
        cardMaterials.Add("Teal", Resources.Load<Material>("Materials/DarkColour/Teal"));

        cardMaterials.Add("Black", Resources.Load<Material>("Materials/Black"));
    }

    public void SetLightSideMaterial()
    {
        // Determine the card type based on properties like lightSideNumber and lightSideColour.
        string cardColour = $"{lightSideColour}";

        Material[] mats = cardRenderer.materials;
        // Apply the material from the dictionary based on the card type.
        if (cardMaterials.ContainsKey(cardColour))
        {
            mats[0] = cardMaterials[cardColour];
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
        // Determine the card type based on properties like darkSideNumber and darkSideColour.
        string cardColour = $"{darkSideColour}";


        Material[] mats = cardRenderer.materials;
        // Apply the material from the dictionary based on the card type.
        if (cardMaterials.ContainsKey(cardColour))
        {
            mats[1] = cardMaterials[cardColour];
            cardRenderer.materials = mats;
        }
        else
        {
            // Handle the case where the card type is not found in the dictionary.
            Debug.LogError($"Material for card type '{cardColour}' not found.");
        }
    }

    public void PrintCardInfo()
    {
        Debug.Log($"Light Side: {this.lightSideNumber} {this.lightSideColour}, Dark Side: {this.darkSideNumber} {this.darkSideColour}");
    }
}
