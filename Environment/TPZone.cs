using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TPZone : ObstacleZone, IInteractable
{
    /// <summary>
    /// 텔포할 곳을 나타낼 문자.
    /// </summary>
    private string _destinationText;
    public string destination;
    private GameObject _portal;
    public TPZone(string destination, Point pos, char glyph, Color color, int width, int height, Direction.Types portalDir = Direction.Types.Down, string destinationText = "") : base(pos, glyph, color, width, height)
    {
        this.destination = destination;
        // 문자 가운데 빈공간에 정렬
        _destinationText = String.IsNullOrEmpty(destinationText) ? destination : destinationText;
        CreatePortal(portalDir);
    }
    /// <summary>
    /// 포탈 게임오브젝트를 색깔과 위치를 정해 생성한다.
    /// </summary>
    private void CreatePortal(Direction.Types portalDir)
    {
        Point portalPos = new Point();
        switch (portalDir)
        {
            case Direction.Types.Down:
                portalPos = new Point(range.X + (range.Width / 2), range.Y + range.Height - 1);
                break;
            case Direction.Types.Left:
                portalPos = new Point(range.X, range.Y + (range.Height/2));
                break;
            case Direction.Types.Up:
                portalPos = new Point(range.X + (range.Width / 2), range.Y);
                break;
            case Direction.Types.Right:
                portalPos = new Point(range.X + range.Width - 1, range.Y + (range.Height / 2));
                break;
            default:
                break;
        }
        _portal = new GameObject(
            portalPos,
            glyph,
            Color.OrangeRed
            );
    }
    /// <summary>
    /// targetPos이 포탈인 경우를 내뱉음. <br/>
    /// 보통 포탈로 이동할때 막히지 않게 하고, 상호작용시 포탈인지 체크하기 위해 씀.
    /// </summary>
    /// <param name="targetPos"></param>
    /// <returns></returns>
    public bool IsPositionPortal(Point targetPos)
    {
        return _portal.pos == targetPos;
    }
    public override void Draw(ScreenSurface surface)
    {
        base.Draw(surface);
        _portal.Draw(surface);
        string alignText = _destinationText.Align(HorizontalAlignment.Center, range.Width - 2);
        surface.Print(range.X + 1, range.Y + (range.Height / 2), alignText, Color.White);
        // 시작 x와 문자열 정렬할때 width-2를 해줌으로써, 가운데정렬로 출력하되, 양옆 외곽선은 침범해 출력 안하도록 함.
    }

    public void Interact()
    {
        LogManager.inst.AddLog($"Go to {_destinationText}");
        
        GameManager.inst.LoadMap(destination);
        //GameManager.inst.player.inv.AddItem(new Sword("Sword", "Just Sword", 100, ItemTier.Common, 1));
    }
}
