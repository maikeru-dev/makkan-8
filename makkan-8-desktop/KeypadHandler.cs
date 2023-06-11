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
            Keys.D1 => 0,
            Keys.D2 => 1,
            Keys.D3 => 2,
            Keys.D4 => 3,
            Keys.Q => 4,
            Keys.W => 5,
            Keys.E => 6,
            Keys.R => 7,
            Keys.A => 8,
            Keys.S => 9,
            Keys.D => 10,
            Keys.F => 11,
            Keys.Z => 12,
            Keys.X => 13,
            Keys.C => 14,
            Keys.V => 15,
            _ => 16
        };

    }
}