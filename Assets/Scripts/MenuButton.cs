using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MenuButton : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    Image image;
        void Awake() {
        image = GetComponent<Image>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        image.transform.localScale *= 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.transform.localScale = new Vector3(1,1);
    }
}
