using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWithEntityFramework.Entities
{
    internal class Grade : BaseEntity
    {
       public decimal Point { get; set; }   
        public int AssigmentId {  get; set; } 
        public Assigment Assigment { get; set; }

        public int StudentId { get; set; }  

        public Student Student { get; set; }

    }
}
