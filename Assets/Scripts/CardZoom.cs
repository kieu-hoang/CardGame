using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardZoom : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject Hand;

    public GameObject ZoomCard;
    private int id;
    private GameObject zoomCard;

    private void Awake()
    {
        Canvas = GameObject.Find("Canvas");
        Hand = GameObject.Find("Hand");
    }

    public void OnHoverEnter()
    {
        // if (!isOwned)
        //     return;
        if (transform.parent != Hand.transform)
            return;
        id = gameObject.GetComponent<ThisCard>().id;
        zoomCard = Instantiate(ZoomCard, new Vector3(transform.position.x, transform.position.y+355,transform.position.z), transform.rotation);
        zoomCard.GetComponent<ZoomInfo>().thisCard = CardDataBase.cardList[id];
        zoomCard.transform.SetParent(Canvas.transform, true);
        zoomCard.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    }

    public void OnHoverExit()
    {
        Destroy(zoomCard);
    }
}
