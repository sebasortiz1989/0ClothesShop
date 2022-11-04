using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject buyingItems;
    [SerializeField] private GameObject sellingItems;
    [SerializeField] private GameObject shopIconTemplate;
    
    void Start()
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

        for (int i = 0; i < 20; i++)
        {
            AddNewItemToBuyList(ManagerLocator.Instance.shopManager.orangeShoes);
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
}
