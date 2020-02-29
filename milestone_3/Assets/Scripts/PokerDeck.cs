using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerDeck : Deck
{

    private LinkedList<PokerCard> pokerDeck;

    public void Merge(PokerDeck deckOne, PokerDeck deckTwo)
    {
        LinkedList<PokerCard> temp = new LinkedList<PokerCard>();
        while (deckOne.pokerDeck.Last != null || deckTwo.pokerDeck.Last != null)
        {
            LinkedListNode<PokerCard> tempNode = deckOne.pokerDeck.First;
            deckOne.pokerDeck.RemoveFirst();
            temp.AddLast(tempNode);

            tempNode = deckTwo.pokerDeck.First;
            deckTwo.pokerDeck.RemoveFirst();
            temp.AddLast(tempNode);
        }
        if (deckOne.pokerDeck.Last != null)
        {
            while (deckOne.pokerDeck.Last != null)
            {
                LinkedListNode<PokerCard> tempNode = deckTwo.pokerDeck.First;
                deckTwo.pokerDeck.RemoveFirst();
                temp.AddLast(tempNode);
            }
        }
        else
        {
            while (deckTwo.pokerDeck.Last != null)
            {
                LinkedListNode<PokerCard> tempNode = deckOne.pokerDeck.First;
                deckOne.pokerDeck.RemoveFirst();
                temp.AddLast(tempNode);
            }
        }
    }

    private void ShufflePokerCards()
    {
        bool random = true; //constant
        int randNum = UnityEngine.Random.Range(5, 11); //constant
        int counter = 0; //constant

        for (int i = 0; i < 100; i++)
        {
            LinkedList<PokerCard> temp = new LinkedList<PokerCard>();
            while (pokerDeck.First != null || pokerDeck.Last != null)
            {
                LinkedListNode<PokerCard> tempNode = pokerDeck.First;
                pokerDeck.RemoveFirst(); //constant
                temp.AddLast(tempNode); //constant
                counter++; //constant
                if (pokerDeck.Last != null)
                {
                    tempNode = pokerDeck.Last;
                    pokerDeck.RemoveLast(); //constant
                    temp.AddLast(tempNode); //constant
                    counter++; //constant
                    if ((pokerDeck.First != null || pokerDeck.Last != null) && counter % randNum == 0)
                    {
                        if (random)
                        {
                            random = false; //constant
                            tempNode = pokerDeck.Last;
                            pokerDeck.RemoveLast(); //constant
                            temp.AddLast(tempNode); //constant
                            counter++;  //constant
                        }
                        else
                        {
                            random = true; //constant
                            tempNode = pokerDeck.First;
                            pokerDeck.RemoveFirst(); //constant
                            temp.AddLast(tempNode); //constant
                            counter++; //constant
                        }
                        randNum = UnityEngine.Random.Range(5, 11); //constant
                    }
                    else
                    {
                        randNum = UnityEngine.Random.Range(5, 11); //constant
                    }
                }
            }
            pokerDeck = temp;
        }
    }
    public PokerDeck(Sprite[] cardFaces, Sprite cardBack, int numDecks) : base(cardFaces, cardBack, numDecks)
    {
        pokerDeck = new LinkedList<PokerCard>();
        while (numDecks-- > 0)
        {
            foreach (Sprite card in cardFaces)
            {
                pokerDeck.AddLast(new PokerCard(card, cardBack));
            }
        }
        ShufflePokerCards();
    }

    public PokerCard DrawPokerCardTop()
    {
        PokerCard chosen = pokerDeck.First.Value;
        pokerDeck.RemoveFirst();
        return chosen;
    }

    public PokerCard DrawPokerCardBottom()
    {
        PokerCard chosen = pokerDeck.Last.Value;
        pokerDeck.RemoveLast();
        return chosen;
    }

    public void AddTop(PokerCard card)
    {
        pokerDeck.AddFirst(card);
    }

    public void AddBottom(PokerCard card)
    {
        pokerDeck.AddLast(card);
    }


    //public PokerCard AddTop(Sprite cardFace, Sprite cardBack)
    //{
    //    PokerCard chosen = new PokerCard(cardFace, cardBack);
    //    pokerDeck.AddFirst(chosen);
    //    return chosen;
    //}

    //public PokerCard AddBottom(Sprite cardFace, Sprite cardBack)
    //{
    //    PokerCard chosen = new PokerCard(cardFace, cardBack);
    //    pokerDeck.AddLast(chosen);
    //    return chosen;
    //}


}
