// See https://aka.ms/new-console-template for more information


using System.Diagnostics;

namespace makkan_8;

internal static class Entry
{
    internal static ushort[] TestROM =
    {
        0x00E0,
        0xA050,
        0x6005,
        0x6104,
        0x7101,
        0xD015,
        0x1AAA
    };
    
    private static void Main()
    {
        // Entry
        var chip = new Chip8();
        
        // Clear screen
        chip.display.FillScreen(0xFFFFFF);
        chip.DEInstruction(0x00E0);
        Console.WriteLine(chip.display.Pixel(5, 5) == 0x000000);

        // Set I register to 0x50
        chip.DEInstruction(0xA050);
        Debug.Assert(chip.I == 0x50);

        // Set 0 register to 5 and 1 register to 4
        chip.DEInstruction(0x6005);
        chip.DEInstruction(0x6104);
        Debug.Assert(chip.V[0] == 5);
        Debug.Assert(chip.V[1] == 4);

        // Add 1 to 1 register
        chip.DEInstruction(0x7101);
        Debug.Assert(chip.V[1] == 5);

        // Draw 0
        chip.DEInstruction(0xD015);
        Debug.Assert(chip.display.Pixel(5, 5) == 0xFFFFFF);

        // Jump to 0xFFF
        chip.DEInstruction(0x1AAA);
        Debug.Assert(chip.PC == 0xAAA);

        //renderer.update();
    }
}