using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IExecutable
{
    void Execute();
}
public interface IInteractable
{
    void Interact();
}
public interface IDestroyable
{
    void Destroy();
}
public interface IEquipable
{
    bool isEquipped { get; }
    void Equip();
    void UnEquip();
}
public interface IUseable
{
    void Use();
}