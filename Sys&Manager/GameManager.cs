using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameManager
{
    public DateTime programStartTime { get; private set; }
    public TitleScreen titleScreen { get; private set; }
    public MainScreen mainScreen { get; private set; }


    public Player player {  get; private set; }
    public Hero currentHero { get; private set; }
    // 맵 리스트와 현재 맵
    private Dictionary<string, Map> _maps = new Dictionary<string, Map>();
    public Map currentMap { get; private set; }

    
    public static GameManager inst { get; private set; } = new GameManager();
    public void StartProgram()
    {
        programStartTime = DateTime.Now;
        
        int width = Game.Instance.ScreenCellsX;
        int height = Game.Instance.ScreenCellsY;
        titleScreen = new TitleScreen(width, height);
        mainScreen = new MainScreen(width, height);
        
        Game.Instance.Screen = titleScreen;
        Game.Instance.Screen.IsFocused = true;
    }
    public void LoadGame()
    {
    }
    public void NewGame()
    {
        Game.Instance.Screen = mainScreen;
        CreateMaps();
        CreatePlayer();
        LoadMap("Home");
        mainScreen.IsFocused = true;
    }
    public void ExitGame()
    {
        System.Environment.Exit(0);
    }
    
    private void CreateMaps()
    {
        Map home = new HomeMap(mainScreen.mainView);
        Map battle1 = new BattleMap(mainScreen.mainView, 1);
        Map battle2 = new BattleMap(mainScreen.mainView, 2);
        Map battle3 = new BattleMap(mainScreen.mainView, 3);
        Map battle4 = new BattleMap(mainScreen.mainView, 4);
        Map battle5 = new BattleMap(mainScreen.mainView, 5);
        _maps.Add(home.mapName, home);
        _maps.Add(battle1.mapName, battle1);
        _maps.Add(battle2.mapName, battle2);
        _maps.Add(battle3.mapName, battle3);
        _maps.Add(battle4.mapName, battle4);
        _maps.Add(battle5.mapName, battle5);
    }
    private void CreatePlayer()
    {
        RegisterPlayer(new Player(new Point(mainScreen.mainView.Width / 2, mainScreen.mainView.Height / 2), '@', Color.Cyan));
        currentHero = new Warrior();
        currentHero.Respawn();
        mainScreen.CreateInvView(); // 플레이어를 만들고 인벤뷰에 등록해야함.
        mainScreen.CreateBattleView();
    }
    private void RegisterPlayer(Player player)
    {
        this.player = player;
    }
    public void LoadMap(string mapName)
    {
        currentMap = _maps[mapName];
        _maps[mapName].Init();
        RenderMap();
        player.TP(currentMap.spawnPoint);
        currentMap.entityManager.Add(player);
    }
    public void RenderMap()
    {
        currentMap.Draw();
    }
}
