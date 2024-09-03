using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
public class Zones : MonoBehaviour
{
    public List<CardGame> CardsInZone;
    public void RefreshZone()
    { 

        // Obtener el componente GridLayoutGroup
        GridLayoutGroup layoutGroup = GetComponent<GridLayoutGroup>();

        // Obtener y destruir los objetos hijos actuales
        DestroyCurrentCards(layoutGroup);

        // Instanciar las nuevas cartas en la zona
        foreach(CardGame card in CardsInZone)
        {
            CreateCardInstance(card, layoutGroup);
        }
    }

    private void DestroyCurrentCards(GridLayoutGroup layoutGroup)
    {
        int childs= layoutGroup.transform.childCount;

        for (int i = childs - 1; i >= 0; i--) // Iterar en reversa para evitar problemas al destruir
        {
            GameObject child = layoutGroup.transform.GetChild(i).gameObject;
            Debug.Log("Destruyendo: " + child.name);
            Destroy(child);
        }
    }

    private void CreateCardInstance(CardGame card, GridLayoutGroup layoutGroup)
    {
        string cardPath = "Assets/Prefabs/Card.prefab";
        GameObject cardPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(cardPath);
        if (cardPrefab != null)
        {
            GameObject cardInstance = GameObject.Instantiate(cardPrefab);
            cardInstance.GetComponent<CardDisplay>().Card = card; // Asignar la carta al componente
            cardInstance.transform.SetParent(layoutGroup.transform, false); // Establecer el padre
            Debug.Log("Carta instanciada: " + card.Name);
        }
        else
        {
            Debug.LogError("No se pudo cargar el prefab de la carta.");
        }
    }
}
