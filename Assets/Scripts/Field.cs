using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public static GameObject playerZone;

    public static GameObject aiZone;

    public bool aeNhaNguyenEffect = false;

    public bool aeNhaNguyenEffectAI = false;
    // Start is called before the first frame update
    private void Start()
    {
        playerZone = GameObject.Find("Zone");
        aiZone = GameObject.Find("EnemyZone");
    }

    // Update is called once per frame
    void Update()
    {
        if (TurnSystem.isYourTurn)
        {
            if (CardsInZone.eHowMany > 0)
                killChargeAI();
        }
        else
        {
            if (CardsInZone.howMany > 0)
                killChargePlayer();
        }

        if (checkAENhaNguyen() && aeNhaNguyenEffect == false)
        {
            foreach (Transform child in aiZone.transform)
            {
                child.GetComponent<AICardToHand>().isTarget = true;
                if (child.GetComponent<AICardToHand>().isTarget)
                {
                    child.GetComponent<AICardToHand>().hurted += 3;
                }
            }
            aeNhaNguyenEffect = true;
        }

        if (checkAENhaNguyenAI() && aeNhaNguyenEffectAI == false)
        {
            foreach (Transform child in playerZone.transform)
            {
                child.GetComponent<ThisCard>().isTarget = true;
                if (child.GetComponent<ThisCard>().isTarget)
                {
                    child.GetComponent<ThisCard>().hurted += 3;
                }
            }
            aeNhaNguyenEffectAI = true;
        }
    }

    public void killChargePlayer()
    {
        foreach (Transform child in playerZone.transform)
        {
            if (child.GetComponent<ThisCard>() != null && child.GetComponent<ThisCard>().id == 4)
            {
                child.GetComponent<ThisCard>().hurted = child.GetComponent<ThisCard>().blood;
            }
        }
    }
    
    public void killChargeAI()
    {
        foreach (Transform child in aiZone.transform)
        {
            if (child.GetComponent<AICardToHand>() != null && child.GetComponent<AICardToHand>().id == 4)
            {
                child.GetComponent<AICardToHand>().hurted = child.GetComponent<AICardToHand>().blood;
            }
        }
    }

    public static bool checkChargePlayer()
    {
        foreach (Transform child in playerZone.transform)
        {
            if (child.GetComponent<ThisCard>() != null && child.GetComponent<ThisCard>().id == 4)
            {
                return true;
            }
        }
        return false;
    }
    public static bool checkChargeAI()
    {
        foreach (Transform child in aiZone.transform)
        {
            if (child.GetComponent<AICardToHand>() != null && child.GetComponent<AICardToHand>().id == 4)
            {
                return true;
            }
        }
        return false;
    }

    public static bool checkPresentPlayer(int id)
    {
        foreach (Transform child in playerZone.transform)
        {
            if (child.GetComponent<AICardToHand>() != null && child.GetComponent<ThisCard>().id == id)
                return true;
        }
        return false;
    }
    
    public static bool checkPresentAI(int id)
    {
        foreach (Transform child in aiZone.transform)
        {
            if (child.GetComponent<AICardToHand>() != null && child.GetComponent<AICardToHand>().id == id)
                return true;
        }
        return false;
    }

    public bool checkAENhaNguyen()
    {
        if (checkPresentPlayer(11) && checkPresentPlayer(16) && checkPresentPlayer(21))
            return true;
        return false;
    }
    public bool checkAENhaNguyenAI()
    {
        if (checkPresentAI(11) && checkPresentAI(16) && checkPresentAI(21))
            return true;
        return false;
    }

    public static bool checkNTN()
    {
        if (checkPresentPlayer(17))
            return true;
        return false;
    }
    public static bool checkNTNAI()
    {
        if (checkPresentAI(17))
            return true;
        return false;
    }
}
