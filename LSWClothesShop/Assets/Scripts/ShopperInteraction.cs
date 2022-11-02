using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopperInteraction : MonoBehaviour
{
    [SerializeField] SpriteRenderer questIcon;

    // Start is called before the first frame update
    void Start()
    {
        questIcon.enabled = false;
    }
    
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }
        
        var player = other.GetComponent<PlayerMovement>();
        if (player.FacingDirection == FacingDirection.Left && !player.interactingWithShopper)
        {
            questIcon.enabled = true;
            player.interactingWithShopper = true;
        }
        else if (player.FacingDirection != FacingDirection.Left)
        {
            questIcon.enabled = false;
            player.interactingWithShopper = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (!other.gameObject.CompareTag("Player")) return;
        questIcon.enabled = false;
        player.interactingWithShopper = false;
    }
}
