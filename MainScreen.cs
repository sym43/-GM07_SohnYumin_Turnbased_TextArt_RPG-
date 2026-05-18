using SadConsole;
using SadConsole.EasingFunctions;
using SadConsole.Entities;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Security.Cryptography;
using static Microsoft.Xna.Framework.Graphics.SpriteFont;



public class MainScreen : ScreenSurface
{
    // 씬의 메인 사이드 로그 뷰
    public ScreenSurface mainView { get; private set; }
    public ScreenSurface sideView {  get; private set; }
    public ScreenSurface logView { get; private set; }
    public InventoryView inventoryView { get; private set; }
    public BattleView battleView { get; private set; }





    public MainScreen(int width, int height) : base(width, height)
    {
        InitViews();
        
        
    }
    private void InitViews()
    {
        int splitX = Width / 4 * 3;
        int splitY = Height / 4 * 3;
        // 4분의 3지점을 중심점으로 잡고 구현
        mainView = new ScreenSurface(splitX, splitY);
        mainView.Position = new Point(0, 0);
        mainView.Parent = this;

        sideView = new ScreenSurface(Width - splitX, splitY);
        sideView.Position = new Point(splitX, 0);
        sideView.Parent = this;

        logView = new ScreenSurface(Width, Height - splitY);
        logView.Position = new Point(0, splitY);
        logView.Parent = this;

        

        DrawViewBorder(mainView, "[ MAIN VIEW ]", Color.Yellow);
        DrawViewBorder(sideView, "[ SIDE VIEW ]", Color.Cyan);
        DrawViewBorder(logView, "[ LOG VIEW ]", Color.Green);
        LogManager.inst.Init(logView);
        // 레이아웃 일단 그리기

    }
    public void CreateInvView()
    {
        inventoryView = new InventoryView(mainView.Width - 2, mainView.Height - 2, GameManager.inst.player.inv);
        inventoryView.Position = new Point(1, 1);
        Children.Add(inventoryView);
    }
    public void CreateBattleView()
    {
        battleView = new BattleView(mainView.Width - 2, mainView.Height - 2);
        battleView.Position = new Point(1, 1);
        Children.Add(battleView);
    }
    private void DrawViewBorder(ScreenSurface surface, string title, Color color)
    {
        surface.Clear();
        surface.DrawBox(new Rectangle(0, 0, surface.Width, surface.Height),
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThin,
            new ColoredGlyph(Color.White, Color.Black)));
        surface.Print(2, 0, title, color);
    }

    
    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        // 인벤토리 창이 켜져 있으면, 메인 맵은 키보드 입력을 무시
        if (inventoryView.IsVisible)
            return false;

        // 인벤토리 열기
        if (keyboard.IsKeyPressed(Keys.E))
        {
            inventoryView.OpenInv();
            return true;
        }

        if (keyboard.IsKeyPressed(Keys.W)) { GameManager.inst.player.Move(Direction.Up); return true; }
        else if (keyboard.IsKeyPressed(Keys.S)) { GameManager.inst.player.Move(Direction.Down); return true; }
        else if (keyboard.IsKeyPressed(Keys.A)) { GameManager.inst.player.Move(Direction.Left); return true; }
        else if (keyboard.IsKeyPressed(Keys.D)) { GameManager.inst.player.Move(Direction.Right); return true; }
        else if (keyboard.IsKeyPressed(Keys.Space)) { GameManager.inst.player.TryInteraction(); return true; }

        return base.ProcessKeyboard(keyboard);
    }
}