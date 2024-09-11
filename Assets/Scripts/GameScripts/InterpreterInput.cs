using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

public class InterpreterInput : MonoBehaviour
{
    public TMP_InputField inputField; // Referencia al Input Field
    public TMP_Text messageText;// Referencia al Text para mensajes
    public Button CompileButton;
    public Button NextSceneButton;
    public Button BackScenceButton;

    private void Start()
    {
        CompileButton.onClick.AddListener(OnCompileButtonClick);
        NextSceneButton.onClick.AddListener(OnNextSceneButtonClicked);
        BackScenceButton.onClick.AddListener(OnBackSceneButtonClicked);
        
    }    
    private void OnCompileButtonClick()
    {
        string inputText = inputField.text;
        if(string.IsNullOrEmpty(inputText))
        {
            messageText.text += "Intrduzca su codigo";
        }
        else
        {
            LexicalAnalyzer lex = Compiling.Lexical;
            List<CompilingError> errors = new List<CompilingError>();
            IEnumerable<Token> tokens = lex.GetTokens(" ",inputText,errors);
            TokenStream stream = new TokenStream(tokens);
            Parse parse = new Parse(stream,errors);
            Context context= new Context();
            Scope scope= new Scope();
            if(errors.Count > 0)
            {
                foreach (CompilingError error in errors)
                {
                    messageText.text += error.ToString() + "\n"; // Mostrar errores
                }
                messageText.text += "Debe solucionar los errores para continuar";
            }
            else
            {
                messageText.text = "No hay errores de análisis." + "\n"; // Mensaje de éxito
                ElementalProgram program = parse.Parser();
                
                if (errors.Count > 0)
                {
                    messageText.text += "Hubo errores de parseo, debe solucionarlos para continuar con el chequeo semantico." + "\n";
                    foreach (var error in errors)
                    {
                        messageText.text += error.ToString() + "\n"; // Mostrar errores
                    }
                    
                }
                else
                {
                    program.CheckSemantic(context, scope, errors);
                    if(errors.Count > 0)
                    {
                        foreach (var error in errors)
                        {
                            messageText.text += error.ToString() + "\n"; // Mostrar errores en la UI
                        }
                        messageText.text += "Hubo errores semanticos, debe solucionarlos para continuar con la evaluacion";
                    }
                    else
                    {
                        program.Evaluate();
                        messageText.text += "Proceso terminado con exito";
                    }
                }
            }
        }
    }
    private void OnNextSceneButtonClicked()
    {
        // Cambiar a la siguiente escena
        SceneManager.LoadScene("Samplescene");
        Debug.Log(CreatedCards.BravasCards.Count);
    }
    private void OnBackSceneButtonClicked()
    {
        // Cambiar a la siguiente escena
        SceneManager.LoadScene("Menu Principal"); // Reemplaza con el nombre de tu escena
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
