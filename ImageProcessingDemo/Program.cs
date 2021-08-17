using Emgu.CV;
using System;
using System.ComponentModel;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using Intel.RealSense;
using System.Threading;
using System.Drawing;
using System.Threading.Tasks;

namespace Imageprocessing
{
    class Program
    {
        
        static void Main(string[] args)
        {

            //var pipe = new Pipeline();
            //pipe.Start();

            ////while (true)
            ////{
            //var frames = pipe.WaitForFrames();
            //var frame = frames.ColorFrame;
            //        //var fra = frame;
            //        Size size = new Size(640, 480);
            //        Mat frame1 = new Mat(size, DepthType.Cv16U, 3, frame.Data, 64);
            //        frame.Dispose();

            //Console.WriteLine(frame1.GetData());
            //        CvInvoke.Imshow("framecaptu re", frame1);
            //        CvInvoke.WaitKey(0);
            //        frame1.Dispose();
            ////}
            //return;
            //Mat srcImage = CvInvoke.Imread("C:/Users/IB917/Desktop/Imageprocessing/Imageprocessing/Imageprocessing/4.jpg");





            /* //show the image
             cv2.putText(image, "{}: {:.2f}".format(text, fm), (10, 30),
                 cv2.FONT_HERSHEY_SIMPLEX, 0.8, (0, 0, 255), 3)
             cv2.imshow("Image", image)
             key = cv2.waitKey(0)*//*
            // Wait for the user button to exit the program
            CvInvoke.WaitKey(0);*/

            //Pipeline pipe = new Pipeline();
            //pipe.Start(frame =>
            //{
            //    using (var fs = frame.As<FrameSet>())
            //    {
            //        // DON'T: Filtered frames will not be disposed
            //        // var frames = fs.Where(f => f.Is(Extension.DepthFrame)).ToList();

            //        // DO: All frames in the collection will be disposed along with the frameset
            //        var frames = fs.Where(f => f.DisposeWith(fs).Is(Extension.DepthFrame)).ToList();

            //        frames.ForEach(f =>
            //        {
            //            using (var p = f.Profile)
            //            using (var vf = f.As<VideoFrame>())
            //                Console.WriteLine($"#{f.Number} {p.Stream} {p.Format} {vf.Width}x{vf.Height}@{p.Framerate}");
            //        });
            //    }
            //});

            /* ImageFilters imageFilters = new ImageFilters();
             imageFilters.contrastOfImage(srcImage);
             imageFilters.isImageBlurry(srcImage);
             imageFilters.facesDetection(srcImage);
             CvInvoke.Imshow("Source Image", srcImage); //Display source image
             CvInvoke.WaitKey(0);*/
            SqliteDataAccess sqliteDataAccess = new SqliteDataAccess();
            IntelRealSense intelRealSense = new IntelRealSense();
          //  Task.Run(()=> intelRealSense.capture());
            //Thread.Sleep(1000);
           // intelRealSense.capture1();
           
            intelRealSense.capture1();

        }
    }
}
