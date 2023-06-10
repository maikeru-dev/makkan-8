using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace makkan_8_desktop;

public class Chip8Window : Game
{
    private GraphicsDeviceManager Graphics;
    private Texture2D Pixel;
    
    public Chip8Window()
    {
        Graphics = new GraphicsDeviceManager(this);
    }

    protected override void Initialize()
    {
        Pixel = new Texture2D(GraphicsDevice, 64, 64);
        
        // fill texture with random pixels
        Color[] colors = new Color[64 * 64];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(
                (byte)Random.Shared.Next(255), // G
                (byte)Random.Shared.Next(255), // R
                (byte)Random.Shared.Next(255), // B
                (byte)Random.Shared.Next(255)); // A
        }
        Pixel.SetData(colors);
        base.Initialize();
    }

    protected override void Update(GameTime deltaTime)
    {
        base.Update(deltaTime);
    }

    protected override void Draw(GameTime deltaTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteBatch.Begin(samplerState : SamplerState.PointClamp);
        var windowWidth = GraphicsDevice.Viewport.Width;
        var windowHeight = GraphicsDevice.Viewport.Height;
        spriteBatch.Draw(Pixel, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
        spriteBatch.End();
        
        base.Draw(deltaTime);
    }
}