using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CardDatabase))]
public class CardDeckGenerator : MonoBehaviour
{
    CardDatabase cardDatabase;
    private List<List<string>> lightSideCards = new List<List<string>>();
    private List<List<string>> darkSideCards = new List<List<string>>();
    private static List<Card> deck = new List<Card>();
    private void Awake()
    {
        Debug.Log("CardDeckGenerator Initialized");
        cardDatabase = GetComponent<CardDatabase>();
        (lightSideCards, darkSideCards) = cardDatabase.GenerateCardDatabase();
    }


    private void Start()
    {
        GenerateFullDeckRandomly();
        PrintDeck();
    }

    public void GenerateFullDeckRandomly()
    {
        // Shuffle the lists of light and dark card faces.
        Shuffle(lightSideCards);
        Shuffle(darkSideCards);

        int minCount = Mathf.Min(lightSideCards.Count, darkSideCards.Count);

        for (int i = 0; i < minCount; i++)
        {
            List<string> lightCardFace = lightSideCards[i];
            List<string> darkCardFace = darkSideCards[i];

            Card card = new Card(
                lightCardFace[1], lightCardFace[0],
                darkCardFace[1], darkCardFace[0]
            );
            deck.Add(card);
        }
        Shuffle(deck);
    }

    public void PrintDeck()
    {
        // Log the created card.
        for(int i=0; i<deck.Count; i++)
        {
            Debug.Log($"Created Card --> Card No : {i + 1}, Light Side: {deck[i].lightSideNumber} {deck[i].lightSideColour}, Dark Side: {deck[i].darkSideNumber} {deck[i].darkSideColour}");
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        for (int i = 0; i < n; i++)
        {
            int randomIndex = Random.Range(i, n);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    public static List<Card> getDeck()
    {
        return deck;
    }
}
