namespace makkan_8;

public class Display
{
    // Notes about graphics2D:
    /* Renders 0,0 @ top left
       All coordinates are integers.
       Increasing Y is downward, Increasing X is rightward.
       Can draw images (sprites?)
    *  */

    private int[] pixels;

    public int height { get; }
    public int width { get; }

    public Display() {

        if (Settings.SUPER_CHIP_PRESENT) {
            width = 128;
            height = 64;
        }
        else {
            width = 64; height = 32; // We should be using a config file
        }
        
        pixels = new int[width*height];
    }
    public void FillScreen(int value)  {
        for (int i = 0; i < pixels.Length; i++) {
            pixels[i] = value;
        }
    }
    public int Pixel(int x, int y) {
        return pixels[x+y*width];
    }

    public bool UpdatePixel(int x, int y, int colour)
    {
        bool state = pixels[x+y*width] == 0xFFFFFF;
        if (colour == 1) { // xor logic
            // flip
            if (state) { // white
                pixels[x+y*width] = 0;
            }else { // black
                pixels[x+y*width] = 0xFFFFFF;
            }
        }

        return state;
    }
    public void PrintPixels(){
        Console.Out.WriteLine();
        for (int y = 0; y < height; y++){
            for (int x = 0; x < width-1; x++){
                if (pixels[x+y*width] == 16777215) {
                    Console.Out.Write(0);
                }
                else Console.Out.Write(pixels[x + y*width]);
            }
            Console.Out.WriteLine(pixels[width-1 + y*width]);
        }
    }
}