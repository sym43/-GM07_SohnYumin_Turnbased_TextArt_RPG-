using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Skill
{
    public string name { get; protected set; }
    public string desc { get; protected set; } = "";
    public Skill(string name, BattleEntity fromEntity)
    {
        this.name = name;
        UpdateDesc(fromEntity);
    }
    public abstract void Use(BattleEntity victim, BattleEntity attacker);
    public abstract void UpdateDesc(BattleEntity fromEntity);
}
public abstract class ActiveSkill : Skill
{
    public ActiveSkill(string name, BattleEntity fromEntity) : base(name, fromEntity)
    {
    }
}
public abstract class PassiveSkill : Skill
{
    public PassiveSkill(string name, BattleEntity fromEntity) : base(name, fromEntity)
    {
    }
    public override void Use(BattleEntity victim, BattleEntity attacker) { }
    public abstract void Apply(BattleEntity target);
}