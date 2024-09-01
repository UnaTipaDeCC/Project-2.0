using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterpreterInput : MonoBehaviour
{
    public TMP_InputField inputField; // Referencia al Input Field
    public TMP_Text messageText;// Referencia al Text para mensajes
    
    public void OnCompilerButtonClick()
    {
        string inputText = inputField.text;
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
                messageText.text += error.ToString() + "\n"; // Mostrar errores en la UI
            }
            messageText.text += "Debe solucionar los errores para continuar";
        }
        else
        {
            messageText.text = "No hay errores de análisis."; // Mensaje de éxito
            ElementalProgram program = parse.Parser();
            
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    messageText.text += error.ToString() + "\n"; // Mostrar errores en la UI
                }
                messageText.text += "Hubo errores de parseo, debe solucionarlos para continuar con el chequeo semantico";
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
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
