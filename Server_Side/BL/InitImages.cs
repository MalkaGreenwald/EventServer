using BL.Helpers;
using Clarifai.API;
using Clarifai.DTOs.Inputs;
using Clarifai.DTOs.Predictions;
using DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Google.Cloud.Vision.V1;
using Google.Cloud.Storage.V1;
using Entities;
using RestSharp;

namespace BL
{
    public class InitImages
    {
        //Clarifai המפתח, של מלכי גרינוולד
        static ClarifaiClient clarifaiClient = new ClarifaiClient("15d9d0972ffb42009c5d3b757c55d679");
        static List<Concept> resClarifai = new List<Concept>();
        //Microsoft vision api המפתח, של מלכי גרינוולד
        const string MicrosoftKey = "26575440cd95406b888237d955b11383";
        //Microsoft vision api endpoint
        const string uriBase =
        "https://event-photos.cognitiveservices.azure.com/face/v1.0/detect";
        public static EventEntities DB = new EventEntities();

        private static string SendToStorage(string filepath)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\My First Project-b781c7f56bda.json");
            // upload the image storage
            //string projectId = "wordproject-249810";
            //char[] fu = filepath.ToCharArray();
            filepath.Remove(4, 1); //delete one slesh 
            string objectName = filepath.Split('\\').Last();
            string bucketName = "bucketmyexample";
            bool IsException;

            string imageURL = "https://storage.cloud.google.com/bucketmyexample/" + objectName + "?walkthrough_tutorial_id=storage_quickstart";
            //https://console.cloud.google.com/storage/browser/forth
            StorageClient storage = StorageClient.Create();

            using (var f = File.OpenRead(@filepath))
                try
                {
                    var res = storage.UploadObject(bucketName, objectName, null, f);
                    Console.WriteLine($"Uploaded {objectName}.");
                    //imageURL = res.SelfLink;
                    return imageURL;

                }
                catch (Exception)
                {
                    Console.WriteLine("your image didnot load.\n");
                    IsException = true;
                    throw;
                }
        }
        private static async Task<List<Concept>> GetResClarifai(string filePath)
        {

            try
            {
                var res = await clarifaiClient.PublicModels.GeneralModel
                .Predict(new ClarifaiFileImage(File.ReadAllBytes(filePath)))
                .ExecuteAsync();
                return res.Get().Data;
            }
            catch (Exception e)
            {

                throw;
            }
        }
        private static JObject getResultFacePP(Stream stream)
        {
            //convert filePath to base64
            string base64String;
            using (System.Drawing.Image image = System.Drawing.Image.FromStream(stream))
            {
                using (MemoryStream m = new MemoryStream())
                {
                    image.Save(m, image.RawFormat);
                    byte[] imageBytes = m.ToArray();
                    base64String = Convert.ToBase64String(imageBytes);
                }
            }
            const string API_Key = "Yoxjj0Tu2hUPY5D5K-iQ4ZkJoGm2W2r3";
            const string API_Secret = "q97dj2QUarKmaC5NdTOdBXjC4XI2USta";
            const string BaseUrl = "https://api-us.faceplusplus.com/facepp/v3/detect";
            IRestClient _client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest(Method.POST);
            request.AddParameter("api_key", API_Key);
            request.AddParameter("api_secret", API_Secret);
            request.AddParameter("image_base64", base64String);
            request.AddParameter("return_attributes", "eyestatus");
            var response = _client.Execute(request);

            JObject results = JObject.Parse(response.Content);
            return results;
        }
        //פונקציה זו בינתיים היא שומרת את התמונות בתיקייה כאן
        public static async Task<List<ImageEntity>> InsertImages()
        {
            HttpResponseMessage response = new HttpResponseMessage();
            var httpRequest = HttpContext.Current.Request;
            string temp = "~/UploadFile/";
            List<Entities.ImageEntity> l = new List<Entities.ImageEntity>();
            string uploaded_image;
            if (httpRequest.Files.Count > 0)
            {
                for (var i = 0; i < httpRequest.Files.Count; i++)
                {
                    var postedFile = httpRequest.Files[i];
                    var filePath = HttpContext.Current.Server.MapPath(temp + postedFile.FileName);
                    postedFile.SaveAs(filePath);
                    //uploaded_image = SendToStorage(filePath);
                    var x = await InitImageDetailsAsync(filePath, postedFile.FileName, postedFile.InputStream);
                }
                l = Images.GetImages();

            }

            return l;
        }
        //הפונקציה מוסיפה שורה לטבלת התמונות, תמונה עם כל הפרטים עליה
        private static async Task<int> InitImageDetailsAsync(string filePath, string fileName, Stream stream)
        {
            image img = new image();
            //img.url = filePath;
            img.url = "http://localhost:50637/UploadFile/" + fileName;
            img.name = fileName;
            var resClarifai = await GetResClarifai(filePath);
            var resFacePP = getResultFacePP(stream);
            //var resGoogleVision = await GetResGoogleV(filePath);
            img.isBlur = IsBlur(resClarifai);
            img.isClosedEye = IsClosedEye(resFacePP);
            img.isCutFace = false;//to delete in db
            img.isDark = IsDark(filePath);
            img.isGroom = IsGroom(resFacePP);
            img.isLight = IsLight(filePath);
            img.numPerson = await NumPerson(filePath);
            img.isIndoors = IsIndoors(resClarifai);
            //bool[] whatHas = ageGroups(resClarifai);
            //img.hasChildren = HasChildren(stream);
            DB.images.Add(img);
            DB.SaveChanges();
            return 1;
        }

