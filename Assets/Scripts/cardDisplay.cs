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
    
    public void OnPointerClick(PointerEventData eventData)
    {
        //Verificar que sea el turno correcto y la carta no haya sido jugada
        if(GameContext.Instance.TriggerPlayer == GameContext.Instance.ReturnPlayer(Card.Owner) && !Card.Played)
        {
            CardsMove.Instance.MoveCard(Card);
            GameManager.gameManager.ChangeTurn();
            Card.ActivateEffect();
        }    
    }
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = Card.name;
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
