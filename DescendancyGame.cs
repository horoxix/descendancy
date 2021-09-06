using descendancy.Commands;
using descendancy.UI;

namespace descendancy
{
    internal class DescendancyGame
    {
        public static UIManager UIManager;
        public static CommandManager CommandManager;

        private static void Main()
        {
            // Setup the engine and create the main window.
            SadConsole.Game.Create("Fonts/spritesheet.font", UIManager.ViewPortWidth, UIManager.ViewPortHeight);

            // Hook the start event so we can add consoles to the system.
            SadConsole.Game.OnInitialize = Init;

            // Start the game.
            SadConsole.Game.Instance.Run();
            SadConsole.Game.Instance.Dispose();
        }

        private static void Init()
        {
            CommandManager = new CommandManager();
            UIManager = new UIManager();
            SadConsole.Global.LoadFont("Fonts/Cheepicus12.font");
            UIManager.CreateConsoles();
        }
    }
}
