using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum ItemTier
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legend,
}
public abstract class Item
{
    public string name { get; private set; }
    public string desc { get; private set; }
    public int price { get; private set; }
    public ItemTier tier { get; private set; }
    public Dictionary<ItemTier, Color> tierColor { get; private set; } = new Dictionary<ItemTier, Color>()
    {
        { ItemTier.Common, Color.White },
        { ItemTier.Uncommon, new Color(0, 255, 0) },
        { ItemTier.Rare, Color.Cyan },
        { ItemTier.Epic, new Color(255, 0, 255) },
        { ItemTier.Legend, Color.Orange },
    };
    public Item(string name, string desc, int price, ItemTier tier)
    {
        this.name = name;
        this.desc = desc;
        this.price = price;
        this.tier = tier;
    }
    public virtual void AddLogForItem(string eventText)
    {
        LogManager.inst.AddLog(this, eventText);
    }
}
public class Potion : Item, IUseable
{
    public int healAmount { get; private set; }
    public Potion(string name, string desc, int price, ItemTier tier, int healAmount) : base(name, desc, price, tier)
    {
        this.healAmount = healAmount;
    }
    public virtual void Use()
    {
    }
}
public class Sword : Item, IEquipable
{
    public bool isEquipped { get; private set; } = false;
    public int atk { get; private set; }
    public Sword(string name, string desc, int price, ItemTier tier, int atk) : base(name, desc, price, tier)
    {
        this.atk = atk;
    }

    public void Equip()
    {
        isEquipped = true;
    }

    public void UnEquip()
    {
        isEquipped = false;
    }
}
public class Armor : Item, IEquipable
{
    public bool isEquipped { get; private set; } = false;
    public int def { get; private set; }
    public Armor(string name, string desc, int price, ItemTier tier, int atk) : base(name, desc, price, tier)
    {
        this.def = def;
    }

    public void Equip()
    {

    }

    public void UnEquip()
    {

    }
}