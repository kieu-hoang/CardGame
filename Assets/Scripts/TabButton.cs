using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour, IPointerClickHandler
{
    public TabGroup tabGroup;

    public Image background;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        background = GetComponent<Image>();
    }
}
