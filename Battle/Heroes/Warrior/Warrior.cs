using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Warrior : Hero
{
    public Warrior() : base("[ Warrior ]", 150, 10, 0)
    {
        basicAttackSkill = new WarriorBasicAttack(this);
        defendSkill = new WarriorDefend(this);
        uniqueSkill = new WarriorUnique(this);
        passive = new WarriorPassive(this);
    }
}