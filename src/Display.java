public class Display {

    // Notes about graphics2D:
    /* Renders 0,0 @ top left
       All coordinates are integers.
       Increasing Y is downward, Increasing X is rightward.
       Can draw images (sprites?)
    *  */

    private final int[] pixels;

    public final int height;
    public final int width;

    public Display() {

        if (Settings.SUPER_CHIP_PRESENT) {
            width = 128; height = 64;
        }
        else {
            width = 64; height = 32;
        }


        pixels = new int[width*height];
    }
    public void fillScreen(int value)  {
        for (int i = 0; i < pixels.length; i++) {
            pixels[i] = value;
        }
    }
    public int pixel(int x, int y) {
        return pixels[x+y*width];
    }

    public boolean updatePixel(int x, int y, int colour)
    {
        boolean state = pixels[x+y*width] == 1;
        pixels[x+y*width] = colour;
        return state;
    }
}
