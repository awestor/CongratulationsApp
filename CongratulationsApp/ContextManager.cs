using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CongratulationsApp
{
    internal class ContextManager
    {
        public void Input(string specification, DateTime birthday)
        {
            var TransformedBirthdayDate = DateTime.SpecifyKind(birthday, DateTimeKind.Utc);
            using (var DataBase = new ApplicationDB())
            {
                var newRecord = new Context { birthday = TransformedBirthdayDate, description = specification };
                DataBase.data.Add(newRecord);
                DataBase.SaveChanges();
            }
        }
        public void Output(List<Context> birthdays)
        {
            foreach (var date in birthdays)
            {
                Console.WriteLine(date);
            }
        }
        public void Delete(ApplicationDB DataBase, int id)
        {
            var delData = DataBase.data.Find(id);
            if (delData != null)
            {
                DataBase.data.Remove(delData);
                DataBase.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запись найдена и удалена.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Запись с данным номером была не найдена.");
            }
        }
        public void Edit(ApplicationDB DataBase, int id)
        {
            var editRecord = DataBase.data.Find(id);
            if (editRecord != null)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" Для сохранения текущего значения оставьте пустым поле ввода");

                Console.Write("Введите новую дату рождения: \n -> ");
                var editDateBirthday = Console.ReadLine();
                if (DateTime.TryParseExact(editDateBirthday, "dd.mm.yyyy", null, DateTimeStyles.None, out DateTime birthday))
                {
                    var transformedEditDateBirthday = DateTime.SpecifyKind(birthday, DateTimeKind.Utc);
                    editRecord.birthday = transformedEditDateBirthday;
                }

                Console.Write("Введите новое описание для дня рождения: \n -> ");
                var editDescription = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(editDescription))
                {
                    editRecord.description = editDescription;
                }

                DataBase.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Запись обновлена.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Запись не найдена.");
            }
            Console.ResetColor();
        }
    }
}
