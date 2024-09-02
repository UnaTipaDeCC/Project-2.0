using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cardDisplay : MonoBehaviour
{
    public CardGame Card;
    public TMP_Text nameText;
    public TMP_Text DescriptionText;
    public Image ArtworkImage;
    public TMP_Text DamageText;
    public TMP_Text Type;
    
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = Card.name;
        DescriptionText.text = Card.Description;
        ArtworkImage.sprite = Card.Artwork;
        DamageText.text = Card.Damage.ToString();
    }
    void Update()
    {
        DamageText.text = Card.Damage.ToString();
    }   
}
