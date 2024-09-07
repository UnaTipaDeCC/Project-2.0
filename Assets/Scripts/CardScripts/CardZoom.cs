using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardZoom : MonoBehaviour, IPointerClickHandler
{
    private GameObject zoomCard; 
    private GameObject padre;//gameObject en la escena encima del cual se instancia la zoomcard
    private Vector2 zoomScale = new Vector2(2, 3);

    public void Awake()
    {
        //Encontrar el gameObject en la escena
        padre = GameObject.Find("ZoomCard");
    }
    public void OnHoverEnter()
    {
        zoomCard = Instantiate(gameObject, new Vector2(105,500),Quaternion.identity);
        zoomCard.transform.SetParent(padre.transform);
        zoomCard.transform.localScale = zoomScale;
    }
    public void OnHoverExit()
    {
        Destroy(zoomCard);

    }
    //Manejar el caso en el que se le haga click a la carta
    public void OnPointerClick(PointerEventData eventData)
    {
        if (padre.transform.childCount > 0)
        {
            Transform child = padre.transform.GetChild(0);
            Destroy(child.gameObject);
        }
    }
}
