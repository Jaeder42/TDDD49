using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Projekt.GameComponents
{
   
    class Player
    {
        private int position;
        private int exit;
       
        private Slot[] slots;
        private Boolean gameOver;
        private int id;
       
        private Boolean playerAI;
        private Boolean moves;
        private Boolean moved;
       
        private Tiles[] availableTiles;
        private Game game;
        private Boolean active = false;
        public Player( Slot[] slots, int id, Game game, Tiles[] availableTiles)
        {
            
            this.availableTiles = availableTiles;
            this.slots = slots;
            this.game = game;
           // this.ai = ai;
            this.id = id;
            playerAI = false;
            moved = false;
            moves = false;

           setStartPos();
            //For testing
            //position = 0;
            //exit = 1;
           


            slots[position].setPlayer(this);
            gameOver = false;
            

        }

        public void setHasMove()
        {
            moves= true;
        }

        public Boolean hasMove()
        {
            return moves;
        }

        public int getID()
        {
            return id;
        }
        public void setAI()
        {
            playerAI = true;
            //this.ai = ai;
        }
        public Boolean isAI()
        {
            return playerAI;
        }

        public Boolean checkGameOver()
        {
            return gameOver;
        }
        public int getPos()
        {
            return position;
        }
        public Slot[] getSlots()
        {
            return slots;
        }

        public void updateSlots(Slot[] slots)
        {
            this.slots = slots;
            //moves = true;
            active = true;
            moved = false;
            //move();
        }

        public int getExit()
        {
            return exit;
        }
        public Boolean hasMoved()
        {
            return moved;
        }

       /* public void forcedMove()
        {
            int[][] pairs = slots[id].getPairs();
            int route = 0;
            if ( pairs[0][0]!= 0)
            {

                for (int i = 0; i < 4; i++)
                {

                    if (exit == pairs[i][0])
                    {
                        route = pairs[i][1];
                        break;
                    }
                    else if (exit == pairs[i][1])
                    {
                        route = pairs[i][0];
                    }


                }

            }
        }*/

        public void AI()
        {
            int select = 10;
            int count = 0;
            int row = slots[position].getPos()[0];
            int column = slots[position].getPos()[1];
            int[] allowed = allowedExits(row, column);

            for (int j = 0; j < 3; j++)
            {
                
                int[][] pairs = availableTiles[j].getPairs();
                int route = 0;
                


                for (int i = 0; i < 4; i++)
                {

                    if (exit == pairs[i][0])
                    {
                        route = pairs[i][1];
                        break;
                    }
                    else if (exit == pairs[i][1])
                    {
                        route = pairs[i][0];
                    }

                   
                }
                if (allowed.Contains(route))
                {
                    select = j;
                    break;
                }

                if (j == 2 && (count < 3))
                {
                    for (int k = 0; k < 3; k++)
                    {
                        availableTiles[k].rotate();
                    }
                    j = 0;
                    count++;
                }

            }

            if (select == 10)
            {
                select = 0;
            }
               
            //Console.WriteLine("Select: "+select);
            availableTiles[select].place(slots[position]);
            moves = true;
            game.drawTiles(select);
        }
        public void setActive()
        {
            active = true;
        }
        public void move()
        {
            moved = false;

            if (this.checkGameOver())
            {
                moved = true;
                moves = false;
            }
            
            else if(playerAI && active)
            {
                active = false;
                AI();
            }
               
                if (this.hasMove())
                {
                    active = false;
                    
                    while (true)
                    {
                        
                        int[][] pairs = slots[position].getPairs();
                        int route = 0;

                        if (pairs[0][0] == 0)
                        {
                            // Console.WriteLine("FUCCCK" + pairs[0][0]);
                            moves = false;
                            
                            break;
                        }
                        moved = true;
                        //Console.WriteLine("YEEES");
                        slots[position].setPlayer(null);

                        for (int i = 0; i < 4; i++)
                        {
                            if (exit == pairs[i][0])
                            {
                                route = pairs[i][1];
                                break;
                            }
                            else if (exit == pairs[i][1])
                            {
                                route = pairs[i][0];
                            }
                        }

                        if (route == 1 || route == 2)
                        {
                            if (slots[position].getPos()[1] == 1)
                            {
                                Console.WriteLine("Player" + id + " Lost");
                                gameOver = true;
                                break;
                            }
                            else
                            {
                                position -= 1;
                                if (route == 1)
                                    exit = 6;
                                else
                                    exit = 5;
                                slots[position].setPlayer(this);
                            }
                        }
                        else if (route == 3 || route == 4)
                        {
                            if (slots[position].getPos()[0] == 8)
                            {
                                Console.WriteLine("Player" + id + " Lost");
                                gameOver = true;
                                break;
                            }
                            else
                            {
                                position += 8;
                                if (route == 3)
                                    exit = 8;
                                else
                                    exit = 7;

                                slots[position].setPlayer(this);
                            }
                        }
                        else if (route == 5 || route == 6)
                        {
                            if (slots[position].getPos()[1] == 8)
                            {
                                Console.WriteLine("Player" + id + " Lost");
                                gameOver = true;
                                break;
                            }
                            else
                            {
                                position += 1;
                                if (route == 5)
                                    exit = 2;
                                else
                                    exit = 1;
                                slots[position].setPlayer(this);
                            }
                        }
                        else if (route == 7 || route == 8)
                        {
                            if (slots[position].getPos()[0] == 1)
                            {
                                Console.WriteLine("Player" + id + " Lost");
                                gameOver = true;
                                break;
                            }
                            else
                            {
                                position -= 8;
                                if (route == 7)
                                    exit = 4;
                                else
                                    exit = 3;
                                slots[position].setPlayer(this);
                            }
                        }



                    }
                
            }
            
        }
        public int[] allowedExits(int row, int column)
        {
            int[] tmp = new int[8]{1,2,3,4,5,6,7,8};
            if (row == 1)
            {
                if (column == 1)
                {
                    tmp = new int[4] { 3, 4, 5, 6 };
                }
                else if (column == 8)
                {
                    tmp = new int[4] { 1, 2, 3, 4 };
                }
                else
                {
                    tmp = new int[6] {1,2, 3, 4, 5, 6 };
                }
            }
            else if (row == 8)
            {
                if (column == 1)
                {
                    tmp = new int[4] {5,6,7,8 };
                }
                else if (column == 8)
                {
                    tmp = new int[4] { 1, 2, 7, 8 };
                }
                else
                {
                    tmp = new int[6] { 1, 2,5, 6,7,8 };
                }
            }
            else if (column == 1)
            {
                if (row == 1)
                {
                    tmp = new int[4] { 3, 4, 5, 6 };
                }
                else if (row == 8)
                {
                    tmp = new int[4] { 5, 6, 7, 8 };
                }
                else
                {
                    tmp = new int[6] { 3, 4, 5, 6, 7, 8 };
                }
            }
            else if (column == 8)
            {
                if (row == 1)
                {
                    tmp = new int[4] {1,2, 3, 4};
                }
                else if (row == 8)
                {
                    tmp = new int[4] { 1, 2, 7, 8 };
                }
                else
                {
                    tmp = new int[6] { 1, 2, 3, 4, 7, 8 };
                }
            }


            return tmp;
        }
       
        public void loadFromXml(XElement gamestate)
        {
            XElement xml = gamestate.Element("Player" + id);
           
            playerAI = (Boolean)xml.Element("AI");
            position = (int)xml.Element("Position");
            exit = (int)xml.Element("Exit");
            gameOver = (Boolean)xml.Element("GameOver");
            moves = (Boolean)xml.Element("Moves");
            moved = (Boolean)xml.Element("Moved");

            slots[position].setPlayer(this);
        }
        public void saveToXml(XElement gamestate)
        {
           XElement player = new XElement("Player" + id,
           new XElement("AI", playerAI),
           new XElement("Position", position),
           new XElement("Exit", exit),
           new XElement("GameOver", gameOver),
           new XElement("Moves", moves),
           new XElement("Moved", moved)
            );

            gamestate.Add(player);
        }

        private void setStartPos()
        {
           

            switch (id)
            {
                case 0:
                    position = 0;
                    exit = 1;
                    break;
                case 1:
                    position = 7;
                    exit = 6;
                    break;
                case 2:
                    position = 56;
                    exit = 2;
                    break;
                case 3:
                    position = 63;
                    exit = 5;
                    break;
            }

            slots[position].setPlayer(this);
            
        }

    }
}
