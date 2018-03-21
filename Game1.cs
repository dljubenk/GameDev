using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using HealthBar;
using ScrollingBackground;

namespace AroseTheExot
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;


        //Health Bar
        Texture2D healthTexture;
        Rectangle healthRectangle;

        //Ooball
        Rectangle Ooball_rectangle;
        Texture2D Ooball_texture;
        Vector2 velocity;

        // Screen Parameters
        int screenWidth;
        int screenHeight;

        Scrolling scrolling1;
        Scrolling scrolling2;
        Scrolling scrolling3;
        Scrolling scrolling4;
        Scrolling scrolling5;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);

            scrolling1 = new Scrolling(Content.Load<Texture2D>("Backgrounds/Background1"), new Rectangle(0, 0, 800, 500));
            scrolling2 = new Scrolling(Content.Load<Texture2D>("Backgrounds/Background2"), new Rectangle(800, 0, 800, 500));
            scrolling3 = new Scrolling(Content.Load<Texture2D>("Backgrounds/Background3"), new Rectangle(1600, 0, 800, 500));
            scrolling4 = new Scrolling(Content.Load<Texture2D>("Backgrounds/Background4"), new Rectangle(2400, 0, 800, 500));
            scrolling5 = new Scrolling(Content.Load<Texture2D>("Backgrounds/Background5"), new Rectangle(3200, 0, 800, 500));

            player = new Player(Content.Load<Texture2D>("Adam_sprite"), new Vector2(0, 200), 100);

            Ooball_texture = Content.Load<Texture2D>("Ooball");
            Ooball_rectangle = new Rectangle(300, 300, 100, 100);

            healthTexture = Content.Load<Texture2D>("health");

            velocity.X = 3f;
            velocity.Y = 3f;

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
        }


        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Scrolling background, creating an illusion of smooth background flow

            if (scrolling1.rectangle.X + scrolling1.texture.Width <= 0)
                scrolling1.rectangle.X = scrolling2.rectangle.X + scrolling2.texture.Width;

            if (scrolling2.rectangle.X + scrolling2.texture.Width <= 0)
                scrolling2.rectangle.X = scrolling3.rectangle.X + scrolling3.texture.Width;

            if (scrolling3.rectangle.X + scrolling3.texture.Width <= 0)
                scrolling3.rectangle.X = scrolling4.rectangle.X + scrolling4.texture.Width;

            if (scrolling4.rectangle.X + scrolling4.texture.Width <= 0)
                scrolling4.rectangle.X = scrolling5.rectangle.X + scrolling5.texture.Width;

            if (scrolling5.rectangle.X + scrolling5.texture.Width <= 0)
                scrolling5.rectangle.X = scrolling1.rectangle.X + scrolling1.texture.Width;

            scrolling1.Update();
            scrolling2.Update();
            scrolling3.Update();
            scrolling4.Update();
            scrolling5.Update();

            healthRectangle = new Rectangle(50, 20, player.health, 20); // ( poèetni x, poèetni y, width, height )

            // Ooball random movement

            Ooball_rectangle.X = Ooball_rectangle.X + (int)velocity.X;
            Ooball_rectangle.Y = Ooball_rectangle.Y + (int)velocity.Y;

            // Adam keyboard control

            player.position += player.velocity;


            if (Keyboard.GetState().IsKeyDown(Keys.Right)) player.velocity.X = 3f;  // Speed adjustment 

            else if (Keyboard.GetState().IsKeyDown(Keys.Left)) player.velocity.X = -3f;

            else player.velocity.X = 0f;

            if (Keyboard.GetState().IsKeyDown(Keys.Space) && player.hasJumped == false)
            {
                player.position.Y -= 10f;
                player.velocity.Y = -7f;
                player.hasJumped = true;
            }

            if (player.hasJumped == true)
            {
                float i = 1;
                player.velocity.Y += 0.15f * i;

            }

            if (player.position.Y + player.texture.Height >= 450)

                player.hasJumped = false;

            if (player.hasJumped == false)

                player.velocity.Y = 0f;

            // Adam doesn't leave a deafult window

            if (player.position.X <= 0)
                player.position.X = 0;

            if (player.position.X + player.texture.Width >= screenWidth)
                player.position.X = screenWidth - player.texture.Width;

            if (player.position.Y <= 0)
                player.position.Y = 0;

            if (player.position.Y + player.texture.Height >= screenHeight)
                player.position.Y = screenHeight - player.texture.Height;


            // Ooball doesn't leave a default window

            if (Ooball_rectangle.X <= 0)
                velocity.X = -velocity.X;
            if (Ooball_rectangle.X + Ooball_texture.Width >= screenWidth)
                velocity.X = -velocity.X;

            if (Ooball_rectangle.Y <= 0)
                velocity.Y = -velocity.Y;
            if (Ooball_rectangle.Y + Ooball_texture.Height >= screenHeight)
                velocity.Y = -velocity.Y;

            // Interaction between sprites

            if (player.rectangle.Intersects(Ooball_rectangle))
                velocity.Y = -velocity.Y;
            if (player.rectangle.Intersects(Ooball_rectangle))
                velocity.X = -velocity.X;

            // OObal and Adam interaction - related to health bar

            if (player.rectangle.Intersects(Ooball_rectangle))
                player.health -= 10; //life fractions adjustment

            player.Update();

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            // Scrolling backgrounds must be drawn first !
            scrolling1.Draw(spriteBatch);
            scrolling2.Draw(spriteBatch);
            scrolling3.Draw(spriteBatch);
            scrolling4.Draw(spriteBatch);
            scrolling5.Draw(spriteBatch);
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            spriteBatch.Draw(Ooball_texture, Ooball_rectangle, Color.White);
            player.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
