using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Inventory
{
    public List<Item> items { get; private set; } = new List<Item>();
    public int money { get; private set; } = 0;
    private int _capacity = 20;
    public void AddItem(Item item)
    {
        if (items.Count >= _capacity)
        {
            LogManager.inst.AddLog("Inventory is full !!");
            return;
        }
        items.Add(item);
        item.AddLogForItem("Pick Up");
    }
    public void RemoveItem(Item item)
    {
        // 아이템이 장착중이면 못버림.
        if (item is IEquipable equipableItem)
        {
            if (equipableItem.isEquipped)
            {
                LogManager.inst.AddLog("You can't remove Equip item");
                return;
            }
        }
        if (items.Remove(item))
        {
            item.AddLogForItem("Remove");
        }
        // 일어날 일 거의 없지만 안전장치
        else
        {
            LogManager.inst.AddLog("You don't have this item");
        }
    }
}
