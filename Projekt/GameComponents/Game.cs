using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using System.Xml;
using System.Xml.Linq;


namespace Projekt.GameComponents
{
    class Game
    {
        private Slot[] slots;
        private Tiles[] tiles;
        private Player[] players;
        private Tiles[] availableTiles;
        private Random random;
        private int activePlayer;
        private Boolean gameover;
        private int playerAmount;
        private int lost;
       
        private int nextTile;
        private int lastPlayer;
        private Boolean stop;
        private int winner;
        private Boolean stopped;
        private Boolean load;
        Boolean change = false;
   
        FileSystemWatcher watcher;

        
        
        public Game(int ainr, Boolean load)
        {
            this.load = load;
            stop = false;
            random = new Random();
            tiles = new Tiles[64];
            gameover = false;
          
            playerAmount = 4;
           // ai = new AI(this);
            lastPlayer = 5;
            nextTile = 0;
            lost = 0;
            availableTiles = new Tiles[3];
            slots = new Slot[64];
            initWatcher();
            //Check if there was a game prior and run it

           
                int count = 0;
                for (int i = 1; i < 9; i++)
                {
                    for (int j = 1; j < 9; j++)
                    {
                        slots[count] = new Slot(i, j);
                        count++;
                    }
                }

                //Vi skickar in en random för att inte få samma randomseed(tidsberoende)
                for (int i = 0; i < 64; i++)
                {
                    tiles[i] = new Tiles(random);
                }
                for (int i = 0; i < 3; i++)
                {
                    drawTiles(i);
                }

                // ai.updateAvailable(availableTiles);

                //Initierar alla spelare, sätter alla AIn så att lägsta siffrorna allti är människospelare.
                players = new Player[playerAmount];
                // ai = new AI(this, availableTiles);
                for (int i = 0; i < playerAmount; i++)
                    players[i] = new Player(slots, i, this, availableTiles);
                for (int i = playerAmount; i > playerAmount - ainr; i--)
                {
                    players[i - 1].setAI();

                }


                activePlayer = 0;

            
            if (File.Exists("GameState.xml")&&load)
            {
                Console.WriteLine("Startar fåregående spel");
                XElement gamestate = loadFromXml();
                players[activePlayer].setActive();
                for (int i = 0; i < 3; i++)
                {
                    availableTiles[i].loadFromXml(gamestate,i);
                }
                stop = false;
                
                //gameLoop();
            }
            else if(File.Exists("GameState.xml"))
            {
                File.Delete("GameState.xml");
                //saveToXml();
            }
            
            
            players[activePlayer].setActive();


            
                
        }

        public void initWatcher()
        {
            //string[] args = System.Environment.GetCommandLineArgs();


            //// If a directory is not specified, exit program. 
            //if (args.Length < 2)
            //{
            //    // Display the proper way to call the program.
            //    Console.WriteLine("Usage: Watcher.exe (directory)"+args);
            //    return;
            //}
           string args = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            watcher = new FileSystemWatcher();
            watcher.Path = args;
            Console.WriteLine("Path: " + args);
            /* Watch for changes in LastAccess and LastWrite times, and
               the renaming of files or directories. */
            watcher.NotifyFilter =  NotifyFilters.LastWrite;
            
            //watcher.Filter = "*.xml";

            // Add event handlers.
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
          
        }
        public void OnChanged(object source, FileSystemEventArgs e)
        {
            load = true;
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);
        }
        public Boolean didLoad()
        {
            return load;
        }
        public void start()
        {
            stop = false;
            gameLoop();
        }
        public int getWinner()
        {
            return winner+1;
        }
        
