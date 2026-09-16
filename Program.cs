using System;
using System.Collections.Generic;
using System.Linq;

class FileItem
{
    public string Name { get; set; }
    public long Size { get; set; }
    public DateTime Modified { get; set; }
    public bool IsDirectory { get; set; }

    public FileItem(string name, long size, DateTime modified, bool isDirectory = false)
    {
        Name = name;
        Size = size;
        Modified = modified;
        IsDirectory = isDirectory;
    }
}

class Program
{
    const int WIDTH = 80;
    const int HEIGHT = 25;

    // Заранее подготовленный список файлов
    static List<FileItem> files = new List<FileItem>
    {
        new FileItem("..", 0,
            new DateTime(2026, 9, 14, 18, 42, 0), true),

        new FileItem("app.exe", 148736,
            new DateTime(2026, 8, 17, 14, 25, 0)),

        new FileItem("archive_data.zip", 1245184,
            new DateTime(2026, 9, 14, 10, 37, 0)),

        new FileItem("backup.zip", 524288,
            new DateTime(2026, 8, 21, 19, 43, 0)),

        new FileItem("bitmap.exe", 61752,
            new DateTime(2026, 8, 25, 11, 16, 0)),

        new FileItem("calculator.exe", 28416,
            new DateTime(2026, 9, 14, 17, 55, 0)),

        new FileItem("config.ini", 2048,
            new DateTime(2026, 8, 28, 9, 34, 0)),

        new FileItem("course_project.cs", 16784,
            new DateTime(2026, 9, 14, 15, 26, 0)),

        new FileItem("data.csv", 18342,
            new DateTime(2026, 9, 1, 16, 28, 0)),

        new FileItem("document.txt", 7341,
            new DateTime(2026, 9, 3, 12, 51, 0)),

        new FileItem("example.cpp", 12543,
            new DateTime(2026, 9, 5, 18, 17, 0)),

        new FileItem("graphics.dll", 238764,
            new DateTime(2026, 9, 6, 10, 42, 0)),

        new FileItem("lesson.pdf", 183456,
            new DateTime(2026, 9, 7, 15, 36, 0)),

        new FileItem("main.cs", 8642,
            new DateTime(2026, 9, 8, 20, 14, 0)),

        new FileItem("MyLongFileName.txt", 18742,
            new DateTime(2026, 9, 14, 12, 49, 0)),

        new FileItem("notes.docx", 43872,
            new DateTime(2026, 9, 9, 8, 27, 0)),

        new FileItem("program.exe", 98304,
            new DateTime(2026, 9, 10, 17, 45, 0)),

        new FileItem("readme.md", 4210,
            new DateTime(2026, 9, 14, 18, 11, 0)),

        new FileItem("report.pdf", 156734,
            new DateTime(2026, 9, 11, 13, 18, 0)),

        new FileItem("screenshot.png", 84562,
            new DateTime(2026, 9, 11, 22, 6, 0)),

        new FileItem("settings.cfg", 3567,
            new DateTime(2026, 9, 12, 9, 52, 0)),

        new FileItem("source.cpp", 16784,
            new DateTime(2026, 9, 12, 16, 33, 0)),

        new FileItem("student_info.dat", 9216,
            new DateTime(2026, 9, 13, 10, 15, 0)),

        new FileItem("test.cpp", 9216,
            new DateTime(2026, 9, 13, 17, 21, 0)),

        new FileItem("video.mp4", 7340032,
            new DateTime(2026, 9, 13, 21, 8, 0))
    };

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        try
        {
            Console.SetBufferSize(WIDTH, HEIGHT);
            Console.SetWindowSize(WIDTH, HEIGHT);
        }
        catch
        {
            // Некоторые терминалы не позволяют менять размер.
        }

        Console.CursorVisible = false;

        DrawInterface();

