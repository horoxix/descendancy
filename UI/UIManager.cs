using Microsoft.Xna.Framework;
using SadConsole;

namespace descendancy.UI
{
    public class UIManager : ContainerConsole
    {
        public ScrollingConsole MapConsole;
        internal MapScreen MapScreen { get; private set; }
        public MessageLogWindow MessageLog;
        public InventoryWindow InventoryWindow;
        public CharacterStatisticsWindow CharacterStatisticsWindow;
        
        // Set the map and viewport dimensions.
        public const int ViewPortWidth = 64;
        public const int ViewPortHeight = 32;

        private const int MapWidth = 500;
        private const int MapHeight = 500;

        public UIManager()
        {
            // must be set to true
            // or will not call each child's Draw method
            IsVisible = true;
            IsFocused = true;

            // The UIManager becomes the only
            // screen that SadConsole processes
            Parent = Global.CurrentScreen;
        }

        // Creates all child consoles to be managed
        // make sure they are added as children
        // so they are updated and drawn
        public void CreateConsoles()
        {
            // Generate and display the map
            MapScreen = new MapScreen(MapWidth, MapHeight, ViewPortWidth, ViewPortHeight / 2);
            Children.Add(MapScreen);

            CharacterStatisticsWindow = new CharacterStatisticsWindow(ViewPortWidth / 2, ViewPortHeight / 2, "Statistics")
            {
                Position = new Point(32, 22)
            };
            
                // Create the Inventory Window
            InventoryWindow = new InventoryWindow(ViewPortWidth - 2, ViewPortHeight / 2 - 2, "Inventory")
            {
                Position = new Point(2, 2)
            };

            // Create the message log window and set its position.
            MessageLog = new MessageLogWindow(ViewPortWidth / 2, ViewPortHeight / 2, "Message Log")
            {
                Position = new Point(0, 22)
            };

            MapScreen.MapRenderer.Children.Add(MessageLog);
            MapScreen.MapRenderer.Children.Add(InventoryWindow);
            MapScreen.MapRenderer.Children.Add(CharacterStatisticsWindow);
            MessageLog.Show();
            CharacterStatisticsWindow.Show();
            
            // Print a test message
            MessageLog.Add("Welcome");
        }

    }
}
