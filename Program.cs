using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

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

    public string BaseName
    {
        get
        {
            int dot = Name.LastIndexOf('.');

            if (IsDirectory || dot <= 0)
                return Name;

            return Name.Substring(0, dot);
        }
    }

    public string Extension
    {
        get
        {
            int dot = Name.LastIndexOf('.');

            if (IsDirectory || dot <= 0)
                return "";

            return Name.Substring(dot + 1);
        }
    }
}

class Program
{
    const int WIDTH = 80;
    const int HEIGHT = 25;

    const int FIRST_ROW = 3;
    const int LAST_ROW = 19;
    const int ROWS = LAST_ROW - FIRST_ROW + 1;

    const ConsoleColor PANEL_BG = ConsoleColor.DarkBlue;
    const ConsoleColor TEXT = ConsoleColor.Cyan;
    const ConsoleColor HEADER = ConsoleColor.White;
    const ConsoleColor BAR_BG = ConsoleColor.DarkCyan;

    const string BAR_BG_ANSI = "\u001b[48;2;72;209;204m";
    static bool trueColor;

    const string TOP_LEFT = "\u2554";
    const string TOP_RIGHT = "\u2557";
    const string BOTTOM_LEFT = "\u255A";
    const string BOTTOM_RIGHT = "\u255D";
    const string DOUBLE_HORIZONTAL = "\u2550";
    const string DOUBLE_VERTICAL = "\u2551";

    const string JOIN_LEFT = "\u255F";
    const string JOIN_RIGHT = "\u2562";
    const string TOP_JOIN = "\u2564";
    const string BOTTOM_JOIN = "\u2534";

    const string HORIZONTAL = "\u2500";
    const string VERTICAL = "\u2502";

    const string DOWN_ARROW = "\u2193";
    const string RIGHT_ARROW = "\u25BA";
    const string LEFT_ARROW = "\u25C4";
    const string SHADE = "\u2592";

    static readonly DateTime OLD_DATE = new DateTime(1995, 5, 25, 5, 0, 0);

    static FileItem NewFile(string name, long size)
    {
        return new FileItem(name, size, OLD_DATE);
    }

    static List<FileItem> files = new List<FileItem>
    {
        new FileItem("..", 0, new DateTime(2002, 10, 11, 19, 48, 0), true),

        NewFile("123view.exe", 128380),
        NewFile("4372ansi.set", 255),
        NewFile("8502ansi.set", 255),
        NewFile("8632ansi.set", 255),
        NewFile("8652ansi.set", 255),
        NewFile("8662ansi.set", 255),
        new FileItem("Ajaccgdo_files", 417392, new DateTime(2002, 10, 12, 9, 2, 0)),
        NewFile("ansi2437.set", 255),
        NewFile("ansi2850.set", 255),
        NewFile("ansi2863.set", 255),
        NewFile("ansi2865.set", 255),
        NewFile("ansi2866.set", 255),
        NewFile("arcview.exe", 81738),
        NewFile("bitmap.exe", 54805),
        NewFile("bug.nss", 16133),
        NewFile("bungee.nss", 41914),

        NewFile("nc.cfg", 1024),
        NewFile("nc_exit.com", 357),
        NewFile("telemax.dat", 9216),
        NewFile("nc_exit.doc", 4096),
        NewFile("clp2dib.exe", 40960),
        NewFile("dbview.exe", 62464),
        NewFile("draw2wmf.exe", 59392),
        NewFile("drw2wmf.exe", 57344),
        NewFile("ico2dib.exe", 38912),
        NewFile("msp2dib.exe", 43008),
        NewFile("nc.exe", 241254),
        NewFile("ncclean.exe", 12288),
        NewFile("ncdd.exe", 8192),
        NewFile("ncedit.exe", 78848),
        NewFile("ncff.exe", 25600),
        NewFile("nclabel.exe", 9728),
        NewFile("ncmain.exe", 151552),
        NewFile("ncnet.exe", 47104),
        NewFile("ncsf.exe", 15360),
        NewFile("ncsi.exe", 28672),
        NewFile("nczip.exe", 34816),
        NewFile("packer.exe", 52224),
        NewFile("paraview.exe", 66560),
        NewFile("pct2dib.exe", 41984),
        NewFile("playwave.exe", 22528),
        NewFile("q&aview.exe", 71680),
        NewFile("rbview.exe", 36864),
        NewFile("refview.exe", 49152),
        NewFile("saver.exe", 18432),
        NewFile("telemax.exe", 94208),
        NewFile("tif2dib.exe", 45056),
        NewFile("vector.exe", 53248),
        NewFile("wpb2dib.exe", 39936),
        NewFile("wpv2wmf.exe", 55296),
        NewFile("wpview.exe", 64512),
        NewFile("nc.ext", 2048),
        NewFile("nc.fil", 1536),
        NewFile("ncpscrip.hdr", 3072),
        NewFile("nc.hlp", 188416),
        NewFile("ncff.hlp", 17408),
        NewFile("telemax.hlp", 30720),
        NewFile("nc.ico", 766),
        NewFile("nc.ini", 2560),
        NewFile("ncclean.ini", 512),
        NewFile("norton.ini", 1792),
        NewFile("telemax.ini", 640)
    };

