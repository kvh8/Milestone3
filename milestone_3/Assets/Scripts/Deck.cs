using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Deck
{

    private LinkedList<Card> deck;
    private void ShuffleCards()
    {
        bool random = true;
        int randNum = UnityEngine.Random.Range(5, 11);
        int count = 0;
        for (int i = 0; i < 100; i++)
        {
            LinkedList<Card> temp = new LinkedList<Card>();
            while (deck.First != null || deck.Last != null)
            {
                temp.AddLast(deck.First);
                deck.RemoveFirst();
                count++;
                if (deck.Last != null)
                {
                    temp.AddLast(deck.Last);
                    deck.RemoveLast();
                    count++;
                    if (deck.First != null || deck.Last != null && counter % randNum == 0)
                    {
                        if (random)
                        {
                            random = false;
                            temp.AddLast(deck.Last);
                            deck.RemoveLast();
                            count++;
                        }
                        else
                        {
                            random = true;
                            temp.AddLast(deck.First);
                            deck.RemoveFirst();
                            count++;
                        }
                    }
                }
            }
            deck.Clear();
            deck = temp;
        }
    }

    public Deck(Sprite[] cardFaces, Sprite cardBack, int numDecks)
    {
        deck = new LinkedList<Card>();
        while (numDecks-- > 0)
        {
            foreach (Sprite card in cardFaces)
            {
                deck.AddLast(new Card(card, cardBack));
            }
        }
    }

    public Card DrawCard()
    {
        Card chosen = deck.First.Value;
        deck.RemoveFirst();
        return chosen;
    }


}
