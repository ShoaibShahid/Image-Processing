
using Intel.RealSense;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
// Emgu.cv 4.4.0: 4099
// Emgu.cv windows runtime 4.4.0: 4099
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Imageprocessing
{
    class IntelRealSense
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        Pipeline pipeline = new Pipeline();
        Luxand luxand = new Luxand();
        VideoCapture _capture = new VideoCapture(0);
        FormMirror formMirror = new FormMirror();
        bool isOpenCVCapturing = false;
        bool addframe = false;
        Mat _frame;
        List<Mat> _frameList = new List<Mat>();
        public double latestDistance { get; set; } = 0;
        public bool captureDepth { get; set; } = true;
        ImageFilters imageFilters = new ImageFilters();
        public void capture1()
        {
            _capture.ImageGrabbed += ProcessFrame;
            _frame = new Mat();
            
            if (_capture != null)
            {
                try
                {
                    _capture.Start();
                    isOpenCVCapturing = true;
                    addframe = true;
                    log.Debug("Start capturing Frames usin open cv");
                    Task.Run(() => formMirror.ShowDialog());
                    Console.WriteLine("start capturing");
                    while (true)
                    {

                    }
                    // Task.Delay(3000).ContinueWith(t => { _capture.Stop();captureDepth = true; });
                    // Console.ReadLine();
                }
                catch (Exception ex)
                {
                    log.Error("Exception occur while capturing frame" + ex);
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void findDevices()
        {   
            var ctx = new Context();
            var devices = ctx.QueryDevices();
            Console.WriteLine("There are {0} connected RealSense devices.", devices.Count);
            if (devices.Count == 0) 
            {
                Console.ReadKey();
                return;
            }
            var dev = devices[0];
            Console.WriteLine("\nUsing device 0, an {0}", dev.Info[CameraInfo.Name]);
            Console.WriteLine("    Serial number: {0}", dev.Info[CameraInfo.SerialNumber]);
            Console.WriteLine("    Firmware version: {0}", dev.Info[CameraInfo.FirmwareVersion]);
            Console.ReadKey();
            
        }
        public void capture()
        {
            var cfg = new Config();
            
            cfg.EnableStream(Intel.RealSense.Stream.Depth, 640, 480);
           // cfg.EnableStream(Intel.RealSense.Stream.Color, Format.Rgb8);
           //  pipeline.Start(cfg);
            //var sframes = pipeline.WaitForFrames();

            //for (int i = 0; i < 5; i++)
            //  {
            //      var sframes = pipeline.WaitForFrames(); 
            //  }

            while (captureDepth)
            {
               // pipeline.Start(cfg);
               // log.Debug("Start Capturing frames using intelrealsense");
               // var frames = pipeline.WaitForFrames();//.Take(1).ToArray();
               //// var frame = frames.ColorFrame;
               // var depth = frames.DepthFrame;
               // latestDistance = depth.GetDistance(depth.Width / 2, depth.Height / 2);

               // Console.WriteLine("The camera is pointing at an object " +latestDistance+ " meters away\t");
               // log.Debug("The camera is pointing at an object " + latestDistance + " meters away");
               // pipeline.Stop();
               // log.Debug("Intelrealsense releases the camera");
                
               // if (latestDistance > 1.5)
               // {
                    //log.Info("Person detected in the range");
                    captureDepth = false;
                if (!isOpenCVCapturing)
                {
                    capture1();
                }
                else
                {
                    addframe = true;
                }
                    while (!captureDepth)
                    {
                        Thread.Sleep(50);
                    }
                addframe = false;
                Console.WriteLine("Frames captured " + _frameList.Count());
                    log.Info("Frames captured "+ _frameList.Count);
                //List<Mat> faultFrameList = new List<Mat>();
                //if (_frameList.Count() > 0)
                //{
                //    foreach (Mat frame in _frameList){
                //        if (!imageFilters.isImageBlurry(frame))
                //        {
                //            if (imageFilters.contrastOfImage(frame) >= 0.75)
                //            {
                //                if (!imageFilters.facesDetection(frame))
                //                {
                //                    faultFrameList.Add(frame);
                //                }
                //            }
                //            else
                //            {
                //                faultFrameList.Add(frame);
                //            }
                //        }
                //        else
                //        {
                //            faultFrameList.Add(frame);
                //        }
                //    }

                //}
                //if (faultFrameList.Count() > 0)
                //{
                //    foreach(Mat frame in faultFrameList)
                //    {
                //        _frameList.Remove(frame);
                //    }
                //}
               
                    _frameList = imageFilters.medianFilter(_frameList);
                    _frameList = imageFilters.contrastOfImage(_frameList);
                    _frameList = imageFilters.facesDetection(_frameList);
                if (_frameList.Count() > 0)
                {
                    _capture.Stop();
                    isOpenCVCapturing = false;
                   
                    formMirror.Invoke((MethodInvoker)(() => formMirror.Close())); 
                    log.Info("No of frames recieved after filtration are:" + _frameList.Count);
                    Random random = new Random();
                    int num = random.Next(_frameList.Count() - 1);
                    log.Info("From filtered frames frameno " + num + "shown");
                    var similarity = luxand.RecognizeFace(_frameList.ElementAt(num));
                    Console.WriteLine("Similarity is " + similarity * 100 + "%");
                   // CvInvoke.Imshow("framecaptured " + num, _frameList.ElementAt(num));
                   // CvInvoke.WaitKey(0);

                }
                else
                {
                    Console.WriteLine("face not detected");
                    log.Info("face not detected");
                    
                }
                _frameList.Clear();
                //Console.ReadKey();
                //}

                //   Console.ReadKey();
                //Thread.Sleep(2000);
            }

            //  Size size = new Size(frame.Height, frame.Width);
            //Size size = new Size(640, 480);
            // Mat frame1 = new Mat(size,DepthType.Cv16U,3 , frame.Data,2);
            //CvInvoke.Imshow("framecapture",frame1);
          /*  Mat frame2 = new Mat(size, DepthType.Cv16U, 3, frame[1].Data, 0);
            CvInvoke.Imshow("frame2capture", frame2);*/
          //  pipeline.Stop();
 
          //  CvInvoke.WaitKey(0);
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame);
                //if (_frameList.Count() == 50)
                //{
                //    // _capture.Stop();
                //    //log.Debug("Open cv releases the camera");
                //    captureDepth = true;
                //}
                //else if (addframe && _frameList.Count() < 50)
                //{
                //    _frameList.Add(_frame);
                //    Console.WriteLine("frame captured" + _frameList.Count());
                //}
                Image image = _frame.ToBitmap();
                if (formMirror.IsHandleCreated)
                {
                    formMirror.frame(image); 
                }
                imageFilters.facesDetection(_frame);
            }
        }


    }
}
