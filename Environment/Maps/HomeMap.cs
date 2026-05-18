using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HomeMap : Map
{
    public HomeMap(ScreenSurface mainView) :
        base("Home", mainView.Width, mainView.Height, mainView, new Point(mainView.Width / 2, mainView.Height / 2))
    {
        AddMapObject(new TPZone("Shop", new Point((Width / 2) - (15 * 1) - (15 / 2) - 4, 3), (char)219, Color.DarkGray, 15, 9)); // 219는 꽉찬 네모
        AddMapObject(new TPZone("Battle1", new Point((Width / 2) - (15 / 2), 3), (char)219, Color.DarkGray, 15, 9, destinationText: "Battle"));
        AddMapObject(new TPZone("Change Hero", new Point((Width / 2) + (15 * 1) - (15 / 2) + 4, 3), (char)219, Color.DarkGray, 15, 9));
    }
    public override void Init()
    {
        mapObjects.Clear();
        AddMapObject(new TPZone("Shop", new Point((Width / 2) - (15 * 1) - (15 / 2) - 4, 3), (char)219, Color.DarkGray, 15, 9)); // 219는 꽉찬 네모
        AddMapObject(new TPZone("Battle1", new Point((Width / 2) - (15 / 2), 3), (char)219, Color.DarkGray, 15, 9, destinationText: "Battle"));
        AddMapObject(new TPZone("Change Hero", new Point((Width / 2) + (15 * 1) - (15 / 2) + 4, 3), (char)219, Color.DarkGray, 15, 9));
    }
}
