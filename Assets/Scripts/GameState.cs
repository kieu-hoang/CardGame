using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using System.Threading;
using UnityEditor;

[System.Serializable]
public class GameState
{
    public List<ThisCard1> deck = new List<ThisCard1>();
    public List<ThisCard1> cardsInHand = new List<ThisCard1>();
    public List<ThisCard1> cardsInZone = new List<ThisCard1>();
    public int playerHp;
    public int playerMana;
    
    public List<AICardToHand1> AIdeck = new List<AICardToHand1>();
    public List<AICardToHand1> cardsInHandAI = new List<AICardToHand1>();
    public List<AICardToHand1> cardsInZoneAI = new List<AICardToHand1>();
    public int aiHp;
    public int aiMana;

    public int playerManaTurn;
    public int aiManaTurn;
    public bool playerTurn;

    public void copy(GameState newGame)
    {
        for (int i = 0; i < newGame.deck.Count; i++)
        {
            deck.Add(newGame.deck[i]);
        }

        for (int i = 0; i < newGame.cardsInHand.Count; i++)
        {
            cardsInHand.Add(newGame.cardsInHand[i]);
        }

        for (int i = 0; i < newGame.cardsInZone.Count; i++)
        {
            cardsInZone.Add(newGame.cardsInZone[i]);
        }
        
        playerHp = newGame.playerHp;
        playerMana = newGame.playerMana;

        for (int i = 0; i < newGame.AIdeck.Count; i++)
        {
            AIdeck.Add(newGame.AIdeck[i]);
        }

        for (int i = 0; i < newGame.cardsInHandAI.Count; i++)
        {
            cardsInHandAI.Add(newGame.cardsInHandAI[i]);
        }

        for (int i = 0; i < newGame.cardsInZoneAI.Count; i++)
        {
            cardsInZoneAI.Add(newGame.cardsInZoneAI[i]);
        }
        aiHp = newGame.aiHp;
        aiMana = newGame.aiMana;
        playerManaTurn = newGame.playerManaTurn;
        aiManaTurn = newGame.aiManaTurn;
        playerTurn = newGame.playerTurn;
    }

