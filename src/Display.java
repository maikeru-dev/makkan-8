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

    public int pixel(int x, int y) {
        return pixels[x+y*width];
    }

    public void updatePixel(int x, int y, int colour)
    {
        pixels[x+y*width] = colour;
    }
}
