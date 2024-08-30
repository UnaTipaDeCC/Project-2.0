using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZoom : MonoBehaviour
{
    private GameObject zoomCard;
    private GameObject padre;//gameObject en la escena encima del cual se instancia la zoomcard
    private Vector2 zoomScale = new Vector2(2, 3);

    public void Awake()
    {
        padre = GameObject.Find("ZoomCard");
    }
    public void OnHoverEnter()
    {
        zoomCard = Instantiate(gameObject, new Vector2(100,500),Quaternion.identity);
        zoomCard.transform.SetParent(padre.transform);
        zoomCard.transform.localScale = zoomScale;
    }
    public void OnHoverExit()
    {
        Destroy(zoomCard);

    }
}
