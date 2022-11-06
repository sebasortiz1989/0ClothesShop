using Controllers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class ServiceLocator : MonoBehaviour
    {
        [SerializeField] private WardroveManager wardroveManager;
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlayerController playerController;

        private void Awake()
        {
            ManagerLocator.Instance.WardroveManager = wardroveManager;
            ManagerLocator.Instance.UImanager = uiManager;
            ManagerLocator.Instance.PlayerController = playerController;
        }
    }
}
