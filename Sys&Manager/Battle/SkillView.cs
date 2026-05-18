using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;

public class SkillView : ScreenSurface
{

    public Hero player { get; private set; }

    public int _selectedIndex = 0;
    private SkillActionPopup _actionPopup;

    private ScreenSurface _listSurface;
    private ScreenSurface _descSurface;

    private List<Skill> _skills;
    private List<string> _skillNames;
    private List<string> _skillDescs;


    public SkillView(int parentWidth, int parentHeight) : base(parentWidth, parentHeight / 4)
    {
        IsVisible = false;
        UseKeyboard = true;
        Surface.DefaultBackground = Color.Black;
        player = GameManager.inst.currentHero;
        _skills = new List<Skill> { player.basicAttackSkill, player.defendSkill, player.uniqueSkill, player.passive };
        _skillNames = new List<string>() { _skills[0].name, _skills[1].name, _skills[2].name, _skills[3].name, "Close(ESC)" };
        _skillDescs = new List<string>() { _skills[0].desc, _skills[1].desc, _skills[2].desc, _skills[3].desc, "Close(ESC)" };
        // 메인 뷰의 맨 아래로 위치 이동
        Position = new Point(0, parentHeight - Height);

        int listWidth = (int)(Width * 0.33f);
        int descWidth = Width - listWidth + 1;

        // 스킬 선택창
        _listSurface = new ScreenSurface(listWidth, Height);
        _listSurface.Position = new Point(0, 0);
        Children.Add(_listSurface);

        // 설명창
        _descSurface = new ScreenSurface(descWidth, Height);
        _descSurface.Position = new Point(listWidth - 1, 0);
        Children.Add(_descSurface);
    }

    public void OpenView()
    {
        IsVisible = true;
        IsFocused = true;
        _selectedIndex = 0;
        Render();
    }

    public void CloseView()
    {
        IsVisible = false;
        IsFocused = false;
        Parent.IsFocused = true;
    }

    public void Render()
    {
        RenderList();
        RenderDesc();
    }

    private void RenderList()
    {
        _listSurface.Surface.Clear();
        _listSurface.Surface.DrawBox(new Rectangle(0, 0, _listSurface.Width, _listSurface.Height), ShapeParameters.CreateStyledBoxThin(Color.White));
        _listSurface.Surface.Print(2, 0, " [ Skills ] ", Color.Cyan);

        for (int i = 0; i < _skillNames.Count; i++)
        {
            
            bool isSelected = (i == _selectedIndex);
            Color fg = isSelected ? Color.Black : Color.White;
            Color bg = isSelected ? Color.White : Color.Black;

            // 패시브 스킬일 경우 색상 다르게
            if (i == 3 && !isSelected) fg = Color.Gray;

            string skillNameText = $" {_skillNames[i]}".PadRight(_listSurface.Width - 2);
            _listSurface.Surface.Print(1, i + 1, skillNameText, fg, bg);
        }
        
    }

    private void RenderDesc()
    {
        _descSurface.Surface.Clear();
        _descSurface.Surface.DrawBox(new Rectangle(0, 0, _descSurface.Width, _descSurface.Height), ShapeParameters.CreateStyledBoxThin(Color.White));

        
        _descSurface.Surface.Print(0, 0, "┬", Color.White);
        _descSurface.Surface.Print(0, _descSurface.Height - 1, "┴", Color.White);

        
        UpdateDescs();
        string skillNameText = _skillNames[_selectedIndex];
        string skillDescText = _skillDescs[_selectedIndex];

        _descSurface.Surface.Print(2, 1, $"- {skillNameText} -", Color.Yellow);
        _descSurface.Surface.Print(2, 2, skillDescText, Color.LightGray);
    }
    private void UpdateDescs()
    {
        for (int i=0; i<_skills.Count; i++)
        {
            _skills[i].UpdateDesc(player);
            _skillDescs[i] = _skills[i].desc;
        }
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        // 턴 진행 중일 땐 조종 불가능
        if (BattleManager.inst.isTurnProgress == true)
        {
            return false;
        }
        if (_actionPopup != null && _actionPopup.IsVisible) return false;

        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            CloseView();
            return true;
        }

        if (keyboard.IsKeyPressed(Keys.W) && _selectedIndex > 0)
        {
            _selectedIndex--;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.S) && _selectedIndex < _skillNames.Count-1)
        {
            _selectedIndex++;
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Space))
        {
            if (_selectedIndex == 4) // Close 옵션
            {
                CloseView();
                return true;
            }
            else if (_selectedIndex != 3) // 패시브가 아닐 때만 팝업 생성
            {
                OpenActionPopup();
            }
            // 패시브일 때는 상호작용 안 함
            handled = true;
        }

        if (handled) Render();
        return true;
    }

    private void OpenActionPopup()
    {
        _actionPopup = new SkillActionPopup(OnPopupClosed, _skills[_selectedIndex]);

        
        int popupX = _descSurface.Position.X - 4;
        int popupY =  _selectedIndex+1;
        _actionPopup.Position = new Point(popupX, popupY);

        Children.Add(_actionPopup);
        this.IsFocused = false;
        _actionPopup.IsFocused = true;
    }

    private void OnPopupClosed(bool isUse)
    {
        Children.Remove(_actionPopup);
        _actionPopup = null;
        this.IsFocused = true;
        Render();
        if (isUse)
        {
            CloseView();
            StartTurn();
        }
    }
    public async void StartTurn()
    {
        await BattleManager.inst.StartTurn(_skills[_selectedIndex]);
    }
}


public class SkillActionPopup : ScreenSurface
{
    private int _selectedIndex = 0;
    private Action<bool> _onClosed;
    private Skill _currentSkill;

    public SkillActionPopup(Action<bool> onClosed, Skill currentSkill) : base(12, 4)
    {
        _onClosed = onClosed;
        _currentSkill = currentSkill;
        UseKeyboard = true;
        Surface.DefaultBackground = Color.Black;
        Render();
    }

    public void Render()
    {
        Surface.Clear();
        Surface.DrawBox(new Rectangle(0, 0, Width, Height), ShapeParameters.CreateStyledBoxThin(Color.White));

        string[] options = { "Use", "Close" };
        for (int i = 0; i < options.Length; i++)
        {
            bool isSelected = (i == _selectedIndex);
            Color fg = isSelected ? Color.Black : Color.White;
            Color bg = isSelected ? Color.White : Color.Black;
            Surface.Print(1, 1 + i, $" {options[i]} ".PadRight(Width - 2), fg, bg);
        }
    }
    
    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        // 턴 진행 중일 땐 조종 불가능
        if (BattleManager.inst.isTurnProgress == true)
        {
            return false;
        }
        if (keyboard.IsKeyPressed(Keys.Escape)) { Close(false); return true; }
        if (keyboard.IsKeyPressed(Keys.W)) { _selectedIndex = 0; Render(); return true; }
        if (keyboard.IsKeyPressed(Keys.S)) { _selectedIndex = 1; Render(); return true; }

        if (keyboard.IsKeyPressed(Keys.Space) || keyboard.IsKeyPressed(Keys.Enter))
        {
            if (_selectedIndex == 0)
            {
                Close(true);
            }
            else
            {
                Close(false);
            }
        }
        return true;
    }

    private void Close(bool isUse)
    {
        IsVisible = false;
        IsFocused = false;
        _onClosed?.Invoke(isUse);
    }
}