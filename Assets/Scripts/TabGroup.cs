using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> buttons;
    public List<GameObject> pages;
    public void OnTabSelected(TabButton selectedButton)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] == selectedButton)
            {
                buttons[i].background.enabled = true;
                pages[i].SetActive(true);
            }
            else
            {
                buttons[i].background.enabled = false;
                pages[i].SetActive(false);
            }
        }
    }
}
