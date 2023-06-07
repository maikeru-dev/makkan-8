
public class Main {
    public static void main(String[] args) {
        Chip8 chip = new Chip8();
        Renderer renderer = new Renderer(chip.getDisplay());
        chip.getRAM().writeBytes(0x50, Font.FONT_DATA, 0, Font.FONT_DATA.limit());
        chip.I = 0x50;
        chip.V[0] = 5; // x
        chip.V[1] = 5; // y


//        chip.loadROM("/Users/mecha/IdeaProjects/makkan-8/out/production/makkan-8/IBM Logo.ch8");
        renderer.update();
        chip.decodeInstruction((short) 0x00E0);
        chip.decodeInstruction((short) 0xD015);
        renderer.update();


    }
}