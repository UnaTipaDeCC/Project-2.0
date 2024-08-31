using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class cardDisplay : MonoBehaviour
{
    public CardGame card;
    public Text nameText;
    public Text DescriptionText;
    public Image ArtworkImage;
    public Text DamageText;
    public Text Type;
    
    // Start is called before the first frame update
    void Start()
    {
        nameText.text = card.name;
        DescriptionText.text = card.Description;
        ArtworkImage.sprite = card.Artwork;
        DamageText.text = card.Damage.ToString();
    
        
    }
    void Update()
    {
        DamageText.text = card.Damage.ToString();
    }

   
}