        public Boolean hasChanged()
        {
            if (change) {
                change = false;
                return true; }

            if (lastPlayer != activePlayer)
            {
                lastPlayer = activePlayer;
                return true;
            }

            else
                return false;
        }
        public void resume()
        {

        
            
        }
        //Vi kollar om lost är större än 3, om så är fallet skriver vi vem som vann.
        public Boolean gameOver()
        {
            lost = 0;
            
            for (int i = 0; i < playerAmount; i++)
            {
                if (players[i].checkGameOver())
                {
                    lost++;
                }
               
            }
           // Console.WriteLine("Lost: " + lost);
            if (lost >= (playerAmount - 1))
            {
                for (int i = 0; i < playerAmount; i++)
                {
                    if (!players[i].checkGameOver())
                    {
                        winner = i;
                       /* MessageBox.Show("Player " + (i + 1) + " Wins!");*/
                        gameover = true ;
                        stop = true;
                        break;
                        
                    }

                }
               
            }
            
            

                return gameover;
        }
        public void rotate(int x)
        {
            
            Console.WriteLine(x);
            availableTiles[x].rotate();
            change = true;
           
        }
      
        public void stopGame()
        {
            stop = true;
            //stopped = true;
            load = false;
            //gameover = true;
            Console.WriteLine("Stop has been called");
            
        }
        public void gameLoop()
        {
           
                
               // Application.DoEvents();
                if (stop || gameover)
                {
                    //stopped = true;
                  
                }
                else
                {
                    gameTick();
                    if (hasChanged())
                    {
                        
                        saveToXml();
                    }
                }
                if (stop || gameover)
                {
                    //stopped = true;
              
                }
            
            return;

        }
        public void gameTick()
        {
            
                
            if (!gameover)
            {

                
                    
                    
                  
                if (load)
                {
                    loadFromXml();
                    load = false;
                }
                    
                    if (stop)
                    {
                        stop = true;
                        saveToXml();
                        stopped = true;
                        return;
                    }

                    players[activePlayer].move();
                    //gui.Refresh();

                    if (players[activePlayer].hasMoved())
                    {
                        for (int i = 0; i < playerAmount; i++)
                        {
                            players[i].move();
                            if (gameOver() )
                            {
                               
                                //MessageBox.Show("Player " + (winner + 1) + " Wins!");
                                gameover = true;
                                //stopped = true;
                                //stop = true;
                              
                                File.Delete("GameState.xml");
                                Console.WriteLine("Save file deleted");
                                break;
                                //return;
                            }

                        }


                        activePlayer++;
                        if (activePlayer >= playerAmount)
                        {
                            activePlayer = 0;
                        }
                        if (!players[activePlayer].checkGameOver())
                            players[activePlayer].setActive();
                    }
                   
                    if (gameover)
                    {
                        activePlayer = winner;
                      
                        
                        gameover = true;
                        //stop = true;
                        //stopped = true;
                        
                        File.Delete("GameState.xml");
                        return;
                        //break;
                    }


                    

                
            }
            else { return; }
                    
                    //ai.updateAvailable(availableTiles);

            if (stop)
            {
                stopped = true;
                Console.WriteLine("Game has stopped!");
            }
           
           
            
        }
       
        public Boolean hasStopped()
        {
            return stopped;
        }
        
        public void dragDrop(String availableTile, int slot)
        {
            
            int tile = Convert.ToInt32(availableTile.Substring(13)) - 1;
            


            //Vi kollar att slot innehåller activeplayer
            if(slots[slot].getPlayer(activePlayer) && !gameover){
            availableTiles[tile].place(slots[slot]);


            /*for (int i = 0; i < playerAmount; i++)
            {
                players[i].updateSlots(slots);
            }*/
            players[activePlayer].updateSlots(slots);
           
            drawTiles(tile);

            //activePlayer++;

            
            


           /* nextPlayer();
           
            Console.WriteLine("Activeplayer: " + activePlayer);

            for (int i = 0; i < playerAmount; i++)
            {
                players[i].move();
                if (players[i].checkGameOver())
                {
                    lost++;
                   // Console.WriteLine("hejsan");
                   // System.Windows.Forms.MessageBox.Show("Player " + activePlayer + " lost");
                    //gameover = true;

                }
            }
            * */
            }

        }
        public void AI()
        {
           /* ai.play(this);
           // Console.WriteLine("HEJ");
           nextPlayer();*/
          
            
        }

