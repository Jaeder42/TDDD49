using Projekt.GameComponents;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projekt
{
    public partial class Window : Form
    {
        Game game;
        Color color;

        public Window()
        {


            InitializeComponent();
            game = new Game(0, true);




            this.Invalidate();




        }




        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            //game.panelClick(panel);

        }

        private void panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
        }

        private void PlayerColorPanel_MouseHover(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackColor = System.Drawing.SystemColors.ControlLight;
        }

        private void PlayerColorPanel_MouseLeave(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackColor = System.Drawing.SystemColors.ControlDark;
        }

        private void PlayerColorPanel_MouseEnter(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.BackColor = System.Drawing.SystemColors.ControlLightLight;
        }

        private void availableTile_click(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            
        }

        private void availableTile_mousedown(object sender, MouseEventArgs e)
        {
            Panel panel = (Panel)sender;
            panel.DoDragDrop(panel.Name, DragDropEffects.Copy);
        }



        private void panel_DragEnter(object sender, DragEventArgs e)
        {
            /*Panel tmp = (Panel)sender;
            Console.WriteLine(tmp.Name);
            String dragger = e.Data.GetData(DataFormats.Text).ToString();
            Console.WriteLine(dragger);*/

            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        //Dragger är den rutan som blir dragen, sender är den som tar emot. Namnet på dragger plockas ut
        //Panelen sender plockas ut. Båda skickas till spelmotorn
        // Sedan målar vi om hela planen. Och till sist kollar vi om spelet är över.
        private void panel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Copy)
            {
                String dragger = e.Data.GetData(DataFormats.Text).ToString();
                Panel panel = (Panel)sender;
                int slot = Convert.ToInt32(panel.Name.Substring(5)) - 1;

                game.dragDrop(dragger, slot);

                Console.WriteLine("Dropped");

                this.Refresh();
                //checkGameOver();
            }
            else
            {
                Console.WriteLine("FAIL");
            }



        }

        private void paint_panel(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            //game.paintPanel(panel, e);
            //Graphics g = e.Graphics;
            slotPaint(e,Convert.ToInt32(panel.Name.Substring(5)) - 1);
        }
        private void paint_availabletile(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            int x = Convert.ToInt32(panel.Name.Substring(13)) - 1;
            tilePaint(e, x);
            //game.paintAvailableTile(panel, e);
        }

        //Anropar spelmotorn så att spelmotorn roterar biten.
        private void rotate_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            int x = Convert.ToInt32(button.Name.Substring(6)) - 1;
            game.rotate(x);
            this.Refresh();
        }
        //1 spelare 3 ain
        private void playerVsAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopGame();
            game = new Game(3, false);

            play();

        }
        //2 spelare 2 ain
        private void playersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopGame();
            game = new Game(2, false);

            play();
        }

        private void PlayerColorPanel_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            //game.drawActivePlayer(panel, e);
            Graphics g = e.Graphics;
            Pen pen;
            PointF point = new PointF(10, 50);
            Font font = new Font("Arial", 20);
            String message;
            Brush brush = new SolidBrush(Color.Black);

            switch (game.getActivePlayer())
            {
                case 0:
                    pen = new Pen(Color.Blue, 5);
                    message = "Player " + 1;
                    break;
                case 1:
                    pen = new Pen(Color.Red, 5);

                    message = "Player " + 2;
                    break;

                case 2:
                    pen = new Pen(Color.Black, 5);
                    message = "Player " + 3;
                    break;
                default:
                    pen = new Pen(Color.Purple, 5);
                    message = "Player " + 4;
                    break;

            }
            g.DrawString(message, font, brush, point);
            g.DrawRectangle(pen, 10, 10, 114, 114);
        }
        //3 spelare 1 ai
        private void playersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            stopGame();
            game = new Game(1, false);

            play();
        }
        // 4 spelare 0 ain
        private void playersToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            stopGame();
            game = new Game(0, false);

            play();
        }

        private void saveGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.saveToXml();
        }

        private void allAIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            game = new Game(4, false);

            play();
        }

        private void play()
        {
            while (!game.hasStopped())
            {
                Application.DoEvents();
                game.gameTick();

                if (game.hasChanged())
                {
                    this.Refresh();
                }
                if (game.hasStopped())
                {
                    return;
                }
                               
                else if (game.gameOver())
                {
                    displayWinner();
                    return;
                }
            }

            //Console.WriteLine("Någon kallade på PLAY");
            //game.start();

        }
        public void stopGame()
        {
            game.stopGame();
            while (!game.hasStopped()) {
                game.gameTick();
                if (game.gameOver())
                {
                    Console.WriteLine("GameOver före stop");
                    break;
                }
                //Console.WriteLine("."); 
            }
        }
        public void displayWinner()
        {
            MessageBox.Show("Player " + game.getWinner()+ " wins!");
        }
        private void loadGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game.loadFromXml();
        }

        private void Window_FormClosed(object sender, FormClosedEventArgs e)
        {

            //isClosed = true;
          

        }

        private void Window_FormClosing(object sender, FormClosingEventArgs e)
        {

            game.stopGame();
           
            /* while (true)
              {

                  game.stopGame();
                  game.gameTick();
                  if(game.hasStopped()){
                      break;
                  }
                  Console.WriteLine("Waitiong to close");
              }
              */
        }



        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            //    game = null;
        }

        private void Window_Shown(object sender, EventArgs e)
        {
            Console.WriteLine("Shown");
            play();
        }

        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Window_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Loaded");
            //this.Refresh();
            //game.resume();
        }

        private void tilePaint(PaintEventArgs e,int x){
            Pen pen = new Pen(Color.Black, 3);
            Graphics g = e.Graphics;
            int[][] exits =game.tileExits(x);
            int[][] pair = game.tilePairs(x);
            



            //Draw the line for the first pair.
            //Point point = exits
            g.DrawLine(pen, exits[pair[0][0] - 1][0], exits[pair[0][0] - 1][1], exits[pair[0][1] - 1][0], exits[pair[0][1] - 1][1]);

            // second

            g.DrawLine(pen, exits[pair[1][0] - 1][0], exits[pair[1][0] - 1][1], exits[pair[1][1] - 1][0], exits[pair[1][1] - 1][1]);
            // third

            g.DrawLine(pen, exits[pair[2][0] - 1][0], exits[pair[2][0] - 1][1], exits[pair[2][1] - 1][0], exits[pair[2][1] - 1][1]);
            //fourth

            g.DrawLine(pen, exits[pair[3][0] - 1][0], exits[pair[3][0] - 1][1], exits[pair[3][1] - 1][0], exits[pair[3][1] - 1][1]);
            g.Dispose();
        }
        //Paint metoder för alla "sprites"
        public void slotPaint(PaintEventArgs e, int x)
        {
            Pen pen = new Pen(Color.Black, 3);
            Graphics g = e.Graphics;
            int[][] exits = game.slotExits(x);
            Player[] players = game.slotPlayers(x);
            int[][] pair = game.slotPairs(x);

            //Om vi har lagt en tile på denna slot ritar vi vägarna annars ritar vi eventuella spelare.
            if (game.drawSlot(x))
            {




                //Draw the line for the first pair.
                //Point point = exits
                g.DrawLine(pen, exits[pair[0][0] - 1][0], exits[pair[0][0] - 1][1], exits[pair[0][1] - 1][0], exits[pair[0][1] - 1][1]);

                // second

                g.DrawLine(pen, exits[pair[1][0] - 1][0], exits[pair[1][0] - 1][1], exits[pair[1][1] - 1][0], exits[pair[1][1] - 1][1]);
                // third

                g.DrawLine(pen, exits[pair[2][0] - 1][0], exits[pair[2][0] - 1][1], exits[pair[2][1] - 1][0], exits[pair[2][1] - 1][1]);
                //fourth

                g.DrawLine(pen, exits[pair[3][0] - 1][0], exits[pair[3][0] - 1][1], exits[pair[3][1] - 1][0], exits[pair[3][1] - 1][1]);


            }
            else
            {
                for (int i = 3; i >=0 ; i--)
                {
                    if (players[i] != null)
                    {
                        int y = players[i].getID();

                        playerPaint(g, players[i].getID());
                        //Console.WriteLine("Player " + i);
                    }
                }


                g.Dispose();
            }
        }
        public void playerPaint(Graphics g, int x)
        {
            
            

            switch (x)
            {
                case 0:
                    color = Color.Blue;
                    break;
                case 1:
                    color = Color.Red;
                    break;
                case 2:
                    color = Color.Black;
                    break;
                case 3:
                    color = Color.Purple;
                    break;
            }
            Pen pen = new Pen(color, 3);
            switch (game.getPlayerExit(x))
            {

                case 1:
                    g.DrawRectangle(pen, 2, 10, 10, 10);
                    break;
                case 2:
                    g.DrawRectangle(pen, 2, 44, 10, 10);
                    break;
                case 3:
                    g.DrawRectangle(pen, 10, 52, 10, 10);
                    break;
                case 4:
                    g.DrawRectangle(pen, 44, 52, 10, 10);
                    break;
                case 5:
                    g.DrawRectangle(pen, 52, 44, 10, 10);
                    break;
                case 6:
                    g.DrawRectangle(pen, 52, 10, 10, 10);
                    break;
                case 7:
                    g.DrawRectangle(pen, 44, 2, 10, 10);
                    break;
                case 8:
                    g.DrawRectangle(pen, 10, 2, 10, 10);
                    break;


            }






        }
    }
}
