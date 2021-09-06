using descendancy.Entities;
using GoRogue;
using Microsoft.Xna.Framework;

namespace descendancy
{
    // Custom class for the player is used in this example just so we can handle input.  This could be done via a component, or in a main screen, but for simplicity we do it here.
    internal class Player : Actor
    {
        public Player(Coord position) : base(Color.LightGreen, Color.Black, 28, position, (int)MapLayer.PLAYER, isWalkable: false, isTransparent: true) => FOVRadius = 10;

    }
}
