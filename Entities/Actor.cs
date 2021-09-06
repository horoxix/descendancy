using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GoRogue;
using SadConsole;
using System;

namespace descendancy.Entities
{
    public abstract class Actor : BasicEntity
    {
        public int Health { get; set; } // current health
        public int MaxHealth { get; set; } // maximum health
        public int Attack { get; set; } // attack strength
        public int AttackChance { get; set; } // percent chance of successful hit
        public int Defense { get; set; } // defensive strength
        public int DefenseChance { get; set; } // percent chance of successfully blocking a hit
        public int Gold { get; set; } // amount of gold carried
        public List<Item> Inventory = new List<Item>(); // the player's collection of items

        private static readonly Dictionary<Keys, Direction> s_movementDirectionMapping = new Dictionary<Keys, Direction>
        {
            { Keys.NumPad7, Direction.UP_LEFT }, { Keys.NumPad8, Direction.UP }, { Keys.NumPad9, Direction.UP_RIGHT },
            { Keys.NumPad4, Direction.LEFT }, { Keys.NumPad6, Direction.RIGHT },
            { Keys.NumPad1, Direction.DOWN_LEFT }, { Keys.NumPad2, Direction.DOWN }, { Keys.NumPad3, Direction.DOWN_RIGHT },
            { Keys.Up, Direction.UP }, { Keys.Down, Direction.DOWN }, { Keys.Left, Direction.LEFT }, { Keys.Right, Direction.RIGHT }
        };

        public int FOVRadius;

        protected Actor(
            Color foreground, 
            Color background, 
            int glyph, 
            Coord position,
            int layer,
            bool isWalkable,
            bool isTransparent) : base(foreground, background, glyph, position, layer, isWalkable, isTransparent)
        {
            Animation.CurrentFrame[0].Foreground = foreground;
            Animation.CurrentFrame[0].Background = background;
            Animation.CurrentFrame[0].Glyph = glyph;
        }

        // Moves the Actor BY positionChange tiles in any X/Y direction
        // returns true if actor was able to move, false if failed to move
        // Checks for Monsters, and Items before moving
        // and allows the Actor to commit an action if one is present.
        //public bool MoveBy(Point positionChange)
        //{
        //    // Check the current map if we can move to this new position
        //    if (GameLoop.World.CurrentMap.IsTileWalkable(Position + positionChange))
        //    {
        //        // if there's a monster here,
        //        // do a bump attack
        //        Monster monster = GameLoop.World.CurrentMap.GetEntityAt<Monster>(Position + positionChange);
        //        Item item = GameLoop.World.CurrentMap.GetEntityAt<Item>(Position + positionChange);
        //        if (monster != null)
        //        {
        //            GameLoop.CommandManager.Attack(this, monster);
        //            return true;
        //        }
        //        // if there's an item here,
        //        // try to pick it up
        //        else if (item != null)
        //        {
        //            GameLoop.CommandManager.Pickup(this, item);
        //            return true;
        //        }

        //        Position += positionChange;
        //        return true;
        //    }
        //    // Handle situations where there are non-walkable tiles that CAN be used
        //    else
        //    {
        //        // Check for the presence of a door
        //        TileDoor door = GameLoop.World.CurrentMap.GetTileAt<TileDoor>(Position + positionChange);
        //        // if there's a door here,
        //        // try to use it
        //        if (door != null)
        //        {
        //            GameLoop.CommandManager.UseDoor(this, door);
        //            return true;
        //        }
        //        return false;
        //    }
        //}

        // Moves the Actor TO newPosition location
        // returns true if actor was able to move, false if failed to move
        public bool MoveTo(Point newPosition)
        {
            Position = newPosition;
            return true;
        }

        public override bool ProcessKeyboard(SadConsole.Input.Keyboard info)
        {
            try
            {
                Direction moveDirection = Direction.NONE;

                // Simplified way to check if any key we care about is pressed and set movement direction.
                foreach (Keys key in s_movementDirectionMapping.Keys)
                {
                    if (info.IsKeyPressed(key))
                    {
                        moveDirection = s_movementDirectionMapping[key];
                        break;
                    }
                }
                if (info.IsKeyPressed(Keys.M))
                {
                    if (DescendancyGame.UIManager.MapScreen.Map.FovVisibilityHandler.Enabled)
                    {
                        DescendancyGame.UIManager.MapScreen.Map.FovVisibilityHandler.Disable();
                    }
                    else
                    {
                        DescendancyGame.UIManager.MapScreen.Map.FovVisibilityHandler.Enable();
                    }
                }
                if (info.IsKeyPressed(Keys.I))
                {
                    if (DescendancyGame.UIManager.InventoryWindow.IsVisible)
                    {
                        DescendancyGame.UIManager.InventoryWindow.Hide();
                    }
                    else
                    {
                        DescendancyGame.UIManager.InventoryWindow.Show();
                    }
                }

                Monster monster = DescendancyGame.UIManager.MapScreen.Map.GetEntity<Monster>(Position + moveDirection, LayerMasker.DEFAULT.Mask((int)MapLayer.MONSTERS));
                Item item = DescendancyGame.UIManager.MapScreen.Map.GetEntity<Item>(Position + moveDirection, LayerMasker.DEFAULT.Mask((int)MapLayer.ITEMS));
                if (monster != null)
                {
                    DescendancyGame.CommandManager.Attack(this, monster);
                }
                if(item != null)
                {
                    DescendancyGame.CommandManager.Pickup(this, item);
                }

                Position += moveDirection;

                if (moveDirection != Direction.NONE)
                    return true;
                else
                    return base.ProcessKeyboard(info);
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                throw ex;
            }
        }
    }
}
