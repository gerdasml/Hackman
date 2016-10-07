using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

    }
}
