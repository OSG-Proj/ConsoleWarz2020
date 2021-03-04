using System;
using System.Collections.Generic;
using System.Threading;

//Burnout warning: This took me longer than you would think. It was hard to get the logic working and yet there is still much more to go. Please take a break if you feel overwhelmed. It will improve your coding expierience in general.
namespace ConsoleWarz_2020
{
    //feel free to fix and change any code. This is how I wrote my code, but maybe you can change it up a bit.

    public class Game
    {
        //random number generator for random action picking for the AI player
        private Random random = new Random();
        public string Name { get; set; }
        public List<Player> Players;
        
        //player action lists
        public PlayerAction[] PossibleActions = { new PlayerAction("punch", 5, false, 0, PlayerAction.Type.DamageAction), new PlayerAction("kick", 10, true, 1, PlayerAction.Type.DamageAction), new PlayerAction("block", 0, true, 1, PlayerAction.Type.ShieldAction), new PlayerAction("fireball", 20, true, 2, PlayerAction.Type.DamageAction), new PlayerAction("poison", 5 , true, 1, PlayerAction.Type.DamageAction), new PlayerAction("powerstab", 20, true, 4, PlayerAction.Type.DamageAction) };
        //this is an enum for the  player classes
        public Player.Class[] PossibleClasses = { Player.Class.Slayer, Player.Class.Warrior, Player.Class.Beast };
        
        public enum Mode
        {
            Singleplayer, Multiplayer
        }
        //right now we only have single player AI
        public Mode GameMode { get; set; }
        
        //this is false by default
        public bool gameOver;
        

        

        
        //constructor that is invoked in program.cs
        public Game(string name, int numberOfPlayers)
        {
            Name = name;
            Players = new List<Player>(numberOfPlayers);
            
            
        }
        //all functionality for now is in this method
        public void Start()
        {
            //player and player class selections
            Console.Write("Player Name: ");
            string playerName = Console.ReadLine();
            Console.Write("Player Class: ");
            string playerClass = Console.ReadLine();

            Console.Clear();
            //add 2 players, 1 is you, and the  other is AI to the list
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
                //setting a class of the random int in PossibleClasses array.
                else if(player.PlayerType == Player.Type.AI)
                {
                    player.GameClass = PossibleClasses[random.Next(3)];
                }
            }
            //Selecting actions
            //adding them based on classes selected.
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
            //this loops till gameOver = true
            while(!gameOver)
            {
                
               
                foreach(Player player in Players)
                {
                    
                    //check if player is alive.
                    if(player.Alive)
                    {
                        //display player stats each time this loop is run.
                        foreach(Player _player in Players)
                        {
                            Console.WriteLine("[" + _player.Name + "] " + "| " + "Health: " + _player.Health + " | Shielded: " + _player.Shielded);
                        }
                        //this is the player that YOU play
                        if(player.PlayerType == Player.Type.Player)
                        {
                            
                            Console.Write(player.Name + "(" + player.Actions[0].Name + " | Cooldown: " + player.Actions[0].CoolDownTime + ", " + player.Actions[1].Name + " | Cooldown: " + player.Actions[1].CoolDownTime + "): ");

                            //Validate actions
                            string actionChosen = Console.ReadLine();
                            
                            //the amount of damage is stored here. Feel free to remove this and create a DamageDone property in the Player class in Player.cs
                            int damage = 0;
                            
                            //make sure the action selected is valid in the player's actions class.
                            foreach(PlayerAction action in player.Actions)
                            {
                                if(actionChosen == action.Name)
                                {
                                    player.actionSelected = action;
                                    break;
                                }
                                
                                
                            }
                            
                            
                            //check if action was selected. If not, skip turn. Also feel free to add a relooping mechanism to the code.
                            if(player.actionSelected != null)
                            {
                                //action has a default cooldown time of 0 as displayed in the CoolDownTime property in the players action.
                                if(player.actionSelected.CoolDownTime == 0)
                                {
                                    //enable the action.
                                    player.actionSelected.Enabled = true;
                                }
                                //if the actions Enabled bool property is set to true, proceed, otherwise
                                if(player.actionSelected.Enabled)
                                {
                                    //check the type of action
                                    switch(player.actionSelected.ActionType)
                                    {
                                        case PlayerAction.Type.DamageAction:
                                            player.actionSelected.Selected = true;
                                            //check if the AI is not shielded, in this case it is Players, index of 1
                                            if(!Players[1].Shielded)
                                            {
                                               Players[1].Health -= player.actionSelected.Damage; 
                                                
                                               //damage as declared at the beginning of the turn will be set so that the program will display how much you damaged the AI
                                               damage = player.actionSelected.Damage;
                                               
                                            }
                                            else
                                            {
                                                //break the shield so that next time you can damage the AI
                                                Console.WriteLine(Players[1].Name + " blocked your attack!");
                                                Players[1].Shielded = false;
                                               
                                            }
                                            //once action is used disable it.
                                            player.actionSelected.Enabled = false;
                                            
                                            //if the action requires cooldown, set it to the CooldownThreshold property in the action.
                                            if(player.actionSelected.RequireCooldown)
                                            {
                                                player.actionSelected.CoolDownTime = player.actionSelected.CooldownThreshold; 
                                            }
                                            break;
                                            
                                        case PlayerAction.Type.ShieldAction:
                                            //this is a shield action which means that the player's Shielded bool property will be set to true and the player will deal no damage for the next turn.
                                            player.actionSelected.Selected = true;
                                            player.Shielded = true;
                                            
                                            //once action is used disable it.
                                            player.actionSelected.Enabled = false;
                                            Console.WriteLine(player.Name + " Applied shield for 1 turn");
                                            //if the action requires cooldown, set it to the CooldownThreshold property in the action.
                                            if(player.actionSelected.RequireCooldown)
                                            {
                                                player.actionSelected.CoolDownTime = player.actionSelected.CooldownThreshold; 
                                            }
                                            break;

                                            
                                    }
                                }
                                else
                                {
                                    //display the cooldown time that will be decremented every turn taken
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
                            
                            //decremeent any action that has a cooldown time set and isn't currently selected otherwise the action that was just used will be decremented before the next turn is played
                            foreach(PlayerAction action in player.Actions)
                            {
                                if(action.CoolDownTime != 0 && !action.Selected)
                                {
                                    
                                    action.CoolDownTime--;
                                }
                                else
                                {
                                    //set the action that was just used to false so that next time it will be decremented except any other of the enabled actions selected.
                                    action.Selected = false;
                                }
                            }
                            if(Players[1].Health <= 0)
                            {
                                Players[1].Alive = false;
                            }
                        }
                        //this is the AI this isnt going to work right now, try to figure how to fix. I will create an AI rulesheet you can refer to.
                        //Originally, I used player.actionSelected = random.Next(player.Actions.Length) it it still worked, but I wanted to make the  AI more smart by using if statements to determine what action the AI should use against you based on the AI rulesheet
                        //EXAMPLE
                        
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
