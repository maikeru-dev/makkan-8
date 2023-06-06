import java.awt.*;
import java.io.File;

public class Main {
    public static void main(String[] args) {
        Chip8 chip = new Chip8();
        Renderer renderer = new Renderer(chip.getDisplay());
        chip.getRAM().writeBytes(0x50, Font.FONT_DATA, 0, Font.FONT_DATA.limit());
        chip.I = 0x50;
        chip.variableRegisters[0] = 5; // x
        chip.variableRegisters[1] = 5; // y
        chip.getDisplay().fillScreen(0xFFFFFF);

//        chip.loadROM("/Users/mecha/IdeaProjects/makkan-8/out/production/makkan-8/IBM Logo.ch8");
        renderer.update();
        System.out.println((short) 0xFF+1);
        chip.decodeInstruction((short) 0xD555);
        renderer.update();


    }
}