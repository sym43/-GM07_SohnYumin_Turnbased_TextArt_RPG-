using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameObject
{
    public Point pos { get; protected set; }
    /// <summary>
    /// char타입으로 텍스트를 그릴때 쓰는 텍스트. <br/>
    /// 보통 한글자 오브젝트(플레이어, 포탈, 상자 등)에 쓰이거나 <br/>
    /// 장애물에 둘러 쌀 문자를 저장해 둘 용도.
    /// </summary>
    public char glyph { get; private set; }
    /// <summary>
    /// glyph의 글자 색을 정해줄 용도.
    /// </summary>
    public Color color { get; private set; }

    /// <summary>
    /// pos는 넣게 되면 처음 위치를 정해주지만,<br/>
    /// 렌더링은 하지않아서 눈에는 안보인다.<br/>
    /// 렌더링은 map이나 gm에서 할것.
    /// </summary>
    public GameObject(Point pos, char glyph, Color color)
    {
        this.pos = pos;
        this.glyph = glyph;
        this.color = color;
    }

    /// <summary>
    /// 자신을 렌더링 하는 함수. <br/>
    /// surface는 그릴 표면을 정해주는 함수로, 아마 나중에 부모 자식관계를 맺으면 안 필요하지 않을까..?
    /// </summary>
    public virtual void Draw(ScreenSurface surface)
    {
        surface.SetGlyph(pos.X, pos.Y, glyph, color);
    }
}