        public void nextPlayer()
        {
            activePlayer++;
            int count = 0;
           // Console.WriteLine("HEJ: " + activePlayer);

            if (activePlayer >= playerAmount)
            {

                activePlayer = 0;

            }
            while (players[activePlayer].checkGameOver())
            {
                
                
                activePlayer++;
                if (activePlayer >= playerAmount)
                {
                   
                    activePlayer = 0;

                    //break;

                }

                //Stoppar evighetsloop om alla har förlorat samtidigt.
                if (count >= playerAmount){
                
                    count = 0;
                    break;
                }
                count++;

            }
            if (players[activePlayer].isAI())
            {
                AI();
            }

        }

       
        /*public void panelClick(Panel panel){
            int tmp = Convert.ToInt32(panel.Name.Substring(5))-1;
            
            slots[tmp].click();
        }*/
        public int[][] tileExits(int x)
        {
            return availableTiles[x].getExits();
            
        }
        public int[][] tilePairs(int x)
        {
            return availableTiles[x].getPairs();
        }
        public int getPlayerExit(int x)
        {
            return players[x].getExit();
        }
        public int getActivePlayer()
        {
            return activePlayer;
        }
       
        
        //Drar nästa tile, eftersom tiles skapas random behövs inte dras random
        public void drawTiles(int x)
        {

            /*
            int select = 0;
            while (true)
            {
                select = random.Next(0,63);
                if(!tiles[select].isPlaced()){
                    availableTiles[x] = tiles[select];
                    tiles[select].setPlaced();
                  //  Console.WriteLine(select);
                    break;
                }
                
            }*/
            availableTiles[x] = tiles[nextTile];
            nextTile++;
            if (nextTile > 63)
            {
                nextTile = 0;
            }
            
            //Console.WriteLine("OMGÅNG: " + x);
            
            

        }
        public int[][] slotExits(int x)
        {
            return slots[x].getExits();
        }
        public Player[] slotPlayers(int x)
        {
            return slots[x].getPlayers();
        }
        public int[][] slotPairs(int x)
        {
            return slots[x].getPairs();
        }
        public Boolean drawSlot(int x)
        {
            return slots[x].isOccupied();
        }

        public XElement loadFromXml()
        {
            XElement gamestate = null;

            //Console.WriteLine("Loading...");
            if (File.Exists("GameState.xml"))
            {
                string str = File.ReadAllText("GameState.xml");
                gamestate = XElement.Parse(str);
                activePlayer = (int)gamestate.Element("ActivePlayer");
                gameover = (Boolean)gamestate.Element("GameOver");
                nextTile = (int)gamestate.Element("NextTile");

                for (int i = 0; i < 64; i++)
                {
                    if (stop)
                    {
                        return null;
                    }
                    tiles[i].loadFromXml(gamestate, i);
                    slots[i].loadFromXml(gamestate, i);
                }
               /* for (int i = 0; i < 3; i++)
                {
                    availableTiles[i].loadFromXml(gamestate, i);
                }*/
                for (int i = 0; i < playerAmount; i++)
                {
                    players[i].loadFromXml(gamestate);
                }

                

            }
            return gamestate;

         //   Console.WriteLine(gamestate);
        }

        //Sparar allt till xmlfil
        public void saveToXml()
        {

            Console.WriteLine("SAVING!!");
            
            XElement gamestate = new XElement("GameState",
            new XElement("ActivePlayer", activePlayer),
            new XElement("NextTile", nextTile),
            new XElement("GameOver", gameover)
            );
            for (int i = 0; i < 64; i++)
            {
                
                tiles[i].saveToXML(gamestate, i);
                slots[i].saveToXml(gamestate, i);
            }
            for (int i = 0; i < 3; i++)
            {
                availableTiles[i].saveToXML(gamestate, i);
            }
                for (int i = 0; i < 4; i++)
                {

                    players[i].saveToXml(gamestate);
                }
            

            //Console.WriteLine("Skriver...");
            
            gamestate.Save("GameState.xml");
         
        }

    }
}
