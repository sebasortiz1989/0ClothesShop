using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ServiceLocator : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private UIManager uiManager;

    private void Awake()
    {
        ManagerLocator.Instance.shopManager = shopManager;
        ManagerLocator.Instance.uImanager = uiManager;
    }
}
