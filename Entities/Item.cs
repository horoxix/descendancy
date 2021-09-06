using GoRogue;
using Microsoft.Xna.Framework;
using SadConsole;

namespace descendancy.Entities
{
    // Item: Describes things that can be picked up or used
    // by actors, or destroyed on the map.
    public class Item : BasicEntity
    {
        public int Weight { get; set; } // mass of the item
        // physical condition of item, in percent
        // 100 = item undamaged
        // 0 = item is destroyed

        // By default, a new Item is sized 1x1, with a weight of 1, and at 100% condition
        public Item(Color foreground,
                    Color background,
                    string name,
                    char glyph,
                    Coord position,
                    bool isWalkable,
                    bool isTransparent,
                    int weight = 1,
                    int layer = 1) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            // assign the object's fields to the parameters set in the constructor
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
            Weight = weight;
            Name = name;
        }
    }
}
