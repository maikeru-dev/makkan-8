using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using makkan_8;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace makkan_8_desktop;

public class Chip8Window : Game
{
    private Chip8 Chip;
    private Display Display;
    private KeypadHandler Keypad;
    
    private GraphicsDeviceManager Graphics;
    private Texture2D Pixel;

    public Chip8Window()
    {   
        Keypad = new KeypadHandler();
        Chip = new Chip8(Keypad);
        Display = Chip.display;
        Graphics = new GraphicsDeviceManager(this);

    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = true;
        Pixel = new Texture2D(GraphicsDevice, Display.width, Display.height);
        
        byte[] rom = {
            0x00, 0xE0,
            0xA0, 0x50,
            0x60, 0x05,
            0x61, 0x04,
            0x71, 0x01,
            0xD0, 0x15,
            0x1A, 0xAA
        };
        Chip.memory.WriteBytes(0x200, new MemoryStream(rom), 0, rom.Length);

        Chip.LoadRom("./../../../Content/IBM Logo.ch8");
        base.Initialize();
    }
    
    protected override void Update(GameTime deltaTime)
    {
        Chip.Update(deltaTime.ElapsedGameTime.TotalMilliseconds);
        var anyNewKey = Keypad.Update();
        base.Update(deltaTime);
    }

    protected override void Draw(GameTime deltaTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch spriteBatch = new SpriteBatch(GraphicsDevice);
        spriteBatch.Begin(samplerState : SamplerState.PointClamp);
        var windowWidth = GraphicsDevice.Viewport.Height * 2;
        var windowHeight = GraphicsDevice.Viewport.Height;
        
        // draw the texture
        Color[] colors = new Color[Display.width * Display.height];
        for(int y = 0; y < Display.height; y++)
        {
            for(int x = 0; x < Display.width; x++)
            {
                colors[y * Display.width + x] = Display.Pixel(x, y) == 0 ? Color.Black : Color.White;
            }
        }
        
        Pixel.SetData(colors);
        spriteBatch.Draw(Pixel, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
        spriteBatch.End();
        
        base.Draw(deltaTime);
    }
}