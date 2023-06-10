import javax.swing.*;
import java.awt.*;
import java.awt.geom.AffineTransform;
import java.awt.image.BufferedImage;
import java.awt.image.BufferedImageOp;
import java.util.Random;

import static java.awt.image.BufferedImage.TYPE_BYTE_BINARY;
import static java.awt.image.BufferedImage.TYPE_INT_RGB;

public class Renderer extends JPanel {
    private BufferedImage image;
    private Display display;
    private int pixelSize;
    public Renderer(Display display) {
        this.display = display;
        this.pixelSize = Settings.DISPLAY_MULTIPLIER;
        Dimension commonDimension = new Dimension(display.width*Settings.DISPLAY_MULTIPLIER, display.height*Settings.DISPLAY_MULTIPLIER);

        this.setSize(commonDimension);
        this.image = new BufferedImage(display.width, display.height, TYPE_INT_RGB);
        image.createGraphics();

        // set size of this
        setPreferredSize(new Dimension(display.width * pixelSize, display.height * pixelSize));
        JFrame JFrame = new JFrame(Settings.PROGRAM_TITLE);
        JFrame.setSize(commonDimension);
        JFrame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        JFrame.setResizable(false);
        JFrame.getContentPane().add(this);
        JFrame.setVisible(true);

        // add button to the right side of the window that prints button whenever pressed
        JButton button = new JButton("Button");
        button.addActionListener(e -> System.out.println("Button"));
        // add button to the right side of the image on the top
        // OOOOB <- O = image, B = button
        // O00O
        JFrame.add(button, BorderLayout.EAST);
        JFrame.pack();
    }
    @Override
    public void paintComponent(Graphics g) {
        super.paintComponent(g);
        Graphics2D g2d = (Graphics2D) g;
        g2d.drawImage(image, 0, 0, image.getWidth() * pixelSize, image.getHeight() * pixelSize, null);
    }
    public void update() {
        for (int i = 0; i < display.height; i++) {
            for (int j = 0; j < display.width; j++) {
                image.setRGB(j, i, display.pixel(j, i));
            }
        }
        this.repaint();
    }

}
