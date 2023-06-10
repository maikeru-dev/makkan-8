import java.io.File;
import java.util.Stack;

public class Chip8 {
    private final static short FIRST_M = (short) 0b1111_0000_0000_0000;
    private final static short SECOND_M = (short) 0b0000_1111_0000_0000;
    private final static short THIRD_M = (short) 0b0000_0000_1111_0000;
    private final static short LAST_M = (short) 0b0000_0000_0000_1111;

    public RAM memory;
    public Display display;
    public int PC;
    public short I;
    public Stack<Short> functionStack;
    public byte delayTimer;
    public byte soundTimer; // Gives off beeping sound if not 0
    public byte[] V;

    public Chip8() {
        this.memory = new RAM(Settings.RAM_SIZE);
        this.display = new Display();
        this.PC = 0x200;
        this.I = 0x000;
        this.functionStack = new Stack<>();
        this.delayTimer = 0x000;
        this.soundTimer = 0x000;
        this.V = new byte[16];
    }
    public Display getDisplay() {
        return display;
    }
    public void update() {
        decodeInstruction(fetchInstruction());
    }
    public RAM getRAM() {
        return memory;
    }

    public void loadROM(String filename) {
        memory.loadROMFile(0x200, new File(filename));
    }
    public void printAllRegisters() {
        System.out.println();
        System.out.println("Variable Registers:");
        for (byte register : V){
            String binaryString = Integer.toBinaryString(register);
            // Add leading zeroes if necessary
            binaryString = String.format("%8s", binaryString).replace(' ', '0');
            System.out.println(binaryString);
        }
        System.out.println();
        System.out.println("PC: " + Integer.toBinaryString(PC));
        System.out.println("I:" + Integer.toBinaryString(I));
    }
    public short fetchInstruction() {
        PC += 2;
        return memory.read16((PC-2));
    }
    public void decodeInstruction(short instruction) {
        // 00E0
        // 1 NNN : JUMP
        // 00EE :
        // 2 NNN : SUBROUTINE EXECUTE : RETURN STATEMENT FOR SUBROUTINE
        //
        switch (instruction) {
            case 0x00E0 -> { // : Clear screen
                display.fillScreen(0x000000);
            }
            case 0x00EE -> { // : SUBROUTINE RETURN : JUMP BUT BEFORE JUMP FIRST PUSH PC

            }
            default -> {
                switch (instruction & FIRST_M) {
                    case 0x1000 -> { // : JUMP - 12 bit NNN
                        PC = instruction & 0x0FFF;
                    }
                    case 0x6000 -> { // : SET REG X TO NN : 6XNN
                        V[(instruction & SECOND_M) >> 8] = (byte) (instruction & 0x00FF);
                    }
                    case 0x7000 -> { // : ADD NN TO REG X
                        V[(instruction & SECOND_M) >> 8] += (byte) (instruction & 0x00FF);
                    }
                    case (short) 0xA000 -> { // : SET INDEX REG TO NNN
                        I = (byte) (instruction & 0x0FFF);
                    }
                    case (short) 0xC000 -> {

                    }
                    case (short) 0xD000 -> { // : DISPLAY, XY = Addresses
                        byte x = (byte) (V[(instruction & SECOND_M) >> 8] % display.width);
                        byte y = (byte) (V[(instruction & THIRD_M) >> 4] % display.height);
                        byte N = (byte) (instruction & LAST_M);

                        V[0xF] = 0;

                        for (int n = 0; n < N; n++) {
                            byte sprite_row = memory.read8(Short.toUnsignedInt(I) + n);
                            for (int i = (x > display.width-7 ? display.width-x : 7); i >= 0; i--) {
                                int bit = (sprite_row >> i) & 1;
                                if (display.updatePixel(x,y, bit)) {
                                    V[0xF] = 1;
                                }
                                x++;
                            }
                            x = (byte) (x-8);
                            y++;
                            if (y > 32) break;
                        }

                    }
                    case 0x3000 -> { // 3XNN: Skip one instruction if X = NN
                        if (V[(instruction & SECOND_M) >> 8] == (byte) (instruction & 0x0FF)) PC++;
                    }
                    case 0x4000 -> { // 4XNN: Skip one instruction if X != NN;
                        if (V[(instruction & SECOND_M) >> 8] != (byte) (instruction & 0x0FF)) PC++;
                    }
                    case 0x5000 -> { // 5XY0: Skip one instruction if X & Y are equal.
                        if (V[(instruction & SECOND_M) >> 8] == V[(instruction & THIRD_M) >> 4]) PC++;
                    }
                    case (short) 0x9000 -> { // 9XY0: Skip one instruction if X & Y are NOT equal.
                        if (V[(instruction & SECOND_M) >> 8] != V[(instruction & THIRD_M) >> 4]) PC++;
                    }
                    case (short) 0x8000 -> {
                        switch (instruction & LAST_M) {
                            case 0x0000 -> { // 8XY0: Set X to the value of Y.
                                V[(instruction & SECOND_M) >> 8] = V[(instruction & THIRD_M) >> 4];
                            }
                            case 0x0001 -> { // 8XY1: Binary OR operation on X & Y, X is set to the result.
                                V[(instruction & SECOND_M) >> 8] = (byte) (V[(instruction & SECOND_M) >> 8] | V[(instruction & THIRD_M) >> 4]);
                            }
                            case 0x0002 -> { // 8XY2: Binary AND operation on X & Y, X is set to the result.
                                V[(instruction & SECOND_M) >> 8] = (byte) (V[(instruction & SECOND_M) >> 8] & V[(instruction & THIRD_M) >> 4]);
                            }
                            case 0x0003 -> { // 8XY3: Binary XOR operation on X & Y, X is set to the result.
                                V[(instruction & SECOND_M) >> 8] = (byte) (V[(instruction & SECOND_M) >> 8] ^ V[(instruction & THIRD_M) >> 4]);
                            }
                            case 0x0004 -> { // 8XY4: Add X to Y, set X to the result.
                                V[(instruction & SECOND_M) >> 8] = (byte) (V[(instruction & SECOND_M) >> 8] + V[(instruction & THIRD_M) >> 4]);
                            }
                            case 0x0005 -> { // 8XY5: Subtract Y from X, set X to the result.
                                V[(instruction & SECOND_M) >> 8] = (byte) (V[(instruction & SECOND_M) >> 8] - V[(instruction & THIRD_M) >> 4]);
                            }
                            case 0x0007 -> { // 8XY7: Subtract X from Y, set X to the result.
                                V[(instruction & SECOND_M) >> 8] = (byte) (V[(instruction & THIRD_M) >> 4] - V[(instruction & SECOND_M) >> 8]);
                            }
                        }

                    }

                    case (short) 0xF000 -> {


                    }

                }
            }
        }


    }




}
