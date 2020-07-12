using System.Drawing;
using System.Windows.Forms;

public class Ball
{
    public float x, y, w, h, dx, dy;
    public int minx, miny, maxx, maxy;
    public void Set(int x, int y, int w, int h, float dx, float dy)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.dx = dx;
        this.dy = dy;
    }
    public void Set(float x, float y, int w, int h, float dx, float dy)
    {
        Set((int)x, (int)y, w, h, dx, dy);
    }
    public void Bounds(int minx, int miny, int maxx, int maxy)
    {
        this.minx = minx;
        this.miny = miny;
        this.maxx = maxx;
        this.maxy = maxy;
    }
    public void MoveX()
    {
        x += dx;
        if (x < minx) BounceX();
        if (x + w > maxx) BounceX();
    }
    public void MoveY()
    {
        y += dy;
        if (y < miny) BounceY();
        if (y + h > maxy) BounceY();
    }
    public void BounceX()
    {
        dx = -dx;
        x += dx;
    }
    public void BounceY()
    {
        dy = -dy;
        y += dy;
    }
    public void Paint(PaintEventArgs e)
    {
        SolidBrush brush = new SolidBrush(Color.White);
        e.Graphics.FillEllipse(brush, x, y, w, h);
    }
}