using System;
using Microsoft.Xna.Framework.Input;

namespace makkan_8_desktop;

public class KeypadHandler
{
    public enum KeyState
    {
        NOINPUT,
        PRESSED,
        HELD,
        RELEASED
    }

    KeyState KeyPressed(KeyState previous)
    {
        return previous switch
        {
            KeyState.NOINPUT => KeyState.PRESSED,
            KeyState.PRESSED => KeyState.HELD,
            KeyState.HELD => KeyState.HELD,
            _ => KeyState.NOINPUT
        };
    }
    
    KeyState KeyReleased(KeyState previous)
    {
        if (previous == KeyState.HELD)
        {
            return KeyState.RELEASED;
        }
        return KeyState.NOINPUT;
    }

    bool Held(KeyState keyState)
    {
        return keyState switch
        {
            KeyState.PRESSED => true,
            KeyState.HELD => true,
            _ => false
        };
    }

    private KeyState[] KeyStates = new KeyState[255];

    public bool Update()
    {
        bool anyNewKey = false;
        foreach(var key in Keyboard.GetState().GetPressedKeys())
        {
            KeyStates[(int)key] = KeyPressed(KeyStates[(int)key]);
            anyNewKey = true;
        }
        foreach(int key in Enum.GetValues(typeof(Keys)))
        {
            if(!Keyboard.GetState().IsKeyDown((Keys) key)) KeyStates[key] = KeyReleased(KeyStates[key]);
        }

        return anyNewKey;
    }
    
    public KeyState GetKeyState(Keys key)
    {
        return KeyStates[(int)key];
    }
    
    public bool GetKeyDown(byte key)
    {
        return key switch
        {
            0 =>  Held(KeyStates[(int)Keys.D1]),
            1 =>  Held(KeyStates[(int)Keys.D2]),
            2 =>  Held(KeyStates[(int)Keys.D3]),
            3 =>  Held(KeyStates[(int)Keys.D4]),
            4 =>  Held(KeyStates[(int)Keys.Q]),
            5 =>  Held(KeyStates[(int)Keys.W]),
            6 =>  Held(KeyStates[(int)Keys.E]),
            7 =>  Held(KeyStates[(int)Keys.R]),
            8 =>  Held(KeyStates[(int)Keys.A]),
            9 =>  Held(KeyStates[(int)Keys.S]),
            10 => Held(KeyStates[(int)Keys.D]),
            11 => Held(KeyStates[(int)Keys.F]),
            12 => Held(KeyStates[(int)Keys.Z]),
            13 => Held(KeyStates[(int)Keys.X]),
            14 => Held(KeyStates[(int)Keys.C]),
            15 => Held(KeyStates[(int)Keys.V]),
        };
    }
}