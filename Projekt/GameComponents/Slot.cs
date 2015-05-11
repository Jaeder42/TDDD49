using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows.Forms;
using System.Xml.Linq;

namespace Projekt.GameComponents
{
    class Slot
    {
        private int[] pair1;
        private int[] pair2;
        private int[] pair3;
        private int[] pair4;
        private int[] position;
        private bool occupied;
        private int[] pos1 = new int[2];
        private int[] pos2 = new int[2];
        private int[] pos3 = new int[2];
        private int[] pos4 = new int[2];
        private int[] pos5 = new int[2];
        private int[] pos6 = new int[2];
        private int[] pos7 = new int[2];
        private int[] pos8 = new int[2];

        private int[][] exits = new int[8][];

        private Player[] player;
        

        public Slot(int x, int y)
        {

            position = new int[2];

            position[0] = x;
            position[1] = y;

            player = new Player[4];
            

            pair1 = new int[2] {0,0};
            pair2 = new int[2] {0, 0};
            pair3 = new int[2] { 0, 0};
            pair4 = new int[2] { 0, 0 };
            occupied = false;

            pos1[0] = 1;
            pos1[1] = 10;
            pos2[0] = 1;
            pos2[1] = 54;
            
            pos3[0] = 10;
            pos3[1] = 63;
            pos4[0] = 54;
            pos4[1] = 63;

            pos5[0] = 63;
            pos5[1] = 54;
            pos6[0] = 63;
            pos6[1] = 10;

            pos7[0] = 54;
            pos7[1] = 1;
            pos8[0] = 10;
            pos8[1] = 1;

            exits[0] = pos1;
            exits[1] = pos2;
            exits[2] = pos3;
            exits[3] = pos4;
            exits[4] = pos5;
            exits[5] = pos6;
            exits[6] = pos7;
            exits[7] = pos8;

        }
        public int[][] getExits(){
            return exits;
        }
        public int[][] getPairs()
        {
            int[][] val = new int[4][];

            val[0] = pair1;
            val[1] = pair2;
            val[2] = pair3;
            val[3] = pair4;

            return val;
        }
        public Player[] getPlayers()
        {
            return player;
            /*int[] tmp = new int[]{5,5,5,5};

            for (int i = 0; i < 4; i++)
            {
                if (player[i] != null)
                {
                    tmp[i] = player[i].getID();
                }
                else
                {
                    tmp[i] = 5;
                }
            }
            return tmp;*/
        }

        public int[] getPos()
        {
            return position;
        }
        public void click()
        {
            Console.WriteLine(position[0] + " " + position[1]);
        }
        public void setPlayer(Player player)
        {
            if (player != null)
            {
                this.player[player.getID()] = player;
            }
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    this.player[i] = null;
                }
            }
            
            
        }
        public Boolean getPlayer(int activePlayer)
        {
            if (player != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (player[i] != null)
                    {
                        if (player[i].getID() == activePlayer)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            else
                return false;
            
        }
 

        public void receiveData(int[] pair1,int[] pair2,int[] pair3,int[] pair4)
        {
            this.pair1 = pair1;
            this.pair2 = pair2;
            this.pair3 = pair3;
            this.pair4 = pair4;
            occupied = true;

            for (int i = 0; i < 4; i++)
            {
                if(player[i] != null)
                player[i].setHasMove();
            }
            

         /*   Console.Write(pair1[0] + "" + pair1[1] + " ");
            Console.Write(pair2[0] + "" + pair2[1] + " ");
            Console.Write(pair3[0] + "" + pair3[1] + " ");
            Console.Write(pair4[0] + "" + pair4[1]);
            Console.WriteLine();*/
        }

        public Boolean isOccupied()
        {
            return occupied;
        }

        

        public void loadFromXml(XElement gamestate, int i)
      
        {
            XElement xml = gamestate.Element("Slot" + i);
            pair1[0] = (int)xml.Element("Pair10");
            pair1[1] = (int)xml.Element("Pair11");
            pair2[0] = (int)xml.Element("Pair20");
            pair2[1] = (int)xml.Element("Pair21");
            pair3[0] = (int)xml.Element("Pair30");
            pair3[1] = (int)xml.Element("Pair31");
            pair4[0] = (int)xml.Element("Pair40");
            pair4[1] = (int)xml.Element("Pair41");
            occupied = (Boolean)xml.Element("Occupied");
            
        }
       
        public void saveToXml(XElement gamestate, int i){
            XElement slot = new XElement("Slot" + i,
             new XElement("Pair10", pair1[0]),
             new XElement("Pair11", pair1[1]),
             new XElement("Pair20", pair2[0]),
             new XElement("Pair21", pair2[1]),
             new XElement("Pair30", pair3[0]),
             new XElement("Pair31", pair3[1]),
             new XElement("Pair40", pair4[0]),
             new XElement("Pair41", pair4[1]),
             new XElement("Occupied", occupied)
             );

            gamestate.Add(slot);

            
        }
        
    }

    
}
    