    static FileItem current = files[0];

    [DllImport("kernel32.dll")]
    static extern IntPtr GetStdHandle(int handle);

    [DllImport("kernel32.dll")]
    static extern bool GetConsoleMode(IntPtr handle, out uint mode);

    [DllImport("kernel32.dll")]
    static extern bool SetConsoleMode(IntPtr handle, uint mode);

    static bool EnableAnsi()
    {
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
            return true;

        try
        {
            IntPtr handle = GetStdHandle(-11);
            uint mode;

            if (!GetConsoleMode(handle, out mode))
                return false;

            return SetConsoleMode(handle, mode | 0x0004);
        }
        catch
        {
            return false;
        }
    }

    static void SetBarColors(ConsoleColor foreground)
    {
        Console.ForegroundColor = foreground;

        if (trueColor)
            Console.Write(BAR_BG_ANSI);
        else
            Console.BackgroundColor = BAR_BG;
    }

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        trueColor = EnableAnsi();

        try
        {
            Console.SetWindowSize(
                Math.Min(Console.WindowWidth, WIDTH),
                Math.Min(Console.WindowHeight, HEIGHT));
            Console.SetBufferSize(WIDTH, HEIGHT);
            Console.SetWindowSize(WIDTH, HEIGHT);
        }
        catch
        {
        }

        Console.CursorVisible = false;

        DrawInterface();

        Console.SetCursorPosition(6, 23);
        Console.CursorVisible = true;

        Console.ReadKey(true);