        Console.ReadKey();
    }

    static void DrawInterface()
    {
        Console.Clear();

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        // Заполняем весь экран
        for (int y = 0; y < HEIGHT; y++)
        {
            for (int x = 0; x < WIDTH; x++)
            {
                WriteAt(x, y, " ");
            }
        }

        DrawTopMenu();
        DrawLeftPanel();
        DrawRightPanel();
        DrawBottom();
    }

    // ----------------------------------------------------
    // Верхнее меню
    // ----------------------------------------------------

    static void DrawTopMenu()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.BackgroundColor = ConsoleColor.DarkBlue;

        WriteAt(5, 0, "Левая");
        WriteAt(17, 0, "Файл");
        WriteAt(28, 0, "Диск");
        WriteAt(40, 0, "Команды");
        WriteAt(55, 0, "Правая");
    }

    // ----------------------------------------------------
    // Левая панель
    // ----------------------------------------------------

    static void DrawLeftPanel()
    {
        DrawBox(0, 3, 39, 21);

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Yellow;

        WriteAt(16, 2, "C:\\NC");

        // Заголовки
        WriteAt(2, 4, "Имя");
        WriteAt(15, 4, "Имя");
        WriteAt(27, 4, "Имя");

        Console.ForegroundColor = ConsoleColor.White;

        // Разделители колонок
        for (int y = 4; y <= 18; y++)
        {
            WriteAt(13, y, "│");
            WriteAt(25, y, "│");
        }

        // Горизонтальная линия
        for (int x = 1; x < 39; x++)
        {
            WriteAt(x, 18, "─");
        }

        DrawLeftFiles();

        // Нижняя информация
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        WriteAt(1, 19, "..");

        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteAt(5, 19, "►КАТАЛОГ◄");

        Console.ForegroundColor = ConsoleColor.White;
        WriteAt(16, 19, "14.09.26");
        WriteAt(26, 19, "18:42");
    }

    // ----------------------------------------------------
    // Правая панель
    // ----------------------------------------------------

    static void DrawRightPanel()
    {
        DrawBox(40, 3, 79, 21);

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Yellow;

        WriteAt(57, 2, "C:\\NC");

        // Заголовки
        WriteAt(42, 4, "Имя");
        WriteAt(56, 4, "Размер");
        WriteAt(65, 4, "Дата");
        WriteAt(74, 4, "Время");

        Console.ForegroundColor = ConsoleColor.White;

        // Разделители
        for (int y = 4; y <= 18; y++)
        {
            WriteAt(55, y, "│");
            WriteAt(64, y, "│");
            WriteAt(73, y, "│");
        }

        // Горизонтальная линия
        for (int x = 41; x < 79; x++)
        {
            WriteAt(x, 18, "─");
        }

        DrawRightFiles();

        // Нижняя информация
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        WriteAt(41, 19, "..");

        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteAt(53, 19, "►КАТАЛОГ◄");

        Console.ForegroundColor = ConsoleColor.White;
        WriteAt(65, 19, "14.09.26");
        WriteAt(74, 19, "18:42");
    }

    // ----------------------------------------------------
    // Файлы левой панели
    // ----------------------------------------------------

    static void DrawLeftFiles()
    {
        List<FileItem> sorted = files
            .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Делим список на 3 примерно равные части.
        // Поэтому все три колонки будут заполнены.
        int total = Math.Min(sorted.Count, 39);
        int rowsPerColumn = (int)Math.Ceiling(total / 3.0);

        for (int i = 0; i < total; i++)
        {
            int column = i / rowsPerColumn;
            int row = i % rowsPerColumn;

            // Координаты начала трёх колонок
            int x;

            if (column == 0)
                x = 1;
            else if (column == 1)
                x = 14;
            else
                x = 26;

            int y = 5 + row;

            // Не выходим за границу панели
            if (y > 17)
                continue;

            FileItem file = sorted[i];

            string baseName;
            string extension = "";

            if (file.IsDirectory || file.Name == "..")
            {
                baseName = ShortenName(file.Name, 7);
            }
            else
            {
                int dot = file.Name.LastIndexOf('.');

                if (dot > 0)
                {
                    baseName = ShortenName(
                        file.Name.Substring(0, dot), 7);

                    extension = file.Name.Substring(dot + 1);

                    if (extension.Length > 3)
                        extension = extension.Substring(0, 3);
                }
                else
                {
                    baseName = ShortenName(file.Name, 7);
                }
            }

            // Каталоги отображаются другим цветом
            if (file.IsDirectory)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else
                Console.ForegroundColor = ConsoleColor.White;

            string text =
                baseName.PadRight(7) +
                " " +
                extension.PadRight(3);

            WriteAt(x, y, text);
        }
    }

    // ----------------------------------------------------
    // Файлы правой панели
    // ----------------------------------------------------

    static void DrawRightFiles()
    {
        List<FileItem> sorted = files
            .Where(f => !f.IsDirectory)
            .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Выделенная строка
        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.ForegroundColor = ConsoleColor.Black;

        for (int x = 41; x < 79; x++)
        {
            WriteAt(x, 5, " ");
        }

        WriteAt(41, 5, "►КАТАЛОГ◄");
        WriteAt(65, 5, "14.09.26");
        WriteAt(74, 5, "18:42");

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        int maxRows = 12;

        for (int i = 0; i < sorted.Count && i < maxRows; i++)
        {
            FileItem file = sorted[i];

            int y = 6 + i;

            string name = ShortenName(file.Name, 14);
            string size = file.Size.ToString().PadLeft(8);
            string date = file.Modified.ToString("dd.MM.yy");
            string time = file.Modified.ToString("HH:mm");

            WriteAt(41, y, name.PadRight(14));
            WriteAt(56, y, size);
            WriteAt(65, y, date);
            WriteAt(74, y, time);
        }
    }

    // ----------------------------------------------------
    // Нижняя часть интерфейса
    // ----------------------------------------------------

    static void DrawBottom()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        // Горизонтальная линия
        for (int x = 0; x < WIDTH; x++)
        {
            WriteAt(x, 22, "─");
        }

        // Командная строка
        WriteAt(1, 23, "C:\\NC>");

        string[] buttons =
        {
            "1Помощь",
            "2Вызов",
            "3Чтение",
            "4Правка",
            "5Копия",
            "6Новая",
            "7НовКат",
            "8Удал-е",
            "9Меню",
            "10Выход"
        };

        int[] positions =
        {
            0, 8, 16, 24, 32,
            40, 48, 56, 64, 72
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;

            WriteAt(
                positions[i],
                24,
                buttons[i].PadRight(8)
            );
        }

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
    }

    // ----------------------------------------------------
    // Рамка
    // ----------------------------------------------------

    static void DrawBox(int left, int top, int right, int bottom)
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        // Верхняя граница
        WriteAt(
            left,
            top,
            "┌" + new string('─', right - left - 1) + "┐"
        );

        // Боковые границы
        for (int y = top + 1; y < bottom; y++)
        {
            WriteAt(left, y, "│");
            WriteAt(right, y, "│");
        }

        // Нижняя граница
        WriteAt(
            left,
            bottom,
            "└" + new string('─', right - left - 1) + "┘"
        );
    }

    // ----------------------------------------------------
    // Сокращение длинных имён
    // ----------------------------------------------------

    static string ShortenName(string name, int maxLength)
    {
        if (name.Length <= maxLength)
            return name;

        return name.Substring(0, maxLength - 1) + "~";
    }

    // ----------------------------------------------------
    // Вывод текста
    // ----------------------------------------------------

    static void WriteAt(int x, int y, string text)
    {
        if (x >= 0 &&
            y >= 0 &&
            x < WIDTH &&
            y < HEIGHT)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }
    }
}