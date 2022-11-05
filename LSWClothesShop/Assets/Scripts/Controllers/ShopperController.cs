using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ShopperController : MonoBehaviour
{
    [SerializeField] SpriteRenderer questIcon;

    // Start is called before the first frame update
    void Start()
    {
        questIcon.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) { return; }
        
        var player = other.GetComponent<PlayerController>();
        if (player.FacingDirection == FacingDirection.Left && !player.InteractingWithShopper)
        {
            questIcon.enabled = true;
            player.InteractingWithShopper = true;
        }
        else if (player.FacingDirection != FacingDirection.Left)
        {
            questIcon.enabled = false;
            player.InteractingWithShopper = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (!other.gameObject.CompareTag("Player")) return;
        questIcon.enabled = false;
        player.InteractingWithShopper = false;
    }
}
