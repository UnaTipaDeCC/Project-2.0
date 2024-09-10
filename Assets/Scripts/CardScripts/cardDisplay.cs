using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardDisplay : MonoBehaviour, IPointerClickHandler
{
    public CardGame Card;
    public TMP_Text nameText;
    public TMP_Text DescriptionText;
    public Image ArtworkImage;
    public TMP_Text DamageText;
    public TMP_Text Type;
    public MessageDisplay messages;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            //Verificar que sea el turno correcto y la carta no haya sido jugada
            if(GameContext.Instance.TriggerPlayer == GameContext.Instance.ReturnPlayer(Card.Owner) && !Card.Played)
            {
                //una vez jugada una carta ya no puede hacer el cambio de cartas
                GameContext.Instance.ReturnPlayer(Card.Owner).CanChange = false;
                //revisar si llamar al efecto primero y al move despues sea una buena idea
                CardsMove.Instance.MoveCard(Card);
                Card.ActivateEffect();
                GameManager.gameManager.ChangeTurn();
            } 
            else messages.ShowMessage("No es su turno o ya esta carta se jugo",2.0f);  
        }
        else if(eventData.button == PointerEventData.InputButton.Right)
        {
            GameContext.Instance.ReturnPlayer(Card.Owner).ChangeCard(Card);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        messages = MessageDisplay.Instance;
        nameText.text = Card.Name;
        DescriptionText.text = Card.Description;
        ArtworkImage.sprite = Card.Artwork;
        DamageText.text = Card.Damage.ToString();
        Type.text = Card.Type.ToString();
    }
    void Update()
    {
        DamageText.text = Card.Damage.ToString();
    }   
}
