import java.awt.*;
import java.io.File;

public class Main {
    public static void main(String[] args) {
        Display display = new Display();

        // using for loop update 100 random pixels
        for (int i = 0; i < 100; i++) {
            int x = (int) (Math.random() * display.width);
            int y = (int) (Math.random() * display.height);
            display.updatePixel(x, y, 0xFF00FF);
        }

        Renderer renderer = new Renderer(display);
        renderer.update();

        RAM ram = new RAM(Settings.RAM_SIZE);
        ram.loadROMFile(0x200, new File("resources/IBM Logo.ch8"));
        var byteBuffer = ram.readBytes(0x200, 0x100);
    }
}