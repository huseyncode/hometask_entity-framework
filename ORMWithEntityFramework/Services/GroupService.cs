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
using System.Threading.Tasks;

namespace ORMWithEntityFramework.Services
{
    public static class GroupService
    {
        private static readonly ApDbContexs _contex;

        static GroupService()
        {
            _contex = new ApDbContexs();
        }

        public static void GetAllGroups()
        {
            foreach (var group in _contex.Groups.ToList())
                Console.WriteLine($"Id - {group.Id}, Name - {group.Name}");
        }

        public static void AddGroup()
        {
            if (_contex.Teachers.Count()== 0)
            {
                Messages.WarningMessage("Teacher");
                return;
            }
        GroupNameInput: Messages.InputMessages("Group name");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Messages.InvalidInputMeesages("Group Name");
                goto GroupNameInput;
            }

            var existGroup = _contex.Groups.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (existGroup is not null)
            {
                Messages.AlreadyExistMessage($"Group name - {name}");
                goto GroupNameInput;
            }

        GroupLimitInput: Messages.InputMessages("Group limit");
            string limitInput = Console.ReadLine();
            int limit;
            bool isSucceeded = int.TryParse(limitInput, out limit);
            if (!isSucceeded || limit <= 0)
            {
                Messages.InvalidInputMeesages("Group limit");
                goto GroupLimitInput;
            }

        GroupBeginDateInput: Messages.InputMessages("Group begin date (format: dd.MM.yyyy)");
            string beginDateInput = Console.ReadLine();
            DateTime beginDate;
            isSucceeded = DateTime.TryParseExact(beginDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out beginDate);
            if (!isSucceeded)
            {
                Messages.InvalidInputMeesages("Group begin date");
                goto GroupBeginDateInput;
            }

        GroupEndDateInput: Messages.InputMessages("Group end date (format: dd.MM.yyyy)");
            string endDateInput = Console.ReadLine();
            DateTime endDate;
            isSucceeded = DateTime.TryParseExact(endDateInput, "dd.MM.yyyy", CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out endDate);
            if (!isSucceeded || beginDate.Date.AddMonths(6) > endDate.Date)
            {
                Messages.InvalidInputMeesages("Group end date");
                goto GroupEndDateInput;
            }

        TeacherList: TeacherService.GetAllTeachers();

            Messages.InputMessages("Teacher id");
            string teacherIdInput = Console.ReadLine();
            int id;
            isSucceeded = int.TryParse(teacherIdInput, out id);
            if (!isSucceeded)
            {
                Messages.InvalidInputMeesages("Teacher id");
                goto TeacherList;
            }

            var teacher = _contex.Teachers.Find(id);
            if (teacher is null)
            {
                Messages.NotFountMessage("Teacher");
                goto TeacherList;
            }

            var group = new Group
            {
                Name = name,
                Limit = limit,
                BeginDate = beginDate,
                EndDate=endDate,
                TeacherId=teacher.Id
        };

            _contex.Groups.Add(group); ;
            try
            {
                _contex.SaveChanges();
            }
            catch (Exception)
            {
                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessages("Group", "added");
                      
        }

