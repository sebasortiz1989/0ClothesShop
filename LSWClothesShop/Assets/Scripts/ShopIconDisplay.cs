using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopIconDisplay : MonoBehaviour
{
    [SerializeField] private ShopItem shopItem;
    [SerializeField] private Image picture;
    public TextMeshProUGUI buyingPrice;
    public TextMeshProUGUI sellingPrice;
    public Image equipedIcon;
    public Image purchasedIcon;

    public ShopItem ShopItem
    {
        get => shopItem;
        set
        {
            shopItem = value;
            UpdateShopItem();
        }
    }

    private void UpdateShopItem()
    {
        picture.sprite = shopItem.picture;
        buyingPrice.text = $"${shopItem.buyingPrice}";
        sellingPrice.text =$"${shopItem.sellingPrice}";
        equipedIcon.sprite = shopItem.equipedIcon;
        purchasedIcon.sprite = shopItem.purchasedIcon;
    }
}
