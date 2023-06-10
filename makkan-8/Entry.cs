// See https://aka.ms/new-console-template for more information


namespace makkan_8;

internal static class Entry 
{
    private static void Main()
    {
        // Entry
        Chip8 chip8 = new Chip8();
        chip8.DEInstruction(0x1E02);

    }
}