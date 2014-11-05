using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO; //zelf


namespace SchetsEditor
{
    public class Hoofdscherm : Form
    {
        MenuStrip menuStrip;
        Schets schets;

        public Hoofdscherm(Schets sch)
        {   this.ClientSize = new Size(800, 600);
            menuStrip = new MenuStrip();
            this.Controls.Add(menuStrip);
            this.maakFileMenu();
            this.maakHelpMenu();
            this.Text = "Schets editor";
            this.IsMdiContainer = true;
            this.MainMenuStrip = menuStrip;

            this.schets = sch;
        }
        private void maakFileMenu()
        {   ToolStripDropDownItem menu;
            menu = new ToolStripMenuItem("File");
            menu.DropDownItems.Add("Nieuw", null, this.nieuw);
            menu.DropDownItems.Add("Openen", null, this.openen);
            menu.DropDownItems.Add("Exit", null, this.afsluiten);
            menuStrip.Items.Add(menu);
        }
        private void maakHelpMenu()
        {   ToolStripDropDownItem menu;
            menu = new ToolStripMenuItem("Help");
            menu.DropDownItems.Add("Over \"Schets\"", null, this.about);
            menuStrip.Items.Add(menu);
        }
        private void about(object o, EventArgs ea)
        {   MessageBox.Show("Schets versie 1.0\n(c) UU Informatica 2010\n Edited by Wouter Sondagh and Lars van Valen"
                           , "Over \"Schets\""
                           , MessageBoxButtons.OK
                           , MessageBoxIcon.Information
                           );
        }

        private void nieuw(object sender, EventArgs e)
        {   SchetsWin s = new SchetsWin();
            s.MdiParent = this;
            s.Show();
        }
//hier van alles gedaan
        private void openen(object sender, EventArgs e)
        {
            SchetsWin s = new SchetsWin();
            s.MdiParent = this;

            schets = s.zetschetsobject;
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Txt|*.txt";
            open.Filter = "text|*.txt|Jpeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            open.Title = "Open a File";
            open.ShowDialog();
            string[] line;
            string regel;

            if (open.FileName != "")
            {
                StreamReader filereader = new StreamReader(open.FileName);

                if (open.FilterIndex == 1)
                {
                    while ((regel = filereader.ReadLine()) != null)
                    {
                        line = regel.Split('_');
                        #region type
                        string[] typesplit;
                        string type;
                        typesplit = line[0].Split('.');
                        type = typesplit[1];
                        #endregion
                        #region kleur
                        string stringkleur;
                        string[] kleursplit;
                        Color kleur;

                        stringkleur = line[1];
                        stringkleur = stringkleur.Trim(new char[] { 'C', 'o', 'l', 'r', ' ', '[', ']' });
                        kleursplit = stringkleur.Split('=', ',');
                        kleur = Color.FromArgb(int.Parse(kleursplit[1]), int.Parse(kleursplit[3]), int.Parse(kleursplit[5]), int.Parse(kleursplit[7]));
                        #endregion
                        #region Rechthoek
                        Rectangle rechthoek;
                        string[] Rechthoeksplit;
                        string x, y, width, height;
                        Rechthoeksplit = line[2].Split(',');

                        x = Rechthoeksplit[0].Substring(3);
                        y = Rechthoeksplit[1].Substring(2);
                        width = Rechthoeksplit[2].Substring(6);
                        height = Rechthoeksplit[3].Substring(7, Rechthoeksplit[3].IndexOf('}') - Rechthoeksplit[3].IndexOf('=') - 1);
                        rechthoek = new Rectangle(int.Parse(x), int.Parse(y), int.Parse(width), int.Parse(height));
                        #endregion
                        #region point1
                        Point point1;
                        string xP1, yP1;
                        string[] point1split;
                        point1split = line[3].Split(',');

                        xP1 = point1split[0].Substring(3);
                        yP1 = point1split[1].Substring(2, point1split[1].IndexOf('}') - point1split[1].IndexOf('=') - 1);
                        point1 = new Point(int.Parse(xP1), int.Parse(yP1));
                        #endregion
                        #region point2
                        Point point2;
                        string xP2, yP2;
                        string[] point2split;
                        point2split = line[4].Split(',');

                        xP2 = point2split[0].Substring(3);
                        yP2 = point2split[1].Substring(2, point2split[1].IndexOf('}') - point2split[1].IndexOf('=') - 1);
                        point2 = new Point(int.Parse(xP2), int.Parse(yP2));
                        #endregion
                        #region startpunt
                        Point startpunt;
                        string startpuntx, startpunty;
                        string[] startpuntsplit;
                        startpuntsplit = line[5].Split(',');

                        startpuntx = startpuntsplit[0].Substring(3);
                        startpunty = startpuntsplit[1].Substring(2, startpuntsplit[1].IndexOf('}') - startpuntsplit[1].IndexOf('=') - 1);
                        startpunt = new Point(int.Parse(startpuntx), int.Parse(startpunty));
                        #endregion
                        #region tekst
                        string tekst;
                        tekst = line[6];
                        #endregion

                        switch (type)
                        {
                            case "Tekstobject":
                                this.schets.ZetInLijst(new Tekstobject(tekst, new Font("Tahoma", 40), new SolidBrush(kleur), startpunt));
                                break;
                            case "MaakLijn":
                                this.schets.ZetInLijst(new MaakLijn(new Pen(kleur, 3), point1, point2));
                                break;
                            case "MaakKader":
                                this.schets.ZetInLijst(new MaakKader(new Pen(kleur, 3), rechthoek));
                                break;
                            case "MaakRechthoek":
                                this.schets.ZetInLijst(new MaakRechthoek(new SolidBrush(kleur), rechthoek));
                                break;
                            case "MaakCirkel":
                                this.schets.ZetInLijst(new MaakCirkel(new Pen(kleur, 3), rechthoek));
                                break;
                            case "MaakRondje":
                                this.schets.ZetInLijst(new MaakRondje(new SolidBrush(kleur), rechthoek));
                                break;
                        }
                        s.Show();
                    }
                }
                else
                {
                        Bitmap bm = new Bitmap(open.FileName);
                        s.setbitmap = bm;
                        s.Show();
                }
            }
        }

        private void afsluiten(object sender, EventArgs e)
        {   this.Close();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Hoofdscherm
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "Hoofdscherm";
            this.ResumeLayout(false);

        }

        
    }
}
