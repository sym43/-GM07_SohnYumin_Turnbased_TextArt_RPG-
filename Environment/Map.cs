using SadConsole;
using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Map : ScreenSurface
{
    public string mapName { get; private set; }
    public ScreenSurface mainView { get; protected set; }
    public Point spawnPoint { get; private set; }
    public List<GameObject> mapObjects { get; private set; } = new List<GameObject>();
    public EntityManager entityManager { get; private set; }
    
    public Map(string mapName, int width, int height, ScreenSurface mainView, Point spawnPoint) : base(width, height)
    {
        this.mapName = mapName;
        this.mainView = mainView;
        Parent = mainView;
        this.spawnPoint = spawnPoint;
        entityManager = new EntityManager();
        SadComponents.Add(entityManager);
        this.mapName = mapName;
    }
    public void AddMapObject(GameObject gameObject)
    {
        mapObjects.Add(gameObject);
    }
    public virtual void Init()
    {

    }
    public virtual void Draw()
    {
        // 메인 뷰 안쪽만 지우기
        mainView.Surface.Clear(new Rectangle(1, 1, mainView.Width - 2, mainView.Height - 2));
        // 오브젝트들 그리기
        foreach (var obj in mapObjects) obj.Draw(mainView);
    }
}