using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Slime : Monster
{
    public Slime() : base("[ Slime ]", 100, 10, 0, 10)
    {
        basicAttackSkill = new MonsterBasicAttack(this, 1.0f);
        defendSkill = new MonsterDefend(this, 0.1f);
    }
    public override void DecideNextSkill()
    {
        int decideRate = rand.Next(0, 100);
        if(decideRate < 50)
        {
            nextSkill = basicAttackSkill;
            return;
        }
        nextSkill = defendSkill;
    }
}