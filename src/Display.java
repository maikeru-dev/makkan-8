import javax.swing.*;
import java.awt.*;
import java.awt.image.BufferedImage;

import static java.awt.image.BufferedImage.TYPE_BYTE_BINARY;

public class Display {

    // Notes about graphics2D:
    /* Renders 0,0 @ top left
       All coordinates are integers.
       Increasing Y is downward, Increasing X is rightward.
       Can draw images (sprites?)
    *  */

    private final int[] pixels;

    private Renderer renderer;
    private final int height;
    private final int width;

    public Display() {

        if (Settings.SUPER_CHIP_PRESENT) {
            width = 128; height = 64;
        }else {width = 64; height = 32;}


        pixels = new int[width*height];

        JFrame frame = new JFrame();
        frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        frame.setSize(new Dimension(Settings.DISPLAY_MULTIPLIER*width, Settings.DISPLAY_MULTIPLIER*height));
        frame.setTitle(Settings.PROGRAM_TITLE);
        frame.setResizable(false);
        frame.setVisible(true);

        renderer = new Renderer(width, height, pixels);
        frame.getContentPane().add(renderer);
    }
    public void updatePixel(int x, int y, int colour) {
        pixels[x+y*width] = colour;
        renderer.setPixels(pixels);
        renderer.update();
    }
}
