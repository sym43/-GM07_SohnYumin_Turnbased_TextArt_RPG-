using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;

public class BattleView : ScreenSurface
{
    private Hero _player; 
    private Monster _enemy;

    private int _selectedIndex = 0;
    private List<string> _menuOptions = new List<string> { "1. Skill", "2. Inventory", "3. Surrender" };

    // 자식 뷰들
    public SkillView skillView { get; private set; }
    public InventoryView inventoryView { get; private set; }

    public BattleView(int width, int height) : base(width, height)
    {
        UseKeyboard = true;
        IsVisible = false;
        IsFocused = false;
        // 스킬뷰는 여기서 만들기
        skillView = new SkillView(width, height);
        skillView.Parent = this;
        inventoryView = GameManager.inst.mainScreen.inventoryView;
        Surface.DefaultBackground = Color.Black;
    }
    /// <summary>
    /// 배틀 뷰 보여주는 내용 배틀 시작전에 초기화.
    /// </summary>
    public void InitWhenStartBattle(Hero player, Monster enemy)
    {
        this._player = player;
        this._enemy = enemy;
        // 설명창 등
    }
    public void OpenView()
    {
        Parent.IsFocused = false;
        IsVisible = true;
        IsFocused = true;
        inventoryView.Parent = this;
        inventoryView.Position = new Point(0, 0);
        Render();
    }
    public void CloseView()
    {
        IsVisible = false;
        IsFocused = false;
        inventoryView.Parent = this.Parent;
        inventoryView.Position = new Point(1, 1);
        Parent.IsFocused = true;
    }

    public void Render()
    {
        Surface.Clear();
        Surface.DrawBox(new Rectangle(0, 0, Width, Height), ShapeParameters.CreateStyledBoxThin(Color.White));

        int midX = Width / 2;
        int midY = Height / 2;
        
        int playerSizeX = 10;
        int playerSizeY = 6;
        int playerOffsetX = midX - (playerSizeX*3);
        int playerOffsetY = midY - ((playerSizeY - 2)/2);
        Rectangle playerRect = new Rectangle(playerOffsetX, playerOffsetY, playerSizeX, playerSizeY);
        Surface.DrawBox(playerRect, ShapeParameters.CreateStyledBoxThick(Color.Cyan));
        Surface.Print(playerOffsetX, playerOffsetY-1, $"{_player.name}", Color.Cyan);
        Surface.Print(playerOffsetX, playerOffsetY + playerSizeY, $"{_player.currentHp}/{_player.maxHp}", Color.Cyan);

        
        int enemySizeX = 10;
        int enemySizeY = 6;
        int enemyOffsetX = midX + (enemySizeX * 2);
        int enemyOffsetY = midY - ((enemySizeY - 2) / 2);
        Rectangle enemyRect = new Rectangle(enemyOffsetX, enemyOffsetY, enemySizeX, enemySizeY);
        Surface.DrawBox(enemyRect, ShapeParameters.CreateStyledBoxThick(Color.Red));
        Surface.Print(enemyOffsetX, enemyOffsetY-1, $"{_enemy.name}", Color.Red);
        Surface.Print(enemyOffsetX, enemyOffsetY + enemySizeY, $"{_enemy.currentHp}/{_enemy.maxHp}", Color.Red);

        
        string nextSkillName = _enemy.nextSkill.name;
        Surface.Print(enemyOffsetX, enemyOffsetY + enemySizeY + 1, $"Next: {nextSkillName}", Color.Orange);

        
        int menuWidth = 16;
        int menuHeight = _menuOptions.Count + 2;
        int menuX = Width - menuWidth - 2;
        int menuY = Height - menuHeight - 2;

        Surface.DrawBox(new Rectangle(menuX, menuY, menuWidth, menuHeight), ShapeParameters.CreateStyledBoxThin(Color.White));

        for (int i = 0; i < _menuOptions.Count; i++)
        {
            bool isSelected = (i == _selectedIndex);
            Color fg = isSelected ? Color.Black : Color.White;
            Color bg = isSelected ? Color.White : Color.Black;
            if(BattleManager.inst.isTurnProgress == true)
            {
                fg = Color.DarkGray;
                bg = Color.Gray;
            }

            string optionText = $" {_menuOptions[i]} ".PadRight(menuWidth - 2);
            Surface.Print(menuX + 1, menuY + 1 + i, optionText, fg, bg);
        }
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        // 턴 진행 중일 땐 조종 불가능
        if (BattleManager.inst.isTurnProgress == true)
        {
            return false;
        }
        if ((skillView != null && skillView.IsVisible) ||
            (inventoryView != null && inventoryView.IsVisible))
        {
            return false;
        }

        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.W) && _selectedIndex > 0)
        {
            _selectedIndex--;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.S) && _selectedIndex < _menuOptions.Count - 1)
        {
            _selectedIndex++;
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Space))
        {
            if (_selectedIndex == 0) // 스킬
            {
                if (skillView != null) skillView.OpenView();
            }
            else if (_selectedIndex == 1) // 인벤토리
            {
                if (inventoryView != null)
                {
                    inventoryView.OpenInv();
                }
            }
            else if (_selectedIndex == 2) // 항복
            {
                BattleManager.inst.Lose();
            }
            handled = true;
        }

        if (handled) Render();
        return true;
    }
}