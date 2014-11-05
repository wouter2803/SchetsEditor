using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SchetsEditor
{
    public interface ISchetsTool
    {
        void MuisVast(SchetsControl s, Point p);
        void MuisDrag(SchetsControl s, Point p);
        void MuisLos(SchetsControl s, Point p);
        void Letter(SchetsControl s, char c);
    }

    public abstract class StartpuntTool : ISchetsTool
    {
        protected Point startpunt;
        protected Brush kwast;

        public virtual void MuisVast(SchetsControl s, Point p)
        {   startpunt = p;
        }
        public virtual void MuisLos(SchetsControl s, Point p)
        {   kwast = new SolidBrush(s.PenKleur);
        }
        public abstract void MuisDrag(SchetsControl s, Point p);
        public abstract void Letter(SchetsControl s, char c);
    }

    public class TekstTool : StartpuntTool
    {
        public override string ToString() { return "tekst"; }

        public override void MuisDrag(SchetsControl s, Point p) { }

        public override void Letter(SchetsControl s, char c)
        {
            if (c >= 32)
            {
                Graphics gr = s.MaakBitmapGraphics();
                Font font = new Font("Tahoma", 40);
                string tekst = c.ToString();
                SizeF sz = 
                gr.MeasureString(tekst, font, this.startpunt, StringFormat.GenericTypographic);
                s.ZetInLijst(new Tekstobject(tekst, font, kwast, this.startpunt));
                startpunt.X += (int)sz.Width;
                s.Invalidate();
            }
        }
    }

    public abstract class TweepuntTool : StartpuntTool
    {
        public static Rectangle Punten2Rechthoek(Point p1, Point p2)
        {   return new Rectangle( new Point(Math.Min(p1.X,p2.X), Math.Min(p1.Y,p2.Y))
                                , new Size (Math.Abs(p1.X-p2.X), Math.Abs(p1.Y-p2.Y))
                                );
        }
        public static Pen MaakPen(Brush b, int dikte)
        {   Pen pen = new Pen(b, dikte);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            return pen;
        }
        public override void MuisVast(SchetsControl s, Point p)
        {   base.MuisVast(s, p);
            kwast = Brushes.Gray;
        }
        public override void MuisDrag(SchetsControl s, Point p)
        {   s.Refresh();
            this.Bezig(s.CreateGraphics(), this.startpunt, p);
        }
        public override void MuisLos(SchetsControl s, Point p)
        {   base.MuisLos(s, p);
            s.Invalidate();
        }
        public override void Letter(SchetsControl s, char c)
        {
        }
        public abstract void Bezig(Graphics g, Point p1, Point p2);
    }

    public class RechthoekTool : TweepuntTool
    {
        public override string ToString() { return "kader"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {   g.DrawRectangle(MaakPen(kwast,3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void MuisLos(SchetsControl s, Point p)//zelf
        {
            base.MuisLos(s, p);
            s.ZetInLijst(new MaakKader(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(startpunt, p)));
        }
    }
    
    public class VolRechthoekTool : TweepuntTool //veranderd omdat ik anders 2 items in mijn lijst krijgt
    {
        public override string ToString() { return "vlak"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawRectangle(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        //toegevoegd
        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            s.ZetInLijst(new MaakRechthoek(kwast, TweepuntTool.Punten2Rechthoek(startpunt, p)));
        }
    }

    public class LijnTool : TweepuntTool
    {
        public override string ToString() { return "lijn"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {   g.DrawLine(MaakPen(this.kwast,3), p1.X, p1.Y, p2.X, p2.Y);
        }

        //toegevoegd
        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            s.ZetInLijst(new MaakLijn(MaakPen(this.kwast,3), startpunt, p));
        }
    }

    public class PenTool : LijnTool
    {
        public override string ToString() { return "pen"; }

        public override void MuisDrag(SchetsControl s, Point p)
        {   
            this.MuisLos(s, p);
            this.MuisVast(s, p);     
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
        }
    }
    
    public class GumTool : TweepuntTool
    {
        public override string ToString() { return "gum"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        { //leeg maar verplicht
        }

        public override void MuisVast(SchetsControl s, Point p)
        {
            for (int i = s.LijstGrootte - 1; i >= 0; i--)
                if (s.objecten[i].geklikt(s, p))
                {
                    s.objecten.RemoveAt(i);
                    s.Refresh();
                    s.TekenLijst();
                    break;
                }
        }
    }

    //toegevoegd
    public class CirkelTool : TweepuntTool
    {
        public override string ToString() { return "cirkel"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            s.ZetInLijst(new MaakCirkel(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(startpunt, p)));
        }
    }

    //toegevoegd
    public class VolCirkelTool : TweepuntTool
    {
        public override string ToString() { return "rondje"; }

        public override void Bezig(Graphics g, Point p1, Point p2)
        {
            g.DrawEllipse(MaakPen(kwast, 3), TweepuntTool.Punten2Rechthoek(p1, p2));
        }

        public override void MuisLos(SchetsControl s, Point p)
        {
            base.MuisLos(s, p);
            s.ZetInLijst(new MaakRondje(kwast, TweepuntTool.Punten2Rechthoek(startpunt, p)));
        }
    }
}
