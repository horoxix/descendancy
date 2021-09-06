using GoRogue;
using Microsoft.Xna.Framework;
using System;

namespace descendancy.Entities
{
    //A generic monster capable of
    //combat and interaction
    //yields treasure upon death
    public class Monster : Actor
    {
        private Random rndNum = new Random();

        public Monster(
            Color foreground, 
            Color background, 
            int glyph,
            Coord position,
            int layer,
            bool isWalkable,
            bool isTransparent) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {

            //number of loot to spawn for monster
            int lootNum = rndNum.Next(1, 4);

            for (int i = 0; i < lootNum; i++)
            {
                // monsters are made out of spork, obvs.
                Item newLoot = new Item(Color.HotPink, Color.Transparent, "Ore", 'L', Position, true, true);
                Inventory.Add(newLoot);
            }
        }
    }
}
