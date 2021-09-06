using descendancy.Entities;
using GoRogue;
using GoRogue.GameFramework;
using GoRogue.MapGeneration;
using GoRogue.MapViews;
using Microsoft.Xna.Framework;
using SadConsole;
using XnaRect = Microsoft.Xna.Framework.Rectangle;

namespace descendancy.UI
{
    internal class MapScreen : UIManager
    {
        public DescendancyMap Map { get; }
        public ScrollingConsole MapRenderer { get; }
        public MultiSpatialMap<BasicEntity> Entities; // Keeps track of all the Entities on the map
        public static IDGenerator IDGenerator = new IDGenerator(); // A static IDGenerator that all Entities can access

        //Generate a map and display it.  Could just as easily pass it into
        public MapScreen(int mapWidth, int mapHeight, int viewportWidth, int viewportHeight)
        {
            Map = GenerateDungeon(mapWidth, mapHeight);
            
            // Get a console that's set up to render the map, and add it as a child of this container so it renders
            MapRenderer = Map.CreateRenderer(new XnaRect(0, 0, viewportWidth, viewportHeight), Global.FontDefault);
            MapRenderer.MouseEnter += MapRenderer_MouseEnter;
            Children.Add(MapRenderer);
            Map.ControlledGameObject.IsFocused = true; // Set player to receive input, since in this example the player handles movement

            // Set up to recalculate FOV and set camera position appropriately when the player moves.  Also make sure we hook the new
            // Player if that object is reassigned.
            Map.ControlledGameObject.Moved += Player_Moved;
            Map.ControlledGameObjectChanged += ControlledGameObjectChanged;

            // Calculate initial FOV and center camera
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private void MapRenderer_MouseEnter(object sender, SadConsole.Input.MouseEventArgs e)
        {
            Map.ControlledGameObject.IsFocused = true;
        }

        private void ControlledGameObjectChanged(object s, ControlledGameObjectChangedArgs e)
        {
            if (e.OldObject != null)
                e.OldObject.Moved -= Player_Moved;

            ((BasicMap)s).ControlledGameObject.Moved += Player_Moved;
        }
        private static DescendancyMap GenerateDungeon(int width, int height)
        {
            // Same size as screen, but we set up to center the camera on the player so expanding beyond this should work fine with no other changes.
            var map = new DescendancyMap(width, height);

            // Generate map via GoRogue, and update the real map with appropriate terrain.
            var tempMap = new ArrayMap<bool>(map.Width, map.Height);
            QuickGenerators.GenerateDungeonMazeMap(tempMap, minRooms:150, maxRooms: 300, roomMinSize: 12, roomMaxSize: 48);
            map.ApplyTerrainOverlay(tempMap, SpawnTerrain);

            Coord posToSpawn;
            // Spawn a few mock enemies
            for (int i = 0; i < 200; i++)
            {
                posToSpawn = map.WalkabilityView.RandomPosition(true); // Get a location that is walkable
                var goblin = new Monster(Color.Red, Color.Transparent, 30, posToSpawn, (int)MapLayer.MONSTERS, isWalkable: false, isTransparent: true);
                map.AddEntity(goblin);
            }

            // Spawn player
            posToSpawn = map.WalkabilityView.RandomPosition(true);
            map.ControlledGameObject = new Player(posToSpawn)
            {
                Attack = 10,
                AttackChance = 40,
                Defense = 5,
                DefenseChance = 20,
                Name = "The King",
                Health = 100,
                MaxHealth = 100
            };
            map.AddEntity(map.ControlledGameObject);

            return map;
        }

        private void Player_Moved(object sender, ItemMovedEventArgs<IGameObject> e)
        {
            Map.CalculateFOV(Map.ControlledGameObject.Position, Map.ControlledGameObject.FOVRadius, Radius.SQUARE);
            MapRenderer.CenterViewPortOnPoint(Map.ControlledGameObject.Position);
        }

        private static IGameObject SpawnTerrain(Coord position, bool mapGenValue)
        {
            // Floor or wall.  This could use the Factory system, or instantiate Floor and Wall classes, or something else if you prefer;
            // this simplistic if-else is just used for example
            if (mapGenValue) // Floor
                return new BasicTerrain(Color.Silver, new Color(26,0,26), 6, position, isWalkable: true, isTransparent: true);
            else             // Wall
                return new BasicTerrain(Color.Gray, Color.DarkGray, 16, position, isWalkable: false, isTransparent: false);
        }


    }
}
