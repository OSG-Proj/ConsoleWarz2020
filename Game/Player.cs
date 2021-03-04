using System;
using System.Collections.Generic;
namespace ConsoleWarz_2020
{
    public class Player
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public bool Alive { get; set; }
        public bool Shielded { get; set; } = false;

        public int TurnsSkipped { get; set; }

        public PlayerAction actionSelected { get; set; }

        public enum Type
        {
            Player, AI
        }
        public Type PlayerType { get; set; }

        public List<PlayerAction> Actions { get; set; }

        public enum Class
        {
            Slayer, Warrior, Beast
        }

        public Class GameClass { get; set; }
        public Player(string name, int health, bool alive, int numberOfActions, Type type)
        {
            Name = name;
            Health = health;
            Alive = alive;
            Actions = new List<PlayerAction>(numberOfActions);
            PlayerType = type;
            

        }
    }
}