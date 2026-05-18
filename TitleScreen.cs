using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;



public abstract class SelectableMenuObject : IExecutable
{
    public string menuText { get; protected set; }
    public bool isEnabled { get; set; } = true; // 활성화 여부
    protected SelectableMenuObject(string menuText)
    {
        this.menuText = menuText;
    }
    public abstract void Execute();
}
public class Title_LoadGame : SelectableMenuObject
{
    public Title_LoadGame() : base("Load Game") { }
    public override void Execute()
    {
        GameManager.inst.LoadGame();
    }
}
public class Title_NewGame : SelectableMenuObject
{
    public Title_NewGame() : base("New Game") { }
    public override void Execute()
    {
        GameManager.inst.NewGame();
    }
}
public class Title_ExitGame : SelectableMenuObject
{
    public Title_ExitGame() : base("Exit Game") { }
    public override void Execute()
    {
        GameManager.inst.ExitGame();
    }
}
public class TitleScreen : ScreenSurface
{
    private int _currentSelection = 0;
    private bool _hasSaveFile;

    // 딕셔너리로 옵션 관리
    private List<SelectableMenuObject> _menuObjects;
    private string _titleArt = "";

    public TitleScreen(int width, int height) : base(width, height)
    {
        _menuObjects = new List<SelectableMenuObject>() { new Title_LoadGame(), new Title_NewGame(), new Title_ExitGame() };
        CheckSaveFile();
        LoadTitleArt();
        DrawStaticUI();
        DrawMenuOptions();
    }
    private void LoadTitleArt()
    {
        string resourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Title.txt");
        try
        {
            if (System.IO.File.Exists(resourcePath))
            {
                _titleArt = System.IO.File.ReadAllText(resourcePath, System.Text.Encoding.UTF8).Replace("\r", "");
            }
        }
        catch { _titleArt = "=== ERROR ==="; }
    } // 중앙에 표시할 제목 아스키 코드 로딩
    private void CheckSaveFile()
    {
        _hasSaveFile = false;
        if (_hasSaveFile)
        {

        }
        else
        {
            _menuObjects[0].isEnabled = _hasSaveFile;
            _currentSelection = 1;
        } // 로드 게임 메뉴 비활성화 및 커서 두번째로 옮기기
    } // 맨 처음에 세이브 파일 있는지 체크

    private void DrawStaticUI()
    {
        this.Clear();
        this.DrawBox(new Rectangle(2, 1, Width - 4, Height - 2),
            ShapeParameters.CreateStyledBox(ICellSurface.ConnectedLineThick, new ColoredGlyph(Color.White, Color.Black)));
        //외곽 빈 사각형 그리기
        string[] lines = _titleArt.Split('\n');
        int startY = (Height / 2) - 10;
        foreach (string line in lines)
            this.Print((Width / 2) - (line.Length / 2), startY++, line, Color.Yellow);
    } // 정적인 ui인 타이틀과 외곽선 렌더링

    private void DrawMenuOptions()
    {
        int startY = (Height / 2) + 5;

        for (int i = 0; i < _menuObjects.Count; i++)
        {
            var item = _menuObjects[i];
            Color textColor = (i == _currentSelection) ? Color.Cyan : Color.White;

            
            if (!item.isEnabled) textColor = Color.Gray;
            // 비활성화된 메뉴는 회색으로 표시

            string prefix = (i == _currentSelection) ? "> " : "  ";
            this.Print((Width / 2) - (item.menuText.Length / 2)-2, startY + (i * 2), prefix + item.menuText, textColor);
        }
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        if (keyboard.IsKeyPressed(Keys.Up))
        {
            MoveSelection(-1);
            return true;
        }
        if (keyboard.IsKeyPressed(Keys.Down))
        {
            MoveSelection(1);
            return true;
        }
        if (keyboard.IsKeyPressed(Keys.Space))
        {

            if (_menuObjects[_currentSelection].isEnabled)
            {
                _menuObjects[_currentSelection].Execute();
            }
            // 선택 불가능한 옵션(세이브 없음 + Load 선택)인 경우 무시
            // 예외 처리 해놨지만 혹시 몰라서.
            return true;
        }

        return base.ProcessKeyboard(keyboard);
    }
    private void MoveSelection(int direction)
    {
        int newSelection = _currentSelection + direction;
        int minIndex = _hasSaveFile ? 0 : 1;
        // 세이브 파일이 없으면 0번으로 못 올라가게 막음
        int maxIndex = _menuObjects.Count - 1;

        if (newSelection >= minIndex && newSelection <= maxIndex)
        {
            _currentSelection = newSelection;
            DrawMenuOptions();
        } // 셀렉트가 성공했을 때만 ui 갱신
    }
    public override void Update(TimeSpan delta)
    {
        base.Update(delta);
    }
}
