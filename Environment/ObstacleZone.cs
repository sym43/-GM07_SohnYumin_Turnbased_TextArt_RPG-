using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ObstacleZone : GameObject
{
    /// <summary>
    /// range는 영역의 범위를 나타낸다.<br/>
    /// rectangle타입은 (시작x, 시작y, 끝x, 끝y)를 가진다.
    /// </summary>
    public Rectangle range { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="glyph"></param>
    /// <param name="color"></param>
    /// <param name="width">가로값, 이걸로 범위를 정한다.</param>
    /// <param name="height">세로값, 이걸로 범위를 정한다.</param>
    public ObstacleZone(Point pos, char glyph, Color color, int width, int height) : base(pos, glyph, color)
    {
        range = new Rectangle(pos.X, pos.Y, width, height);
    }
    /// <summary>
    /// 렌더함수. 외곽선을 그린다.
    /// </summary>
    /// <param name="surface"></param>
    public override void Draw(ScreenSurface surface)
    {
        surface.DrawBox(range, ShapeParameters.CreateStyledBox(ICellSurface.CreateLine(glyph),
                   new ColoredGlyph(color, Color.Black, glyph)));
    }

}