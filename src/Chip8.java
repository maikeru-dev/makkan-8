import java.util.Stack;

public class Chip8 {
    private final static short FIRST_M = (short) 0b1111_0000_0000_0000;
    private final static short SECOND_M = (short) 0b0000_1111_0000_0000;
    private final static short THIRD_M = (short) 0b0000_0000_1111_0000;
    private final static short LAST_M = (short) 0b0000_0000_0000_11111;

    private RAM memory;
    private Display display;
    private int PC;
    private short I;
    private Stack<Short> functionStack;
    private byte delayTimer;
    private byte soundTimer; // Gives off beeping sound if not 0
    private byte[] variableRegisters;


    public Chip8() {
        this.memory = new RAM(Settings.RAM_SIZE);
        this.display = new Display();
        this.PC = 0x200;
        this.I = 0x000;
        this.functionStack = new Stack<>();
        this.delayTimer = 0x000;
        this.soundTimer = 0x000;
        this.variableRegisters = new byte[16];
    }
    private short fetchInstruction() {
        PC += 2;
        return memory.read16((PC-2)*8);
    }
    private void decodeInstruction(short instruction) {
        // 00E0 : Clear screen
        // 1 NNN : JUMP
        // 00EE : SUBROUTINE RETURN : JUMP BUT BEFORE JUMP FIRST PUSH PC
        // 2 NNN : SUBROUTINE EXECUTE : RETURN STATEMENT FOR SUBROUTINE
        //
        switch (instruction & FIRST_M) {
            case 0x0000 -> {

            }
            case 0x1000 -> {

            }
            case 0
        }
    }




}