        Console.ResetColor();
        Console.Clear();
    }

    static void DrawInterface()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Clear();

        Console.BackgroundColor = PANEL_BG;

        for (int y = 1; y <= 22; y++)
        {
            WriteAt(0, y, new string(' ', WIDTH));
        }

        DrawTopMenu();
        DrawLeftPanel();
        DrawRightPanel();
        DrawBottom();
    }

    static void DrawTopMenu()
    {
        SetBarColors(ConsoleColor.Black);
        WriteAt(0, 0, new string(' ', WIDTH));

        DrawMenuWord(4, 0, "Левая");
        DrawMenuWord(13, 0, "Файл");
        DrawMenuWord(21, 0, "Диск");
        DrawMenuWord(29, 0, "Команды");
        DrawMenuWord(40, 0, "Правая");

        Console.BackgroundColor = ConsoleColor.Cyan;
        Console.ForegroundColor = ConsoleColor.Black;
        WriteAt(75, 0, " 8 30");
    }

    static void DrawMenuWord(int x, int y, string word)
    {
        SetBarColors(ConsoleColor.Yellow);
        WriteAt(x, y, word.Substring(0, 1));

        SetBarColors(ConsoleColor.Black);
        WriteAt(x + 1, y, word.Substring(1));
    }

    static void DrawLeftPanel()
    {
        DrawPanelFrame(0, 39, false, new[] { 13, 26 });

        Console.BackgroundColor = PANEL_BG;
        Console.ForegroundColor = HEADER;

        WriteAt(1, 2, "C:" + DOWN_ARROW);
        WriteAt(5, 2, "Имя");
        WriteAt(18, 2, "Имя");
        WriteAt(31, 2, "Имя");

        DrawLeftFiles();
        DrawStatusLine(0, current);
    }

    static void DrawRightPanel()
    {
        DrawPanelFrame(40, 79, true, new[] { 53, 63, 72 });

        Console.BackgroundColor = PANEL_BG;
        Console.ForegroundColor = HEADER;

        WriteAt(41, 2, "C:" + DOWN_ARROW);
        WriteAt(45, 2, "Имя");
        WriteAt(56, 2, "Размер");
        WriteAt(66, 2, "Дата");
        WriteAt(73, 2, "Время");

        DrawRightFiles();
        DrawStatusLine(40, current);
    }

    static void DrawPanelFrame(int left, int right, bool active, int[] separators)
    {
        DrawDoubleBox(left, 1, right, 22);

        Console.BackgroundColor = PANEL_BG;
        Console.ForegroundColor = TEXT;

        WriteAt(left, 20, JOIN_LEFT);
        WriteAt(left + 1, 20, new string(HORIZONTAL[0], right - left - 1));
        WriteAt(right, 20, JOIN_RIGHT);

        foreach (int x in separators)
        {
            WriteAt(x, 1, TOP_JOIN);

            for (int y = 2; y <= LAST_ROW; y++)
            {
                WriteAt(x, y, VERTICAL);
            }

            WriteAt(x, 20, BOTTOM_JOIN);
        }

        string title = " C:\\NC ";
        int titleX = left + (right - left + 1 - title.Length) / 2;

        if (active)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        WriteAt(titleX, 1, title);
    }

    static void DrawLeftFiles()
    {
        List<FileItem> sorted = files
            .OrderByDescending(f => f.IsDirectory)
            .ThenBy(f => f.Extension, StringComparer.OrdinalIgnoreCase)
            .ThenBy(f => f.BaseName, StringComparer.OrdinalIgnoreCase)
            .ToList();

        int[] columnX = { 1, 14, 27 };

        Console.BackgroundColor = PANEL_BG;
        Console.ForegroundColor = TEXT;

        for (int i = 0; i < sorted.Count && i < ROWS * columnX.Length; i++)
        {
            int x = columnX[i / ROWS];
            int y = FIRST_ROW + i % ROWS;

            WriteAt(x, y, FormatName(sorted[i]));
        }
    }

    static void DrawRightFiles()
    {
        List<FileItem> sorted = files
            .Where(f => !f.IsDirectory)
            .OrderBy(f => f.Name, StringComparer.OrdinalIgnoreCase)
            .ToList();

        DrawFullRow(FIRST_ROW, current, true);

        for (int i = 0; i < sorted.Count && i < ROWS - 1; i++)
        {
            DrawFullRow(FIRST_ROW + 1 + i, sorted[i], false);
        }
    }

    static void DrawFullRow(int y, FileItem file, bool selected)
    {
        if (selected)
        {
            Console.BackgroundColor = ConsoleColor.Cyan;
            Console.ForegroundColor = ConsoleColor.Black;
        }
        else
        {
            Console.BackgroundColor = PANEL_BG;
            Console.ForegroundColor = TEXT;
        }

        WriteAt(41, y, new string(' ', 38));

        WriteAt(41, y, FormatName(file));

        foreach (int x in new[] { 53, 63, 72 })
        {
            WriteAt(x, y, VERTICAL);
        }

        DrawFileInfo(40, y, file);
    }

    static void DrawStatusLine(int panelX, FileItem file)
    {
        Console.BackgroundColor = PANEL_BG;
        Console.ForegroundColor = TEXT;

        WriteAt(panelX + 1, 21, file.Name);
        DrawFileInfo(panelX, 21, file);
    }

    static void DrawFileInfo(int panelX, int y, FileItem file)
    {
        WriteAt(panelX + 14, y, SizeText(file));
        WriteAt(panelX + 24, y, DateText(file));
        WriteAt(panelX + 34, y, TimeText(file));
    }

    static string SizeText(FileItem file)
    {
        if (file.IsDirectory)
            return RIGHT_ARROW + "КАТАЛОГ" + LEFT_ARROW;

        return file.Size.ToString().PadLeft(9);
    }

    static string DateText(FileItem file)
    {
        return file.Modified.ToString("dd.MM.yy", CultureInfo.InvariantCulture);
    }

    static string TimeText(FileItem file)
    {
        return file.Modified.ToString("H:mm", CultureInfo.InvariantCulture).PadLeft(5);
    }

    static string FormatName(FileItem file)
    {
        string name = file.BaseName;
        string extension = file.Extension;
        string separator = " ";

        if (name.Length > 8 || extension.Length > 3)
            separator = SHADE;

        if (name.Length > 8)
            name = name.Substring(0, 8);

        if (extension.Length > 3)
            extension = extension.Substring(0, 3);

        return name.PadRight(8) + separator + extension.PadRight(3);
    }

    static void DrawBottom()
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Gray;

        WriteAt(0, 23, "C:\\NC>");

        string[] buttons =
        {
            "Помощь",
            "Вызов",
            "Чтение",
            "Правка",
            "Копия",
            "НовИмя",
            "НовКат",
            "Удал-е",
            "Меню",
            "Выход"
        };

        for (int i = 0; i < buttons.Length; i++)
        {
            string number = (i + 1).ToString();

            int x = i * 8;
            int labelX = x + number.Length;

            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            WriteAt(x, 24, number);

            int width = Math.Min(6, WIDTH - 1 - labelX);

            SetBarColors(ConsoleColor.Black);
            WriteAt(labelX, 24, buttons[i].PadRight(width));
        }

        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    static void DrawDoubleBox(
        int left,
        int top,
        int right,
        int bottom)
    {
        Console.BackgroundColor = PANEL_BG;
        Console.ForegroundColor = TEXT;

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

        for (int y = top + 1; y < bottom; y++)
        {
            WriteAt(left, y, DOUBLE_VERTICAL);
            WriteAt(right, y, DOUBLE_VERTICAL);
        }

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
