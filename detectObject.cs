using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
//Requires the AForge Library
using AForge.Imaging.Filters;

namespace Rovio
{
    public class Detection
    {

        /// <summary>
        /// Get camera image and converts it to a bitmap
        /// </summary>
        /// <param name="camImg">Image, saved image from a camera</param>
        /// <returns>Bitmap Image</returns>
        public Bitmap getcamImg(Image newImg)
        {
            System.Drawing.Bitmap bImg = new System.Drawing.Bitmap(newImg);
            return bImg;
        }


        /// <summary>
        /// Filter the image froma HSV colour range
        /// </summary>
        /// <param name="image">Bitmap Image, any suitable image</param>
        /// <returns>Bitmap Image</returns>
        public Bitmap addFilters(Bitmap image)
        {
			//Instantiate new HSL filter 
            AForge.Imaging.Filters.HSLFiltering hslFilter = new AForge.Imaging.Filters.HSLFiltering();
			
			//Define HSL colour range (default colour is orange)
            hslFilter.Hue = new AForge.IntRange(0, 50);
            hslFilter.Saturation = new AForge.DoubleRange(0.7, 1);
            hslFilter.Luminance = new AForge.DoubleRange(0.2, 0.7);

			//Instantiate new RGB filter 
            /*AForge.Imaging.Filters.ColorFiltering rgbFilter = new AForge.Imaging.Filters.ColorFiltering();
			
			//Define RGB colour range (default colour is orange)
            rgbFilter.Red = new AForge.IntRange(60, 180);
            rgbFilter.Green = new AForge.IntRange(10, 80);
            rgbFilter.Blue = new AForge.IntRange(5, 25);*/

			//Apply the HSL filter to the input image
            hslFilter.ApplyInPlace(image);
			
			//Create a 5 by 5 matrix using a short array
            int rows = 5;
            int cols = 5;
            short[,] Matrix = new short[rows, cols];

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    Matrix[i, j] = 1;
			
			//Instantiate new dilate filter with the 5 by 5 matrix
			AForge.Imaging.Filters.Dilatation dilate = new AForge.Imaging.Filters.Dilatation(Matrix);
			//Apply the filter
			dilate.ApplyInPlace(image);

            return image;
        }

        /// <summary>
        /// Apply rectangle to image
        /// </summary>
        /// <param name="image">Bitmap, any suitable image</param>
        /// <returns>Rectangle</returns>
        public Rectangle DrawRectangle(Bitmap image)
        {
			//Instantiate new blob counter
            AForge.Imaging.BlobCounter detectBlob = new AForge.Imaging.BlobCounter();

            //Get object by size
            detectBlob.ObjectsOrder = AForge.Imaging.ObjectsOrder.Size;
			
			//Apply BlobCounter
            detectBlob.ProcessImage(image);

            //objects in an array.0
            Rectangle[] rects = detectBlob.GetObjectsRectangles();
			
			//Create graphics from image
            Graphics g = Graphics.FromImage(image);

            try
            {
				//Draw rectangle around largest rectangle (default colour is red)
                g.DrawRectangle(new Pen(Color.Red), rects[0]);
				
				//Optional to save the result image to the startup directory
                //string startupPath = System.IO.Directory.GetCurrentDirectory();
                //image.Save(startupPath + @"\nameyourfile.jpg");
				
				//Return the rantngla
                return rects[0];

            }
            catch (IndexOutOfRangeException e)
            {
				//If no blobs return empty rectangle
                Rectangle rrr = new Rectangle(0, 0, 0, 0);
                return rrr;
            }


        }
    }

}



