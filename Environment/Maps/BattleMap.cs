using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BattleMap : Map
{
    public int stageLevel {  get; set; }
    public BattleMap(ScreenSurface mainView, int stageLevel) :
        base($"Battle{stageLevel}", mainView.Width, mainView.Height, mainView, new Point(mainView.Width / 2, mainView.Height / 2 + 10))
    {
        this.stageLevel = stageLevel;
        AddMapObject(new TPZone("Home", new Point((Width / 2) - (15 / 2), Height - 6), (char)219, Color.DarkGray, 15, 5, Direction.Types.Up));
        if(stageLevel == 5)
        {
            AddMapObject(new TPZone($"Home", new Point(Width / 2 - 15 / 2, Height / 2 - 5), (char)219, Color.DarkGray, 15, 5, Direction.Types.Down));
        }
        else
        {
            AddMapObject(new TPZone($"Battle{stageLevel + 1}", new Point(Width / 2 - 15 / 2, Height / 2 - 5), (char)219, Color.DarkGray, 15, 5, Direction.Types.Down, "Next Area"));
        }

        SetStage(this.stageLevel);
    }
    public void SetStage(int stageLevel)
    {
        Random rand = new Random();
        int rate = rand.Next(0, 2);
        switch (stageLevel)
        {
            case <= 2:
                if(rate == 0)
                {
                    //슬라임
                    AddMapObject(new Enemy(new Point(Width / 2, Height / 2), '&', Color.Red, 1, 1, new Slime()));
                    break;
                }
                AddMapObject(new Enemy(new Point(Width / 2, Height / 2), '&', Color.Red, 1, 1, new Goblin()));
                //고블린
                break;
            case <=4:
                if (rate == 0)
                {
                    //마녀
                    AddMapObject(new Enemy(new Point(Width / 2, Height / 2), '&', Color.Red, 1, 1, new Witch()));
                    break;
                }
                AddMapObject(new Enemy(new Point(Width / 2, Height / 2), '&', Color.Red, 1, 1, new Demon()));
                //악마
                break;
            default:
                //보스
                AddMapObject(new Enemy(new Point(Width / 2, Height / 2), '&', Color.Red, 1, 1, new DemonKing()));
                break;
        }
    }
    public override void Init()
    {
        mapObjects.Clear();
        AddMapObject(new TPZone("Home", new Point((Width / 2) - (15 / 2), Height - 6), (char)219, Color.DarkGray, 15, 5, Direction.Types.Up));
        if (stageLevel == 5)
        {
            AddMapObject(new TPZone($"Home", new Point(Width / 2 - 15 / 2, Height / 2 - 5), (char)219, Color.DarkGray, 15, 5, Direction.Types.Down));
        }
        else
        {
            AddMapObject(new TPZone($"Battle{stageLevel + 1}", new Point(Width / 2 - 15 / 2, Height / 2 - 5), (char)219, Color.DarkGray, 15, 5, Direction.Types.Down, "Next Area"));
        }
        SetStage(this.stageLevel);
    }
    public override void Draw()
    {
        base.Draw();
        string levelText = $"[Stage Level : {stageLevel}]";
        mainView.Surface.Print(Width/2 - 9, 1, levelText, Color.Yellow);
    }
}
