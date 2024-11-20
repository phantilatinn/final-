using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Cloud.Firestore;

namespace dxclusive
{
    public class FirestoreService
    {
        private FirestoreDb db;

        public FirestoreService()
        {
            this.SetupFireStore().Wait(); // เรียก SetupFireStore ให้เสร็จก่อนใช้บริการ
        }

        private async Task SetupFireStore()
        {
            if (db == null)
            {
                var stream = await FileSystem.OpenAppPackageFileAsync("dxclusive-7459c-firebase-adminsdk-36sk6-0ad789642b.json");
                var reader = new StreamReader(stream);
                var contents = reader.ReadToEnd();
                db = new FirestoreDbBuilder
                {
                    ProjectId = "dxclusive-7459c",
                    JsonCredentials = contents
                }.Build();
            }
        }

        // ดึงรายชื่อ Terms
        public async Task<List<string>> GetTermListAsync()
        {
            var termList = new List<string>();
            var snapshot = await db.Collection("terms").GetSnapshotAsync();

            foreach (var document in snapshot.Documents)
            {
                if (document.Exists && document.TryGetValue("name", out string termName))
                {
                    termList.Add(termName);
                }
            }

            return termList;
        }

        // ดึงรายชื่อ Classes สำหรับ Term ที่กำหนด
        public async Task<List<Dictionary<string, string>>> GetClassDataAsync(string termDocumentId)
        {
            var classList = new List<Dictionary<string, string>>();
            var snapshot = await db.Collection("terms").Document(termDocumentId).Collection("classes").GetSnapshotAsync();

            foreach (var document in snapshot.Documents)
            {
                if (document.Exists)
                {
                    classList.Add(new Dictionary<string, string>
                    {
                        { "classId", document.Id }, // เก็บ Document ID ของ class
                        { "name", document.GetValue<string>("name") ?? "" },
                        { "description", document.GetValue<string>("description") ?? "" },
                        { "teacher", document.GetValue<string>("teacher") ?? "" }
                    });
                }
            }

            return classList;
        }

        // ดึงรายละเอียดของ Class หนึ่งใน Term ที่กำหนด
        public async Task<Dictionary<string, object>> GetClassDetailsAsync(string termDocumentId, string classDocumentId)
        {
            var classDetails = new Dictionary<string, object>();
            var docRef = db.Collection("terms").Document(termDocumentId).Collection("classes").Document(classDocumentId);
            var snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                classDetails = snapshot.ToDictionary();
            }

            return classDetails;
        }
    }

    // ตัวอย่างการใช้งาน
    class Program
    {
        static async Task Main(string[] args)
        {
            var firestoreService = new FirestoreService();

            // ดึงรายชื่อ Terms
            var terms = await firestoreService.GetTermListAsync();
            Console.WriteLine("Terms:");
            foreach (var term in terms)
            {
                Console.WriteLine($"- {term}");
            }

            // เลือก Term Document ID ตัวอย่าง
            var termDocumentId = "EAIUGT05c4J..."; // ใส่ Document ID จริงของ Term ที่ต้องการ
            Console.WriteLine($"\nClasses in Term '{termDocumentId}':");

            // ดึงรายชื่อ Classes ภายใต้ Term ที่เลือก
            var classes = await firestoreService.GetClassDataAsync(termDocumentId);
            foreach (var classData in classes)
            {
                Console.WriteLine($"- {classData["name"]} (Teacher: {classData["teacher"]})");
            }

            // เลือก Class Document ID ตัวอย่าง
            var classDocumentId = "dx212"; // ใส่ Document ID จริงของ Class ที่ต้องการ
            Console.WriteLine($"\nDetails for Class '{classDocumentId}':");

            // ดึงรายละเอียดของ Class
            var classDetails = await firestoreService.GetClassDetailsAsync(termDocumentId, classDocumentId);
            foreach (var detail in classDetails)
            {
                Console.WriteLine($"{detail.Key}: {detail.Value}");
            }
        }
    }
}

