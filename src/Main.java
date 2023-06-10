import java.nio.ByteBuffer;

public class Main {

    public static byte[] ROM(int... instructions) {
        var rom = new byte[instructions.length*2];
        for(int i = 0; i < instructions.length; i++) {
            var instruction = instructions[i];
            if(instruction > 0xFFFF) throw new ArrayStoreException("Instruction " + Integer.toBinaryString(instruction) + " can not fit into a short! Invalid instruction!");
            rom[i] = (byte) ((instruction & 0xFF00) >> 8);
            rom[i + 1] = (byte) (instruction & 0x00FF);
        }
        return rom;
    }

    public static void main(String[] args) {
        Chip8 chip = new Chip8();
        Renderer renderer = new Renderer(chip.getDisplay());
        chip.getRAM().writeBytes(0x50, Font.FONT_DATA, 0, Font.FONT_DATA.limit());

        final boolean IBM = true;
        if(IBM) {
            chip.loadROM("resources/IBM Logo.ch8");
            while(true) {
                renderer.update();
                chip.update();
            }
        }
        else {
            final byte[] rom = ROM(
                0x00E0,
                0xA050,
                0x6005,
                0x6104,
                0x7101,
                0xD015,
                0x1AAA
            );

            chip.memory.writeBytes(0x200, ByteBuffer.wrap(rom), 0, rom.length);
            for(int i = 0; i < rom.length; i++) {
                // print out programm counter and current instruction in binary
                System.out.println("PC: " + chip.PC);
                System.out.println("Instruction: " + Integer.toBinaryString(chip.memory.read16(chip.PC)));
                chip.update();
                renderer.update();
            }

            // Clear screen
            chip.display.fillScreen(0xFFFFFF);
            chip.decodeInstruction((short) 0x00E0);
            assert(chip.display.pixel(5, 5) == 0x000000);

            // Set I register to 0x50
            chip.decodeInstruction((short) 0xA050);
            assert(chip.I == 0x50);

            // Set 0 register to 5 and 1 register to 4
            chip.decodeInstruction((short) 0x6005);
            chip.decodeInstruction((short) 0x6104);
            assert(chip.V[0] == 5);
            assert(chip.V[1] == 4);

            // Add 1 to 1 register
            chip.decodeInstruction((short) 0x7101);
            assert(chip.V[1] == 5);

            // Draw 0
            chip.decodeInstruction((short) 0xD015);
            assert(chip.display.pixel(5, 5) == 0xFFFFFF);

            // Jump to 0xFFF
            chip.decodeInstruction((short) 0x1AAA);
            assert(chip.PC == 0xAAA);

            renderer.update();
        }
    }
}