using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DemonKing : Monster
{
    private ActiveSkill _uniqueSkill;
    public DemonKing() : base("[ Demon King ]", 500, 20, 0, 10)
    {
        basicAttackSkill = new MonsterBasicAttack(this, 1.0f);
        defendSkill = new MonsterDefend(this, 0.9f);
        _uniqueSkill = new DemonKingUniqueSkill(this, 2.0f);
    }
    public override void DecideNextSkill()
    {
        int decideRate = rand.Next(0, 100);
        // https://dev-junwoo.tistory.com/90 스위치 식 사용법
        nextSkill = decideRate switch
        {
            < 45 => basicAttackSkill,
            < 90 => defendSkill,
            _ => _uniqueSkill
        };
    }
}


public class DemonKingUniqueSkill : ActiveSkill
{
    public float amount { get; private set; }
    public DemonKingUniqueSkill(BattleEntity fromEntity, float amount) : base("[Devil's power]", fromEntity)
    {
        this.amount = amount;
    }

    public override void UpdateDesc(BattleEntity fromEntity)
    {
        int gainDmg = (int)(fromEntity.atk * amount);
        desc = $"A very powerful demonic power";
    }

    public override void Use(BattleEntity victim, BattleEntity attacker)
    {
        int gainDmg = (int)(attacker.atk * amount);
        LogManager.inst.AddLog($"{attacker.name} use {name} => {victim.name}");
        victim.TakeDamage(gainDmg, out bool isDoged);
        victim.AddEffect(new Bleeding((int)(victim.maxHp * 0.05f), 10));
    }
}