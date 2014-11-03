using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections.Generic;

namespace SchetsEditor
{

    public abstract class MaakObject
    {
        public Pen pen;
        public Brush Kwast;
        public Rectangle Rechthoek;
        public Point point1, point2;

        public abstract void Teken(Graphics gr);
        public abstract bool geklikt(SchetsControl s, Point p);

        public double FormuleCirkel(Point p, Size sz)
        {
            return (double)(p.X * p.X) / (double)(sz.Width * sz.Width) + (double)(p.Y * p.Y) / (double)(sz.Height * sz.Height);
        }

        public double FormuleAfstandLijn(Point p)
        {
            double dx = point2.X - point1.X;
            double dy = point2.Y - point1.Y;
            //http://nl.wikipedia.org/wiki/Afstand#Afstand_tussen_een_punt_en_een_lijn
            double afstand = Math.Abs(dy * p.X - dx * p.Y - point1.X * point2.Y + point2.X * point1.Y) / Math.Sqrt(dx * dx + dy * dy);
            return afstand;
        }
        
    }

    public class Startpunt : MaakObject
    {
        public Point startpunt;

        public override void Teken(Graphics gr)
        {
            throw new NotImplementedException();
        }
        public override bool geklikt(SchetsControl s, Point p)
        {
            return false;
        }
    }

    public class Tekstobject : Startpunt
    {
        public Font font;
        public string tekst;

        public Tekstobject(string s, Font f, Brush kwast, Point p)
        {
            this.font = f;
            this.tekst = s;
            this.Kwast = kwast;
            this.startpunt = p;
        }

        public override void Teken(Graphics gr)
        {
 	        gr.DrawString(tekst, font, Kwast, startpunt, StringFormat.GenericTypographic);
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            Graphics g = s.MaakBitmapGraphics();
            Rectangle rec = new Rectangle(startpunt.X, startpunt.Y, (int)font.Size, (int)font.Height);
            return (p.X >= rec.Left && p.X <= rec.Right) && (p.Y >= rec.Top && p.Y <= rec.Bottom);
        }
    }

    public class MaakLijn : MaakObject
    {
        public MaakLijn(Pen p, Point p1, Point p2)
        {
            this.pen = p;
            this.point1 = p1;
            this.point2 = p2;
        }
        public override void Teken(Graphics gr)
        {
            gr.DrawLine(pen, point1, point2);
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            return FormuleAfstandLijn(p) <= 4;
        }
    }

    public class MaakPenObj : MaakObject

    {
        public MaakPenObj(Pen p, Point p1, Point p2)
        {
            this.pen = p;
            this.point1 = p1;
            this.point2 = p2;
        }

        public override void Teken(Graphics gr)
        {
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            return FormuleAfstandLijn(p) == 0;
        }

    }

    public class MaakKader : MaakObject
    {
        public MaakKader(Pen p, Rectangle rechthoek)
        {
            this.pen = p;
            this.Rechthoek = rechthoek;
        }

        public override void Teken(Graphics gr)
        {
            gr.DrawRectangle(pen, this.Rechthoek);
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            bool geklikt = false;
            Rectangle rec = Rechthoek;
            rec.X += 4;
            rec.Y += 4;
            rec.Width -= 8;
            rec.Height -= 8;

            if ((p.X >= Rechthoek.Left - 2 && p.X <= Rechthoek.Right) && (p.Y >= Rechthoek.Top && p.Y <= Rechthoek.Bottom))
                if ((p.X <= rec.Left || p.X >= rec.Right) || (p.Y <= rec.Top || p.Y >= rec.Bottom))
                    geklikt = true;

            return geklikt;
        }
    }

    public class MaakRechthoek : MaakObject
    {
        public MaakRechthoek(Brush kwast, Rectangle rechthoek)
        {
            this.Kwast = kwast;
            this.Rechthoek = rechthoek;
        }

        public override void Teken(Graphics gr)
        {
            gr.FillRectangle(this.Kwast, this.Rechthoek);
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            return (p.X >= Rechthoek.Left && p.X <= Rechthoek.Right) && (p.Y >= Rechthoek.Top && p.Y <= Rechthoek.Bottom);
        }
    }

    public class MaakCirkel : MaakObject
    {
        public MaakCirkel(Pen p, Rectangle rechthoek)
        {
            this.pen = p;
            this.Rechthoek = rechthoek;
        }

        public override void Teken(Graphics gr)
        {
            gr.DrawEllipse(pen, Rechthoek);
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            Size size = new Size(Rechthoek.Width / 2, Rechthoek.Height / 2);
            p = new Point(p.X - Rechthoek.X - size.Width, p.Y - Rechthoek.Y - size.Height);
            return FormuleCirkel(p, size) <= 1.15 && FormuleCirkel(p, size) >= 0.85;
        }
    }

    public class MaakRondje : MaakObject
    {
        public MaakRondje(Brush kwast, Rectangle rechthoek)
        {
            this.Kwast = kwast;
            this.Rechthoek = rechthoek;
        }

        public override void Teken(Graphics gr)
        {
            gr.FillEllipse(Kwast, Rechthoek);
        }

        public override bool geklikt(SchetsControl s, Point p)
        {
            Size size = new Size(Rechthoek.Width / 2, Rechthoek.Height / 2);
            p = new Point(p.X - Rechthoek.X - size.Width, p.Y - Rechthoek.Y - size.Height);
            return FormuleCirkel(p, size) <= 1;
        }
    }
}
