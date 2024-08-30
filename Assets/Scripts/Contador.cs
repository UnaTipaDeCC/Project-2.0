/*using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Contador : MonoBehaviour
{
    public TextMeshProUGUI contador;
    public GameObject  Melee;
    public GameObject Ranged;
    public GameObject Siege; 
    public int puntos = 0;

    // Update is called once per frame
    void Update()
    {
        int adicionar = 0;
        adicionar = Melee.GetComponent<Tablero>().Suma + Siege.GetComponent<Tablero>().Suma + Ranged.GetComponent<Tablero>().Suma;
        puntos = adicionar;
        contador.text = puntos.ToString();   
    }
}*/
