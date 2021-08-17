using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luxand;
using System.IO;

namespace Imageprocessing
{
    class Luxand
    {
       
        List<FaceModel> faces = new List<FaceModel>();
        public Luxand()
        {
            FSDK.ActivateLibrary("ZMVE+zWPzhXivihDYPL3xEP/fV5QVJtQrigJPrKyCqvXg9j04tH3I4vlJ/E5PQrOT3W5p24+t6g77+CIIjhWE6n68Ybtb7gMKWn6LBERpevwa6MeX9qeH8MffvpiWR5A8nksig8yizTdZHJID0bbjVgeoLPliGmKPWT8Vg4IC2E=");
            FSDK.InitializeLibrary();
        }
        public float RecognizeFace(Mat face)
        {
            float similarity=0;
            var img = face.ToImage<Bgra, Byte>();
            var imageJpegByteArray1 = img.ToJpegData();
            Image testImage1 = byteArrayToImage(imageJpegByteArray1);
            FSDK.CImage image1 = new FSDK.CImage(testImage1); // loading
           // Image frameImage1 = image1.ToCLRImage();
            byte[] faceTemplate1;
            FSDK.GetFaceTemplate(image1.ImageHandle, out faceTemplate1);
            Console.WriteLine("Checking database");
            
            faces = SqliteDataAccess.LoadFaces();
            if (faces.Count > 0)
            {
                foreach(FaceModel fac in faces)
                {
                    byte[] faceTemplate2 = fac.Face;
                    
                    FSDK.MatchFaces(ref faceTemplate1, ref faceTemplate2, ref similarity);
                    if (similarity > 0.80)
                    {
                        Console.WriteLine("Record found. ID is " + fac.Id);
                        return similarity;
                    }
                    
                }
                AddFace(faceTemplate1);
                Console.WriteLine("No record found. New Face added");
                return 0;

            }
            else
            {
                AddFace(faceTemplate1);
                Console.WriteLine("No record found. New Face added");
                return 0;
            }
            //FSDK.CImage image2 = new FSDK.CImage("sample.jpg");
            //byte[] faceTemplate2;
            //FSDK.GetFaceTemplate(image2.ImageHandle, out faceTemplate2);




           // return similarity;
        }

        private static void AddFace(byte[] faceTemplate)
        {
            FaceModel f = new FaceModel();
            f.Face = faceTemplate;
            SqliteDataAccess.SaveFace(f);
        }

        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            try
            {
                using (var ms = new MemoryStream(byteArrayIn))
                {
                    return Image.FromStream(ms);
                }
                // MemoryStream ms = new MemoryStream(byteArrayIn, 0, byteArrayIn.Length);
                // return Image.FromStream(ms, true);//Exception occurs here
            }
            catch { }
            return null;
        }

    }
}
