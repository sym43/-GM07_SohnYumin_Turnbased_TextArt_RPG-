using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WarriorBasicAttack : ActiveSkill
{
    private float _amount = 1.0f;
    public WarriorBasicAttack(BattleEntity fromEntity) : base("[Basic Attack]", fromEntity)
    {
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDmg = (int)(fromEntity.atk * _amount);
        desc = $"Attack {gainDmg}({fromEntity.atk} * {_amount}) damage to target";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDmg = (int)(attacker.atk * _amount);
        LogManager.inst.AddLog($"{attacker.name} use {name} => {victim.name}");
        victim.TakeDamage(gainDmg, out bool isDoged);
    }
}
public class WarriorDefend : ActiveSkill
{
    private float _amount = 0.6f;
    public WarriorDefend(BattleEntity fromEntity) : base("[Defend]", fromEntity)
    {
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDef = (int)(_amount * 100);
        desc = $"Gain Damage reduction rate +{gainDef}% (Total Damage Reduce Rate : {fromEntity.def + gainDef}%)";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDef = (int)(victim.def * _amount);
        LogManager.inst.AddLog($"{attacker.name} Use Defend. Damage reduction rate +{(int)(_amount*100)}%");
        victim.AddEffect(new DefendBuff((int)(_amount * 100)));
    }
}
public class WarriorUnique : ActiveSkill
{
    private float _amount = 0.2f;
    public WarriorUnique(BattleEntity fromEntity) : base("[Stun Attack]", fromEntity)
    {
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDmg = (int)(fromEntity.atk * _amount);
        desc = $"Attack {gainDmg}({fromEntity.atk} * {_amount}) damage to target. And Apply [Stun Debuff] next turn";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDmg = (int)(attacker.atk * _amount);
        LogManager.inst.AddLog($"{attacker.name} use {name} => {victim.name}");
        victim.TakeDamage(gainDmg, out bool isDoged);
        if (isDoged)
        {
            LogManager.inst.AddLog($"{victim.name} Doged. [Stun Debuff] is not Applied.");
        }
        else
        {
            victim.AddEffect(new WarriorStunDebuff());
        }
    }
}
public class WarriorPassive : PassiveSkill
{
    public WarriorPassive(BattleEntity fromEntity) : base("[Resist]", fromEntity)
    {
    }


    public override void Apply(BattleEntity target)
    {
        target.AddEffect(new WarriorResistBuff());
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        desc = $"The Warrior gained +20 Def from the armor supplied by the army.";
    }
}