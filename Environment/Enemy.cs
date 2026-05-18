using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Enemy : ObstacleZone, IInteractable
{
    public Monster monster { get; private set; }
    public Enemy(Point pos, char glyph, Color color, int width, int height, Monster monster) : base(pos, glyph, color, width, height)
    {
        this.monster = monster;
    }
    public void Interact()
    {
        BattleManager.inst.EnterBattle(GameManager.inst.currentHero, monster);
    }
}
