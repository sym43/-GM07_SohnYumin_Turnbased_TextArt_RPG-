using SadConsole.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class InventoryView : ScreenSurface
{
    private Inventory _inv;
    private int _selectedIndex = 0;

    private ItemActionPopup _actionPopup;

    
    private ScreenSurface _listSurface;
    private ScreenSurface _descSurface;
    public InventoryView(int width, int height, Inventory inv) : base(width, height)
    {
        _inv = inv;
        IsVisible = false;
        UseKeyboard = true;
        Surface.DefaultBackground = Color.Black;

        
        int descHeight = (int)(height * 0.33f);
        int listHeight = height - descHeight;

        // 설명 창
        _descSurface = new ScreenSurface(width, descHeight);
        _descSurface.Position = new Point(0, 0);
        _descSurface.Surface.DefaultBackground = Color.Black;
        Children.Add(_descSurface);

        // 리스트 창
        _listSurface = new ScreenSurface(width, listHeight+1);
        _listSurface.Position = new Point(0, descHeight-1);
        _listSurface.Surface.DefaultBackground = Color.Black;
        Children.Add(_listSurface);
    }
    public void OpenInv()
    {
        IsVisible = true;
        IsFocused = true;
        Render();
    }
    public void CloseInv()
    {
        IsVisible = false;
        IsFocused = false;
        Parent.IsFocused = true;
    }
    public void Render()
    {
        RenderDesc();
        RenderList();
    }
    
    private void RenderDesc()
    {
        _descSurface.Surface.Clear();
        _descSurface.Surface.DrawBox(new Rectangle(0, 0, _descSurface.Width, _descSurface.Height), ShapeParameters.CreateStyledBoxThick(Color.White));

        
        _descSurface.Surface.Print(2, 0, " [ Inventory ] ", Color.Yellow);

        if (_inv.items.Count > 0)
        {
            Item selectedItem = _inv.items[_selectedIndex];

            _descSurface.Surface.Print(2, 1, $"- {selectedItem.name} -", Color.Cyan);

            
            List<string> wrappedDesc = WrapText(selectedItem.desc, _descSurface.Width - 4);
            for (int i = 0; i < wrappedDesc.Count; i++)
            {
                if (2 + i >= _descSurface.Height - 1) break;
                _descSurface.Surface.Print(2, 2 + i, wrappedDesc[i], Color.LightGray);
            }
        }
    }
    private void RenderList()
    {
        _listSurface.Surface.Clear();
        _listSurface.Surface.DrawBox(new Rectangle(0, 0, _listSurface.Width, _listSurface.Height), ShapeParameters.CreateStyledBoxThick(Color.White));

        _listSurface.Surface.Print(2, _listSurface.Height - 1, " [Space] Use/Equip  [Esc / E] Close ", Color.Gray);

        for (int i = 0; i < _inv.items.Count; i++)
        {
            if (1 + i >= _listSurface.Height - 1) break;

            Item item = _inv.items[i];
            bool isSelected = (i == _selectedIndex);
            Color fg = isSelected ? Color.Black : Color.White;
            Color bg = isSelected ? Color.White : Color.Transparent;

            string equipMark = (item is IEquipable equipItem && equipItem.isEquipped) ? "[E]" : "   ";
            string itemLine = $"{equipMark} {item.name}".PadRight(_listSurface.Width - 4);

            _listSurface.Surface.Print(2, 1 + i, itemLine, fg, bg);
        }
        _listSurface.Surface.Print(0, 0, $"{(char)204}", Color.White);
        _listSurface.Surface.Print(_listSurface.Width - 1, 0, $"{(char)185}", Color.White);
    }
    private List<string> WrapText(string text, int maxWidth)
    {
        List<string> lines = new List<string>();
        if (string.IsNullOrEmpty(text)) return lines;

        for (int i = 0; i < text.Length; i += maxWidth)
        {
            if (i + maxWidth > text.Length) lines.Add(text.Substring(i));
            else lines.Add(text.Substring(i, maxWidth));
        }
        return lines;
    }
    private bool OpenActionPopup()
    {
        Item selectedItem = _inv.items[_selectedIndex];

        _actionPopup = new ItemActionPopup(selectedItem, _inv, OnPopupClosed);

        
        int popupX = Width / 2;
        int popupY =  _selectedIndex + (int)(Height * 0.33f);
        if (popupY + _actionPopup.Height > Height)
        {
            popupY = Height - _actionPopup.Height;
        }
        _actionPopup.Position = new Point(popupX, popupY);

        
        Children.Add(_actionPopup);
        this.IsFocused = false;
        _actionPopup.IsFocused = true;

        return true;
    }
    private void OnPopupClosed()
    {
        // 아이템을 버려서 리스트가 줄어들었을 때 커서가 바깥으로 나가는 것 방지
        if (_selectedIndex >= _inv.items.Count && _selectedIndex > 0)
        {
            _selectedIndex--;
        }

        // 팝업 완전 제거
        Children.Remove(_actionPopup);
        _actionPopup = null;

        // 인벤토리가 키보드 주도권 잡기
        this.IsFocused = true;
        Render();
    }
    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Escape) || keyboard.IsKeyPressed(Keys.E))
        {
            CloseInv();
            return true;
        }

        if (keyboard.IsKeyPressed(Keys.W) && _selectedIndex > 0)
        {
            _selectedIndex--;
            handled = true;
        }
        else if (keyboard.IsKeyPressed(Keys.S) && _selectedIndex < _inv.items.Count - 1)
        {
            _selectedIndex++;
            handled = true;
        }

        if (keyboard.IsKeyPressed(Keys.Space) && _inv.items.Count > 0)
        {
            handled = OpenActionPopup();
            handled = true;
        }

        if (handled) Render();

        return true;
    }
}
public class ItemActionPopup : ScreenSurface
{
    private Item _targetItem;
    private Inventory _inv;
    private int _selectedIndex = 0;
    private List<string> _menuOptions = new List<string>();

