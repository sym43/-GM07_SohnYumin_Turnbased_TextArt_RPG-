using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SadConsole.Readers.TheDrawFont;

public abstract class Hero : BattleEntity
{
    public ActiveSkill basicAttackSkill { get; protected set; }
    public ActiveSkill defendSkill {  get; protected set; }
    public ActiveSkill uniqueSkill {  get; protected set; }
    public PassiveSkill passive {  get; protected set; }
    public ActiveSkill nextSkill { get; protected set; }

    public Hero(string name, int hp, int atk, int def)
    {
        this.name = name;
        this.maxHp = hp;
        this.currentHp = maxHp;
        this.atk = atk;
        this.def = def;
    }
    public void Respawn()
    {
        currentHp = maxHp;
        effects.Clear();
        passive.Apply(this);
    }
    public void DecideNextSkill(ActiveSkill skill)
    {
        nextSkill = skill;
    }
}
