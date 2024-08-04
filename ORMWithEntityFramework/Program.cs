using Azure;
using ORMWithEntityFramework.Constant;
using ORMWithEntityFramework.Services;

namespace ORMWithEntityFramework
{
    internal class Program
    {


        static void Main(string[] args)
        {
            while (true)
            {
                ShowMenu();

                Messages.InputMessages("Choice");

                string choiceInput = (Console.ReadLine());
                int choice;
                bool isSucceeded = int.TryParse(choiceInput, out choice);
                if (isSucceeded)
                {
                    switch ((Operations)choice)
                    {
                        case Operations.AllTeachers:
                            TeacherService.GetAllTeachers();    
                            break;
                        case Operations.CreateTeacher:
                            TeacherService.AddTeacher();
                            break;
                        case Operations.UpdateTeacher:
                            TeacherService.UpdateTeacher();
                            break;
                        case Operations.RemoveTeacher:
                            TeacherService.DeleteTeacher();
                            break;
                        case Operations.DetailsTeacher:
                            TeacherService.GetTeacherDetails();
                            break;
                        case Operations.AllGroups:
                            GroupService.GetAllGroups();
                            break;
                        case Operations.CreateGroup:
                            GroupService.AddGroup();
                            break;
                        case Operations.UpdateGroup:
                            GroupService.UpdateGroup();
                            break;
                        case Operations.DeleteGroup:                           
                            break;
                        case Operations.DetailsGroup:
                          GroupService.GetDetailsofGroup();
                            break;
                        case Operations.AllStudents:
                            StudentService.GetAllStudents();
                            break;
                        case Operations.CreateStudent:
                            StudentService.AddStudent();                            
                            break;
                        case Operations.UpdateStudent:
                            StudentService.UpdateStudent();                           
                            break;
                        case Operations.DeleteStudent:
                            StudentService.DeleteStudent();
                            break;
                        case Operations.DetailsStudent:
                            StudentService.GetDetailsofStudent();
                            break;
                        case Operations.Exit:
                            return;
                        default:
                            Messages.InvalidInputMeesages("Choice");
                            break;
                    }
                }
                else
                {
                    Messages.InvalidInputMeesages("Choice");
                }
            }
        }
        public static void ShowMenu()
        {
            Console.WriteLine("---MENU----");
            Console.WriteLine("1.All teachers ");
            Console.WriteLine("2. Add teacher");
            Console.WriteLine("3. Update teacher");
            Console.WriteLine("4. Delete teacher");
            Console.WriteLine("5. Details of teacher");
            Console.WriteLine("6. All groups");
            Console.WriteLine("7. Add group");
            Console.WriteLine("8. Update group");
            Console.WriteLine("9. Delete group");
            Console.WriteLine("10. Details of group");
            Console.WriteLine("11. All students");
            Console.WriteLine("12. Add student");
            Console.WriteLine("13. Update student");
            Console.WriteLine("14. Delete student");
            Console.WriteLine("15. Details of student");
            Console.WriteLine("0. Exit");
        }
    }
}
