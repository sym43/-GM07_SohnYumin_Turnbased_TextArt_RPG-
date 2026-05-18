using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Rogue : Hero
{
    public Rogue() : base("[ Rogue ]", 100, 10, 0)
    {
        basicAttackSkill = new RogueBasicAttack(this);
        defendSkill = new RogueDefend(this);
        uniqueSkill = new RogueUnique(this);
        passive = new RoguePassive(this);
    }

}