using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Imageprocessing
{
    class ImageFilters
    {
        CascadeClassifier faceCasacdeClassifier = new CascadeClassifier("haarcascade_frontalface_alt.xml");
        CascadeClassifier eye_cascade = new CascadeClassifier("haarcascade_eye.xml");
        public List<Mat> facesDetection(List<Mat> frames)
        {
            List<Mat> faultFrameList = new List<Mat>();

            foreach (var frame in frames)
            {

                //Convert from Bgr to Gray Image
                Mat grayImage = new Mat();
                CvInvoke.CvtColor(frame, grayImage, ColorConversion.Bgr2Gray);
                //Enhance the image to get better result
                CvInvoke.EqualizeHist(grayImage, grayImage);

                Rectangle[] faces = faceCasacdeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
                //If faces detected
                if (faces.Length > 0)
                {
                    // Console.Write("Face:detected");
                   // drawRectangle(grayImage,faces);
                    faultFrameList.Add(frame);
                    if (faultFrameList.Count == 5)
                    {
                        return faultFrameList;
                    }
                }
                //else
                //{
                //    // Console.Write("Face:no face detected");
                //    return false;
                //} 
            }

            //if (faultFrameList.Count > 0)
            //{
            //    foreach (var frame in faultFrameList)
            //    {
            //        frames.Remove(frame);
            //    }
            //}
            return faultFrameList;
        }

        private  void drawRectangle(Mat grayImage, Rectangle[] faces)
        {
            // Rectangle rectangle = new Rectangle(faces[0].X, faces[0].Y, faces[0].Width, faces[0].Height);
            for (int i = 0; i < 2; i++)
            {
                CvInvoke.Rectangle(grayImage, faces[i], new Bgr(Color.Red).MCvScalar, 2); 
            }
            CvInvoke.Imshow("image", grayImage);
            CvInvoke.WaitKey();
        }

        public List<Mat> contrastOfImage(List<Mat> frames)
        {
            List<Mat> faultFrameList = new List<Mat>();
            foreach (var frame in frames)
            {
                Mat yccImage = new Mat(); //Define the yccImage variable and store the transformed graph
                CvInvoke.CvtColor(frame, yccImage, ColorConversion.Bgr2YCrCb); //Color space conversion
                VectorOfMat channels = new VectorOfMat(); //Define the channel image after the channels store the split
                CvInvoke.Split(yccImage, channels); //Invoke the Split function to separate the yccImage color channel
                var range = channels[0].GetValueRange();//low(0)-High(1)
                var contrast=(range.Max - range.Min) / (range.Max + range.Min);
                if (contrast < 0.75)
                {
                    faultFrameList.Add(frame);
                }
            }
            if (faultFrameList.Count > 0)
            {
                foreach(var frame in faultFrameList)
                {
                    frames.Remove(frame);
                }
            }
            return frames;
        }
        public bool isImageBlurry(Mat image)
        {
            var laplacian = new Mat();
            var yccImage = new Mat();
            //load the image, convert it to grayscale, and compute the
            //focus measure of the image using the Variance of Laplacian
            //method
            CvInvoke.CvtColor(image, yccImage, ColorConversion.Bgr2Gray);
            CvInvoke.Laplacian(yccImage, laplacian, DepthType.Cv64F);
          //  string text = "Not Blurry";
            //if the focus measure is less than the supplied threshold,
            //then the image should be considered "blurry"
            var sngMean = new Mat();
            var sngStDev = new Mat();
            CvInvoke.MeanStdDev(laplacian, sngMean, sngStDev);
            // Console.Write(sngStDev.GetData().GetValue(0,0));
            var variance = Math.Pow(Convert.ToDouble(sngStDev.GetData().GetValue(0, 0)), 2);
            //laplacian = sngStDev * sngStDev;

            if (variance < 100.0)
            {
              //  text = "Blurry";
                return true;

            }
            return false;
          //  Console.Write(text+'\n');
        }

        public List<Mat> medianFilter(List<Mat> frames)
        {
            var laplacian = new Mat();
            var yccImage = new Mat();
            List<Mat> frameList = new List<Mat>();
            List<float> varianceList = new List<float>();
            List<ListModel> list = new List<ListModel>();
            foreach (var frame in frames)
            {
                CvInvoke.CvtColor(frame, yccImage, ColorConversion.Bgr2Gray);
                CvInvoke.Laplacian(yccImage, laplacian, DepthType.Cv64F);
                var sngMean = new Mat();
                var sngStDev = new Mat();
                CvInvoke.MeanStdDev(laplacian, sngMean, sngStDev);
                var variance = (float)Math.Pow(Convert.ToDouble(sngStDev.GetData().GetValue(0, 0)), 2);
                varianceList.Add(variance);
                ListModel listModel = new ListModel();
                listModel.variance = variance;
                listModel.frame = frame;
                list.Add(listModel);
            }
            varianceList.Sort();
            int centre = list.Count / 2;
            double median =0;
            if (list.Count % 2 == 0)
            {
                median = (varianceList[centre] + varianceList[centre + 1]) / 2;
            }
            else
            {
                median = varianceList[centre];
            }
            
            foreach(var frame in list)
            {
                if (frame.variance >= median)
                {
                    frameList.Add(frame.frame);
                }
            }
        
            return frameList;
        }

        public void facesDetection(Mat frame)
        {
            List<Mat> faultFrameList = new List<Mat>();


                //Convert from Bgr to Gray Image
                Mat grayImage = new Mat();
                CvInvoke.CvtColor(frame, grayImage, ColorConversion.Bgr2Gray);
                //Enhance the image to get better result
                CvInvoke.EqualizeHist(grayImage, grayImage);

                Rectangle[] faces = faceCasacdeClassifier.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
            //If faces detected
            if (faces.Length > 0)
            {
                Console.WriteLine("Face Width is " + faces[0].Width + " and face height is " + faces[0].Height);
                eyeDistance(grayImage);
            }

        }


        public void eyeDistance(Mat grayImage)
        {
            //Mat grayImage = new Mat();
            //CvInvoke.CvtColor(frame, grayImage, ColorConversion.Bgr2Gray);
            ////Enhance the image to get better result
            //CvInvoke.EqualizeHist(grayImage, grayImage);

            Rectangle[] eyes = eye_cascade.DetectMultiScale(grayImage, 1.1, 3, Size.Empty, Size.Empty);
            if (eyes.Length>=2)
            {
                int distance = (int)Math.Sqrt(Math.Pow((eyes[0].X - eyes[1].X), 2) + Math.Pow((eyes[0].Y - eyes[1].Y), 2));
                Console.WriteLine("Distance between both eyes is :" +distance); 
                  drawRectangle(grayImage, eyes);
               // Console.WriteLine(Math.Sqrt(Math.Pow((faces[0].X - faces[0].Y), 2) + Math.Pow((faces[0].Width - faces[0].Height), 2))); 
            }
         //   Console.ReadKey();
        }

        
                
               
    }
}
