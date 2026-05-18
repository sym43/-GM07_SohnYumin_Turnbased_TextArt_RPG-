using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Monster : BattleEntity
{
    public static Random rand { get; private set; } = new Random();

    public ActiveSkill basicAttackSkill { get; protected set; }
    public ActiveSkill defendSkill { get; protected set; }
    public ActiveSkill nextSkill { get; protected set; }
    public int exp {  get; protected set; }

    public Monster(string name, int hp, int atk, int def, int exp)
    {
        this.name = name;
        this.maxHp = hp;
        this.currentHp = maxHp;
        this.atk = atk;
        this.def = def;
        this.exp = exp;
    }
    public virtual void DecideNextSkill()
    {

    }
    public void Execute(BattleEntity target)
    {
        nextSkill.Use(target, this);
    }
}







public class MonsterBasicAttack : ActiveSkill
{
    public float amount { get; private set; }
    public MonsterBasicAttack(BattleEntity fromEntity, float amount) : base("[Basic Attack]", fromEntity)
    {
        this.amount = amount;
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDmg = (int)(fromEntity.atk * amount);
        desc = $"Attack {gainDmg}({fromEntity.atk} * {amount}) damage to target";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDmg = (int)(attacker.atk * amount);
        LogManager.inst.AddLog($"{attacker.name} use {name} => {victim.name}");
        victim.TakeDamage(gainDmg, out bool isDoged);
    }
}
public class MonsterDefend : ActiveSkill
{
    public float amount { get; private set; }
    public MonsterDefend(BattleEntity fromEntity, float amount) : base("[Defend]", fromEntity)
    {
        this.amount = amount;
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDef = (int)(amount * 100);
        desc = $"Gain Damage reduction rate +{gainDef}% (Total Damage Reduce Rate : {fromEntity.def + gainDef}%)";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDef = (int)(victim.def * amount);
        LogManager.inst.AddLog($"{attacker.name} Use Defend. Damage reduction rate +{(int)(amount * 100)}%");
        victim.AddEffect(new DefendBuff((int)(amount * 100)));
    }
}