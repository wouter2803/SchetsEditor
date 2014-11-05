using System;
using System.Collections.Generic;
using System.Drawing;

namespace SchetsEditor
{
    public class Schets
    {
        private Bitmap bitmap;
        //toegevoegd, dit wordt de lijst waarin de getekende objecten worden opgeslagen
        private List<MaakObject> LijstObjecten = new List<MaakObject>();
         
        public Schets()
        {
            bitmap = new Bitmap(1, 1);
        }
        public Graphics BitmapGraphics
        {
            get { return Graphics.FromImage(bitmap); }
        }

        //toegevoegd om de bitmap te kunnen gebruiken in andere klassen
        public Bitmap returnbitmap 
        {
            get { return this.bitmap; }
        }

        //toegevoegd om de bitmap te kunnen veranderen vanuit andere klassen
        public Bitmap setbitmap //zelf
        {
            set { this.bitmap = value; }
        }

        public void VeranderAfmeting(Size sz)
        {
            if (sz.Width > bitmap.Size.Width || sz.Height > bitmap.Size.Height)
            {
                Bitmap nieuw = new Bitmap( Math.Max(sz.Width,  bitmap.Size.Width)
                                         , Math.Max(sz.Height, bitmap.Size.Height)
                                         );
                Graphics gr = Graphics.FromImage(nieuw);
                gr.FillRectangle(Brushes.White, 0, 0, sz.Width, sz.Height);
                gr.DrawImage(bitmap, 0, 0);
                bitmap = nieuw;
            }
        }
        public void Teken(Graphics gr)
        {
            gr.DrawImage(bitmap, 0, 0);
        }
        public void Schoon()
        {
            Graphics gr = Graphics.FromImage(bitmap);
            gr.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        }
        public void Roteer()
        {
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }
        
        //toegevoegd, 
        public void ZetInLijst(MaakObject obj)
        {
            LijstObjecten.Add(obj);
            TekenLijst();
        }

        //toegevoegd
        public void TekenLijst()
        {
            Schoon();
            Graphics gr = BitmapGraphics;
            foreach (MaakObject obj in LijstObjecten)
            {
                obj.Teken(gr);
            }
        }

        //toegevoegd
        public int LijstGrootte
        {
            get { return LijstObjecten.Count; }
        }

        //toegevoegd
        public List<MaakObject> objecten
        {
            get { return LijstObjecten; }
            set { LijstObjecten = value; }
        }

        //toegevoegd
        public void RemoveLastFromList()
        {
            if (LijstGrootte > 0)
                LijstObjecten.RemoveAt(LijstObjecten.Count - 1);
        }
    }
}
