using UnityEngine;
using UnityEngine.EventSystems;

public class TestButtonAlphaRaycaster : MonoBehaviour, IPointerClickHandler
{
    public string name;

    #region IPointerClickHandler implementation

    public void OnPointerClick(PointerEventData eventData)
    {
        print("Clicked "+ name);
    }

    #endregion
}