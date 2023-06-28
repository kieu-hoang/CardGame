using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AnimatedCard : MonoBehaviour
{
    public Sprite[] clips;

    public Image card;

    public int numberOfClips;

    public int stage;

    public float wait;

    public float x;
    // Start is called before the first frame update
    void Start()
    {
        stage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        card.sprite = clips[stage-1];
        x += Time.deltaTime;
        if (x >= wait)
        {
            stage++;
            x = 0;
            if (stage == numberOfClips)
            {
                stage = 1;
            }
        }
    }
}
