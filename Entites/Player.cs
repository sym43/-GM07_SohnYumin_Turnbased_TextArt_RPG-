using SadConsole.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Player : Entity
{
    public Inventory inv {  get; private set; }
    /// <summary>
    /// 생성자 base에선 투명색을 배경색으로 지정해준다.<br/>
    /// 그러므로 플레이어의 색깔만 그리면 된다.
    /// </summary>
    /// <param name="pos"> 첫 위치</param>
    /// <param name="glyph"> 플레이어를 나타낼 문자 </param>
    /// <param name="color"> 그 문자의 색깔 </param>
    public Player(Point pos, char glyph, Color color) : base(color, Color.Transparent, glyph, 0)
    {
        Position = pos;
        inv = new Inventory();
    }
    public bool CheckObstacle(Point nextPos)
    {
        int width = GameManager.inst.mainScreen.mainView.Width;
        int height = GameManager.inst.mainScreen.mainView.Height;
        if (nextPos.X <= 0 || nextPos.X >= width - 1 || nextPos.Y <= 0 || nextPos.Y >= height - 1)
            return false;
        foreach(GameObject obj in GameManager.inst.currentMap.mapObjects)
        {
            //포탈이면 포탈위로 이동가능
            if (obj is TPZone tpZone && tpZone.IsPositionPortal(nextPos)) return true;
            if (obj is ObstacleZone zone)
            {
                // 플레이어의 다음 좌표가 장애물의 Range(범위) 내에 있는지 확인
                if (zone.range.Contains(nextPos))
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void Move(Direction dir)
    {
        Point nextPos = Position + dir;
        if (!CheckObstacle(nextPos)) return;
        else
        {
            Position = nextPos;
        }
    }
    public void TP(Point targetPos)
    {
        Position = targetPos;
    }
    public void TryInteraction()
    {
        Point currentPos = Position;
        List<Point> nearPos = new List<Point>() {
            currentPos + Direction.Up,
            currentPos + Direction.Right,
            currentPos + Direction.Down,
            currentPos + Direction.Left,
        };
        // 발 밑이 포탈일 때
        foreach (GameObject obj in GameManager.inst.currentMap.mapObjects)
        {
            if (obj is TPZone tpZone)
            {
                if (tpZone.IsPositionPortal(currentPos))
                {
                    LogManager.inst.StartNewEvent();
                    tpZone.Interact();
                    return;
                }
            }
            // 상하좌우에 상호작용가능한 오브젝트가 있을 때
            else if (obj is IInteractable interactableObj)
            {
                if (nearPos.Contains(obj.pos))
                {
                    LogManager.inst.StartNewEvent();
                    interactableObj.Interact();
                    return;
                }
            }
        }
    }
}