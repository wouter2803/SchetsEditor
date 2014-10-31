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

        public abstract bool geklikt();
    }

    public class MaakCirkel : MaakObject
    {
        public MaakCirkel(Pen p, Rectangle rechthoek)
        {
            this.pen = p;
            this.Rechthoek = rechthoek;
        }

        public override bool geklikt()
        {
            return false;
        }
    }

    public class MaakRondje : MaakObject
    {
        public MaakRondje(Brush kwast, Rectangle rechthoek)
        {
            this.Kwast = kwast;
            this.Rechthoek = rechthoek;
        }

        public override bool geklikt()
        {
            return false;
        }
    }
}
