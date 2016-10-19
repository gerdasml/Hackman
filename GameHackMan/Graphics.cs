using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameHackMan
{
    static class Graphics
    {
        public static Texture2D Wall { get; private set; }
        public static Texture2D Food { get; private set; }
        public static Texture2D HackManFront { get; private set; }
        public static Texture2D HackManRight { get; private set; }
        public static Texture2D HackManLeft { get; private set; }
        public static Texture2D HackManUp { get; private set; }
        public static Texture2D HackManDown { get; private set; }
        public static SpriteFont Font { get; private set; }
        public static Texture2D GhostFront { get; private set; }
        public static Texture2D GhostRight { get; private set; }
        public static Texture2D GhostLeft { get; private set; }
        public static Texture2D GhostUp { get; private set; }
        public static Texture2D GhostDown { get; private set; }

        public static void Load(ContentManager content)
        {
            Wall = content.Load<Texture2D>("Wall");
            Food = content.Load<Texture2D>("Food");
            HackManFront = content.Load<Texture2D>("Hackman");
            HackManRight = content.Load<Texture2D>("Hackman_right");
            HackManLeft = content.Load<Texture2D>("Hackman_left");
            HackManUp = content.Load<Texture2D>("Hackman_up");
            HackManDown = content.Load<Texture2D>("Hackman_down");
            Font = content.Load<SpriteFont>("Font");
            GhostFront = content.Load<Texture2D>("Ghost_front");
            GhostRight = content.Load<Texture2D>("Ghost_right");
            GhostLeft = content.Load<Texture2D>("Ghost_left");
            GhostUp = content.Load<Texture2D>("Ghost_up");
            GhostDown = content.Load<Texture2D>("Ghost_down");

        }

    }
}
