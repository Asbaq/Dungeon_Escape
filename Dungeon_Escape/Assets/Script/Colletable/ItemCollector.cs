using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private AudioSource CollectionSoundEffect;
    
    private int Gems = 0;
    [SerializeField] private Text GemsText;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Gems")
        {
            Destroy(collision.gameObject);
            CollectionSoundEffect.Play();
            Gems++;
            GemsText.text = "Gems: " + Gems;
        }    
    }
}
