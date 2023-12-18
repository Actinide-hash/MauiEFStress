using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MauiEFStress.Entity
{
    [Table("Person")]
    public class Person
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public decimal Weight { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
