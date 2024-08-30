/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RondasGanadas : MonoBehaviour
{
    public TextMeshProUGUI rondasGanadas;
    public GameManager gameManager;
    private int cuantas = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    

    // Update is called once per frame
    void Update()
    {
        if(rondasGanadas.CompareTag("Rondas Ganadas"))
        {
            cuantas = gameManager.RondasGanadas1;
        }
        else if(rondasGanadas.CompareTag("Rondas Ganadas (1)"))
        {
            cuantas = gameManager.RondasGanadas2;
        }
        rondasGanadas.text = cuantas.ToString();
    }
}*/
