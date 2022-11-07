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
            PopulateShoppingListFirstTime();
        }

        public void OpenCloseShop(bool open)
        {
            BuySellTabSelection(true);
            gameObject.SetActive(open);
            ManagerLocator.Instance.PlayerController.ShopAccesed = open;

            if (!open) { return; }
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
            
            for (var i = 0; i < ManagerLocator.Instance.PlayerController.OwnedItems.Count; i++)
            {
                if (ManagerLocator.Instance.PlayerController.OwnedItems[i].name != shopItem.name) continue;
                ManagerLocator.Instance.PlayerController.OwnedItems.RemoveAt(i);
                i--;
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
            if (_selectedItem == null) return;
            AddNewItemToSellTab(_selectedItem.GetComponent<ShopIconDisplay>().ShopItem);
            Destroy(_selectedItem);
        }
        
        public void SellItem()
        {
            if (_selectedItem == null) return;
            if (!_selectedItem.GetComponent<ShopIconDisplay>().equipedIcon.IsActive())
            {
                AddNewItemToBuyTab(_selectedItem.GetComponent<ShopIconDisplay>().ShopItem);
                Destroy(_selectedItem);    
            }
            else
            {
                _selectedItem = null;
            }
        }
        
        public void EquipItem()
        {
            if (_selectedItem == null) return;
            ShopIconDisplay shopIconDisplay = _selectedItem.GetComponent<ShopIconDisplay>();
            shopIconDisplay.equipedIcon.gameObject.SetActive(true);

            for (var i = 0; i < ManagerLocator.Instance.PlayerController.EquipedItems.Count; i++)
            {
                if (ManagerLocator.Instance.PlayerController.EquipedItems[i].clothingType != shopIconDisplay.ShopItem.clothingType) continue;
                ManagerLocator.Instance.PlayerController.EquipedItems.RemoveAt(i);
                i--;
            }
            ManagerLocator.Instance.PlayerController.EquipedItems.Add(shopIconDisplay.ShopItem);

            var sellingIcons = sellingItemsTab.GetComponentsInChildren<ShopIconDisplay>();
            foreach (var item in sellingIcons)
            {
                if (item.ShopItem.clothingType != shopIconDisplay.ShopItem.clothingType || item.ShopItem == shopIconDisplay.ShopItem) continue;
                item.equipedIcon.gameObject.SetActive(false);
            }

            var equipedItemsString = ManagerLocator.Instance.PlayerController.EquipedItems.Select(x => x.name.ToString()).ToArray();
            Debug.Log(message: $"Equiped items: {equipedItemsString[0]}, {equipedItemsString[1]}, {equipedItemsString[2]}");
            ManagerLocator.Instance.PlayerController.UpdateCloths();
        }

        private void PopulateShoppingListFirstTime()
        {
            DestroyAllIconsInShoppingList();
            
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.greenShirt);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.blueShoes);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.redShirt);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.redPants);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.bluePants);
            AddNewItemToBuyTab(ManagerLocator.Instance.WardroveManager.redShoes);

            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.blueShirt, equip: true);
            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.greenPants, equip: true);
            AddNewItemToSellTab(ManagerLocator.Instance.WardroveManager.greenShoes, equip: true);
            
            ManagerLocator.Instance.PlayerController.UpdateCloths();
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
