using Microsoft.EntityFrameworkCore;
using ORMWithEntityFramework.Constant;
using ORMWithEntityFramework.contexs;
using ORMWithEntityFramework.Entities;
using ORMWithEntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMWithEntityFramework.Services
{
    public static class TeacherService
    {
        private static readonly ApDbContexs _context;
        static TeacherService ()
        {
            _context = new ApDbContexs ();        
        }

        public static void GetAllTeachers()
        {
            foreach (var teacher in _context.Teachers.ToList())
            {
                Console.WriteLine($"id:{teacher.Id}, Name: {teacher.Name}, Surname:{teacher.Surname}");
            }
        }

        public static void AddTeacher()
        {
            TeacherNameInput: Messages.InputMessages("Teacher name");
            string name = Console.ReadLine();   

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMeesages("Teacher name");
                goto TeacherNameInput;
            }

            TeacherSurnameInput:  Messages.InputMessages("Teacher surname");
            string surname = Console.ReadLine();    
            if (string.IsNullOrWhiteSpace(surname))
            {
                Messages.InvalidInputMeesages("Teacher surname");
                goto TeacherSurnameInput;
            }



            Teacher teacher = new Teacher
            {
                Name = name,
                Surname = surname
            };
             
             _context.Teachers.Add(teacher);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessages("Teacher", "added");
            
           
        }

        public static void UpdateTeacher()
        {
            GetAllTeachers();

            TeacherIdInput: var id = Console.ReadLine();
            int TeacherId;
            bool isSucceeded = int.TryParse(id, out TeacherId);
            if (isSucceeded)
            {
                Messages.InvalidInputMeesages("Teacher id");
                goto TeacherIdInput;
            }

             var teacher = _context.Teachers.FirstOrDefault(t=> t.Id==TeacherId);
            if (teacher != null)
            {
                Messages.NotFountMessage("Teacher");
                return;
            }

            TeacherNameInput:  Messages.WantToChangeMessage("name");
            var choiceInput = Console.ReadLine();
            char choice;
            isSucceeded=char.TryParse(choiceInput, out choice);
            if (!isSucceeded || !choice.IsValidChoice()) 
            {
                Messages.InvalidInputMeesages("Choice");
                goto TeacherNameInput;
            }
            string newName= string.Empty;
            if (choice == 'y')
            {
                NewNameInput: Messages.InputMessages("new name");
                newName=Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newName))
                {
                    goto NewNameInput;
                }
            }

        TeacherSurnameInput: Messages.WantToChangeMessage("surname");
            choiceInput = Console.ReadLine();
            isSucceeded = char.TryParse(choiceInput, out choice);         
            if (!isSucceeded || !choice.IsValidChoice())
            {
                Messages.InvalidInputMeesages("Choice");
                goto TeacherSurnameInput;
            }
            string newSurname=string.Empty;
            if (choice == 'y')
            {
            NewSurnameInput: Messages.InputMessages("new surname");
                newSurname = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newSurname))
                {
                    goto NewSurnameInput;
                }
            }
            if (!string.IsNullOrEmpty(newName))
                teacher.Name = newName;

            if (!string.IsNullOrEmpty(newSurname))
                teacher.Surname = newSurname;

            _context.Teachers.Update(teacher);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception )
            {
                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessages("Teacher", "Updated");
        }

        public static void DeleteTeacher()
        {
            GetAllTeachers();
        TeacherIdInput: Messages.InputMessages("Teacher id");
            var idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (!isSucceeded) 
            {
                Messages.InvalidInputMeesages("Teacher id");
                goto TeacherIdInput;
            }

            var teacher = _context.Teachers.Find(id);
            if ( teacher is null)
            {
                Messages.NotFountMessage("Teacher");
                    return;
            }
            
                _context.Teachers.Remove(teacher);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {

                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessages("Teacher", "deleted");

        }

        public static void GetTeacherDetails()
        {
              GetAllTeachers();
        TeacherIdInput: Messages.InputMessages("Teacher id");
            var idInput = Console.ReadLine();
            int id;
            bool isSucceeded = int.TryParse(idInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMeesages("Teacher id");
                goto TeacherIdInput;
            }

            var teacher = _context.Teachers.Include(x => x.Groups).FirstOrDefault(x => x.Id == id);
            if ( teacher is null) 
            {
                Messages.NotFountMessage("Teacher");
                return;
            }

            Console.WriteLine($"Id:{teacher.Id}. Name:{teacher.Name}");
            foreach (var group in teacher.Groups)
                Console.WriteLine(group.Name);

        }
    }
}