    public List<List<Move>> getValidMoves()
    {
        List<List<Move>> result = new List<List<Move>>();
        List<Move> validMove = new List<Move>();
        // Debug.Log("result at first: " + result.Count);
        if (playerTurn)
        {
            // Debug.Log("cardsInHand at validMove: " + cardsInHand.Count);
            // Debug.Log("cardsInZone at validMove: " + cardsInZone.Count);
            FindCombinations(cardsInHand, 0, playerMana, validMove, result, cardsInZone.Count);
            result.Add(new List<Move>());
            // Debug.Log("Result After Summon: " + result.Count);
            // ----------------------- Thread -----------------------
            // for (int a=0; a < result.Count;a++)
            // {
            //     int index = a;
            //     // Debug.Log("Result thread " + index + " before: " + result[index].Count);
            //     Thread newthrd = new Thread(() =>
            //     {
            //         GameState nw = new GameState();
            //         nw.copy(this);
            //         nw.make_move(result[index]);
            //         List<ThisCard1> atkcard = new List<ThisCard1>();
            //         for (int i = 0; i < nw.cardsInZone.Count; i++)
            //         {
            //             if (nw.cardsInZone[i].canAttack)
            //             {
            //                 // Debug.Log("cardsInZone[] " + i + ": " + nw.cardsInZone[i].id);
            //                 atkcard.Add(nw.cardsInZone[i]);
            //             }
            //         }
            //         // Debug.Log("atkCount: " + atkcard.Count);
            //         List<Move> validMove2 = new List<Move>();
            //         List<AICardToHand1> taunt = new List<AICardToHand1>();
            //         List<AICardToHand1> notTaunt = new List<AICardToHand1>();
            //         for (int i = 0; i < nw.cardsInZoneAI.Count; i++)
            //         {
            //             if (nw.cardsInZoneAI[i].id == 1 || nw.cardsInZoneAI[i].id == 13 || nw.cardsInZoneAI[i].id == 19)
            //             {
            //                 taunt.Add(nw.cardsInZoneAI[i]);
            //             }
            //             else
            //             {
            //                 notTaunt.Add(nw.cardsInZoneAI[i]);
            //             }
            //         }
            //         for (int i = 0, j=0, k=0; i < atkcard.Count; i++)
            //         {
            //             if (j<taunt.Count)
            //             {
            //                 validMove2.Add(new Move(true, atkcard[i].id, false, true, taunt[j].id));
            //                 playerAttack(atkcard[i], taunt[j]);
            //                 // taunt[j].hurted += atkcard[i].actualDame;
            //                 if (taunt[j].blood - taunt[j].hurted <= 0)
            //                     j++;
            //             }
            //             else if (j >= taunt.Count && k < notTaunt.Count)
            //             {
            //                 validMove2.Add(new Move(true, atkcard[i].id, false, true, notTaunt[k].id));
            //                 // notTaunt[k].hurted += atkcard[i].actualDame;
            //                 playerAttack(atkcard[i], notTaunt[k]);
            //                 if (notTaunt[k].blood - notTaunt[k].hurted <= 0)
            //                     k++;
            //             }
            //             else if (k >= notTaunt.Count)
            //             {
            //                 validMove2.Add(new Move(true, atkcard[i].id, false, true, 0));
            //             }
            //         }
            //         result[index].AddRange(validMove2);
            //     });
            //     newthrd.Start();
            // }
            // ----------------------- Thread -----------------------
            // One thread with loop -----------------------------
            for (int a=0; a < result.Count;a++)
            {
                int index = a;
                // Debug.Log("Result thread " + index + " before: " + result[index].Count);
                GameState nw = new GameState();
                nw.copy(this);
                nw.make_move(result[index]);
                List<ThisCard1> atkcard = new List<ThisCard1>();
                for (int i = 0; i < nw.cardsInZone.Count; i++)
                {
                    if (nw.cardsInZone[i].canAttack)
                    {
                        // Debug.Log("cardsInZone[] " + i + ": " + nw.cardsInZone[i].id);
                        atkcard.Add(nw.cardsInZone[i]);
                    }
                }
                // Debug.Log("atkCount: " + atkcard.Count);
                List<Move> validMove2 = new List<Move>();
                List<AICardToHand1> taunt = new List<AICardToHand1>();
                List<AICardToHand1> notTaunt = new List<AICardToHand1>();
                for (int i = 0; i < nw.cardsInZoneAI.Count; i++)
                {
                    if (nw.cardsInZoneAI[i].id == 1 || nw.cardsInZoneAI[i].id == 13 || nw.cardsInZoneAI[i].id == 19)
                    {
                        taunt.Add(nw.cardsInZoneAI[i]);
                    }
                    else
                    {
                        notTaunt.Add(nw.cardsInZoneAI[i]);
                    }
                }

                int tauntCount = taunt.Count;
                int notTauntCount = notTaunt.Count;
                for (int i = 0, j=0, k=0; i < atkcard.Count; i++)
                {
                    if (j<tauntCount)
                    {
                        validMove2.Add(new Move(true, atkcard[i].id, false, true, taunt[j].id));
                        playerAttack(atkcard[i], taunt[j]);
                        if (taunt[j].blood - taunt[j].hurted <= 0)
                            j++;
                    }
                    else if (j >= tauntCount && k < notTauntCount)
                    {
                        validMove2.Add(new Move(true, atkcard[i].id, false, true, notTaunt[k].id));
                        playerAttack(atkcard[i], notTaunt[k]);
                        if (notTaunt[k].blood - notTaunt[k].hurted <= 0)
                            k++;
                    }
                    else if (j >= tauntCount && k >= notTauntCount)
                    {
                        validMove2.Add(new Move(true, atkcard[i].id, false, true, 0));
                    }
                }
                result[index].AddRange(validMove2);
                // Debug.Log("Thread " + index + " Result increase to: " + result[index].Count);
            }
            // One thread with loop
        }
        else
        {
            FindCombinationsAI(cardsInHandAI, 0, aiMana, validMove, result, cardsInZoneAI.Count);
            result.Add(new List<Move>());
            // Debug.Log("Result After Summon: " + result.Count);
            // ----------------------- Thread -----------------------
            // for (int a=0; a< result.Count;a++)
            // {
            //     int index = a;
            //     // Debug.Log("Result thread " + index + " before: " + result[index].Count);
            //     Thread newthrd = new Thread(() =>
            //     {
            //         GameState nw = new GameState();
            //         nw.copy(this);
            //         for (int i = 0; i < nw.cardsInZoneAI.Count; i++)
            //         {
            //             Debug.Log("this.cardsInZoneAI id: "  + cardsInZoneAI[i].id);
            //             Debug.Log("nw.cardsInZoneAI id: " + nw.cardsInZoneAI[i].id);
            //         }
            //         for (int i = 0; i < nw.cardsInHandAI.Count; i++)
            //         {
            //             Debug.Log("this.cardsInHandAI id: "  + cardsInHandAI[i].id);
            //             Debug.Log("nw.cardsInHandAI id: " + nw.cardsInHandAI[i].id);
            //         }
            //         nw.make_move(result[index]);
            //         List<AICardToHand1> atkcard = new List<AICardToHand1>();
            //         for (int i = 0; i < nw.cardsInZoneAI.Count; i++)
            //         {
            //             if (nw.cardsInZoneAI[i].canAttack)
            //             {
            //                 atkcard.Add(nw.cardsInZoneAI[i]);
            //             }
            //         }
            //         // Debug.Log("atkCount: " + atkcard.Count);
            //         List<Move> validMove2 = new List<Move>();
            //         List<ThisCard1> taunt = new List<ThisCard1>();
            //         List<ThisCard1> notTaunt = new List<ThisCard1>();
            //
            //         for (int i = 0; i < nw.cardsInZone.Count; i++)
            //         {
            //             if (nw.cardsInZone[i].id == 1 || nw.cardsInZone[i].id == 13 || nw.cardsInZone[i].id == 19)
            //             {
            //                 taunt.Add(nw.cardsInZone[i]);
            //                 // Debug.Log("taunt id: " + taunt[taunt.Count-1].id);
            //             }
            //             else
            //             {
            //                 notTaunt.Add(nw.cardsInZone[i]);
            //                 // Debug.Log("nottaunt id: " + notTaunt[notTaunt.Count-1].id);
            //             }
            //         }
            //         Debug.Log("notTaunt Count: " + notTaunt.Count);
            //         for (int i = 0, j=0, k=0; i < atkcard.Count; i++)
            //         {
            //             if (j<taunt.Count)
            //             {
            //                 validMove2.Add(new Move(false, atkcard[i].id, false, true, taunt[j].id));
            //                 // taunt[j].hurted += atkcard[i].actualDame;
            //                 aiAttack(atkcard[i], taunt[j]);
            //                 if (taunt[j].blood - taunt[j].hurted <= 0)
            //                     j++;
            //             }
            //             else if (j >= taunt.Count && k < notTaunt.Count)
            //             {
            //                 validMove2.Add(new Move(false, atkcard[i].id, false, true, notTaunt[k].id));
            //                 // notTaunt[k].hurted += atkcard[i].actualDame;
            //                 aiAttack(atkcard[i], notTaunt[k]);
            //                 if (notTaunt[k].blood - notTaunt[k].hurted <= 0)
            //                     k++;
            //             }
            //             else if (k >= notTaunt.Count)
            //             {
            //                 validMove2.Add(new Move(false, atkcard[i].id, false, true, 0));
            //             }
            //         }
            //         result[index].AddRange(validMove2);
            //     });
            //     newthrd.Start();
            //     // Debug.Log("Thread " + index + " Result increase to: " + result[index].Count);
            // }
            // ----------------------- Thread -----------------------
            // One thread with loop --------------------------------
            for (int a=0; a< result.Count;a++)
            {
                int index = a;
                // Debug.Log("Result thread " + index + " before: " + result[index].Count);
                GameState nw = new GameState();
                nw.copy(this);
                nw.make_move(result[index]);
                List<AICardToHand1> atkcard = new List<AICardToHand1>();
                for (int i = 0; i < nw.cardsInZoneAI.Count; i++)
                {
                    if (nw.cardsInZoneAI[i].canAttack)
                    {
                        atkcard.Add(nw.cardsInZoneAI[i]);
                    }
                }
                // Debug.Log("atkCount: " + atkcard.Count);
                List<Move> validMove2 = new List<Move>();
                List<ThisCard1> taunt = new List<ThisCard1>();
                List<ThisCard1> notTaunt = new List<ThisCard1>();
            
                for (int i = 0; i < nw.cardsInZone.Count; i++)
                {
                    if (nw.cardsInZone[i].id == 1 || nw.cardsInZone[i].id == 13 || nw.cardsInZone[i].id == 19)
                    {
                        taunt.Add(nw.cardsInZone[i]);
                        // Debug.Log("taunt id: " + taunt[taunt.Count-1].id);
                    }
                    else
                    {
                        notTaunt.Add(nw.cardsInZone[i]);
                        // Debug.Log("nottaunt id: " + notTaunt[notTaunt.Count-1].id);
                    }
                }
                int tauntCount = taunt.Count;
                int notTauntCount = notTaunt.Count;
                for (int i = 0, j = 0, k = 0; i < atkcard.Count; i++)
                {
                    if (j < tauntCount)
                    {
                        validMove2.Add(new Move(false, atkcard[i].id, false, true, taunt[j].id));
                        aiAttack(atkcard[i], taunt[j]);
                        if (taunt[j].blood - taunt[j].hurted <= 0)
                            j++;
                    }
                    else if (j >= tauntCount && k < notTauntCount)
                    {
                        validMove2.Add(new Move(false, atkcard[i].id, false, true, notTaunt[k].id));
                        aiAttack(atkcard[i], notTaunt[k]);
                        if (notTaunt[k].blood - notTaunt[k].hurted <= 0)
                            k++;
                    }
                    else if (j >=tauntCount && k >= notTauntCount)
                    {
                        validMove2.Add(new Move(false, atkcard[i].id, false, true, 0));
                    }
                }
                result[index].AddRange(validMove2);
                // Debug.Log("Thread " + index + " Result increase to: " + result[index].Count);
            }
        }
        return result;
    }
    static void FindCombinations(List<ThisCard1> cardsInHand, int index, int mana, List<Move> currentCombination, List<List<Move>> result, int count)
    {
        // Debug.Log("currentCombination at first: " + currentCombination.Count);
        // Debug.Log("cardsInHand at first: " + cardsInHand.Count);
        if (mana <= 0)
        {
            return;
        }
        
        if (index == cardsInHand.Count)
        {
            return;
        }
        
        // Include the current element and explore further
        if (mana >= cardsInHand[index].mana && currentCombination.Count + count < 5)
        {
            currentCombination.Add(new Move(true, cardsInHand[index].id, true, false, 0));
            if (!(result.Contains(currentCombination)))
                result.Add(new List<Move>(currentCombination));
            // Debug.Log("Result#: " + result.Count);
            FindCombinations(cardsInHand, index + 1, mana - cardsInHand[index].mana, currentCombination, result, count);
            currentCombination.RemoveAt(currentCombination.Count - 1); // Backtrack

        }

        // Exclude the current element and explore further
        FindCombinations(cardsInHand, index + 1, mana, currentCombination, result, count);
    }
    
