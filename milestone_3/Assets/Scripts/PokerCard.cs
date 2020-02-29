using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerCard : Card
{
    private string cardsuit;
    private int point;


    public string Suit { get { return cardsuit; } }
    public int Point { get { return this.point; } }


    public PokerCard(Sprite CardFace, Sprite CardBack) : base(CardFace, CardBack)
    {
        string suit = "";
        switch (Name[0])
        {
            case 'C':
                suit = "Clubs";
                break;
            case 'D':
                suit = "Diamonds";
                break;
            case 'H':
                suit = "Hearts";
                break;
            case 'S':
                suit = "Spades";
                break;
        }
        this.cardsuit = suit;

        int point;
        switch (Name.Substring(Name.Length - 1))
        {
            // ace
            case "e":
                point = 11;
                break;
            case "k":// jacK
            case "g": // kinG
            case "n": // queeN
            case "0": // 10
                point = 10;
                break;
            default:
                // other remaining possible cards, 2 - 9
                point = Convert.ToInt16(Name.Substring(Name.Length - 1));
                break;
        }
        this.point = point;
    }

}