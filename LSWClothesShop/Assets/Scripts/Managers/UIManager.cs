using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject shopIconTemplate;
        [SerializeField] private GameObject buyItemButton;
        [SerializeField] private GameObject sellItemButton;
        [SerializeField] private GameObject equipItemButton;
        [SerializeField] private GameObject buyingItemsTab;
        [SerializeField] private GameObject sellingItemsTab;
        private bool _isBuyTabSelected;
        [CanBeNull] private GameObject _selectedItem;

        void Start()
        {
            OpenCloseShop(false);
            BuySellTabSelection(true);
            PopulateShoppingListFirstTime();
        }

        public void OpenCloseShop(bool open)
        {
            gameObject.SetActive(open);
            ManagerLocator.Instance.PlayerController.ShopAccesed = open;

            if (!open) return;
            ManagerLocator.Instance.PlayerController.MyRigidBody.velocity = Vector2.zero;
            ManagerLocator.Instance.PlayerController.UpdatePlayerAnimation(Vector2.zero);
        }

        public void BuySellTabSelection(bool buyTabSelected)
        {
            buyItemButton.gameObject.SetActive(false);
            sellItemButton.gameObject.SetActive(false);
            equipItemButton.gameObject.SetActive(false);

            if (buyTabSelected)
            {
                buyingItemsTab.gameObject.SetActive(true);
                sellingItemsTab.gameObject.SetActive(false);
                _isBuyTabSelected = true;
            }
            else
            {
                buyingItemsTab.gameObject.SetActive(false);
                sellingItemsTab.gameObject.SetActive(true);
                _isBuyTabSelected = false;
            }
        }

        public void AddNewItemToBuyTab(ShopItem shopItem)
        {
            GameObject newItem = Instantiate(shopIconTemplate, parent: buyingItemsTab.transform.GetChild(0).GetChild(0));
            var iconData = newItem.GetComponent<ShopIconDisplay>();
            iconData.ShopItem = shopItem;
            iconData.sellingPrice.gameObject.SetActive(false);
            iconData.equipedIcon.gameObject.SetActive(false);
            iconData.purchasedIcon.gameObject.SetActive(false);
            IEnumerable<ShopItem> shopItemsToRemove =
                ManagerLocator.Instance.PlayerController.OwnedItems.Where(x => x.name == shopItem.name);
            foreach (ShopItem item in shopItemsToRemove)
            {
                ManagerLocator.Instance.PlayerController.OwnedItems.Remove(item);
            }
        }

        public void AddNewItemToSellTab(ShopItem shopItem, bool equip = false)
        {
            GameObject newItem = Instantiate(shopIconTemplate, parent: sellingItemsTab.transform.GetChild(0).GetChild(0));
            var iconData = newItem.GetComponent<ShopIconDisplay>();
            iconData.ShopItem = shopItem;
            iconData.buyingPrice.gameObject.SetActive(false);
            iconData.equipedIcon.gameObject.SetActive(equip);
            iconData.purchasedIcon.gameObject.SetActive(true);
            ManagerLocator.Instance.PlayerController.OwnedItems.Add(shopItem);
            if (equip)
            {
                ManagerLocator.Instance.PlayerController.EquipedItems.Add(shopItem);
            }
        }

        public void SetSelectItem(GameObject newSelectedItem)
        {
            _selectedItem = newSelectedItem;
            buyItemButton.gameObject.SetActive(_isBuyTabSelected);
            sellItemButton.gameObject.SetActive(!_isBuyTabSelected);
            equipItemButton.gameObject.SetActive(!_isBuyTabSelected && !ManagerLocator.Instance.PlayerController.EquipedItems.Contains(newSelectedItem.GetComponent<ShopIconDisplay>().ShopItem));
        }
        
        public void BuyItem()
        {
            if (_selectedItem != null)
            {
                AddNewItemToSellTab(_selectedItem.GetComponent<ShopIconDisplay>().ShopItem);
            }
            Destroy(_selectedItem);
        }
        
        public void SellItem()
        {
            
        }
        
        public void EquipItem()
        {
            
        }

        private void PopulateShoppingListFirstTime()
        {
            DestroyAllIconsInShoppingList();
            
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.greenShirt);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.greenPants);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.blueShoes);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.redShirt);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.redPants);

            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.blueShirt, equip: true);
            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.bluePants, equip: true);
            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.greenShoes, equip: true);
            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.redShoes, equip: false);
        }
        
        private void DestroyAllIconsInShoppingList()
        {
            var buyIcons = buyingItemsTab.GetComponentsInChildren<ShopIconDisplay>().Select(x => x.GameObject());
            foreach (var icon in buyIcons)
            {
                Destroy(icon);
            }

            var sellIcons = sellingItemsTab.GetComponentsInChildren<ShopIconDisplay>().Select(x => x.GameObject());
            foreach (var icon in sellIcons)
            {
                Destroy(icon);
            }
        }
    }
}
