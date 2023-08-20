using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class Item : ScriptableObject
{
    [SerializeField] int id;
    [SerializeField] public string itemName;
    [SerializeField] public int value;
    [SerializeField] public int quantity;
    [SerializeField] public Sprite icon;
    [SerializeField] public ItemType itemType;

    public enum ItemType
    {
        BookOfXP,
        RingOfHealth
    }
}