using System;
using System.Drawing;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SchetsEditor
{
    public class SchetsControl : UserControl
    {
        private Schets schets;
        private Color penkleur;

        public Color PenKleur 
        {   
            get { return penkleur; } 
        }


        //toegevoegd
        public Bitmap GetBitmap
        {
            get { return this.schets.returnbitmap; }
        }

        //toegevoegd
        public Bitmap SetBitmap
        {
            set { this.schets.setbitmap = value; }
        }

        public SchetsControl()
        {   
            this.BorderStyle = BorderStyle.Fixed3D;
            this.schets = new Schets();
            this.Paint += this.teken;
            this.Resize += this.veranderAfmeting;
            this.veranderAfmeting(null, null);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void teken(object o, PaintEventArgs pea)
        {   schets.Teken(pea.Graphics);
        }
        private void veranderAfmeting(object o, EventArgs ea)
        {   schets.VeranderAfmeting(this.ClientSize);
            this.Invalidate();
        }
        public Graphics MaakBitmapGraphics()
        {   Graphics g = schets.BitmapGraphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            return g;
        }
        //aangepast
        public void Schoon(object o, EventArgs ea)
        {   schets.Schoon();
            this.objecten.Clear();
            this.Invalidate();
        }
        public void Roteer(object o, EventArgs ea)
        {   schets.Roteer();
            this.veranderAfmeting(o, ea);
        }

        //toegevoegd
        public void Undo(object o, EventArgs ea)
        {
            schets.RemoveLastFromList();
            schets.Schoon();
            this.Invalidate();
            schets.TekenLijst();
        }
        public void VeranderKleur(object obj, EventArgs ea)
        {   string kleurNaam = ((ComboBox)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        public void VeranderKleurViaMenu(object obj, EventArgs ea)
        {   string kleurNaam = ((ToolStripMenuItem)obj).Text;
            penkleur = Color.FromName(kleurNaam);
        }
        
        //toegevoegd
        public void ZetInLijst(MaakObject obj)
        {
            schets.ZetInLijst(obj);
        }
        
        //toegevoegd
        public int LijstGrootte
        {
            get { return schets.LijstGrootte; }
        }

        public List<MaakObject> objecten
        {
            get { return schets.objecten; }
            set { schets.objecten = value; }
        }

        //toegevoegd
        public void TekenLijst()
        {
            schets.TekenLijst();
        }

        //toegevoegd
        public Schets zetschetobject
        {
            get { return schets; }
            set { schets = value; }
        }
    }
}
