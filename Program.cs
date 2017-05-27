using System;
using System.Linq;

namespace storagesasdemo
{
    class Program
    {
        private const string ConnectionString = "UseDevelopmentStorage=true";
        static void Main(string[] args)
        {
            var storageService = new StorageService(ConnectionString);
            var studentRepo = new StudentRepo(storageService);
            
            var studentToInsert = new Student("P2", "John Smith2 Jr");
            studentToInsert.Age = 6;
            studentToInsert.Address = "1 Infinite Loop, San Fran, US";

            studentRepo.InsertOrUpdateStudentAsync(studentToInsert).GetAwaiter().GetResult();
            
            var result = studentRepo.GetAllstudents().GetAwaiter().GetResult();
            var insertedStudent = result.ToList().FirstOrDefault();

            Console.WriteLine(insertedStudent.ToString());
            //Console.ReadKey();
        }
    }
}
