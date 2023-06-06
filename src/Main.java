import java.io.File;

public class Main {
    public static void main(String[] args) {
        Display display = new Display();
        RAM ram = new RAM(Settings.RAM_SIZE);
        ram.writeBytes(0x50, Font.FONT_DATA, 0, Font.FONT_DATA.limit());
        ram.loadROMFile(0x200, new File("resources/IBM Logo.ch8"));
    }
}