    // 팝업이 닫힐 때 부모에게 알려주기 위한 콜백
    private Action _onClosed;

    public ItemActionPopup(Item targetItem, Inventory inv, Action onClosed) : base(18, 5)
    {
        _targetItem = targetItem;
        _inv = inv;
        _onClosed = onClosed;

        UseKeyboard = true;
        Surface.DefaultBackground = Color.Black;

        // 장비면 장착/해제, 소비템이면 사용
        if (_targetItem is IEquipable equipableItem)
        {
            _menuOptions.Add(equipableItem.isEquipped ? "UnEquip" : "Equip");
        }
        else if(_targetItem is IUseable useableItem)
        {
            _menuOptions.Add("Use");
        }

        _menuOptions.Add("Remove");
        _menuOptions.Add("Close ( ESC )");

        Render();
    }

    public void Render()
    {
        Surface.Clear();
        Surface.DrawBox(new Rectangle(0, 0, Width, Height), ShapeParameters.CreateStyledBoxThin(Color.White));

        for (int i = 0; i < _menuOptions.Count; i++)
        {
            bool isSelected = (i == _selectedIndex);
            Color fg = isSelected ? Color.Black : Color.White;
            Color bg = isSelected ? Color.White : Color.Black;

            string optionText = $" {_menuOptions[i]} ".PadRight(Width - 2);
            Surface.Print(1, 1 + i, optionText, fg, bg);
        }
    }

    public override bool ProcessKeyboard(Keyboard keyboard)
    {
        bool handled = false;

        if (keyboard.IsKeyPressed(Keys.Escape))
        {
            ClosePopup();
            return true;
        }

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

        if (keyboard.IsKeyPressed(Keys.Space) || keyboard.IsKeyPressed(Keys.Enter))
        {
            ExecuteAction();
            return true;
        }

        if (handled) Render();

        return true;
    }

    private void ExecuteAction()
    {
        LogManager.inst.StartNewEvent();
        string selectedOption = _menuOptions[_selectedIndex];

        if (selectedOption == "Equip" || selectedOption == "UnEquip")
        {
            if (_targetItem is IEquipable equipItem)
            {
                if (equipItem.isEquipped) equipItem.UnEquip();
                else equipItem.Equip();
            }
        }
        else if (selectedOption == "Use")
        {
            // 나중에 구현
        }
        else if (selectedOption == "Remove")
        {
            _inv.RemoveItem(_targetItem);
        }

        ClosePopup();
    }

    private void ClosePopup()
    {
        IsVisible = false;
        IsFocused = false;
        _onClosed?.Invoke(); //콜백
    }
}