using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.IO;

namespace SpriteMapGenerator
{
    public class MyCanvas : Canvas
    {
        BitmapImage background = null;

        public void LoadImage(string filename)
        {
            background = new BitmapImage(new Uri(filename));
            this.InvalidateVisual();
        }
        protected override void OnRender(DrawingContext dc)
        {
            if (background != null)
            {
                dc.DrawImage(background, new Rect(0, 0, background.PixelWidth, background.PixelHeight));
            }
        }
        public void GenSheet(string[] images)
        {
            BitmapFrame[] frames = new BitmapFrame[images.Length];
            int[] imageWidth = new int[images.Length];
            int[] imageHeight = new int[images.Length];

            int iW = images.Length;
            int iH = images.Length; 

            // Loads the images to tile (no need to specify PngBitmapDecoder, the correct decoder is automatically selected)
            for (int i = 0; i < images.Length; i++)
            {
               frames[i] = BitmapDecoder.Create(new Uri(images[i]), BitmapCreateOptions.None, BitmapCacheOption.OnLoad).Frames.First();
               imageWidth[i] = frames[i].PixelWidth;
               imageHeight[i] = frames[i].PixelHeight; 
            }
            // Gets the size of the images (I assume each image has the same size)
            for (int i = 0; i < images.Length; i++)
            {
                iW += frames[i].PixelWidth;
                iH += frames[i].PixelHeight;
            }

            // Draws the images into a DrawingVisual component
            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext drawingContext = drawingVisual.RenderOpen())
            {
                drawingContext.DrawImage(frames[0], new Rect(0, 0, imageWidth[0], imageHeight[0]));
                drawingContext.DrawImage(frames[1], new Rect(imageWidth[1], 0, imageWidth[1], imageHeight[1]));
                drawingContext.DrawImage(frames[2], new Rect(0, imageHeight[2], imageWidth[2], imageHeight[2]));
                drawingContext.DrawImage(frames[3], new Rect(imageWidth[3], imageHeight[3], imageWidth[3], imageHeight[3]));
            }

            // Converts the Visual (DrawingVisual) into a BitmapSource
            RenderTargetBitmap bmp = new RenderTargetBitmap(iW * 2, iH * 2, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(drawingVisual);

            // Creates a PngBitmapEncoder and adds the BitmapSource to the frames of the encoder
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bmp));

            string directory = Directory.GetCurrentDirectory();

            // Saves the image into a file using the encoder
            using (Stream stream = File.Create(directory + @"\tile.png"))
                encoder.Save(stream);
        }
    }
}
