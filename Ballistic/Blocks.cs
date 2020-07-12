using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Blocks
{
    public int x, y, w, h, nh;
    public int points;
    private int cr, cb, cg;
    private Color color, insidecolor;
    Random r = new Random();
    public void Set(int x, int y, int w, int h, int nh)
    {
        this.x = x;
        this.y = y;
        this.w = w;
        this.h = h;
        this.nh = nh;
        points = nh * 100;
        cr = r.Next(128, 256);
        cb = r.Next(128, 256);
        cg = r.Next(128, 256);
        color = Color.FromArgb(255, cr, cg, cb);
        insidecolor = Color.FromArgb(255, cr - 128, cg - 128, cb - 128);
    }
    public bool IsHit(Ball b)
    {
        if (b.x > x + w) return false;
        if (b.x + b.w < x) return false;
        if (b.y > y + h) return false;
        if (b.y + b.h < y) return false;
        return true;
    }
    public bool DoBall(Ball b)
    {
        bool ret = false;
        b.MoveX();
        if (IsHit(b))
        {
            nh--;
            b.BounceX();
            ret = true;
        }
        b.MoveY();
        if (IsHit(b))
        {
            nh--;
            b.BounceY();
            ret = true;
        }
        return ret;
    }
    public bool StillExists()
    {
        if (nh > 0) return true;
        return false;
    }
    public void Drop()
    {
        y += 50;
    }
    public void Paint(PaintEventArgs e)
    {
        Random r = new Random();
        SolidBrush brush = new SolidBrush(Color.White);
        Font font = new Font("Comic Sans", 15.0F, FontStyle.Bold);
        Pen pen = new Pen(color);
        SolidBrush insidebrush = new SolidBrush(insidecolor);
        Rectangle rect = new Rectangle(x + 1, y + 1, w - 2, h - 2);
        StringFormat sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Center;
        e.Graphics.FillRectangle(insidebrush, x, y, w, h);
        e.Graphics.DrawRectangle(pen, x, y, w, h);
        e.Graphics.DrawString($"{nh}", font, brush, rect, sf);
    }
}
