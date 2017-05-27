
using Microsoft.WindowsAzure.Storage.Table;

namespace storagesasdemo
{
public class Student: TableEntity
    {
        public Student(string className, string Name)
        {
            PartitionKey = className;
            RowKey = Name;
        }

        public Student() {}
        
        public int Age  { get; set; }  
        public string Address {get;set;}

        public override string ToString()
        {
            return $"Student name: {RowKey}, Student class: {PartitionKey}, Age: {Age}, Address: {Address}";
        }
    }
}