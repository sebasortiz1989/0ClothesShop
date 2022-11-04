using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject buyingItems;
        [SerializeField] private GameObject sellingItems;
        [SerializeField] private GameObject shopIconTemplate;
        private bool _isBuyTabSelected;

        void Start()
        {
            OpenCloseShop(false);
            BuySellTabSelection(true);
            PopulateShoppingList();
        }

        public void OpenCloseShop(bool open)
        {
            gameObject.SetActive(open);
            ManagerLocator.Instance.PlayerController.ShopAccesed = open;

            if (open)
            {
                ManagerLocator.Instance.PlayerController.MyRigidBody.velocity = Vector2.zero;
                ManagerLocator.Instance.PlayerController.UpdatePlayerAnimation(Vector2.zero);
            }
        }

        public void BuySellTabSelection(bool buyTabSelected)
        {
            if (buyTabSelected)
            {
                buyingItems.gameObject.SetActive(true);
                sellingItems.gameObject.SetActive(false);
                _isBuyTabSelected = true;
            }
            else
            {
                buyingItems.gameObject.SetActive(false);
                sellingItems.gameObject.SetActive(true);
                _isBuyTabSelected = false;
            }
        }
        
        public void AddNewItemToBuyList(ShopItem shopItem)
        {
            GameObject newItem = Instantiate(shopIconTemplate, buyingItems.transform);
            var iconData = newItem.GetComponent<ShopIconDisplay>();
            iconData.ShopItem = shopItem;
            iconData.sellingPrice.gameObject.SetActive(false);
            iconData.equipedIcon.gameObject.SetActive(false);
            iconData.purchasedIcon.gameObject.SetActive(false);
        }
    
        public void AddNewItemToSellList(ShopItem shopItem)
        {
            GameObject newItem = Instantiate(shopIconTemplate, sellingItems.transform);
            var iconData = newItem.GetComponent<ShopIconDisplay>();
            iconData.ShopItem = shopItem;
            iconData.buyingPrice.gameObject.SetActive(false);
            iconData.equipedIcon.gameObject.SetActive(false);
            iconData.purchasedIcon.gameObject.SetActive(false);
        }

        private void PopulateShoppingList()
        {
            DestroyAllIconsInShoppingList();
            
            for (int i = 0; i < 25; i++)
            {
                AddNewItemToBuyList(ManagerLocator.Instance.WardroveManager.orangeShoes);
            }
            
            for (int i = 0; i < 5; i++)
            {
                AddNewItemToSellList(ManagerLocator.Instance.WardroveManager.orangeShoes);
            }
        }
        
        private void DestroyAllIconsInShoppingList()
        {
            var buyIcons = buyingItems.GetComponentsInChildren<ShopIconDisplay>().Select(x => x.GameObject());
            foreach (var icon in buyIcons)
            {
                Destroy(icon);
            }

            var sellIcons = sellingItems.GetComponentsInChildren<ShopIconDisplay>().Select(x => x.GameObject());
            foreach (var icon in sellIcons)
            {
                Destroy(icon);
            }
        }
    }
}
