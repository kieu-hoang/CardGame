using System;
using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class GameManager : NetworkBehaviour
{
    [Header("Health")]
    public int maxHealth = 30;

    [Header("Mana")]
    public int maxMana = 7;

    [Header("Hand")]
    public int handSize = 30;
    public PlayerHand playerHand;
    public PlayerHand enemyHand;

    [Header("Deck")]
    public int deckSize = 30; // Maximum deck size
    public int identicalCardCount = 2; // How many identical cards we allow to have in a deck

    [Header("Battlefield")]
    public PlayerField playerField;
    public PlayerField enemyField;

    [Header("Graveyard")] 
    public Transform graveyard;
    public Transform eGraveyard;

    [Header("Turn Management")]
    public GameObject endTurnButton;
    public GameObject enemyText;
    public TMPro.TextMeshProUGUI timerText;
    [HideInInspector] public bool isOurTurn = false;
    public int seconds = 0;
    [SyncVar, HideInInspector] public int turnCount = 1; // Start at 1
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shuffle, draw;

    // isHovering is only set to true on the Client that called the OnCardHover function.
    // We only want the hovering to appear on the enemy's Client, so we must exclude the OnCardHover caller from the Rpc call.
    [HideInInspector] public bool isHovering = false;
    [HideInInspector] public bool isHoveringField = false;
    [HideInInspector] public bool isSpawning = false;
    
    [Header("Concede Defeat")]
    //public TMPro.TextMeshProUGUI LoseText;
    //public GameObject LoseTextGameObject;

    public GameObject concedeWindow;
    //public string menu = "Menu";

    public SyncListPlayerInfo players = new SyncListPlayerInfo(); // Information of all players online. One is player, other is opponent.

    // Not sent from Player / Object with Authority, so we need to ignoreAuthority. 
    // We could also have this command run on the Player instead
    // [Command(ignoreAuthority = true)]
    // public void CmdOnCardHover(float moveBy, int index)
    // {
    //     // Only move cards if there are any in our opponent's opponent's hand (our hand from our opponent's point of view).
    //     if (enemyHand.handContent.transform.childCount > 0 && isServer) RpcCardHover(moveBy, index);
    // }

    // [ClientRpc]
    // public void RpcCardHover(float moveBy, int index)
    // {
    //     // Only move card for the player that isn't currently hovering
    //     if (!isHovering)
    //     {
    //         HandCard card = enemyHand.handContent.transform.GetChild(index).GetComponent<HandCard>();
    //         card.transform.localPosition = new Vector2(card.transform.localPosition.x, moveBy);
    //     }
    // }

    [Command(ignoreAuthority = true)]
    public void CmdOnFieldCardHover(GameObject cardObject, bool activateShine, bool targeting)
    {
        /*
        FieldCard card = cardObject.GetComponent<Card>();
        card.shine.gameObject.SetActive(true);*/
        if (isServer) RpcFieldCardHover(cardObject, activateShine, targeting);
    }

    [ClientRpc]
    public void RpcFieldCardHover(GameObject cardObject, bool activateShine, bool targeting)
    {
        if (!isHoveringField)
        {
            FieldCard card = cardObject.GetComponent<FieldCard>();
            Color shine = activateShine ? card.hoverColor : Color.clear;
            card.shine.color = targeting ? card.targetColor : shine;
            //card.shine.gameObject.SetActive(activateShine);
        }
    }

    [Command(ignoreAuthority = true)]
    public void CmdAttack(GameObject attacker)
    {
        if (isServer) RpcAttack(attacker);
    }

    [ClientRpc]
    public void RpcAttack(GameObject attacker)
    {
        if (attacker.GetComponent<Entity>() is Player)
        {
            attacker.GetComponent<Player>().portrait = Resources.Load<Sprite>("target");
        }
        else 
            attacker.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
    }
    
    [Command(ignoreAuthority = true)]
    public void CmdNormal(GameObject attacker, GameObject target)
    {
        if (isServer) RpcNormal(attacker, target);
    }

    [ClientRpc]
    public void RpcNormal(GameObject attacker, GameObject target)
    {
        attacker.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        if (target.GetComponent<Entity>() is Player)
        {
            target.GetComponent<Player>().portrait = Resources.Load<Sprite>("Trainer_6");
        }
        target.transform.localScale = new Vector3(0.8f,0.8f, 0.8f);
    }
    
    // Ends our turn and starts our opponent's turn.
    [Command(ignoreAuthority = true)]
    public void CmdEndTurn()
    {
        RpcSetTurn();
    }

    [ClientRpc]
    public void RpcSetTurn()
    {
        // If isOurTurn was true, set it false. If it was false, set it true.
        isOurTurn = !isOurTurn;
        endTurnButton.SetActive(isOurTurn);
        enemyText.SetActive(!isOurTurn);
        seconds = 30;
        // If isOurTurn (after updating the bool above)
        if (isOurTurn)
        {
            playerField.UpdateFieldCards();
            if (Player.localPlayer.cardCount < 7)
                Player.localPlayer.deck.DrawCard(1);
            Player.localPlayer.deck.CmdStartNewTurn();
            if (seconds > 0)
            {
                StartCoroutine(Timer());
            }
        }
    }
    IEnumerator Timer()
    {
        yield return new WaitForSeconds(1);
        if (seconds > 0 && isOurTurn)
        {
            seconds--;
            timerText.text = "Thời gian: " + seconds + "s";
            StartCoroutine(Timer());
        }
        else if (seconds == 0 && isOurTurn)
        {
            CmdEndTurn();
        }
    }

    public void StartGame()
    {
        endTurnButton.SetActive(true);
        Player player = Player.localPlayer;
        player.deck.CmdStartNewTurn();
        isOurTurn = true;
        seconds = 30;
        if (isOurTurn)
        {
            if (seconds > 0)
            {
                StartCoroutine(Timer());
            }
        }
    }
    public void OpenWindow()
    {
        concedeWindow.SetActive(true);
    }
    
    public void CloseWindow()
    {
        concedeWindow.SetActive(false);
    }

    public void ConcedeDefeat()
    {
        //StartCoroutine(EndGame());
        concedeWindow.SetActive(false);
        Player player = Player.localPlayer;
        if (isOurTurn)
        {
            player.GetComponent<Player>().combat.CmdChangeHealth(-30);
        }
    }

    IEnumerator EndGame()
    {
        // LoseTextGameObject.SetActive(true);
        // LoseText.text = "BẠN THUA RỒI";
        concedeWindow.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        // SceneManager.LoadScene(menu);
    }
}
