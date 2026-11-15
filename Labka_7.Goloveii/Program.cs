using System;
using System.Text;

namespace Labka_7.Goloveii
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running) // Вихід тільки через running = false
            {
                Console.WriteLine("=== Піцерія ===");
                Console.WriteLine("1 — довідка");
                Console.WriteLine("2 — вартість піци залежно від діаметра");
                Console.WriteLine("3 — продажі за днями тижня");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");

                string choice = (Console.ReadLine() ?? "").Trim(); // Рядок, щоб не падати на літерах
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowSalesTable(); Pause(); break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
        }

        // Пауза й очищення екрана після дії
        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // Пункт 1: тема, сутність і поля
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

        // Пункт 2: табулювання, ціна = 0,12 грн за см² площі + 40 грн за роботу
        static void ShowCostTable()
        {
            double from = 20, to = 45, step = 2.5;

            Console.WriteLine($"{"Діаметр, см",12}{"Площа, см²",14}{"Ціна, грн",12}");
            for (double d = from; d <= to + 1e-9; d += step) // 1e-9 — запас для дробового кроку
            {
                double area = Math.PI * d * d / 4;
                double price = 0.12 * area + 40;
                Console.WriteLine($"{d,12:F1}{area,14:F2}{price,12:F2}");
            }
        }

        // Пункт 3: таблиця на два входи з підсумками
        static void ShowSalesTable()
        {
            string[] pizzas = { "Маргарита", "Пепероні", "Чотири сири", "Гавайська", "Диявола" };
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };

            if (!TryReadInt($"Кількість рядків (позицій), 1–{pizzas.Length}: ", pizzas.Length, out int rows)) return;
            if (!TryReadInt($"Кількість стовпців (днів), 1–{days.Length}: ", days.Length, out int cols)) return;

            int[] colSums = new int[cols]; // Підсумки стовпців
            int grand = 0;                 // Загальний підсумок

            // Шапка
            Console.Write($"{"",-14}");
            for (int c = 0; c < cols; c++) Console.Write($"{days[c],6}");
            Console.WriteLine($"{"Разом",8}");

            for (int r = 0; r < rows; r++)
            {
                int rowSum = 0; // Свій для кожного рядка
                Console.Write($"{pizzas[r],-14}");
                for (int c = 0; c < cols; c++)
                {
                    int sold = 5 + (r * 3 + c * 2) % 7; // Умовні дані, поки немає сховища
                    Console.Write($"{sold,6}");
                    rowSum += sold;
                    colSums[c] += sold;
                }
                Console.WriteLine($"{rowSum,8}"); // WriteLine після внутрішнього циклу
                grand += rowSum;
            }

            // Рядок підсумків
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
    }
}