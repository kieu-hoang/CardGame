using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        playerTurn = newGame.playerTurn;
    }

    public List<List<Move>> getValidMoves()
    {
        List<List<Move>> result = new List<List<Move>>();
        List<Move> validMove = new List<Move>();
        if (playerTurn)
        {
            // int currentMana = playerMana;
            // int summonedMana = 0;
            // for (int i = 0; i < cardsInHand.Count; i++)
            // {
            //     if (cardsInHand[i].mana <= playerMana && summonedMana+cardsInHand[i].mana <= currentMana)
            //     {
            //         summonedMana += cardsInHand[i].mana;
            //         validMove.Add(new Move(true, cardsInHand[i].id, true, false, 0));
            //     }
            // }
            //
            // for (int i = 0; i < cardsInZone.Count; i++)
            // {
            //     validMove.Add(new Move(true, cardsInZone[i].id, false, true, 0));
            // }
            FindCombinations(cardsInHand, 0, playerMana, validMove, result);
            List<ThisCard1> atkcard = new List<ThisCard1>();
            for (int i = 0; i < cardsInZone.Count; i++)
            {
                if (cardsInZone[i].canAttack)
                    atkcard.Add(cardsInZone[i]);
            }
            List<Move> validMove2 = new List<Move>();
            if (cardsInZoneAI.Count == 0)
            {
                for (int i = 0; i < atkcard.Count; i++)
                {
                    validMove2.Add(new Move(true, atkcard[i].id, false, true, 0));
                }
            }
            if (checkTauntAI())
            {
                List<AICardToHand1> taunt = new List<AICardToHand1>();
                
                for (int i = 0; i < cardsInZoneAI.Count; i++)
                {
                    if (cardsInZoneAI[i].id == 1 || cardsInZoneAI[i].id == 13 || cardsInZoneAI[i].id == 19)
                    {
                        taunt.Add(cardsInZoneAI[i]);
                    }
                }
                for (int i = 0, j=0; i < atkcard.Count; i++)
                {
                    validMove2.Add(new Move(false, atkcard[i].id, false, true, taunt[j].id));
                    if (taunt[j].actualblood <= atkcard[i].actualDame)
                        j++;
                }
            }
            foreach (List<Move> res in result)
            {
                res.AddRange(validMove2);
            }
        }
        else
        {
            // int currentMana = aiMana;
            // int summonedMana = 0;
            // for (int i = 0; i < cardsInHandAI.Count; i++)
            // {
            //     if (cardsInHandAI[i].mana <= aiMana && summonedMana+cardsInHandAI[i].mana <= currentMana)
            //     {
            //         summonedMana += cardsInHandAI[i].mana;
            //         validMove.Add(new Move(false, cardsInHandAI[i].id, true, false, 0));
            //     }
            // }
            //
            // for (int i = 0; i < cardsInZoneAI.Count; i++)
            // {
            //     validMove.Add(new Move(false, cardsInZoneAI[i].id, false, true, 0));
            // }
            FindCombinationsAI(cardsInHandAI, 0, aiMana, validMove, result);
            List<AICardToHand1> atkcard = new List<AICardToHand1>();
            for (int i = 0; i < cardsInZoneAI.Count; i++)
            {
                if (cardsInZoneAI[i].canAttack)
                    atkcard.Add(cardsInZoneAI[i]);
            }
            List<Move> validMove2 = new List<Move>();
            if (cardsInZone.Count == 0)
            {
                for (int i = 0; i < atkcard.Count; i++)
                {
                    validMove2.Add(new Move(false, atkcard[i].id, false, true, 0));
                }
            }
            if (checkTaunt())
            {
                List<ThisCard1> taunt = new List<ThisCard1>();
                
                for (int i = 0; i < cardsInZone.Count; i++)
                {
                    if (cardsInZone[i].id == 1 || cardsInZone[i].id == 13 || cardsInZone[i].id == 19)
                    {
                        taunt.Add(cardsInZone[i]);
                    }
                }

                for (int i = 0; i < atkcard.Count; i++)
                {
                    validMove2.Add(new Move(false, atkcard[i].id, false, true, taunt[0].id));
                }
            }
            foreach (List<Move> res in result)
            {
                res.AddRange(validMove2);
            }
        }
        //result.Add(validMove);
        return result;
    }
    static void FindCombinations(List<ThisCard1> cardsInHand, int index, int mana, List<Move> currentCombination, List<List<Move>> result)
    {
        if (mana <= 0)
        {
            return;
        }
        
        if (index == cardsInHand.Count)
        {
            return;
        }
        
        
        // Include the current element and explore further
        if (mana >= cardsInHand[index].mana)
        {
            currentCombination.Add(new Move(true, cardsInHand[index].id, true, false, 0));
            if (!(result.Contains(currentCombination)))
                result.Add(currentCombination);
            FindCombinations(cardsInHand, index + 1, mana - cardsInHand[index].mana, currentCombination, result);
            currentCombination.RemoveAt(currentCombination.Count - 1); // Backtrack

        }

        // Exclude the current element and explore further
        FindCombinations(cardsInHand, index + 1, mana, currentCombination, result);
    }
    
    static void FindCombinationsAI(List<AICardToHand1> cardsInHand, int index, int mana, List<Move> currentCombination, List<List<Move>> result)
    {
        if (mana <= 0)
        {
            return;
        }
        

        if (index == cardsInHand.Count)
        {
            return;
        }
        
        
        // Include the current element and explore further
        if (mana >= cardsInHand[index].mana)
        {
            currentCombination.Add(new Move(false, cardsInHand[index].id, true, false, 0));
            if (!(result.Contains(currentCombination)))
                result.Add(currentCombination);
            FindCombinationsAI(cardsInHand, index + 1, mana - cardsInHand[index].mana, currentCombination, result);
            currentCombination.RemoveAt(currentCombination.Count - 1); // Backtrack

        }

        // Exclude the current element and explore further
        FindCombinationsAI(cardsInHand, index + 1, mana, currentCombination, result);
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
                            cardsInHandAI.Add(AIdeck[AIdeck.Count - 1]);
                            AIdeck.RemoveAt(AIdeck.Count - 1);
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
                    cardsInZoneAI[cardsInZoneAI.Count - 1].canAttack = true;
                }

                cardsInHandAI[i] = cardsInHandAI[cardsInHandAI.Count - 1];
                cardsInHandAI.RemoveAt(cardsInHandAI.Count-1);
                break;
            }
        }
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
                            cardsInHand.Add(deck[deck.Count - 1]);
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
                    cardsInZone[cardsInZone.Count - 1].canAttack = true;
                }
                cardsInHand[i] = cardsInHand[cardsInHand.Count - 1];
                cardsInHand.RemoveAt(cardsInHand.Count-1);
                break;
            }
        }
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
        checkBlood();
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
        checkBlood();
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
        playerTurn = !playerTurn;
        if (playerTurn)
        {
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
        int x = Random.Range(0, cardsInZone.Count);
        cardsInZone[x].hurted -= healXpower;
        cardsInZone[x].Update();
    }
    
    public void HealOneAI(int healXpower)
    {
        if (cardsInZoneAI.Count == 0)
            return;
        int x = Random.Range(0, cardsInZoneAI.Count);
        cardsInZoneAI[x].hurted -= healXpower;
        cardsInZoneAI[x].Update();
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
        int x = Random.Range(0, cardsInZoneAI.Count);
        cardsInZoneAI[x].hurted += damageDealtBySpell;
        cardsInZoneAI[x].Update();
    }
    public void dealOneAI(int damageDealtBySpell)
    {
        int x = Random.Range(0, cardsInZone.Count);
        cardsInZone[x].hurted += damageDealtBySpell;
        cardsInZone[x].Update();
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
                cardsInZone[i] = cardsInZone[cardsInZone.Count-1];
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
                cardsInZoneAI[i] = cardsInZoneAI[cardsInZoneAI.Count-1];
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
