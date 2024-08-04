using Microsoft.EntityFrameworkCore;
using ORMWithEntityFramework.Constant;
using ORMWithEntityFramework.contexs;
using ORMWithEntityFramework.Entities;
using ORMWithEntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ORMWithEntityFramework.Services
{
    public static class StudentService
    {
        private static readonly ApDbContexs _context;
        static StudentService()
        {
            _context = new ApDbContexs();
        }

        public static void GetAllStudents()
        {
            foreach (var student in _context.Students.ToList())
            {
                Console.WriteLine($"id:{student.Id}, Name: {student.Name}, Surname:{student.Surname}");
            }
        }

        public static void AddStudent()
        {
        StudentNameInput: Messages.InputMessages("Student name");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMeesages("Student name");
                goto StudentNameInput;
            }

        StudentSurnameInput: Messages.InputMessages("Student surname");
            string surname = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(surname))
            {
                Messages.InvalidInputMeesages("Student surname");
                goto StudentSurnameInput;
            }

        StudentEmailInput: Messages.InputMessages("Student email");
            string email = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(email))
            {
                Messages.InvalidInputMeesages("Student email");
                goto StudentEmailInput;
            }

        StudentBirhDateInput: Messages.InputMessages("Student BirthDate (format: dd.MM.yyyy)");
            string studentBirhDateInput = Console.ReadLine();
            DateTime studentBirhDate;
            bool isSucceeded = DateTime.TryParseExact(studentBirhDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out studentBirhDate);
            if (!isSucceeded)
            {
                Messages.InvalidInputMeesages("Student BirhDate");
                goto StudentBirhDateInput;
            }

        GroupList: GroupService.GetAllGroups();

            Messages.InputMessages("Group id");
            string groupIdInput = Console.ReadLine();
            int id;
            isSucceeded = int.TryParse(groupIdInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMeesages("Group id");
                goto GroupList;
            }

            var group = _context.Groups.Find(id);
            if (group is null)
            {
                Messages.NotFountMessage("Group");
                goto GroupList;
            }


            Student student = new Student
            {
                Name = name,
                Surname = surname,
                Email = email,
                BirthDate = studentBirhDate,
                GroupId = group.Id
            };

            _context.Students.Add(student);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Messages.ErrorOccuredMessage();
            }

            Messages.SuccessMessages("Student", "added");

        }

        public static void UpdateStudent()
        {
        StudentIdInput: GetAllStudents();
            Messages.InputMessages("student id");
            string IdInput = Console.ReadLine();
            int studentId;
            bool issucceeded = int.TryParse(IdInput, out studentId);
            if (!issucceeded)
            {
                Messages.InvalidInputMeesages("student id");
                goto StudentIdInput;
            }
            var existStudent = _context.Students.Find(studentId);
            if (existStudent is null)
            {
                Messages.NotFountMessage("student");
                goto StudentIdInput;
            }
            string Name = existStudent.Name;
        StudentNameInput: Messages.WantToChangeMessage("name");
            string input = Console.ReadLine();
            issucceeded = char.TryParse(input, out char answer);
            if (!issucceeded || !answer.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answer");
                goto StudentNameInput;
            }
            if (answer == 'y')
            {
            NameInput: Messages.InputMessages("New name");
                Name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(Name))
                {
                    Messages.InvalidInputMeesages("name");
                    goto NameInput;
                }
            }
        StudentSurnameInput: Messages.WantToChangeMessage("surname");
            string surnameinput = Console.ReadLine();
            issucceeded = char.TryParse(surnameinput, out char answerr);
            if (!issucceeded || !answerr.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answerr");
                goto StudentSurnameInput;
            }
            if (answer == 'y')
            {
            SurnameInput: Messages.InputMessages("New surname");
                surnameinput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(surnameinput))
                {
                    Messages.InvalidInputMeesages("surname");
                    goto SurnameInput;
                }
            }
        StudentEmailInput: Messages.WantToChangeMessage("email");
            string emailinput = Console.ReadLine();
            issucceeded = char.TryParse(emailinput, out char answerrr);
            if (!issucceeded || !answerrr.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answerrr");
                goto StudentEmailInput;
            }
            if (answer == 'y')
            {
            EmailInput: Messages.InputMessages("New email");
                emailinput = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(emailinput))
                {
                    Messages.InvalidInputMeesages("email");
                    goto EmailInput;
                }
            }
        BirthDateInput: Messages.WantToChangeMessage("Birth date");
            input = Console.ReadLine();
            issucceeded = char.TryParse(input, out answer);
            if (!issucceeded || !answer.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answer");
                goto BirthDateInput;
            }
            DateTime BirthDate = existStudent.BirthDate;
            if (answer == 'y')
            {
            BirthDate: Messages.InputMessages("birth date(format: dd.MM.yyyy)");
                input = Console.ReadLine();
                issucceeded = DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out BirthDate);
                if (!issucceeded)
                {
                    Messages.InvalidInputMeesages("birth date");
                    goto BirthDate;
                }
            }
            int Groupid = existStudent.GroupId;
            if (_context.Groups.Count() > 1)
            {
            GroupInput: Messages.WantToChangeMessage("group");
                input = Console.ReadLine();
                issucceeded = char.TryParse(input, out answer);
                if (issucceeded || !answer.IsValidChoice())
                {
                    Messages.InvalidInputMeesages("answer");
                    goto GroupInput;
                }
                if (answer == 'y')
                {
                Groupid: GroupService.GetAllGroups();
                    Messages.InputMessages("group id");
                    string GroupIdInput = Console.ReadLine();
                    issucceeded = int.TryParse(GroupIdInput, out Groupid);
                    if (!issucceeded)
                    {
                        Messages.InvalidInputMeesages("group id");
                        goto GroupInput;
                    }
                    var ExistGroup = _context.Groups.FirstOrDefault(x => x.Id != existStudent.GroupId && x.Id == Groupid);
                    if (ExistGroup is null)
                    {
                        Messages.NotFountMessage("id");
                        goto Groupid;
                    }
                }
            }
            existStudent.Name = Name;
            existStudent.Surname = surnameinput;
            existStudent.Email = emailinput;
            existStudent.BirthDate = BirthDate;
            existStudent.GroupId = Groupid;

            _context.Students.Update(existStudent);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {

                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessages("student", "updated");

        }

        public static void DeleteStudent()
        {
            GetAllStudents();
        InputStudentId: Messages.InputMessages("student id");
            string inputId = Console.ReadLine();
            int studentId;
            bool issucceeded = int.TryParse(inputId, out studentId);
            if (!issucceeded)
            {
                Messages.InvalidInputMeesages("student id");
                goto InputStudentId;
            }

            var student = _context.Students.Find(studentId);
            if (student is null)
            {
                Messages.NotFountMessage("student");
                return;
            }
            student.IsDeleted = true;
            _context.Students.Update(student);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {

                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessages("Student", "deleted");
        }

        public static void GetDetailsofStudent()
        {
            GetAllStudents();
        InputStudentId: Messages.InputMessages("Student id");
            string inputId = Console.ReadLine();
            int studentId;
            bool issucceeded = int.TryParse(inputId, out studentId);
            if (!issucceeded)
            {
                Messages.InvalidInputMeesages("Student id");
                goto InputStudentId;
            }
            var student = _context.Students.Include(x => x.Group).FirstOrDefault(x => x.Id == studentId);

            if (student == null)
            {
                Messages.NotFountMessage("Student");
                return;
            }

            Console.WriteLine($"{student.Name}, {student.Surname}, {student.Email}, {student.BirthDate}, {student.Group.Name}");

            
        }

    }
}
