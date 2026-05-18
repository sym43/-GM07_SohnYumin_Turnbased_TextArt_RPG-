using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SadConsole.Readers.TheDrawFont;

public class RogueBasicAttack : ActiveSkill
{
    private float _amount = 0.4f;
    public RogueBasicAttack(BattleEntity fromEntity) : base("[Basic Attack]", fromEntity)
    {
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDmg = (int)(fromEntity.atk * _amount);
        desc = $"Attack twice with {gainDmg}({fromEntity.atk} * {_amount}) damage to target";
    }
    // 크리티컬 시스템 추가시 이득인 스킬.
    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDmg = (int)(attacker.atk * _amount);
        LogManager.inst.AddLog($"{attacker.name} use {name} => {victim.name}");
        victim.TakeDamage(gainDmg, out bool isDoged);
        victim.TakeDamage(gainDmg, out isDoged);
    }
}
public class RogueDefend : ActiveSkill
{
    private float _amount = 0.2f;
    public RogueDefend(BattleEntity fromEntity) : base("[Defend]", fromEntity)
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
        LogManager.inst.AddLog($"{attacker.name} Use Defend. Damage reduction rate +{(int)(_amount * 100)}%");
        victim.AddEffect(new DefendBuff((int)(_amount * 100)));
    }
}
public class RogueUnique : ActiveSkill
{
    private float _amount = 0.2f;
    public RogueUnique(BattleEntity fromEntity) : base("[Bleeding Slash]", fromEntity)
    {
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDmg = (int)(fromEntity.atk * _amount);
        desc = $"Attack {gainDmg}({fromEntity.atk} * {_amount}) damage to target. And Apply [Bleeding Debuff] for 5 turn";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDmg = (int)(attacker.atk * _amount);
        LogManager.inst.AddLog($"{attacker.name} use {name} => {victim.name}");
        victim.TakeDamage(gainDmg, out bool isDoged);
        if (isDoged)
        {
            LogManager.inst.AddLog($"{victim.name} Doged. [Bleeding Debuff] is not Applied.");
        }
        else
        {
            // 적의 최대 체력의 5% 5턴
            victim.AddEffect(new Bleeding((int)(victim.maxHp * 0.05f), 5));
        }
    }
}
public class RoguePassive : PassiveSkill
{
    public RoguePassive(BattleEntity fromEntity) : base("[Evasion]", fromEntity)
    {
    }


    public override void Apply(BattleEntity target)
    {
        target.AddEffect(new RogueEvasionBuff());
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        desc = $"Rogue evades attacks with a 25% chance as a result of training.";
    }
}
