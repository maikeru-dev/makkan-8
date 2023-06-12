using System;
using makkan_8;
using Microsoft.Xna.Framework.Input;

namespace makkan_8_desktop;

public class KeypadHandler : Input
{
    public bool Update() // In theory, diagonal movement should be possible. This is optimized, do not touch.
    {
        bool anyNewKey = false;
        foreach(var key in Keyboard.GetState().GetPressedKeys())
        {
            this.Enable(ConvertKeyPressed(key));
            anyNewKey = true;
        }
        foreach(int key in Enum.GetValues(typeof(Keys)))
        {
            if(!Keyboard.GetState().IsKeyDown((Keys) key)) this.Disable(ConvertKeyPressed((Keys) key));
        }

        return anyNewKey;
    }
    public byte ConvertKeyPressed(Keys key)
    {
        return key switch
        {
            Keys.D1 => 1,
            Keys.D2 => 2,
            Keys.D3 => 3,
            Keys.D4 => 0xC,
            Keys.Q => 4,
            Keys.W => 5,
            Keys.E => 6,
            Keys.R => 0xD,
            Keys.A => 7,
            Keys.S => 8,
            Keys.D => 9,
            Keys.F => 0xE,
            Keys.Z => 0xA,
            Keys.X => 0,
            Keys.C => 0xB,
            Keys.V => 0xF,
            _ => 16
        };

    }
}