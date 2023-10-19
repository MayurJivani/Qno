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
    public Dictionary<string, Material> cardMaterials;
    public Dictionary<string, Texture2D> lightTextures;
    public Dictionary<string, Texture2D> darkTextures;
    public Dictionary<string, Color[]> materialColours;

    private void Start()
    {
        cardRenderer = GetComponent<MeshRenderer>();
        animatorController = GetComponent<Animator>();

        cardMaterials = new Dictionary<string, Material>();
        lightTextures = new Dictionary<string, Texture2D>();
        darkTextures = new Dictionary<string, Texture2D>();
        materialColours = new Dictionary<string, Color[]>();

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

    private void InitializeCardMaterials()
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

    private Color hexToRGB(string hexValue)
    {
        hexValue = "#" + hexValue.ToUpper();
        Color color = new Color(
            int.Parse(hexValue.Substring(1, 2), System.Globalization.NumberStyles.HexNumber) / 255f,
            int.Parse(hexValue.Substring(3, 2), System.Globalization.NumberStyles.HexNumber) / 255f,
            int.Parse(hexValue.Substring(5, 2), System.Globalization.NumberStyles.HexNumber) / 255f);
        return color;
    }

    public void SetLightSideMaterial()
    {
        // Determine the card type based on properties like lightSideNumber and lightSideColour.
        string cardColour = $"{lightSideColour}";
        string cardNumber = $"{lightSideNumber}";

        Material[] mats = cardRenderer.materials;
        // Apply the material from the dictionary based on the card type.
        if (materialColours.ContainsKey(cardColour))
        {
            Color[] colours = materialColours[cardColour];
            mats[1] = cardMaterials["FrontFace_Card"];
            mats[1].SetColor("_TopColor", colours[0]);
            mats[1].SetColor("_BottomColor", colours[1]);
            mats[1].SetColor("_BorderColor", Color.white);
            mats[1].SetTexture("_Texture", lightTextures[cardNumber]);
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
        if (materialColours.ContainsKey(cardColour))
        {
            Color[] colours = materialColours[cardColour];
            mats[0] = cardMaterials["BackFace_Card"];
            mats[0].SetColor("_TopColor", colours[0]);
            mats[0].SetColor("_BottomColor", colours[1]);
            mats[0].SetColor("_BorderColor", Color.black);
            mats[0].SetTexture("_Texture", darkTextures[cardNumber]);
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
