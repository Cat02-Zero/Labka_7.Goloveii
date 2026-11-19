using System;
using System.Globalization;
using System.Text;

namespace Labka_8.Goloveii
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = (Console.ReadLine() ?? "").Trim();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowSalesTable(); Pause(); break;
                    case "0": running = false; ShowGoodbye(); break;
                    default: ShowUnknownChoice(); Pause(); break;
                }
            }
        }

        // ===== Меню =====
        static void ShowMenu()
        {
            Console.WriteLine("=== Піцерія ===");
            Console.WriteLine("1 — довідка");
            Console.WriteLine("2 — вартість піци залежно від діаметра");
            Console.WriteLine("3 — продажі за днями тижня");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static void ShowGoodbye()
        {
            Console.WriteLine("До побачення.");
        }

        static void ShowUnknownChoice()
        {
            Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
        }

        // Пауза й очищення екрана після дії
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // ===== Довідка =====
        static void ShowAbout()
        {
            Console.WriteLine("Тема: Піцерія (облік позицій меню).");
            Console.WriteLine("Сутності: Користувач, Позиція меню, Розмір (довідник).");
            Console.WriteLine("Поля сутності «Позиція меню»:");
            Console.WriteLine("  код              — int");
            Console.WriteLine("  назва            — string");
            Console.WriteLine("  код розміру      — int (з довідника «Розмір»)");
            Console.WriteLine("  ціна             — double");
            Console.WriteLine("  час приготування — int, хвилин");
            Console.WriteLine("  склад            — string");
            Console.WriteLine("  вегетаріанська   — bool");
            Console.WriteLine("Підсумкова величина: середній чек і найпопулярніші позиції.");
        }

        // ===== Обчислення: нічого не виводять, лише повертають =====
        // Площа піци за діаметром, см²
        static double CalculateArea(double diameter)
        {
            return Math.PI * diameter * diameter / 4;
        }

        // Ціна = 0,12 грн за см² площі + 40 грн за роботу
        static double CalculatePrice(double diameter)
        {
            return 0.12 * CalculateArea(diameter) + 40;
        }

        // ===== Пункт 2: таблиця вартості =====
        // Межі й крок вводить користувач
        static void ShowCostTable()
        {
            double from = ReadDouble("Початковий діаметр, см (1–100): ", 1, 100);
            double to = ReadDouble($"Кінцевий діаметр, см ({from}–100): ", from, 100);
            double step = ReadDouble("Крок, см (0,1–100): ", 0.1, 100);
            Console.WriteLine();
            PrintCostTable(from, to, step);
        }

        // Табулювання: початок, кінець, крок
        static void PrintCostTable(double from, double to, double step)
        {
            Console.WriteLine($"{"Діаметр, см",12}{"Площа, см²",14}{"Ціна, грн",12}");
            for (double d = from; d <= to + 1e-9; d += step) // 1e-9 — запас для дробового кроку
            {
                Console.WriteLine($"{d,12:F1}{CalculateArea(d),14:F2}{CalculatePrice(d),12:F2}");
            }
        }

        // ===== Пункт 3: продажі за днями тижня =====
        static void ShowSalesTable()
        {
            string[] pizzas = { "Маргарита", "Пепероні", "Чотири сири", "Гавайська", "Диявола" };
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };

            if (!TryReadInt($"Кількість рядків (позицій), 1–{pizzas.Length}: ", pizzas.Length, out int rows)) return;
            if (!TryReadInt($"Кількість стовпців (днів), 1–{days.Length}: ", days.Length, out int cols)) return;

            int[] colSums = new int[cols];
            int grand = 0;

            Console.Write($"{"",-14}");
            for (int c = 0; c < cols; c++) Console.Write($"{days[c],6}");
            Console.WriteLine($"{"Разом",8}");

            for (int r = 0; r < rows; r++)
            {
                int rowSum = 0;
                Console.Write($"{pizzas[r],-14}");
                for (int c = 0; c < cols; c++)
                {
                    int sold = 5 + (r * 3 + c * 2) % 7;
                    Console.Write($"{sold,6}");
                    rowSum += sold;
                    colSums[c] += sold;
                }
                Console.WriteLine($"{rowSum,8}");
                grand += rowSum;
            }

            Console.Write($"{"Разом",-14}");
            for (int c = 0; c < cols; c++) Console.Write($"{colSums[c],6}");
            Console.WriteLine($"{grand,8}");
        }

        // Читає число від 1 до max; при помилці виводить повідомлення й повертає false
        static bool TryReadInt(string prompt, int max, out int value)
        {
            Console.Write(prompt);
            if (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.WriteLine("Помилка: потрібне ціле число.");
                return false;
            }
            if (value < 1 || value > max)
            {
                Console.WriteLine($"Помилка: число має бути від 1 до {max}.");
                return false;
            }
            return true;
        }

        // ===== Безпечне введення =====
        // Питає, поки не отримає число в межах [min; max]
        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                string text = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

                if (!double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value))
                {
                    Console.WriteLine("Помилка: потрібне число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }

        // Те саме для цілих чисел
        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (!int.TryParse(Console.ReadLine(), out int value))
                {
                    Console.WriteLine("Помилка: потрібне ціле число.");
                    continue;
                }
                if (value < min || value > max)
                {
                    Console.WriteLine($"Помилка: число має бути від {min} до {max}.");
                    continue;
                }
                return value;
            }
        }
    }
}