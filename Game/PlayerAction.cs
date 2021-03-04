using System;

namespace ConsoleWarz_2020
{
    public class PlayerAction
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public bool RequireCooldown { get; set; }
        public int CooldownThreshold { get; set; }

        public int CoolDownTime { get; set; } = 0;


        public bool Enabled { get; set; } = true;

        public bool Selected { get; set; } = false;

        public enum Type
        {
            DamageAction, ShieldAction, EmoteAction
        }

        public Type ActionType { get; set; }
        public PlayerAction(string name, int damage, bool requireCooldown, int cooldownThreshold, Type type)
        {
            Name = name;
            Damage = damage;
            RequireCooldown = requireCooldown;

            if(RequireCooldown)
            {
                CooldownThreshold = cooldownThreshold;
            }
            ActionType = type;
            
        }
    }
}