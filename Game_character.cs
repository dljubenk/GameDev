using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HealthBar
{
    class Player
    {
        public Texture2D texture;
        public Rectangle rectangle;
        public Vector2 position;
        public Vector2 velocity;

        public bool hasJumped;

        public int health;

        public Player(Texture2D newTexture, Vector2 newPosition, int newHealth)
        {
            texture = newTexture;
            position = newPosition;

            health = newHealth;

        }

        public void Update()
        {



            rectangle = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (health > 0)
                spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
