using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryCollection : MonoBehaviour
{
    public TextMeshProUGUI storyPanel;
    public int current;
    public int storiesInCollection;
    public int numberOfStoryOnPage;

    void Start()
    {
        current = 1;
        storiesInCollection = 8;
        numberOfStoryOnPage = 1;
    }

    void Update()
    {
        storyPanel.text = StoryDatabase.Stories[current].content;
    }
    public void Left()
    {
        if (current > 1)
        {
            current -= 1;
        }
    }

    public void Right()
    {
        if (current < (storiesInCollection - numberOfStoryOnPage)+1)
        {
            current += 1;
        }
    }
}
