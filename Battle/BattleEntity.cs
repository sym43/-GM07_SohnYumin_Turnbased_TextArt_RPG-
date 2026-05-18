using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class BattleEntity
{
    public string name { get; protected set; }
    public int maxHp { get; protected set; }
    public int currentHp { get; protected set; }
    public int baseAtk { get; protected set; }
    public int baseDef { get; protected set; }
    public int atk;
    public int def;
    public float dodgeChance = 0.0f; // 0.0~1.0 확률로.
    public bool isStunned = false;
    public bool isDead = false;
    public List<StatusEffect> effects { get; protected set; } = new List<StatusEffect>();

    public void AddEffect(StatusEffect effect)
    {
        effect.OnApply(this);
        effects.Add(effect);
    }
    /// <summary>
    /// 턴 시작가 끝에 실행하기
    /// </summary>
    /// <param name="isTickEarly"></param>
    public void TickEffects(bool isTickEarly)
    {
        for (int i = effects.Count - 1; i >= 0; i--)
        {
            if (effects[i].isTickEarly != isTickEarly) continue;

            effects[i].OnTick(this);
            if (effects[i].IsEnded())
            {
                effects[i].OnRemove(this);
                effects.RemoveAt(i);
            }
        }
    }
    public virtual void TakeDamage(int dmg, out bool isDoged, bool isTrueDmg = false)
    {
        // 회피 검사
        if (new Random().NextDouble() < dodgeChance && !isTrueDmg)
        {
            LogManager.inst.AddLog($"{name} success Dodge!");
            isDoged = true;
            return;
        }
        isDoged = false;
        dmg = isTrueDmg ? dmg : Math.Max(0, (int)(dmg * ((100f - (float)def) / 100f)));
        currentHp = Math.Max(0, currentHp - dmg);
        LogManager.inst.AddLog($"{name} took {dmg} damage");
        
        if (currentHp <= 0) isDead = true;
    }
    public virtual void OnDead()
    {
        LogManager.inst.AddLog($"{name} is dead");
    }
    public virtual void Heal(int healAmount)
    {
        currentHp = Math.Clamp(currentHp + healAmount, 0, maxHp);
    }
}