    static void FindCombinationsAI(List<AICardToHand1> cardsInHand, int index, int mana, List<Move> currentCombination, List<List<Move>> result, int count)
    {
        // Debug.Log("currentCombinationAI at first: " + currentCombination.Count);
        // Debug.Log("cardsInHandAI at first: " + cardsInHand.Count);
        // Debug.Log("manaAI: " + mana);
        if (mana <= 0)
        {
            return;
        }

        if (index == cardsInHand.Count)
        {
            return;
        }
        
        // Include the current element and explore further
        if (mana >= cardsInHand[index].mana && currentCombination.Count + count < 5)
        {
            currentCombination.Add(new Move(false, cardsInHand[index].id, true, false, 0));
            if (!(result.Contains(currentCombination)))
                result.Add(new List<Move>(currentCombination));
            // Debug.Log("Result#: " + result.Count);
            FindCombinationsAI(cardsInHand, index + 1, mana - cardsInHand[index].mana, currentCombination, result, count);
            currentCombination.RemoveAt(currentCombination.Count - 1); // Backtrack
        }

        // Exclude the current element and explore further
        FindCombinationsAI(cardsInHand, index + 1, mana, currentCombination, result, count);
    }

    public bool checkTaunt()
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            if (cardsInZone[i].id == 1 || cardsInZone[i].id == 13 || cardsInZone[i].id == 19)
                return true;
        }

        return false;
    }
    public bool checkTauntAI()
    {
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            if (cardsInZoneAI[i].id == 1 || cardsInZoneAI[i].id == 13 || cardsInZoneAI[i].id == 19)
                return true;
        }

        return false;
    }

    public bool checkGameOver()
    {
        return playerHp <= 0 || aiHp <= 0;
    }

    public int evaluate()
    {
        int value = 0;
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            value -= (cardsInZone[i].dame + cardsInZone[i].blood + cardsInZone[i].dameIncrease - cardsInZone[i].hurted);
        }
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            value += (cardsInZoneAI[i].dame + cardsInZoneAI[i].blood + cardsInZoneAI[i].dameIncrease - cardsInZoneAI[i].hurted);
        }
    
        value += aiHp;
        value -= playerHp;
        return value;
    }
    // public int evaluate()
    // {
    //     return aiHp - playerHp;
    // }

    public void aiSummon(int id)
    {
        for (int i = 0; i < cardsInHandAI.Count; i++)
        {
            if (cardsInHandAI[i].id == id)
            {
                aiMana -= cardsInHandAI[i].mana;
                //After adding to ZoneList, add effects...
                if (cardsInHandAI[i].drawXcards > 0 && !cardsInHandAI[i].deathcrys)
                {
                    for (int k = 0; k < cardsInHandAI[i].drawXcards; k++)
                    {
                        if (cardsInHandAI.Count < 7 && AIdeck.Count > 0)
                        {
                            int inx = AIdeck.Count - 1;
                            cardsInHandAI.Add(AIdeck[inx]);
                            AIdeck.RemoveAt(inx);
                        }
                    }
                }

                if (cardsInHandAI[i].healXpower > 0 && !cardsInHandAI[i].deathcrys)
                {
                    if (cardsInHandAI[i].id == 2 || cardsInHandAI[i].id == 3)
                        HealOneAI(cardsInHandAI[i].healXpower);
                    else if (cardsInHandAI[i].id == 20 || cardsInHandAI[i].id == 24 || cardsInHandAI[i].id == 21 || cardsInHandAI[i].id == 11 
                             || cardsInHandAI[i].id == 16 || cardsInHandAI[i].id == 12 || cardsInHandAI[i].id == 23)
                        HealAllAI(cardsInHandAI[i].healXpower);
                    else if (cardsInHandAI[i].id == 10)
                    {
                        HealOneAI(cardsInHandAI[i].healXpower);
                        HealHeroAI(cardsInHandAI[i].healXpower-1);
                    }
                }

                if (cardsInHandAI[i].damageDealtBySpell > 0 && !cardsInHandAI[i].deathcrys)
                {
                    if ((cardsInHandAI[i].id == 5 && !checkChargeAI()) || cardsInHandAI[i].id == 14)
                        dealHeroAI(cardsInHandAI[i].damageDealtBySpell);
                    else if (cardsInHandAI[i].id == 5 && checkChargeAI())
                        dealHeroAI(2*cardsInHandAI[i].damageDealtBySpell);
                    else if (cardsInHandAI[i].id == 7)
                        dealOneAI(cardsInHandAI[i].damageDealtBySpell);
                    else if (cardsInHandAI[i].id == 18 && checkNTNAI())
                        dealAllAI(1+cardsInHandAI[i].damageDealtBySpell);
                    else
                        dealAllAI(cardsInHandAI[i].damageDealtBySpell);
                }

                if (cardsInHandAI[i].increaseXdame > 0 && !cardsInHandAI[i].deathcrys)
                {
                    if (cardsInZoneAI.Count != 0)
                    {
                        for (int j = 0; j < cardsInZoneAI.Count; j++)
                        {
                            cardsInZoneAI[j].dameIncrease += cardsInHandAI[i].increaseXdame;
                        }
                    }
                }
                if (!cardsInHandAI[i].spell)
                {
                    cardsInZoneAI.Add(cardsInHandAI[i]);
                    if ((cardsInHandAI[i].id == 11 && checkPresentAI(16) && checkPresentAI(21)) ||
                        (cardsInHandAI[i].id == 16 && checkPresentAI(11) && checkPresentAI(21)) || 
                        (cardsInHandAI[i].id == 21 && checkPresentAI(11) && checkPresentAI(16)))
                    {
                        dealAllAI(3);
                    }
                }

                if (cardsInHandAI[i].id == 4)
                {
                    int x = cardsInZoneAI.Count - 1;
                    cardsInZoneAI[x].canAttack = true;
                }
                else if (!cardsInHandAI[i].spell)
                {
                    int x = cardsInZoneAI.Count - 1;
                    cardsInZoneAI[x].canAttack = false;
                }
                int index = cardsInHandAI.Count - 1;
                cardsInHandAI[i] = cardsInHandAI[index];
                cardsInHandAI.RemoveAt(index);
                break;
            }
        }

        checkBlood();
        checkBlood();
    }

    public void playerSummon(int id)
    {
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            if (cardsInHand[i].id == id)
            {
                playerMana -= cardsInHand[i].mana;
                //After adding to ZoneList, add effects...
                if (cardsInHand[i].drawXcards > 0 && !cardsInHand[i].deathcrys)
                {
                    for (int k = 0; k < cardsInHand[i].drawXcards; k++)
                    {
                        if (cardsInHand.Count < 7 && deck.Count > 0)
                        {
                            int indx = deck.Count - 1;
                            cardsInHand.Add(deck[indx]);
                            deck.RemoveAt(deck.Count - 1);
                        }
                    }
                }

                if (cardsInHand[i].healXpower > 0 && !cardsInHand[i].deathcrys)
                {
                    if (cardsInHand[i].id == 2 || cardsInHand[i].id == 3)
                        HealOne(cardsInHand[i].healXpower);
                    else if (cardsInHand[i].id == 20 || cardsInHand[i].id == 24 || cardsInHand[i].id == 21 || cardsInHand[i].id == 11 
                             || cardsInHand[i].id == 16 || cardsInHand[i].id == 12 || cardsInHand[i].id == 23)
                        HealAll(cardsInHand[i].healXpower);
                    else if (cardsInHand[i].id == 10)
                    {
                        HealOne(cardsInHand[i].healXpower);
                        HealHero(cardsInHand[i].healXpower-1);
                    }
                }

                if (cardsInHand[i].damageDealtBySpell > 0 && !cardsInHand[i].deathcrys)
                {
                    if ((cardsInHand[i].id == 5 && !checkChargePlayer()) || cardsInHand[i].id == 14)
                        dealHero(cardsInHand[i].damageDealtBySpell);
                    else if (cardsInHand[i].id == 5 && checkChargePlayer())
                        dealHero(2*cardsInHand[i].damageDealtBySpell);
                    else if (cardsInHand[i].id == 7)
                        dealOne(cardsInHand[i].damageDealtBySpell);
                    else if (cardsInHand[i].id == 18 && checkNTN())
                        dealAll(1+cardsInHand[i].damageDealtBySpell);
                    else
                        dealAll(cardsInHand[i].damageDealtBySpell);
                }

                if (cardsInHand[i].increaseXdame > 0 && !cardsInHand[i].deathcrys)
                {
                    if (cardsInZone.Count != 0)
                    {
                        for (int j = 0; j < cardsInZone.Count; j++)
                        {
                            cardsInZone[j].dameIncrease += cardsInHand[i].increaseXdame;
                        }
                    }
                }
                if (!cardsInHand[i].spell)
                {
                    cardsInZone.Add(cardsInHand[i]);
                    if ((cardsInHand[i].id == 11 && checkPresent(16) && checkPresent(21)) ||
                        (cardsInHand[i].id == 16 && checkPresent(11) && checkPresent(21)) || 
                        (cardsInHand[i].id == 21 && checkPresent(11) && checkPresent(16)))
                    {
                        dealAll(3);
                    }
                }
                if (cardsInHand[i].id == 4)
                {
                    int ind = cardsInZone.Count - 1;
                    cardsInZone[ind].canAttack = true;
                }
                else if (!cardsInHand[i].spell)
                {
                    int ind = cardsInZone.Count - 1;
                    cardsInZone[ind].canAttack = false;
                }

                int idx = cardsInHand.Count - 1;
                cardsInHand[i] = cardsInHand[idx];
                cardsInHand.RemoveAt(idx);
                break;
            }
        }
        checkBlood();
        checkBlood();
    }

    public void aiAtk(int id1, int id2)
    {
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            if (cardsInZoneAI[i].id == id1)
            {
                if (id2 == 0)
                {
                    playerHp -= cardsInZoneAI[i].actualDame;
                }
                else
                {
                    for (int j = 0; j < cardsInZone.Count; j++)
                    {
                        if (cardsInZone[j].id == id2)
                        {
                            aiAttack(cardsInZoneAI[i], cardsInZone[j]);
                            checkBlood();
                            checkBlood();
                            break;
                        }
                    }
                }
                break;
            }
        }
    }
    public void aiAttack(AICardToHand1 card1, ThisCard1 card2)
    {
        if (card1.id == 17)
        {
            card1.hurted += 1;
        }
        else
            card1.hurted += card2.actualDame;

        if (card2.id == 17)
        {
            card2.hurted += 1;
        }
        else
            card2.hurted += card1.actualDame;

        if (isMutualBirthAI(card1, card2))
        {
            card2.hurted -= 2;
        }

        if (isOppositionAI(card1, card2))
        {
            card2.hurted += 2;
        }
        card1.Update();
        card2.Update();
        // checkBlood();
    }

    public void playerAtk(int id1, int id2)
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            if (cardsInZone[i].id == id1)
            {
                if (id2 == 0)
                {
                    aiHp -= cardsInZone[i].actualDame;
                }
                else
                {
                    for (int j = 0; j < cardsInZoneAI.Count; j++)
                    {
                        if (cardsInZoneAI[j].id == id2)
                        {
                            playerAttack(cardsInZone[i], cardsInZoneAI[j]);
                            checkBlood();
                            checkBlood();
                            break;
                        }
                    }
                }
                break;
            }
        }
    }

    public void playerAttack(ThisCard1 card1, AICardToHand1 card2)
    {
        if (card1.id == 17)
        {
            card1.hurted += 1;
        }
        else
            card1.hurted += card2.actualDame;

        if (card2.id == 17)
        {
            card2.hurted += 1;
        }
        else
            card2.hurted += card1.actualDame;

        if (isMutualBirth(card1, card2))
        {
            card2.hurted -= 2;
        }

        if (isOpposition(card1, card2))
        {
            card2.hurted += 2;
        }
        // checkBlood();
    }

    public bool checkDone()
    {
        if (playerTurn)
        {
            // for (int i = 0; i < cardsInHand.Count; i++)
            // {
            //     if (cardsInHand[i].mana <= playerMana)
            //         return false;
            // }
            for (int i = 0; i < cardsInZone.Count; i++)
            {
                if (cardsInZone[i].canAttack)
                    return false;
            }
        }
        else
        {
            // for (int i = 0; i < cardsInHandAI.Count; i++)
            // {
            //     if (cardsInHandAI[i].mana <= aiMana)
            //         return false;
            // }
            for (int i = 0; i < cardsInZoneAI.Count; i++)
            {
                if (cardsInZoneAI[i].canAttack)
                    return false;
            }
        }
        return true;
    }

    public void nextTurn()
    {
        checkBlood();
        playerTurn = !playerTurn;
        if (playerTurn)
        {
            playerManaTurn += 1;
            if (playerManaTurn > 7)
                playerManaTurn = 7;
            playerMana = playerManaTurn;
            if (cardsInHand.Count < 7 && deck.Count > 0)
            {
                cardsInHand.Add(deck[deck.Count - 1]);
                deck.RemoveAt(deck.Count - 1);
            }

            if (deck.Count <= 0)
            {
                playerHp -= 1;
            }

            for (int i = 0; i < cardsInZoneAI.Count; i++)
            {
                cardsInZoneAI[i].canAttack = true;
            }
        }
        else
        {
            aiManaTurn += 1;
            if (aiManaTurn > 7)
                aiManaTurn = 7;
            aiMana = aiManaTurn;
            if (cardsInHandAI.Count < 7 && AIdeck.Count > 0)
            {
                cardsInHandAI.Add(AIdeck[AIdeck.Count - 1]);
                AIdeck.RemoveAt(AIdeck.Count - 1);
            }

            if (AIdeck.Count <= 0)
            {
                aiHp -= 1;
            }
            for (int i = 0; i < cardsInZone.Count; i++)
            {
                cardsInZone[i].canAttack = true;
            }
        }
    }

    public void HealOne(int healXpower)
    {
        if (cardsInZone.Count == 0)
            return;
        // int x = Random.Range(0, cardsInZone.Count);
        // cardsInZone[x].hurted -= healXpower;
        // cardsInZone[x].Update();
        cardsInZone[0].hurted -= healXpower;
        cardsInZone[0].Update();
    }
    
    public void HealOneAI(int healXpower)
    {
        if (cardsInZoneAI.Count == 0)
            return;
        // int x = Random.Range(0, cardsInZoneAI.Count);
        // cardsInZoneAI[x].hurted -= healXpower;
        // cardsInZoneAI[x].Update();
        cardsInZoneAI[0].hurted -= healXpower;
        cardsInZoneAI[0].Update();
    }

    public void HealAll(int healXpower)
    {
        if (cardsInZone.Count == 0)
            return;
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            cardsInZone[i].hurted -= healXpower;
            cardsInZone[i].Update();
        }
    }
    
    public void HealAllAI(int healXpower)
    {
        if (cardsInZoneAI.Count == 0)
            return;
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            cardsInZoneAI[i].hurted -= healXpower;
            cardsInZoneAI[i].Update();
        }
    }

    public void HealHero(int healXpower)
    {
        playerHp += healXpower;
        if (playerHp > PlayerHp.maxHp)
            playerHp = PlayerHp.maxHp;
    }
    
    public void HealHeroAI(int healXpower)
    {
        aiHp += healXpower;
        if (aiHp > EnemyHp.maxHp)
            aiHp = EnemyHp.maxHp;
    }

    public void dealHero(int damageDealtBySpell)
    {
        aiHp -= damageDealtBySpell;
    }
    
    public void dealHeroAI(int damageDealtBySpell)
    {
        playerHp -= damageDealtBySpell;
    }

    public bool checkChargePlayer()
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            if (cardsInZone[i].id == 4)
                return true;
        }
        return false;
    }
    
    public bool checkChargeAI()
    {
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            if (cardsInZoneAI[i].id == 4)
                return true;
        }
        return false;
    }
    
    public void dealAll(int damageDealtBySpell)
    {
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            cardsInZoneAI[i].hurted += damageDealtBySpell;
            cardsInZoneAI[i].Update();
        }
    }
    
    public void dealAllAI(int damageDealtBySpell)
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            cardsInZone[i].hurted += damageDealtBySpell;
            cardsInZone[i].Update();
        }
    }

    public bool checkNTN()
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            if (cardsInZone[i].id == 17)
                return true;
        }

        return false;
    }
    public bool checkNTNAI()
    {
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            if (cardsInZoneAI[i].id == 17)
                return true;
        }

        return false;
    }
    
    public void dealOne(int damageDealtBySpell)
    {
        if (cardsInZoneAI.Count > 0)
        {
            // int x = Random.Range(0, cardsInZoneAI.Count);
            // cardsInZoneAI[x].hurted += damageDealtBySpell;
            // cardsInZoneAI[x].Update();
            cardsInZoneAI[0].hurted += damageDealtBySpell;
            cardsInZoneAI[0].Update();
        }
    }
    public void dealOneAI(int damageDealtBySpell)
    {
        if (cardsInZone.Count > 0)
        {
            // int x = Random.Range(0, cardsInZone.Count);
            // cardsInZone[x].hurted += damageDealtBySpell;
            // cardsInZone[x].Update();
            cardsInZone[0].hurted += damageDealtBySpell;
            cardsInZone[0].Update();
        }
    }
    public bool isMutualBirth(ThisCard1 player, AICardToHand1 enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement  || enemy.thisCard.element == Card.Element.NoElement)
            return false;
        if (player.thisCard.element == Card.Element.Earth && enemy.thisCard.element == Card.Element.Metal)
            return true;
        if (player.thisCard.element == Card.Element.Metal && enemy.thisCard.element == Card.Element.Water)
            return true;
        if (player.thisCard.element == Card.Element.Water && enemy.thisCard.element == Card.Element.Wood)
            return true;
        if (player.thisCard.element == Card.Element.Wood && enemy.thisCard.element == Card.Element.Fire)
            return true;
        if (player.thisCard.element == Card.Element.Fire && enemy.thisCard.element == Card.Element.Earth)
            return true;
        return false;
    }
    public bool isOpposition(ThisCard1 player, AICardToHand1 enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement  || enemy.thisCard.element == Card.Element.NoElement)
            return false;
        if (player.thisCard.element == Card.Element.Earth && enemy.thisCard.element == Card.Element.Water)
            return true;
        if (player.thisCard.element == Card.Element.Metal && enemy.thisCard.element == Card.Element.Wood)
            return true;
        if (player.thisCard.element == Card.Element.Water && enemy.thisCard.element == Card.Element.Fire)
            return true;
        if (player.thisCard.element == Card.Element.Wood && enemy.thisCard.element == Card.Element.Earth)
            return true;
        if (player.thisCard.element == Card.Element.Fire && enemy.thisCard.element == Card.Element.Metal)
            return true;
        return false;
    }
    public bool isMutualBirthAI(AICardToHand1 player, ThisCard1 enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement  || enemy.thisCard.element == Card.Element.NoElement)
            return false;
        if (player.thisCard.element == Card.Element.Earth && enemy.thisCard.element == Card.Element.Metal)
            return true;
        if (player.thisCard.element == Card.Element.Metal && enemy.thisCard.element == Card.Element.Water)
            return true;
        if (player.thisCard.element == Card.Element.Water && enemy.thisCard.element == Card.Element.Wood)
            return true;
        if (player.thisCard.element == Card.Element.Wood && enemy.thisCard.element == Card.Element.Fire)
            return true;
        if (player.thisCard.element == Card.Element.Fire && enemy.thisCard.element == Card.Element.Earth)
            return true;
        return false;
    }
    public bool isOppositionAI(AICardToHand1 player, ThisCard1 enemy)
    {
        if (player.thisCard.element == Card.Element.NoElement  || enemy.thisCard.element == Card.Element.NoElement)
            return false;
        if (player.thisCard.element == Card.Element.Earth && enemy.thisCard.element == Card.Element.Water)
            return true;
        if (player.thisCard.element == Card.Element.Metal && enemy.thisCard.element == Card.Element.Wood)
            return true;
        if (player.thisCard.element == Card.Element.Water && enemy.thisCard.element == Card.Element.Fire)
            return true;
        if (player.thisCard.element == Card.Element.Wood && enemy.thisCard.element == Card.Element.Earth)
            return true;
        if (player.thisCard.element == Card.Element.Fire && enemy.thisCard.element == Card.Element.Metal)
            return true;
        return false;
    }

    public void checkBlood()
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            cardsInZone[i].Update();
            if (cardsInZone[i].actualblood <= 0 && i != cardsInZone.Count-1)
            {
                if (cardsInZone[i].deathcrys)
                {
                    if (cardsInZone[i].healXpower > 0)
                    {
                        if (cardsInZone[i].id == 2 || cardsInZone[i].id == 3)
                            HealOne(cardsInZone[i].healXpower);
                        else if (cardsInZone[i].id == 20 || cardsInZone[i].id == 24 || cardsInZone[i].id == 21 || cardsInZone[i].id == 11 
                                 || cardsInZone[i].id == 16 || cardsInZone[i].id == 12 || cardsInZone[i].id == 23)
                            HealAll(cardsInZone[i].healXpower);
                        else if (cardsInZone[i].id == 10)
                        {
                            HealOne(cardsInZone[i].healXpower);
                            HealHero(cardsInZone[i].healXpower-1);
                        }
                    }

                    if (cardsInZone[i].damageDealtBySpell > 0)
                    {
                        if ((cardsInZone[i].id == 5 && !checkChargePlayer()) || cardsInZone[i].id == 14)
                            dealHero(cardsInZone[i].damageDealtBySpell);
                        else if (cardsInZone[i].id == 5 && checkChargePlayer())
                            dealHero(2*cardsInZone[i].damageDealtBySpell);
                        else if (cardsInZone[i].id == 7)
                            dealOne(cardsInZone[i].damageDealtBySpell);
                        else if (cardsInZone[i].id == 18 && checkNTN())
                            dealAll(1+cardsInZone[i].damageDealtBySpell);
                        else
                            dealAll(cardsInZone[i].damageDealtBySpell);
                    }
                }

                for (int j = i; j < cardsInZone.Count-1; j++)
                {
                    cardsInZone[j] = cardsInZone[j + 1];
                }
                // cardsInZone[i] = cardsInZone[cardsInZone.Count-1];
                cardsInZone.RemoveAt(cardsInZone.Count-1);
            }
            else if (cardsInZone[i].actualblood <= 0 && i == cardsInZone.Count - 1)
            {
                if (cardsInZone[i].deathcrys)
                {
                    if (cardsInZone[i].healXpower > 0)
                    {
                        if (cardsInZone[i].id == 2 || cardsInZone[i].id == 3)
                            HealOne(cardsInZone[i].healXpower);
                        else if (cardsInZone[i].id == 20 || cardsInZone[i].id == 24 || cardsInZone[i].id == 21 || cardsInZone[i].id == 11 
                                 || cardsInZone[i].id == 16 || cardsInZone[i].id == 12 || cardsInZone[i].id == 23)
                            HealAll(cardsInZone[i].healXpower);
                        else if (cardsInZone[i].id == 10)
                        {
                            HealOne(cardsInZone[i].healXpower);
                            HealHero(cardsInZone[i].healXpower-1);
                        }
                    }

                    if (cardsInZone[i].damageDealtBySpell > 0)
                    {
                        if ((cardsInZone[i].id == 5 && !checkChargePlayer()) || cardsInZone[i].id == 14)
                            dealHero(cardsInZone[i].damageDealtBySpell);
                        else if (cardsInZone[i].id == 5 && checkChargePlayer())
                            dealHero(2*cardsInZone[i].damageDealtBySpell);
                        else if (cardsInZone[i].id == 7)
                            dealOne(cardsInZone[i].damageDealtBySpell);
                        else if (cardsInZone[i].id == 18 && checkNTN())
                            dealAll(1+cardsInZone[i].damageDealtBySpell);
                        else
                            dealAll(cardsInZone[i].damageDealtBySpell);
                    }
                }
                cardsInZone.RemoveAt(cardsInZone.Count-1);
            }
        }
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            cardsInZoneAI[i].Update();
            if (cardsInZoneAI[i].actualblood <= 0 && i != cardsInZoneAI.Count-1)
            {
                if (cardsInZoneAI[i].deathcrys)
                {
                    if (cardsInZoneAI[i].healXpower > 0)
                    {
                        if (cardsInZoneAI[i].id == 2 || cardsInZoneAI[i].id == 3)
                            HealOneAI(cardsInZoneAI[i].healXpower);
                        else if (cardsInZoneAI[i].id == 20 || cardsInZoneAI[i].id == 24 || cardsInZoneAI[i].id == 21 || cardsInZoneAI[i].id == 11 
                                 || cardsInZoneAI[i].id == 16 || cardsInZoneAI[i].id == 12 || cardsInZoneAI[i].id == 23)
                            HealAllAI(cardsInZoneAI[i].healXpower);
                        else if (cardsInZoneAI[i].id == 10)
                        {
                            HealOneAI(cardsInZoneAI[i].healXpower);
                            HealHeroAI(cardsInZoneAI[i].healXpower-1);
                        }
                    }

                    if (cardsInZoneAI[i].damageDealtBySpell > 0)
                    {
                        if ((cardsInZoneAI[i].id == 5 && !checkChargeAI()) || cardsInZoneAI[i].id == 14)
                            dealHeroAI(cardsInZoneAI[i].damageDealtBySpell);
                        else if (cardsInZoneAI[i].id == 5 && checkChargeAI())
                            dealHeroAI(2*cardsInZoneAI[i].damageDealtBySpell);
                        else if (cardsInZoneAI[i].id == 7)
                            dealOneAI(cardsInZoneAI[i].damageDealtBySpell);
                        else if (cardsInZoneAI[i].id == 18 && checkNTNAI())
                            dealAllAI(1+cardsInZoneAI[i].damageDealtBySpell);
                        else
                            dealAllAI(cardsInZoneAI[i].damageDealtBySpell);
                    }
                }

                for (int z = i; z < cardsInZoneAI.Count - 1; z++)
                {
                    cardsInZoneAI[z] = cardsInZoneAI[z + 1];
                }
                // cardsInZoneAI[i] = cardsInZoneAI[cardsInZoneAI.Count-1];
                cardsInZoneAI.RemoveAt(cardsInZoneAI.Count-1);
            }
            else if (cardsInZoneAI[i].actualblood <= 0 && i == cardsInZoneAI.Count - 1)
            {
                if (cardsInZoneAI[i].deathcrys)
                {
                    if (cardsInZoneAI[i].healXpower > 0)
                    {
                        if (cardsInZoneAI[i].id == 2 || cardsInZoneAI[i].id == 3)
                            HealOneAI(cardsInZoneAI[i].healXpower);
                        else if (cardsInZoneAI[i].id == 20 || cardsInZoneAI[i].id == 24 || cardsInZoneAI[i].id == 21 || cardsInZoneAI[i].id == 11 
                                 || cardsInZoneAI[i].id == 16 || cardsInZoneAI[i].id == 12 || cardsInZoneAI[i].id == 23)
                            HealAllAI(cardsInZoneAI[i].healXpower);
                        else if (cardsInZoneAI[i].id == 10)
                        {
                            HealOneAI(cardsInZoneAI[i].healXpower);
                            HealHeroAI(cardsInZoneAI[i].healXpower-1);
                        }
                    }

                    if (cardsInZoneAI[i].damageDealtBySpell > 0)
                    {
                        if ((cardsInZoneAI[i].id == 5 && !checkChargeAI()) || cardsInZoneAI[i].id == 14)
                            dealHeroAI(cardsInZoneAI[i].damageDealtBySpell);
                        else if (cardsInZoneAI[i].id == 5 && checkChargeAI())
                            dealHeroAI(2*cardsInZoneAI[i].damageDealtBySpell);
                        else if (cardsInZoneAI[i].id == 7)
                            dealOneAI(cardsInZoneAI[i].damageDealtBySpell);
                        else if (cardsInZoneAI[i].id == 18 && checkNTNAI())
                            dealAllAI(1+cardsInZoneAI[i].damageDealtBySpell);
                        else
                            dealAllAI(cardsInZoneAI[i].damageDealtBySpell);
                    }
                }
                cardsInZoneAI.RemoveAt(cardsInZoneAI.Count-1);
            }
        }
    }

    public void make_move(List<Move> moves)
    {
        if (moves.Count == 0)
            return;
        if (playerTurn)
        {
            foreach (Move move in moves)
            {
                if (move.summon)
                {
                    playerSummon(move.id);
                }

                if (move.attack)
                {
                    for (int i = 0; i < cardsInZone.Count; i++)
                    {
                        if (cardsInZone[i].id == move.id)
                        {
                            for (int j = 0; j < cardsInZoneAI.Count; j++)
                            {
                                if (cardsInZoneAI[j].id == move.idAtk)
                                {
                                    playerAttack(cardsInZone[i], cardsInZoneAI[j]);
                                    checkBlood();
                                    checkBlood();
                                    break;
                                }
                            }

                            break;
                        }
                    }
                }
            }
        }
        else
        {
            foreach (Move move in moves)
            {
                if (move.summon)
                {
                    aiSummon(move.id);
                }

                if (move.attack)
                {
                    for (int i = 0; i < cardsInZoneAI.Count; i++)
                    {
                        if (cardsInZoneAI[i].id == move.id)
                        {
                            for (int j = 0; j < cardsInZone.Count; j++)
                            {
                                if (cardsInZone[j].id == move.idAtk)
                                {
                                    aiAttack(cardsInZoneAI[i], cardsInZone[j]);
                                    checkBlood();
                                    checkBlood();
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
    }

    public bool checkPresent(int id)
    {
        for (int i = 0; i < cardsInZone.Count; i++)
        {
            if (cardsInZone[i].id == id)
                return true;
        }

        return false;
    }
    public bool checkPresentAI(int id)
    {
        for (int i = 0; i < cardsInZoneAI.Count; i++)
        {
            if (cardsInZoneAI[i].id == id)
                return true;
        }

        return false;
    }
    public string toString()
    {
        string result = "" + "\n";
        for (int i = 0; i < 30; i++)
        {
            if (i >= PlayerDeck.deckSize)
            {
                result += "\n";
            }
            else
            {
                result += deck[i].toString();
                result += "\n";
            }
        }

        for (int i = 0; i < 7; i++)
        {
            if (i >= cardsInHand.Count)
            {
                result += "\n";
            }
            else
            {
                result += cardsInHand[i].toString();
                result += "\n";
            }
        }
        
        for (int i = 0; i < 5; i++)
        {
            if (i >= cardsInZone.Count)
            {
                result += "\n";
            }
            else
            {
                result += cardsInZone[i].toString();
                result += "\n";
            }
        }

        result += playerHp + "\n";
        result += playerManaTurn + "\n";
        result += playerMana + "\n";
        string playerT = playerTurn ? "1" : "0";
        result += playerT + "\n";
        result += aiHp + "\n";
        result += aiManaTurn + "\n";
        result += aiMana + "\n";
        
        for (int i = 0; i < 5; i++)
        {
            if (i >= cardsInZoneAI.Count)
            {
                result += "\n";
            }
            else
            {
                result += cardsInZoneAI[i].toString();
                result += "\n";
            }
        }
        
        for (int i = 0; i < 7; i++)
        {
            if (i >= cardsInHandAI.Count)
            {
                result += "\n";
            }
            else
            {
                result += cardsInHandAI[i].toString();
                result += "\n";
            }
        }
        
        for (int i = 0; i < 30; i++)
        {
            if (i >= AI.deckSize)
            {
                result += "\n";
            }
            else
            {
                result += AIdeck[i].toString();
                result += "\n";
            }
        }
        //result += "+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++" + "\n";
        return result;
    }
}

public class Move
{
    bool playerTurn;
    public int id;
    public bool summon;
    public bool attack;
    public int idAtk;

    public Move(bool pTurn,int Id, bool Summon, bool Attack, int id2)
    {
        playerTurn = pTurn;
        id = Id;
        summon = Summon;
        attack = Attack;
        idAtk = id2;
    }
}
