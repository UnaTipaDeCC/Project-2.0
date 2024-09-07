using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessageDisplay : MonoBehaviour
{
    public GameObject messagePanel; // Asigna el Panel desde el Inspector
    public TMP_Text messageText; // Asigna el objeto de texto desde el Inspector
    public static MessageDisplay Instance; // Instancia estática   
    
    private void Awake()
    {
        // Asegurarse de que solo haya una instancia de MessagePanelDisplay
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Opcional: no destruir al cargar nuevas escenas
        }
        else
        {
            Destroy(gameObject); // Destruir instancias duplicadas
        }
    }

    void Start()
    {
        messagePanel.SetActive(false); // Ocultar el panel al inicio
    }

    public void ShowMessage(string message, float duration = 2.0f)
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }
        StartCoroutine(DisplayMessage(message, duration));
        //messagePanel.SetActive(false);
    }

    private IEnumerator DisplayMessage(string message, float duration)
    {
        messagePanel.SetActive(true); // Mostrar el panel
        messageText.text = message; // Establecer el mensaje
        yield return new WaitForSeconds(duration); // Esperar el tiempo especificado

        messagePanel.SetActive(false); // Ocultar el panel
    }
}
