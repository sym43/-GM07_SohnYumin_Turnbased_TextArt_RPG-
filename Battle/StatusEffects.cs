using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SadConsole.Readers.TheDrawFont;

public abstract class StatusEffect
{
    public string name { get; private set; }
    public string desc { get; private set; }
    public int duration { get; private set; }
    public bool isTickEarly;
    public StatusEffect(string name, string desc, int duration, bool isTickEarly = true)
    {
        this.name = name;
        this.desc = desc;
        this.duration = duration;
        this.isTickEarly = isTickEarly;
    }
    public bool IsEnded()
    {
        return duration == 0;
    }
    public virtual void OnApply(BattleEntity target) { }
    public virtual void OnTick(BattleEntity target)
    {
        if (duration > 0) duration--;
    }
    public virtual void OnRemove(BattleEntity target) { }
}













public class RogueEvasionBuff : StatusEffect
{
    public static float amount { get; private set; } = 0.25f;
    public RogueEvasionBuff() : base("Evasion (Rogue)", $"Gain Dodge rate {(int)(amount * 100)}%", -1)
    {
        
    }
    public override void OnApply(BattleEntity target)
    {
        target.dodgeChance += amount;
    }
    public override void OnRemove(BattleEntity target)
    {
        target.dodgeChance -= amount;
    }
}

public class Bleeding : StatusEffect
{
    public int amount { get; private set; }
    public Bleeding(int amount, int duration) : base("[Bleeding]", $"Took {amount} [True damage] for {duration} turn", duration)
    {
        this.amount = amount;
    }
    public override void OnApply(BattleEntity target)
    {
        LogManager.inst.AddLog($"{target.name} is Bleeding!!");
    }
    public override void OnTick(BattleEntity target)
    {
        base.OnTick(target);
        LogManager.inst.AddLog($"{target.name} still Bleeding!!");
        target.TakeDamage(amount, out bool isDoged, true);
    }
    public override void OnRemove(BattleEntity target)
    {
        LogManager.inst.AddLog($"{target.name}'s Bleeding has stopped");
    }
}
public class DefendBuff : StatusEffect
{
    public int amount { get; private set; }
    public DefendBuff(int amount) : base("Defend", $"Gain +{amount} Def", 1, false)
    {
        this.amount = amount;
    }
    public override void OnApply(BattleEntity target)
    {
        target.def += amount;
    }
    public override void OnRemove(BattleEntity target)
    {
        target.def -= amount;
    }
}
public class WarriorResistBuff : StatusEffect
{
    public static int amount { get; private set; } = 20;
    public WarriorResistBuff() : base("Resist (Warrior)", $"Gain +20 Def", -1)
    {

    }
    public override void OnApply(BattleEntity target)
    {
        target.def += amount;
    }
    public override void OnRemove(BattleEntity target)
    {
        target.def -= amount;
    }
}
public class WarriorStunDebuff : StatusEffect
{
    public WarriorStunDebuff() : base("Stun (Warrior)", "Stun 1 turn.", 1)
    {

    }
    public override void OnApply(BattleEntity target)
    {
        target.isStunned = true;
        LogManager.inst.AddLog($"{target.name} is Stunned!!");
    }
    public override void OnRemove(BattleEntity target)
    {
        target.isStunned = false;
    }
}
