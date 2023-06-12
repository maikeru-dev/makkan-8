using System.Configuration;


namespace makkan_8;

public struct Settings
{
    public const String PROGRAM_TITLE = "makkan-8";

    public const int DISPLAY_MULTIPLIER = 8;

    // CHIP 8 SPECS
    public const int INSTRUCTIONS_PER_SECOND = 700;
    public const bool SUPER_CHIP_PRESENT = true;
    public const int RAM_SIZE = 4096;
    public const bool QUIRKY = false;
    
    // CONSTANTS
    public const ushort FONT_ADRESS = 0x50;
    public const ushort ROM_ADRESS = 0x200;
}