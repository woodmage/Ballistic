using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ballistic
{
    public partial class Form1 : Form
    {
        static int maxballs = 200;
        static int maxblocks = 200;
        Timer timer = new Timer();
        Ball[] balls = new Ball[maxballs];
        bool[] isBalls = new bool[maxballs];
        int numballs;
        Blocks[] blocks = new Blocks[maxblocks];
        bool[] isBlocks = new bool[maxblocks];
        int numblocks;
        int level, score;
        bool aimmode = true;
        int ballno;
        float m_x, m_y, m_x2, m_y2, b_x, b_y, b_a, b_dx, b_dy;
        public Form1()
        {
            InitializeComponent();
            InitGame();
        }
        private void InitGame()
        {
            level = 0;
            score = 0;
            InitLevel();
        }
        private void InitLevel()
        {
            for (int i = 0; i < maxballs; i++)
                isBalls[i] = false;
            for (int i = 0; i < maxblocks; i++)
            {
                if (isBlocks[i])
                {
                    blocks[i].Drop();
                }
            }
            int b = (level / 2) + 1;
            if (b > 15) b = 15;
            for (int i = 0; i < b; i++)
            {
                AddBlock();
            }
            for (int i = 0; i < maxballs; i++)
            {
                if (isBalls[i])
                {
                    isBalls[i] = false;
                }
            }
            numballs = (level / 2) + 1;
            ballno = 0;
            aimmode = true;
            b_x = m_x2 = m_x = 500;
            b_y = m_y2 = m_y = 850;
            pictureBox2.Cursor = Cursors.Cross;
            Inval();
        }
        private void AddBlock()
        {
            Blocks tbo;
            Random r = new Random();
            int nh, x;
            nh = (level / 2) + 1;
            tbo = new Blocks();
            do
            {
                x = r.Next(0, 19);
                tbo.Set(x * 50, 50, 50, 50, nh);
            }
            while (CheckBlock(tbo));
            for (int i = 0; i < maxblocks; i++)
            {
                if (!isBlocks[i])
                {
                    blocks[i] = tbo;
                    numblocks++;
                    isBlocks[i] = true;
                    i = maxblocks;
                }
            }
        }
        private bool CheckBlock(Blocks b)
        {
            bool result;
            for (int i = 0; i < maxblocks; i++)
            {
                result = false;
                if (isBlocks[i])
                {
                    if (b.x > blocks[i].x + blocks[i].w - 1) result = true;
                    if (b.x + b.w - 1 < blocks[i].x) result = true;
                    if (b.y > blocks[i].y + blocks[i].h - 1) result = true;
                    if (b.y + b.h - 1 < blocks[i].y) result = true;
                    if (!result) return true;
                }
            }
            return false;
        }
        private void PaintPB1(object sender, PaintEventArgs e)
        {
            string buffer = "Click mouse to return all balls.";
            if (aimmode) buffer = "Move mouse to aim, click to fire.";
            SolidBrush brush = new SolidBrush(Color.Black);
            Font font = new Font("Comic Sans", 32, FontStyle.Bold);
            e.Graphics.DrawString($"Score: {score}    Level {level + 1}", font, brush, 0, 0);
            e.Graphics.DrawString(buffer, font, brush, 0, 50);
        }
        private void PaintPB2(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < maxblocks; i++)
                if (isBlocks[i])
                    blocks[i].Paint(e);
            Font font = new Font("Comics San", 6);
            SolidBrush brush = new SolidBrush(Color.White);
            e.Graphics.FillEllipse(brush, m_x, m_y, 5, 5);
            e.Graphics.DrawString($"{numballs}", font, brush, m_x, m_y + 5);
            if (aimmode)
            {
                Pen pen = new Pen(Color.White, 3);
                pen.DashStyle = DashStyle.Dash;
                pen.DashCap = DashCap.Round;
                pen.DashPattern = new float[] { 10, 5, 10, 5 };
                e.Graphics.DrawLine(pen, m_x, m_y, m_x2, m_y2);
            }
            else
            {
                for (int i = 0; i < maxballs; i++)
                    if (isBalls[i])
                        balls[i].Paint(e);
            }
        }
        private void HandleMouse(object sender, MouseEventArgs e)
        {
            if (aimmode)
            {
                m_x2 = e.X;
                m_y2 = e.Y;
                if ((m_y - m_y2) < 10) m_y2 = m_y - 10;
                Inval();
            }
        }
        private void HandleClick(object sender, MouseEventArgs e)
        {
            if (aimmode)
            {
                m_x2 = e.X;
                m_y2 = e.Y;
                if ((m_y - m_y2) < 10) m_y2 = m_y - 10;
                b_a = (float)Math.Atan2(m_y2 - m_y, m_x2 - m_x);
                b_dx = (float)Math.Cos(b_a) * 25;
                b_dy = (float)Math.Sin(b_a) * 25;
                aimmode = false;
                timer.Tick += new EventHandler(TimerProc);
                timer.Interval = 50;
                timer.Start();
                pictureBox2.Cursor = Cursors.PanSouth;
                Inval();
            }
            else
            {
                for (int i = 0; i < maxballs; i++)
                {
                    if (isBalls[i])
                    {
                        isBalls[i] = false;
                    }
                }
                numballs = 0;
            }
        }
        private void TimerProc(object sender, EventArgs e)
        {
            Ball ball = new Ball();
            bool useball = false;
            if (ballno < numballs)
                AddBall();
            for (int i = 0; i < ballno; i++)
            {
                if (isBalls[i])
                {
                    CopyBall(ball, balls[i]);
                    for (int j = 0; j < maxblocks; j++)
                    {
                        if (isBlocks[j])
                        {
                            if (blocks[j].DoBall(balls[i]))
                            {
                                isBlocks[j] = blocks[j].StillExists();
                                if (!isBlocks[j])
                                {
                                    numblocks--;
                                    score += blocks[j].points;
                                    Inval(1);
                                }
                                useball = true;
                                j = maxblocks;
                            }
                        }
                    }
                    if (!useball)
                    {
                        CopyBall(balls[i], ball);
                        balls[i].MoveX();
                        balls[i].MoveY();
                    }
                    if (balls[i].y > 900)
                    {
                        numballs--;
                        isBalls[i] = false;
                    }
                }
            }
            if (numblocks == 0)
            {
                score += level * 100;
                numballs = 0;
            }
            if (numballs == 0)
            {
                timer.Stop();
                timer.Dispose();
                for (int i = 0; i < maxballs; i++)
                {
                    if (isBalls[i])
                    {
                        isBalls[i] = false;
                    }
                }

                level++;
                InitLevel();
            }
            Inval(2);
        }
        private void CopyBall(Ball b1, Ball b2)
        {
            b1.x = b2.x;
            b1.y = b2.y;
            b1.w = b2.w;
            b1.h = b2.h;
            b1.dx = b2.dx;
            b1.dy = b2.dy;
        }
        private void AddBall()
        {
            Ball tbo = new Ball();
            tbo.Set(b_x, b_y, 5, 5, b_dx, b_dy);
            tbo.Bounds(0, 0, 995, 9999);
            for (int i = 0; i < maxballs; i++)
            {
                if (!isBalls[i])
                {
                    balls[i] = tbo;
                    isBalls[i] = true;
                    i = maxballs;
                    ballno++;
                }
            }
        }
        private void Inval(int n)
        {
            switch (n)
            {
                case 1:
                    pictureBox1.Invalidate();
                    break;
                case 2:
                    pictureBox2.Invalidate();
                    break;
                default:
                    pictureBox1.Invalidate();
                    pictureBox2.Invalidate();
                    break;
            }
        }
        private void Inval()
        {
            Inval(0);
        }
    }
}
