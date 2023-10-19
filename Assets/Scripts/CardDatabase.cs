using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<List<string>> lightSideCards = new List<List<string>>();
    public static List<List<string>> darkSideCards = new List<List<string>>();

    private static string[] lightColours = { "Red", "Blue", "Green", "Yellow" };
    private static string[] lightColourActionCards = { "Pauli X", "Pauli Z" };
    private static string[] darkColours = { "Orange", "Purple", "Teal", "Pink" };
    private static string[] darkColourActionCards = { "Pauli Y", "Teleportation" };

    private void Awake()
    {
        Debug.Log("CardDatabase Initialized");
        GenerateLightSideList();
        GenerateDarkSideList();
    }

    private void AddCard(List<List<string>> cardList, string colour, string number)
    {
        List<string> card = new List<string> {colour, number};
        cardList.Add(card);
    }

    public void GenerateLightSideList()
    {
        lightSideCards.Clear();

        //Generating cards 0-9
        for (int i = 0; i <= 9; i++)
        {
            for (int j = 0; j < lightColours.Length; j++)
            {
                AddCard(lightSideCards, lightColours[j], i.ToString());

                if (i != 0)
                {
                    AddCard(lightSideCards, lightColours[j], i.ToString());
                }
            }
        }

        //Generating light coloured action cards
        for (int i = 0; i < lightColourActionCards.Length; i++)
        {
            for (int j = 0; j < lightColours.Length; j++)
            {
                AddCard(lightSideCards, lightColours[j], lightColourActionCards[i]);
                AddCard(lightSideCards, lightColours[j], lightColourActionCards[i]);
            }
        }

        //Generating wild action cards (black coloured)
        for (int i = 0; i < 8; i++)
        {
            AddCard(lightSideCards, "Black", "Entanglement");
            if(2 * i < 8)
            {
                AddCard(lightSideCards, "Black", "Measurement");
                AddCard(lightSideCards, "Black", "Colour Superposition");
            }
        }
    }

    public void GenerateDarkSideList()
    {
        darkSideCards.Clear();

        //Generating cards 0-9
        for (int i = 0; i <= 9; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                AddCard(darkSideCards, darkColours[j], i.ToString());

                if (i != 0)
                {
                    AddCard(darkSideCards, darkColours[j], i.ToString());
                }
            }
        }

        //Generating dark coloured action cards
        for (int i = 0; i < darkColourActionCards.Length; i++)
        {
            for (int j = 0; j < darkColours.Length; j++)
            {
                AddCard(darkSideCards, darkColours[j], darkColourActionCards[i]);
                AddCard(darkSideCards, darkColours[j], darkColourActionCards[i]);
            }
        }

        //Generating wild action cards (black coloured)
        for (int i = 0; i < 8; i++)
        {
            AddCard(darkSideCards, "Black", "Superposition");
            if (2 * i < 8)
            {
                AddCard(darkSideCards, "Black", "Measurement");
                AddCard(darkSideCards, "Black", "Colour Superposition");
            }
        }
    }

    public (List<List<string>> lightCards, List<List<string>> darkCards) GenerateCardDatabase()
    {
        List<List<string>> lightCards = new List<List<string>>();
        List<List<string>> darkCards = new List<List<string>>();

        lightCards.AddRange(lightSideCards);
        darkCards.AddRange(darkSideCards);

        Debug.Log("Light Card Count: " + lightSideCards.Count);
        Debug.Log("Dark Card Count: " + darkSideCards.Count);

        return (lightCards, darkCards);
    }
}
