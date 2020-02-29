using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Sprite[] CardFace;
    [SerializeField]
    private Sprite[] CardBack;
    [SerializeField]
    private GameObject[] playerCardPosition, dealerCardPosition;
    [SerializeField]
    private GameObject blankCard;
    [SerializeField]
    private Button hitDeal, stand, resetBet, resetBalance, betFiveHundred, betOneHundred, betFifty, deckAdd, deckSub, deckComplete;
    [SerializeField]
    private Text textMoney, textBet, textPlayerPoints, textDealerPoints, textPlaceYourBet, textSelectingBet, textWinner, textNumDecks;
    [SerializeField]
    private Image resetImgBtn;

    private LinkedList<PokerCard> playerCards;
    private LinkedList<PokerCard> dealerCards;
    private bool isPlaying;
    private int playerPoints;
    private int actualDealerPoints, displayDealerPoints;
    private int playerMoney;
    private int currentBet;
    private int playerCardPointer, dealerCardPointer;
    private int numDecks; 
    private string cardImage;
    private string BackOfCard;
    private PokerDeck playingDeck;

    // Start is called before the first frame update
    void Start()
    {
        beforePlay();
        playerMoney = 1000;
        currentBet = 0;
        resetGame();

        hitDeal.onClick.AddListener(delegate
        {
            if (isPlaying)
            {
                playerDrawCard();
            }
            else
            {
                startGame();
            }
        });

        stand.onClick.AddListener(delegate
        {
            playerEndTurn();
        });

        updateCurrentBet();

        resetBalance.onClick.AddListener(delegate
        {
            playerMoney = 1000;
        });

        resetBet.onClick.AddListener(delegate
        {
            currentBet = 0;
            textSelectingBet.text = "$" + currentBet.ToString();

        });
    }

    void Update()
    {
        textMoney.text = "$" + playerMoney.ToString();
    }

    public void startGame()
    {
        if (playerMoney > 0)
        {
            playerMoney -= currentBet;
            if (playerMoney < 0)
            {
                playerMoney += currentBet;

                return;
            }

            isPlaying = true;

            // Update UI 

            textSelectingBet.gameObject.SetActive(false);
            textPlaceYourBet.gameObject.SetActive(false);
            hitDeal.GetComponentInChildren<Text>().text = "HIT";
            stand.gameObject.SetActive(true);
            textBet.text = "Bet: $" + currentBet.ToString();
            resetBalance.gameObject.SetActive(false);
            resetBet.gameObject.SetActive(false);

            // assign the playing deck with 2 deck of cards
            playingDeck = new PokerDeck(CardFace, CardBack, numDecks);

            // draw 1 card for player then 1 for dealer
            playerDrawCard();
            dealerDrawCard();
            // draw 1 card for player then 1 for dealer
            playerDrawCard();
            dealerDrawCard();
            updatePlayerPoints();
            updateDealerPoints(true);

            checkIfPlayerBlackjack();
        }
    }

    private void checkIfPlayerBlackjack()
    {
        if (playerPoints == 21)
        {
            playerBlackjack();
        }
    }

    public void endGame()
    {
        hitDeal.gameObject.SetActive(false);
        stand.gameObject.SetActive(false);
        betFiveHundred.gameObject.SetActive(false);
        betOneHundred.gameObject.SetActive(false);
        betFifty.gameObject.SetActive(false);
        textPlaceYourBet.text = "";
        textSelectingBet.text = "";

        resetImgBtn.gameObject.SetActive(true);
        resetImgBtn.GetComponent<Button>().onClick.AddListener(delegate {
            resetGame();
        });
    }

    public void dealerDrawCard()
    {
        Debug.Log("Starts Dealer Draw");
        PokerCard drawnCard = playingDeck.DrawPokerCardTop();
        GameObject cardFace;
        dealerCards.AddFirst(drawnCard);
        if (dealerCardPointer <= 0)
        {
            Debug.Log("Dealer Draws First Card");
            drawnCard.isFaceUp = false;
            drawnCard.makeCard(blankCard);
        }
        else
        {
            Debug.Log("Dealer Draws Next First Card");
            drawnCard.makeCard(blankCard);
        }
        cardFace = drawnCard.Face;
        Instantiate(cardFace, dealerCardPosition[dealerCardPointer++].transform);
        updateDealerPoints(false);
    }  //edited for m3

    public void playerDrawCard()
    {
        PokerCard drawnCard = playingDeck.DrawPokerCardTop();
        drawnCard.makeCard(blankCard);
        playerCards.AddFirst(drawnCard);
        Instantiate(drawnCard.Face, playerCardPosition[playerCardPointer++].transform);
        updatePlayerPoints();
        if (playerPoints > 21)
        {
            playerBusted();
        }
            
    } //edited for m3

    private void playerEndTurn()
    {
        Debug.Log("Player End Turn");
        revealDealersDownFacingCard();

        // dealer start drawing
        while (actualDealerPoints < 17 && actualDealerPoints < playerPoints)
        {
            Debug.Log("Dealer Draw Card");
            dealerDrawCard();
        }
        updateDealerPoints(false);
        if (actualDealerPoints > 21)
        {
            Debug.Log("Player Wins Dealer Busted");
            dealerBusted();
        }
            
        else if (actualDealerPoints > playerPoints)
        {
            Debug.Log("Dealer Wins Player Loses");
            dealerWin(false);
        }
            
        else if (actualDealerPoints == playerPoints)
        {
            Debug.Log("Draw");
            gameDraw();
        }
            
        else
        {
            Debug.Log("Player wins Dealer Loses");
            playerWin(false);
        }
            
    }  //edited for m3

    private void revealDealersDownFacingCard()
    {
        // reveal the dealer's down-facing card
        Destroy(dealerCardPosition[0].transform.GetChild(0).gameObject);
        dealerCards.Last.Value.isFaceUp = true;
        dealerCards.Last.Value.makeCard(blankCard);
        Instantiate(dealerCards.Last.Value.Face, dealerCardPosition[0].transform);
    }  //edited for m3

    private void updatePlayerPoints()
    {
        playerPoints = 0;
        foreach (PokerCard c in playerCards)
        {
            playerPoints += c.Point;
        }

        // transform ace to 1 if there is any
        if (playerPoints > 21)
        {
            playerPoints = 0;
            foreach (PokerCard c in playerCards)
            {
                if (c.Point == 11)
                {
                    playerPoints += 1;
                }
                    
                else
                {
                    playerPoints += c.Point;
                }   
            }
        }

        textPlayerPoints.text = playerPoints.ToString();
    }  //edited for m3

    private void updateDealerPoints(bool hideFirstCard)
    {
        Debug.Log("Start Dealer Points Update");
        actualDealerPoints = 0;
        foreach (PokerCard c in dealerCards)
        {
            actualDealerPoints += c.Point;
        }

        // transform ace to 1 if there is any
        if (actualDealerPoints > 21)
        {
            actualDealerPoints = 0;
            foreach (PokerCard c in dealerCards)
            {
                if (c.Point == 11)
                {
                    actualDealerPoints += 1;
                } 
                else
                {
                    actualDealerPoints += c.Point;
                } 
            }
        }

        if (hideFirstCard)
        {
            displayDealerPoints = dealerCards.Last.Previous.Value.Point;
        }  
        else
        {
            displayDealerPoints = actualDealerPoints;
        }
            
        textDealerPoints.text = displayDealerPoints.ToString();
    }  //edited for m3

    private void updateCurrentBet()
    {
        betFiveHundred.onClick.AddListener(delegate
        {
            currentBet += 500;
            textSelectingBet.text = "$" + currentBet.ToString();
        });

        betOneHundred.onClick.AddListener(delegate
        {
            currentBet += 100;
            textSelectingBet.text = "$" + currentBet.ToString();
        });

        betFifty.onClick.AddListener(delegate
        {
            currentBet += 50;
            textSelectingBet.text = "$" + currentBet.ToString();
        });
    }

    private void playerBusted()
    {
        dealerWin(true);
    }

    private void dealerBusted()
    {
        playerWin(true);
    }

    private void playerBlackjack()
    {
        textWinner.text = "Blackjack!";
        playerMoney += currentBet * 2;
        endGame();
    }

    private void playerWin(bool winByBust)
    {
        if (winByBust)
        {
            textWinner.text = "Dealer Busted\nPlayer Wins!";
        }
        else
        {
            textWinner.text = "Player Wins!";
        }
            
        playerMoney += currentBet * 2;
        endGame();
    }


    private void dealerWin(bool winByBust)
    {
        if (winByBust)
        {
            textWinner.text = "Player Busted\nDealer Wins";
        }   
        else
        {
            textWinner.text = "Dealer Wins";
        }
        endGame();
    }

    private void gameDraw()
    {
        textWinner.text = "Draw";
        playerMoney += currentBet;
        endGame();
    }

    private void beforePlay()       //added code
    {
        deckAdd.onClick.AddListener(delegate
        {
            numDecks += 1;
            textNumDecks.text = "Number of Decks:\n" + numDecks;
        });
        deckSub.onClick.AddListener(delegate
        {
            numDecks -= 1;
            if (numDecks < 1)
            {
                numDecks = 1;
                textNumDecks.text = "Number of Decks:\n" + numDecks;
            }
            else
            {
                textNumDecks.text = "Number of Decks:\n" + numDecks;
            }

        });
        deckComplete.GetComponent<Button>().onClick.AddListener(delegate
        {
            deckSub.gameObject.SetActive(false);
            deckAdd.gameObject.SetActive(false);
            textNumDecks.gameObject.SetActive(false);
            deckComplete.gameObject.SetActive(false);
            resetGame();
        });

    } //dont have to change for m3

    private void resetGame()
    {
        isPlaying = false;


        // reset points
        playerPoints = 0;
        actualDealerPoints = 0;
        playerCardPointer = 0;
        dealerCardPointer = 0;
        currentBet = 0;

        // reset cards
        playingDeck = new PokerDeck(CardFace, CardBack, numDecks);
        playerCards = new LinkedList<PokerCard>();
        dealerCards = new LinkedList<PokerCard>();

        // reset UI
        hitDeal.gameObject.SetActive(true);
        hitDeal.GetComponentInChildren<Text>().text = "DEAL";
        stand.gameObject.SetActive(false);
        betFiveHundred.gameObject.SetActive(true);
        betOneHundred.gameObject.SetActive(true);
        betFifty.gameObject.SetActive(true);
        textSelectingBet.gameObject.SetActive(true);
        textSelectingBet.text = "$" + currentBet.ToString();
        textPlaceYourBet.gameObject.SetActive(true);
        textPlayerPoints.text = "";
        textDealerPoints.text = "";
        textBet.text = "";
        textWinner.text = "";
        resetImgBtn.gameObject.SetActive(false);
        resetBalance.gameObject.SetActive(true);
        resetBet.gameObject.SetActive(true);

        // clear cards on table
        clearCards();
    }

    private void clearCards()
    {
        foreach (GameObject g in playerCardPosition)
        {
            if (g.transform.childCount > 0)
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    Destroy(g.transform.GetChild(i).gameObject);
                }
        }
        foreach (GameObject g in dealerCardPosition)
        {
            if (g.transform.childCount > 0)
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    Destroy(g.transform.GetChild(i).gameObject);
                }
        }
    }

    private PokerCard randomPokerCard()
    {
        Sprite randSprite = CardFace[UnityEngine.Random.Range(0, 52)];
        PokerCard card = new PokerCard(randSprite, CardBack);
        return card;
    }


}