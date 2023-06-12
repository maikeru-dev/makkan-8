using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using makkan_8;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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
    private SoundEffect BeepSoundEffect;
    private double SoundDelay = 0.1;
    private double SoundTimer = 0.0;

    public Chip8Window()
    {
        Chip = new Chip8();
        Display = Chip.display;
        Keypad = new KeypadHandler();
        Graphics = new GraphicsDeviceManager(this);
        Keypad = new KeypadHandler();
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
        Chip.memory.WriteBytes(Settings.ROM_ADRESS, new MemoryStream(rom), 0, rom.Length);
        
        Chip.LoadRom("C:/Users/mailr/Workspace/C#/makkan-8/makkan-8-desktop/Content/Space Invaders [David Winter].ch8");
        base.Initialize();
    }

    protected override void LoadContent()
    {
        Content.RootDirectory = "Content";
        BeepSoundEffect = Content.Load<SoundEffect>("Beep Sound");
        base.LoadContent();
    }

    protected override void Update(GameTime deltaTime)
    {
        Chip.Update(deltaTime.ElapsedGameTime.TotalSeconds);
        
        if (Chip.soundTimer > 0 && SoundTimer <= 0)
        {
            BeepSoundEffect.Play();
            SoundTimer = SoundDelay;
        }
        SoundTimer -= deltaTime.ElapsedGameTime.TotalSeconds;
        
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