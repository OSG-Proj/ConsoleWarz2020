using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleWarz_2020
{

    public class Game
    {
        private Random random = new Random();
        public string Name { get; set; }
        public List<Player> Players;
        
        public PlayerAction[] PossibleActions = { new PlayerAction("punch", 5, false, 0, PlayerAction.Type.DamageAction), new PlayerAction("kick", 10, true, 1, PlayerAction.Type.DamageAction), new PlayerAction("block", 0, true, 1, PlayerAction.Type.ShieldAction), new PlayerAction("fireball", 20, true, 2, PlayerAction.Type.DamageAction), new PlayerAction("poison", 5 , true, 1, PlayerAction.Type.DamageAction), new PlayerAction("powerstab", 20, true, 4, PlayerAction.Type.DamageAction) };
        public Player.Class[] PossibleClasses = { Player.Class.Slayer, Player.Class.Warrior, Player.Class.Beast };
        
        public enum Mode
        {
            Singleplayer, Multiplayer
        }

        public Mode GameMode { get; set; }

        public bool gameOver;
        

        

        
        
        public Game(string name, int numberOfPlayers)
        {
            Name = name;
            Players = new List<Player>(numberOfPlayers);
            
            
        }
        public void Start()
        {
            Console.Write("Player Name: ");
            string playerName = Console.ReadLine();
            Console.Write("Player Class: ");
            string playerClass = Console.ReadLine();

            Console.Clear();

            Players.Add(new Player(playerName, 100, true, 2, Player.Type.Player));
            Players.Add(new Player("AI", 100, true, 2, Player.Type.AI));

            //Setting up game class for each player
            foreach(Player player in Players)
            {
                if(player.PlayerType == Player.Type.Player)
                {
                    switch(playerClass)
                    {
                        case "slayer":
                            player.GameClass = Player.Class.Slayer;
                            break;
                        case "warrior":
                            player.GameClass = Player.Class.Warrior;
                            break;
                        case "beast":
                            player.GameClass = Player.Class.Beast;
                            break;
                    }
                
                }
                else if(player.PlayerType == Player.Type.AI)
                {
                    player.GameClass = PossibleClasses[random.Next(3)];
                }
            }
            //Selecting actions
            foreach(Player player in Players)
            {
                switch(player.GameClass)
                {
                    case Player.Class.Slayer:
                        player.Actions.Add(PossibleActions[1]);
                        player.Actions.Add(PossibleActions[5]);
                        break;
                    case Player.Class.Warrior:
                        player.Actions.Add(PossibleActions[0]);
                        player.Actions.Add(PossibleActions[2]);
                        break;
                    case Player.Class.Beast:
                        player.Actions.Add(PossibleActions[3]);
                        player.Actions.Add(PossibleActions[4]);
                        break;
                }
            }
            
            //Actual game starts here
            while(!gameOver)
            {
                
               
                foreach(Player player in Players)
                {
                    
                    if(player.Alive)
                    {
                        foreach(Player _player in Players)
                        {
                            Console.WriteLine("[" + _player.Name + "] " + "| " + "Health: " + _player.Health + " | Shielded: " + _player.Shielded);
                        }
                        if(player.PlayerType == Player.Type.Player)
                        {
                            
                            Console.Write(player.Name + "(" + player.Actions[0].Name + " | Cooldown: " + player.Actions[0].CoolDownTime + ", " + player.Actions[1].Name + " | Cooldown: " + player.Actions[1].CoolDownTime + "): ");

                            //Validate actions
                            string actionChosen = Console.ReadLine();
                            
                            int damage = 0;
                            
                            foreach(PlayerAction action in player.Actions)
                            {
                                if(actionChosen == action.Name)
                                {
                                    player.actionSelected = action;
                                    break;
                                }
                                
                                
                            }
                            
                            if(player.actionSelected != null)
                            {
                                if(player.actionSelected.CoolDownTime == 0)
                                {
                                    player.actionSelected.Enabled = true;
                                }
                                if(player.actionSelected.Enabled)
                                {
                                    switch(player.actionSelected.ActionType)
                                    {
                                        case PlayerAction.Type.DamageAction:
                                            player.actionSelected.Selected = true;
                                            if(!Players[1].Shielded)
                                            {
                                               Players[1].Health -= player.actionSelected.Damage; 
                                               damage = player.actionSelected.Damage;
                                               
                                            }
                                            else
                                            {
                                                Console.WriteLine(Players[1].Name + " blocked your attack!");
                                                Players[1].Shielded = false;
                                               
                                            }
                                            
                                            player.actionSelected.Enabled = false;
                                            if(player.actionSelected.RequireCooldown)
                                            {
                                                player.actionSelected.CoolDownTime = player.actionSelected.CooldownThreshold; 
                                            }
                                            break;
                                        case PlayerAction.Type.ShieldAction:
                                            player.actionSelected.Selected = true;
                                            player.Shielded = true;
                                            player.actionSelected.Enabled = false;
                                            Console.WriteLine(player.Name + " Applied shield for 1 turn");
                                            if(player.actionSelected.RequireCooldown)
                                            {
                                                player.actionSelected.CoolDownTime = player.actionSelected.CooldownThreshold; 
                                            }
                                            break;

                                            
                                    }
                                }
                                else
                                {
                            
                                    if(player.actionSelected.CoolDownTime !> 1)
                                    {
                                        Console.WriteLine("You muust wait " + player.actionSelected.CoolDownTime  + " turn to use that action");
                                    }
                                    else
                                    {
                                        Console.WriteLine("You muust wait " + player.actionSelected.CoolDownTime  + " turns to use that action");
                                    }  
                                }
                            }
                            else
                            {
                                Console.WriteLine("Thats not a valid action! Skipping turn.");
                            }
                            Console.WriteLine("You damaged " + Players[1].Name + " for " + damage + ". " + Players[1].Name + "'s health is now " + Players[1].Health + ".");
                            foreach(PlayerAction action in player.Actions)
                            {
                                if(action.CoolDownTime != 0 && !action.Selected)
                                {
                                    
                                    action.CoolDownTime--;
                                }
                                else
                                {
                                    
                                    action.Selected = false;
                                }
                            }
                            if(Players[1].Health <= 0)
                            {
                                Players[1].Alive = false;
                            }
                        }
                        
                        else if(player.PlayerType == Player.Type.AI)
                        {
                            
                            //AI Rules
                            foreach(PlayerAction action in player.Actions)
                            {
                                if(action.Enabled)
                                {
                                    if(Players[0].Shielded)
                                    {
                                        if(!action.RequireCooldown)
                                        {
                                            player.actionSelected = action;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if(action.RequireCooldown && action.Damage > Players[0].actionSelected.Damage)
                                        {
                                            player.actionSelected = action;
                                            break;
                                            
                                        }
                                    }
                                }
                                
                            }
                            

                            
                            int damage = 0;
                            Console.WriteLine(player.Name + " has chosen action: " + player.actionSelected.Name);
                            
                            if(player.actionSelected.CoolDownTime == 0)
                            {
                                player.actionSelected.Enabled = true;
                            }
                            if(player.actionSelected.Enabled)
                            {
                                switch(player.actionSelected.ActionType)
                                {
                                    case PlayerAction.Type.DamageAction:
                                        player.actionSelected.Selected = true;
                                        if(!Players[0].Shielded)
                                        {
                                            Players[0].Health -= player.actionSelected.Damage;
                                            damage = player.actionSelected.Damage;
                                        }
                                        else
                                        {
                                            Players[0].Shielded = false;
                                        }
                                        player.actionSelected.Enabled = false;

                                        if(player.actionSelected.RequireCooldown)
                                        {
                                            player.actionSelected.CoolDownTime = player.actionSelected.CooldownThreshold;
                                        }
                                        break;
                                    case PlayerAction.Type.ShieldAction:
                                        player.actionSelected.Selected = true;
                                        player.Shielded = true;
                                        player.actionSelected.Enabled = false;

                                        if(player.actionSelected.RequireCooldown)
                                        {
                                            player.actionSelected.CoolDownTime = player.actionSelected.CooldownThreshold;
                                        }
                                        break;
                                }
                            }
                            else
                            {
                                 

                                
                            }
                            Console.WriteLine(player.Name + " damaged " + Players[0].Name + " for " + damage + ". " + Players[0].Name + "'s health is now " + Players[0].Health + ".");
                            foreach(PlayerAction action in player.Actions)
                            {
                                if(action.CoolDownTime != 0 && !action.Selected)
                                {
                                    action.CoolDownTime--;
                                }
                                else
                                {
                                    action.Selected = false;
                                }
                            }
                            if(Players[0].Health <= 0)
                            {
                                Players[0].Alive = false;
                            }
                        }
                    }
                    else
                    {
                        gameOver = true;
                    }
                    Thread.Sleep(2000);
                    Console.Clear();
                }
                

            }
            End();
            
        }
        
        public void End()
        {
            
            Console.Clear();
            if(Players[0].Alive)
            {
                Console.WriteLine("You win!");
            }
            else if(Players[1].Alive)
            {
                Console.WriteLine("You lose!");
            }
        }


    }
}