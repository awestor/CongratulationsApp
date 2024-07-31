using System;
using System.Text;
using System.Globalization;
using CongratulationsApp;

class Program
{
    private static ContextManager _contextManager = new ContextManager();
    static void Main(string[] args)
    {
        
        using (var DataBase = new ApplicationDB())
        {

            Console.OutputEncoding = Encoding.UTF8;

            Init(DataBase);

            while (true)
            {
                Console.Clear();
                ChangeColorOutput(" ------------------ Главное меню --------------------");
                ChangeColorOutput(" |                                                  |");
                ChangeColorOutput(" |    [1] Добавить новую запись                     |");
                ChangeColorOutput(" |    [2] Просмотр списка хранящихся записей        |");
                ChangeColorOutput(" |    [3] Просмотр списка ближайших дат рождения    |");
                ChangeColorOutput(" |    [4] Редактирование записей                    |");
                ChangeColorOutput(" |    [5] Удалить запись                            |");
                ChangeColorOutput(" |    [6] Завершение работы                         |");
                ChangeColorOutput(" |                                                  |");
                ChangeColorOutput(" ----------------------------------------------------");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(" Выберете опцию:");
                Console.Write(" ->");
                var choice = Console.ReadLine();
                Console.ResetColor();

                switch (choice)
                {
                    case "1":
                        InputNewRecord();
                        break;
                    case "2":
                        OutputAllRecords(DataBase);
                        break;
                    case "3":
                        OutputUpcomingBirthdays(DataBase);
                        break;
                    case "4":
                        DeleteOneRecord(DataBase);
                        break;
                    case "5":
                        EditRecords(DataBase);
                        break;
                    case "6":
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine(" Данная опция отсутствует, попробуйте снова.");
                        Console.ResetColor();
                        break;
                }
                Console.WriteLine(" Для продолжения нажмите любую клавишу ");
                Console.ReadKey();
            }
        }
    }
    private static void OutputUpcomingBirthdays(ApplicationDB DataBase)
    {
        var today = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
        var upcomingDate = DataBase.data.Where(currentDate =>
            currentDate.birthday.Day >= today.Day && currentDate.birthday.Month == today.Month &&
            currentDate.birthday.Year == today.Year ||
            currentDate.birthday.Month < today.AddMonths(2).Month &&
            currentDate.birthday.Year == today.Year);
        SortRecords(upcomingDate);
    }

    private static void InputNewRecord()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(" Введите дату дня рождения (в формате ДД.ММ.ГГГГ с точками):");
        string _birthday = Console.ReadLine();
        if (DateTime.TryParseExact(_birthday, "dd.MM.yyyy", null, DateTimeStyles.None, out DateTime birthday))
        {
            Console.WriteLine(" Введите описание для этой даты:");
            string specification = Console.ReadLine();
            _contextManager.Input(specification, birthday);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(" Запись добавлена.");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Некорректный или неверный формат записи даты.");
        }
        Console.ResetColor();
    }
    private static void OutputAllRecords(ApplicationDB DataBase)
    {
        var birthdays = DataBase.data;
        SortRecords(birthdays);
    }

    
    private static void DeleteOneRecord(ApplicationDB DataBase)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" Введите ID дня рождения для удаления: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _contextManager.Delete(DataBase, id);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Введённый ID некорректен.");
        }
        Console.ResetColor();
    }
    private static void EditRecords(ApplicationDB DataBase)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(" Введите ID дня рождения для редактирования: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            _contextManager.Edit(DataBase, id);
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Некорректный ID.");
        }
        Console.ResetColor();
    }
    private static void ChangeColorOutput(string message)
    {
        foreach (char col in message)
        {
            if (col == '-' || col == '|')
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(col);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(col);
                Console.ResetColor();
            }
        }
        Console.WriteLine();
    }
    private static void SortRecords(IQueryable<Context> birthdays)
    {
        var choice = "";
        while (true)
        {
            Console.Clear();
            ChangeColorOutput(" ---------- Записи дней рождения ----------");
            ChangeColorOutput(" |                                        |");
            ChangeColorOutput(" |    [1] Вывести без сортировки          |");
            ChangeColorOutput(" |    [2] Вывести с сортировкой по ID     |");
            ChangeColorOutput(" |    [3] Вывести с сортировкой по дате   |");
            ChangeColorOutput(" |    [4] Главное меню                    |");
            ChangeColorOutput(" |                                        |");
            ChangeColorOutput(" ------------------------------------------");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(" Выберите опцию:");
            Console.Write(" ->");
            choice = Console.ReadLine();
            Console.ResetColor();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine(" Список записей:");
                    _contextManager.Output(birthdays.ToList());
                    break;
                case "2":
                    Console.WriteLine(" Список записей:");
                    _contextManager.Output(birthdays.OrderBy(b => b.id).ToList());
                    break;
                case "3":
                    Console.WriteLine(" Список записей:");
                    _contextManager.Output(birthdays.OrderBy(b => b.birthday).ToList());
                    break;
                case "4":
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Выбранная опция отсутствует, попробуйте снова.");
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine(" Для продолжения нажмите любую клавишу... ");
            Console.ReadKey();
        }
    }
    private static void Init(ApplicationDB DataBase)
    {
        var today = DateTime.SpecifyKind(DateTime.Today, DateTimeKind.Utc);
        IQueryable<Context> upcomingDate = DataBase.data.Where(currentDate =>
            currentDate.birthday.Day >= today.Day && currentDate.birthday.Month == today.Month &&
            currentDate.birthday.Year == today.Year ||
            currentDate.birthday.Month < today.AddMonths(2).Month &&
            currentDate.birthday.Year == today.Year);

        if (upcomingDate.Any())
        {
            _contextManager.Output(upcomingDate.OrderBy(b => b.birthday).ToList());
            Console.WriteLine(" Для дальнейшей рабоьы нажмите любую клавишу... ");
            Console.ReadKey();
        }
        return;
    }
}