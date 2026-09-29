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

    const string TOP_LEFT = "\u2554";          // ╔
    const string TOP_RIGHT = "\u2557";         // ╗
    const string BOTTOM_LEFT = "\u255A";       // ╚
    const string BOTTOM_RIGHT = "\u255D";      // ╝
    const string DOUBLE_HORIZONTAL = "\u2550"; // ═
    const string DOUBLE_VERTICAL = "\u2551";   // ║

    const string JOIN_LEFT = "\u255F";         // ╟
    const string JOIN_RIGHT = "\u2562";        // ╢

    const string HORIZONTAL = "\u2500";        // ─
    const string VERTICAL = "\u2502";          // │

    const string DOWN_ARROW = "\u2193";        // ↓
    const string RIGHT_ARROW = "\u25BA";       // ►
    const string LEFT_ARROW = "\u25C4";        // ◄

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
            new DateTime(2026, 9, 13, 21, 8, 0)),

        new FileItem("calculator_data.dat", 16384,
            new DateTime(2026, 9, 15, 10, 22, 0)),

        new FileItem("database.db", 45056,
            new DateTime(2026, 9, 15, 12, 41, 0)),

        new FileItem("design.png", 76432,
            new DateTime(2026, 9, 16, 14, 18, 0)),

        new FileItem("engine.dll", 196608,
            new DateTime(2026, 9, 17, 9, 35, 0)),

        new FileItem("installer.exe", 326784,
            new DateTime(2026, 9, 18, 16, 27, 0)),

        new FileItem("journal.txt", 5832,
            new DateTime(2026, 9, 19, 11, 14, 0)),

        new FileItem("kernel.sys", 87296,
            new DateTime(2026, 9, 20, 18, 43, 0)),

        new FileItem("library.zip", 628736,
            new DateTime(2026, 9, 21, 13, 52, 0)),

        new FileItem("manual.pdf", 245760,
            new DateTime(2026, 9, 22, 15, 38, 0)),

        new FileItem("project_backup.zip", 2097152,
            new DateTime(2026, 9, 23, 20, 11, 0)),

        new FileItem("summary.docx", 62418,
            new DateTime(2026, 9, 24, 17, 29, 0)),

        new FileItem("tools.exe", 57344,
            new DateTime(2026, 9, 25, 8, 47, 0)),

        new FileItem("workspace.cfg", 2946,
            new DateTime(2026, 9, 26, 19, 16, 0))
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
            // Некоторые терминалы не позволяют изменить размер.
        }

        Console.CursorVisible = false;

        DrawInterface();

        Console.ReadKey();

        Console.ResetColor();
        Console.Clear();
    }

    // Основной интерфейс

    static void DrawInterface()
    {
        Console.Clear();

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

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

    // Верхнее меню

    static void DrawTopMenu()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;

        DrawMenuWord(4, 0, "Левая");
        DrawMenuWord(17, 0, "Файл");
        DrawMenuWord(28, 0, "Диск");
        DrawMenuWord(40, 0, "Команды");
        DrawMenuWord(55, 0, "Правая");

        Console.ForegroundColor = ConsoleColor.Cyan;

        for (int x = 0; x < WIDTH; x++)
        {
            WriteAt(x, 1, DOUBLE_HORIZONTAL);
        }
    }

    static void DrawMenuWord(int x, int y, string word)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        WriteAt(x, y, word[0].ToString());

        Console.ForegroundColor = ConsoleColor.Cyan;

        for (int i = 1; i < word.Length; i++)
        {
            WriteAt(x + i, y, word[i].ToString());
        }
    }

    // Левая панель

    static void DrawLeftPanel()
    {
        int left = 0;
        int right = 39;
        int top = 2;
        int bottom = 22;

        DrawDoubleBox(left, top, right, bottom);

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Yellow;

        WriteAt(16, 2, "C:\\NC");

        WriteAt(2, 4, "C:" + DOWN_ARROW);
        WriteAt(7, 4, "Имя");
        WriteAt(19, 4, "Имя");
        WriteAt(31, 4, "Имя");

        Console.ForegroundColor = ConsoleColor.White;

        for (int y = 4; y <= 19; y++)
        {
            WriteAt(13, y, VERTICAL);
            WriteAt(26, y, VERTICAL);
        }

        WriteAt(0, 20, JOIN_LEFT);

        for (int x = 1; x < 39; x++)
        {
            WriteAt(x, 20, HORIZONTAL);
        }

        WriteAt(39, 20, JOIN_RIGHT);

        DrawLeftFiles();

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        WriteAt(1, 21, "..");

        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteAt(5, 21, RIGHT_ARROW + "КАТАЛОГ" + LEFT_ARROW);

        Console.ForegroundColor = ConsoleColor.White;
        WriteAt(17, 21, "14.09.26");
        WriteAt(27, 21, "18:42");
    }

    // Правая панель

    static void DrawRightPanel()
    {
        int left = 40;
        int right = 79;
        int top = 2;
        int bottom = 22;

        DrawDoubleBox(left, top, right, bottom);

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Yellow;

        WriteAt(56, 2, "C:\\NC");

        Console.ForegroundColor = ConsoleColor.Yellow;

        WriteAt(42, 4, "C:" + DOWN_ARROW);
        WriteAt(47, 4, "Имя");
        WriteAt(56, 4, "Размер");
        WriteAt(66, 4, "Дата");
        WriteAt(74, 4, "Время");

        Console.ForegroundColor = ConsoleColor.White;

        for (int y = 4; y <= 19; y++)
        {
            WriteAt(54, y, VERTICAL);
            WriteAt(64, y, VERTICAL);
            WriteAt(73, y, VERTICAL);
        }

        WriteAt(40, 20, JOIN_LEFT);

        for (int x = 41; x < 79; x++)
        {
            WriteAt(x, 20, HORIZONTAL);
        }

        WriteAt(79, 20, JOIN_RIGHT);

        DrawRightFiles();

        // Нижняя строка панели
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        WriteAt(41, 21, "..");

        Console.ForegroundColor = ConsoleColor.Cyan;
        WriteAt(53, 21, RIGHT_ARROW + "КАТАЛОГ" + LEFT_ARROW);

        Console.ForegroundColor = ConsoleColor.White;
        WriteAt(65, 21, "14.09.26");
        WriteAt(74, 21, "18:42");
    }
    // Файлы левой панели

    static void DrawLeftFiles()
    {
        List<FileItem> sorted = files
            .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        int count = Math.Min(sorted.Count, 39);

        int rows = (int)Math.Ceiling(count / 3.0);

        for (int i = 0; i < count; i++)
        {
            int column = i / rows;
            int row = i % rows;

            int x;

            if (column == 0)
                x = 1;
            else if (column == 1)
                x = 14;
            else
                x = 27;

            int y = 5 + row;

            if (y > 19)
                continue;

            FileItem file = sorted[i];

            string name;
            string extension = "";

            if (file.IsDirectory)
            {
                name = ShortenName(file.Name, 7);
            }
            else
            {
                int dot = file.Name.LastIndexOf('.');

                if (dot > 0)
                {
                    name = ShortenName(
                        file.Name.Substring(0, dot),
                        7
                    );

                    extension = file.Name.Substring(dot + 1);

                    if (extension.Length > 3)
                        extension = extension.Substring(0, 3);
                }
                else
                {
                    name = ShortenName(file.Name, 7);
                }
            }

            if (file.IsDirectory)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else
                Console.ForegroundColor = ConsoleColor.White;

            string text =
                name.PadRight(7) +
                " " +
                extension.PadRight(3);

            WriteAt(x, y, text);
        }
    }

    // Файлы правой панели

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

        WriteAt(
            41,
            5,
            RIGHT_ARROW + "КАТАЛОГ" + LEFT_ARROW
        );

        WriteAt(65, 5, "14.09.26");

        WriteAt(74, 5, "18:48");

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        int maxRows = 14;

        for (int i = 0; i < sorted.Count && i < maxRows; i++)
        {
            FileItem file = sorted[i];

            int y = 6 + i;

            string name = ShortenName(file.Name, 13);
            string size = file.Size.ToString();
            string date = file.Modified.ToString("dd.MM.yy");
            string time = file.Modified.ToString("HH:mm");

            WriteAt(41, y, name.PadRight(13));

            WriteAt(55, y, size.PadLeft(9));

            WriteAt(65, y, date);

             WriteAt(74, y, time);
        }

        Console.ForegroundColor = ConsoleColor.White;
        Console.BackgroundColor = ConsoleColor.DarkBlue;

        for (int y = 5; y <= 19; y++)
        {
            WriteAt(54, y, VERTICAL);
            WriteAt(64, y, VERTICAL);
            WriteAt(73, y, VERTICAL);
        }

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;
    }

   // Нижняя часть

    static void DrawBottom()
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        for (int x = 0; x < WIDTH; x++)
        {
            WriteAt(x, 23, DOUBLE_HORIZONTAL);
        }

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

    static void DrawDoubleBox(
        int left,
        int top,
        int right,
        int bottom)
    {
        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.Cyan;

        // Верхняя граница
        WriteAt(
            left,
            top,
            TOP_LEFT +
            new string(
                DOUBLE_HORIZONTAL[0],
                right - left - 1
            ) +
            TOP_RIGHT
        );

        // Боковые границы
        for (int y = top + 1; y < bottom; y++)
        {
            WriteAt(left, y, DOUBLE_VERTICAL);
            WriteAt(right, y, DOUBLE_VERTICAL);
        }

        // Нижняя граница
        WriteAt(
            left,
            bottom,
            BOTTOM_LEFT +
            new string(
                DOUBLE_HORIZONTAL[0],
                right - left - 1
            ) +
            BOTTOM_RIGHT
        );
    }


    static string ShortenName(string name, int maxLength)
    {
        if (name.Length <= maxLength)
            return name;

        return name.Substring(0, maxLength - 1) + "~";
    }

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