        public static void UpdateGroup()
        {
        GroupIdInput: GetAllGroups();
            Messages.InputMessages("group id");
            string IdInput= Console.ReadLine();
            int groupId;
            bool issucceeded = int.TryParse(IdInput, out groupId);
            if (!issucceeded)
            {
                Messages.InvalidInputMeesages("group id");
                goto GroupIdInput;
            }
            var existGroup = _contex.Groups.Find(groupId);
            if (existGroup is null)
            {
                Messages.NotFountMessage("group");
                goto GroupIdInput;
            }
            string Name = existGroup.Name;
        GroupNameInput: Messages.WantToChangeMessage("name");
            string input = Console.ReadLine();
            issucceeded = char.TryParse(input, out char answer);
            if (!issucceeded || !answer.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answer");
                goto GroupNameInput;
            }
            if (answer == 'y')
            {
            NameInput: Messages.InputMessages("New name");
                Name=Console.ReadLine();
                if (string.IsNullOrWhiteSpace(Name)) 
                {
                    Messages.InvalidInputMeesages("name");
                    goto NameInput;
                }
                var ExistGroupName=_contex.Groups.FirstOrDefault(n=>n.Name.ToLower()== Name.ToLower() && n.Id!=groupId);
                if (ExistGroupName is not null)
                {
                    Messages.AlreadyExistMessage("group name");
                    goto GroupNameInput;
                }
            }
            int Limit = existGroup.Limit;
        GroupLimitInput: Messages.WantToChangeMessage("limit");
            input = Console.ReadLine();
            issucceeded = char.TryParse(input, out answer);
            if (!issucceeded || !answer.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answer");
                goto GroupLimitInput;
            }
            if (answer == 'y')
            {
            LimitInput: Messages.InputMessages("limit");
                 string newLimitInput=Console.ReadLine();
                issucceeded = int.TryParse(newLimitInput, out Limit);
                if (!issucceeded)
                {
                    Messages.InvalidInputMeesages("limit");
                    goto LimitInput;
                }
                var CountStudentsInGroup = _contex.Students.Where(x=>x.GroupId==groupId).Count();
                if (CountStudentsInGroup > Limit)
                {
                    Messages.WarningMessage("limit");
                }
            }
        BeginDateInput: Messages.WantToChangeMessage("Begin date");
            input = Console.ReadLine();
            issucceeded=char.TryParse(input, out answer);
            if (!issucceeded || !answer.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answer");
                goto BeginDateInput;
            }
            DateTime BeginDate = existGroup.BeginDate;
            if (answer == 'y')
            {
            BeginDate: Messages.InputMessages("begin date(format: dd.MM.yyyy)");
                input= Console.ReadLine();
                issucceeded=DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out BeginDate);
                if (!issucceeded) 
                {
                    Messages.InvalidInputMeesages("begin date");
                    goto BeginDate;
                }
            }
        EndDateInput: Messages.WantToChangeMessage("End date");
            input = Console.ReadLine();
            issucceeded = char.TryParse(input, out answer);
            if (!issucceeded || !answer.IsValidChoice())
            {
                Messages.InvalidInputMeesages("answer");
                goto EndDateInput;
            }
            DateTime EndDate = existGroup.EndDate;
            if (answer == 'y')
            {
            EndDate: Messages.InputMessages("end date(format: dd.MM.yyyy)");
                input = Console.ReadLine();
                issucceeded = DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out EndDate);
                if (!issucceeded)
                {
                    Messages.InvalidInputMeesages("end date");
                    goto EndDate;
                }
            }
            if (BeginDate.AddMonths(6).Date>EndDate.Date)
            {
                Messages.DatePeriodMessage("6");
                goto BeginDateInput;
            }
            int Teacherid = existGroup.TeacherId;
            if (_contex.Teachers.Count() > 1)
            {
            TeacherInput: Messages.WantToChangeMessage("teacher");
                input = Console.ReadLine();
                issucceeded=char.TryParse(input, out answer);
                if (issucceeded || !answer.IsValidChoice())
                {
                    Messages.InvalidInputMeesages("answer");
                    goto TeacherInput;
                }
                if (answer == 'y')
                {
                Teacherid: TeacherService.GetAllTeachers();
                    Messages.InputMessages("teacher id");
                    string TeacherIdInput=Console.ReadLine();
                    issucceeded = int.TryParse(TeacherIdInput, out Teacherid);
                if (!issucceeded)
                    {
                        Messages.InvalidInputMeesages("teacher id");
                        goto TeacherInput;  
                    }
                var ExistTeacher = _contex.Teachers.FirstOrDefault(x=>x.Id !=existGroup.TeacherId && x.Id==Teacherid);
                    if (ExistTeacher is null)
                    {
                        Messages.NotFountMessage("id");
                        goto Teacherid;
                    }
                }
            }
            existGroup.Name = Name;
            existGroup.Limit = Limit;
            existGroup.TeacherId = Teacherid;
            existGroup.BeginDate=BeginDate;
            existGroup.EndDate=EndDate;
            _contex.Groups.Update(existGroup);
            try
            {
                _contex.SaveChanges();
            }
            catch (Exception)
            {

                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessages("group", "updated");

        }

        public static void DeleteGroup()
        {
            GetAllGroups();
        InputGroupId: Messages.InputMessages("Group id");
            string inputId=Console.ReadLine();
            int groupId;
            bool issucceeded=int.TryParse(inputId, out groupId);
            if (!issucceeded)
            {
                Messages.InvalidInputMeesages("Group id");
                goto InputGroupId;
            }
            var group = _contex.Groups.Find(groupId);
            if (group is null)
            {
                Messages.NotFountMessage("Group");
                return;
            }
            
            group.IsDeleted = true;
            _contex.Groups.Update(group);
            try
            {
                _contex.SaveChanges();            }
            catch (Exception)
            {

                Messages.ErrorOccuredMessage();
            }
            Messages.SuccessMessages("Group", "deleted");
        }

        public static void GetDetailsofGroup()
        {
            GetAllGroups();
        InputGroupId: Messages.InputMessages("Group id");
            string inputId=Console.ReadLine();
            int groupId;
            bool issucceeded=int.TryParse(inputId, out groupId);
            if(!issucceeded )
            {
                Messages.InvalidInputMeesages("Group id");
                goto InputGroupId;
            }
            var group = _contex.Groups.Include(x => x.Teacher).Include(x => x.Students).FirstOrDefault(x => x.Id == groupId);
            if ( group is null)
            {
                Messages.NotFountMessage("Group");
                return;
            }
            Console.WriteLine($"{group.Name}, {group.Limit}, {group.Teacher.Name}");
            foreach (var student in group.Students)
            {
                Console.WriteLine($"{student.Name}, {student.Surname}");
            }
        }
    }
}
