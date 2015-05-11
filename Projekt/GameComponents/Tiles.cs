using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Projekt.GameComponents
{

    class Tiles
    {
        private int[] pair1;
        private int[] pair2;
        private int[] pair3;
        private int[] pair4;
        private Random random;
        private int[] used;
        private Boolean placed;

        private int[] pos1 = new int[2];
        private int[] pos2 = new int[2];
        private int[] pos3 = new int[2];
        private int[] pos4 = new int[2];
        private int[] pos5 = new int[2];
        private int[] pos6 = new int[2];
        private int[] pos7 = new int[2];
        private int[] pos8 = new int[2];

        

        private int[][] exits = new int[8][];

        public Tiles(Random random)
        {   
            used = new int[8];
            pair1 = new int[2];
            pair2 = new int[2];
            pair3 = new int[2];
            pair4 = new int[2];
            this.random = random;
            placed = false;
            
            randomizeTile(pair1);
            randomizeTile(pair2);
            randomizeTile(pair3);
            randomizeTile(pair4);

            used = new int[8];

          /*  Console.Write(pair1[0] + ""+ pair1[1]+ " ");
            Console.Write(pair2[0] + "" + pair2[1]+ " ");
            Console.Write(pair3[0] + "" + pair3[1]+ " ");
            Console.Write(pair4[0] + "" + pair4[1]);
            Console.WriteLine();

           /* rotate();

            Console.Write(pair1[0] + " " + pair1[1] + " ");
            Console.Write(pair2[0] + " " + pair2[1] + " ");
            Console.Write(pair3[0] + " " + pair3[1] + " ");
            Console.Write(pair4[0] + " " + pair4[1]);*/

           

            pos1[0] = 0;
            pos1[1] = 30;
            pos2[0] = 0;
            pos2[1] = 134;

            pos3[0] = 30;
            pos3[1] = 164;
            pos4[0] = 104;
            pos4[1] = 164;

            pos5[0] = 134;
            pos5[1] = 134;
            pos6[0] = 134;
            pos6[1] = 30;

            pos7[0] = 104;
            pos7[1] = 0;
            pos8[0] = 30;
            pos8[1] = 0;

            exits[0] = pos1;
            exits[1] = pos2;
            exits[2] = pos3;
            exits[3] = pos4;
            exits[4] = pos5;
            exits[5] = pos6;
            exits[6] = pos7;
            exits[7] = pos8;
            
        }
        public int[][] getPairs()
        {
            int[][] pairs = new int[4][];
            pairs[0] = pair1;
            pairs[1] = pair2;
            pairs[2] = pair3;
            pairs[3] = pair4;

            return pairs;
        }
        public void place(Slot slot)
        {
            //Console.WriteLine("HEJ");
            slot.receiveData(pair1,pair2,pair3,pair4);
        }

        public void rotate()
        {
            rotatePair(pair1);
            rotatePair(pair2);
            rotatePair(pair3);
            rotatePair(pair4);
        }
        public void rotatePair(int[] pair)
        {
            int tmp0 = pair[0]+2;
            int tmp1 = pair[1]+2;

            if (tmp0 == 9)
            {
                tmp0 = 1;
            }
            else if (tmp0 == 10)
            {
                tmp0 = 2;
            }

            if (tmp1 == 9)
            {
                tmp1 = 1;
            }
            else if (tmp1 == 10)
            {
                tmp1 = 2;
            }

            pair[0] = tmp0;
            pair[1] = tmp1;
        }

     

        public void setPlaced()
        {
            placed = true;
        }
        public Boolean isPlaced()
        {
            return placed;
        }
        public void click()
        {
            Console.WriteLine(pair1[0] + " " + pair1[1]);
            Console.WriteLine(pair2[0] + " " + pair2[1]);
            Console.WriteLine(pair3[0] + " " + pair3[1]);
            Console.WriteLine(pair4[0] + " " + pair4[1]);
        }

        public int[][] getExits()
        {
            return exits;
        }

        public void paint(PaintEventArgs e)
        {
           
                Pen pen = new Pen(Color.Black, 3);
                Graphics g = e.Graphics;



                //Draw the line for the first pair.
                //Point point = exits
                g.DrawLine(pen, exits[pair1[0] - 1][0], exits[pair1[0] - 1][1], exits[pair1[1] - 1][0], exits[pair1[1] - 1][1]);

                // second

                g.DrawLine(pen, exits[pair2[0] - 1][0], exits[pair2[0] - 1][1], exits[pair2[1] - 1][0], exits[pair2[1] - 1][1]);
                // third

                g.DrawLine(pen, exits[pair3[0] - 1][0], exits[pair3[0] - 1][1], exits[pair3[1] - 1][0], exits[pair3[1] - 1][1]);
                //fourth

                g.DrawLine(pen, exits[pair4[0] - 1][0], exits[pair4[0] - 1][1], exits[pair4[1] - 1][0], exits[pair4[1] - 1][1]);
                g.Dispose();
            
        }
        public void loadFromXml(XElement gamestate, int i)
        {
            XElement xml = gamestate.Element("Tile" + i);
            pair1[0] = (int)xml.Element("Pair10");
            pair1[1] = (int)xml.Element("Pair11");
            pair2[0] = (int)xml.Element("Pair20");
            pair2[1] = (int)xml.Element("Pair21");
            pair3[0] = (int)xml.Element("Pair30");
            pair3[1] = (int)xml.Element("Pair31");
            pair4[0] = (int)xml.Element("Pair40");
            pair4[1] = (int)xml.Element("Pair41");
        }
        public void saveToXML(XElement gameState, int i)
        {
            XElement tile = new XElement ("Tile"+i,
                new XElement("ID", i),
               new XElement("Pair10", pair1[0]),
             new XElement("Pair11", pair1[1]),
             new XElement("Pair20", pair2[0]),
             new XElement("Pair21", pair2[1]),
             new XElement("Pair30", pair3[0]),
             new XElement("Pair31", pair3[1]),
             new XElement("Pair40", pair4[0]),
             new XElement("Pair41", pair4[1])
             
             );

            gameState.Add(tile);
        }
        //Slumpar ihop två utgångar.
        public void randomizeTile(int[] pair)
        {
            //random = new Random();
            int tmp = 0;
            
                for (int j = 0; j <= 1; j++)
                {
                    while (true)
                    {
                        tmp = random.Next(0, 9);


                       
                        if (!used.Contains(tmp))
                        {
                        
                            //
                            pair[j] = tmp;
                            for (int i = 0; i < used.Length; i++)
                            {
                                if (used[i] == 0)
                                {
                                    used[i] = tmp;
                                    break;
                                }
                            }
                            break;
                        }

                    }
                }
        }
    }
}