        private static bool IsBlur(List<Concept> res)
        {

            foreach (var concept in res)
                if (concept.Name == "blur")
                    return true;
            return false;
        }

        private static bool IsClosedEye(JObject results)
        {
            //face++
            //רשימה של פנים:
            var faces = results["faces"].Children().ToList();
            int i = 1;
            foreach (var item in faces)
            {
                if (++i > 5)
                    return false;
                var eyestatus = item["attributes"]["eyestatus"];
                var jsonData = eyestatus.Children().ToList();
                List<JToken> tokens = jsonData.Children().ToList();
                if (double.Parse(tokens[0]["no_glass_eye_close"].ToString()) > 0.3)
                    return true;
                if (double.Parse(tokens[0]["normal_glass_eye_close"].ToString()) > 0.3)
                    return true;
                if (double.Parse(tokens[1]["no_glass_eye_close"].ToString()) > 0.3)
                    return true;
                if (double.Parse(tokens[1]["normal_glass_eye_close"].ToString()) > 0.3)
                    return true;

            }
            return false;
        }

        private static bool IsDark(string filePath)
        {
            //TODO
            return false;
        }

        private static bool IsGroom(JObject results)
        {
            const string API_Key = "Yoxjj0Tu2hUPY5D5K-iQ4ZkJoGm2W2r3";
            const string API_Secret = "q97dj2QUarKmaC5NdTOdBXjC4XI2USta";
            const string BaseUrl = "https://api-us.faceplusplus.com/facepp/v3/compare";
            IRestClient _client;
            RestRequest request;
            string token1 = DB.Grooms.First(f => f.name == "groom.jpg").token;
            var faces = results["faces"].Children().ToList();
            foreach (var item in faces)
            {
                string token2 = item.Last.First.ToString();
                _client = new RestClient(BaseUrl);
                request = new RestRequest(Method.POST);
                request.AddParameter("api_key", API_Key);
                request.AddParameter("api_secret", API_Secret);
                request.AddParameter("face_token1", token1);
                request.AddParameter("face_token2", token2);
                var response = _client.Execute(request);
                JObject res = JObject.Parse(response.Content);
                if (double.Parse(res["confidence"].ToString()) > 80)
                    return true;
            }
            return false;
        }
        //try vision-api
        public static Boolean IsInside(string filePath)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "D:\\eventyehudit-34566794ca28.json");
            // Instantiates a client
            var client = ImageAnnotatorClient.Create();
            // Load the image file into memory
            var path = HttpContext.Current.Server.MapPath("/UploadFile/" + filePath);
            var image = Image.FromFile(path);
            // Performs label detection on the image file
            var response = client.DetectLabels(image);

            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    Console.WriteLine(annotation.Description + " :  " + annotation.Confidence.ToString());

                }
            }
            Console.ReadLine();
            return true;
        }
        public static bool IsIndoors(List<Concept> res)
        {
            decimal num = Convert.ToDecimal(0.88);
            foreach (var concept in res)
            {
                if (concept.Name == "indoors")
                    if (concept.Value >= num)
                        return true;
                if (concept.Name == "outdoors"
                    || concept.Name == "nature")
                    if (concept.Value >= num)
                        return false;
            }
            return true;
        }
        public static bool HasChildren(Stream stream)
        {
            const string BaseUrl =
"https://automl.googleapis.com/v1beta1/projects/eventesti/locations/us-central1/models/ICN5997183896619379064:predict";
            IRestClient _client = new RestClient(BaseUrl);
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "Bearer $(gcloud auth application-default print-access-token)");
            request.AddParameter("image_url", "https://images1.calcalist.co.il/PicServer3/2019/04/11/898728/9_l.jpg");//open
            request.AddJsonBody(JObject.Parse(" {'payload':{'image':{'imageBytes':'"+ stream+"'}} }"));
            var response = _client.Execute(request);
            Console.WriteLine(" ");





            return false;
        }
        public static bool[] ageGroups(List<Concept> res)
        {
            decimal num = Convert.ToDecimal(0.90);
            bool[] whatHas = new bool[2];
            int flag = 0;
            foreach (var concept in res)
            {
                //if has children
                if (concept.Name == "no person")
                    if (concept.Value >= num)
                        return whatHas;
                if (concept.Name == "child" || concept.Name == "girl" || concept.Name == "baby" || concept.Name == "boy")
                    if (concept.Value >= num)
                    {
                        whatHas[0] = true;
                        if (++flag == 2)
                            return whatHas;
                    }

                if (concept.Name == "man" || concept.Name == "woman" || concept.Name == "adult" || concept.Name == "people")
                    if (concept.Value >= num)
                    {
                        whatHas[1] = true;
                        if (++flag == 2)
                            return whatHas;
                    }
            }
            return whatHas;
        }
        private static bool IsLight(string filePath)
        {
            //TODO
            return false;
        }

        //Clarifai פונקציה זו ניגשת ל
        //כי זה מודל לא גנרלי api צריך כאן לגשת מחדש ל
        private static async Task<int> NumPerson(string filePath)
        {
            try
            {
                var res = await clarifaiClient.PublicModels.FaceDetectionModel
                .Predict(new ClarifaiFileImage(File.ReadAllBytes(filePath)))
                .ExecuteAsync();
                return res.Get().Data.Count;
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}

