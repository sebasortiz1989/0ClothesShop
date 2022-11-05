using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Shop Item")]
public class ShopItem : ScriptableObject
{
    public new string name;
    public Sprite picture;
    public string buyingPrice;
    public string sellingPrice;
    public Sprite equipedIcon;
    public Sprite purchasedIcon;
    public ClothingType clothingType;
}

public enum ClothingType
{
    Shirt,
    Pants,
    Shoes,
}
