using Microsoft.Xna.Framework;
using SadConsole;
using SadConsole.Controls;
using System;

namespace descendancy.UI
{
    public class CharacterStatisticsWindow : Window
    {
        private SadConsole.Console _statsConsole;
        private SadConsole.Themes.Colors CustomColors;

        public CharacterStatisticsWindow(int width, int height, string title) : base(width, height)
        {
            CanDrag = true;
            Title = title.Align(HorizontalAlignment.Center, Width);
            Theme.FillStyle = new Cell();
            var normalSizedFont = Global.Fonts["Cheepicus12"].GetFont(Font.FontSizes.One);
            Font = normalSizedFont;
            _statsConsole = new SadConsole.Console(width - 1, height - 1)
            {
                Position = new Point(1, 1),
                Font = normalSizedFont
            };

            Button closeButton = new Button(3, 1)
            {
                Position = new Point(0, 0),
                Text = "[X]"
            };
            closeButton.Click += CloseButton_Click;
            Add(closeButton);

            CloseOnEscKey = true;

            UseMouse = true;
            InitColors();

            Children.Add(_statsConsole);
            UpdatePlayerStatistics();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        //Remember to draw the window!
        public override void Draw(TimeSpan drawTime)
        {
            base.Draw(drawTime);
        }

        public override void Show(bool modal)
        {
            UpdatePlayerStatistics();
            base.Show(modal);
        }

        private void InitColors()
        {
            // Create a set of default colours that we will modify
            CustomColors = SadConsole.Themes.Colors.CreateDefault();

            // Pick a couple of background colours that we will apply to all consoles.
            Color backgroundColor = Color.Black;

            // Set background colour for controls consoles and their controls
            CustomColors.ControlHostBack = backgroundColor;
            CustomColors.ControlBack = backgroundColor;

            // Generate background colours for dark and light themes based on
            // the default background colour.
            CustomColors.ControlBackLight = (backgroundColor * 1.3f).FillAlpha();
            CustomColors.ControlBackDark = (backgroundColor * 0.7f).FillAlpha();

            // Set a color for currently selected controls. This should always
            // be different from the background colour.
            CustomColors.ControlBackSelected = CustomColors.GrayDark;

            // Rebuild all objects' themes with the custom colours we picked above.
            CustomColors.RebuildAppearances();

            // Now set all of these colours as default for SC's default theme.
            this.ThemeColors = CustomColors;
        }

        private void UpdatePlayerStatistics()
        {
            _statsConsole.Print(1, 1, "Name : ");
            _statsConsole.Print(15, 1, DescendancyGame.UIManager.MapScreen.Map.ControlledGameObject.Name);
            _statsConsole.Print(1, 3, "Health : ");
            _statsConsole.Print(15, 3, DescendancyGame.UIManager.MapScreen.Map.ControlledGameObject.Health.ToString() + " / " + DescendancyGame.UIManager.MapScreen.Map.ControlledGameObject.MaxHealth.ToString());
            _statsConsole.Print(1, 5, "Attack : ");
            _statsConsole.Print(15, 5, DescendancyGame.UIManager.MapScreen.Map.ControlledGameObject.Attack.ToString());
            _statsConsole.Print(1, 7, "Defense : ");
            _statsConsole.Print(15, 7, DescendancyGame.UIManager.MapScreen.Map.ControlledGameObject.Defense.ToString());
        }
    }
}
