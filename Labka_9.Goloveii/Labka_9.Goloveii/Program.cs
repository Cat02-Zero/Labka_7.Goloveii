using System;
using System.Globalization;
using System.Text;

namespace Labka_9.Goloveii
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            double[] prices = new double[100]; // ціни позицій меню
            int count = 0;                     // скільки елементів заповнено
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
                    case "4": AddValue(prices, ref count, ReadDouble("Ціна позиції, грн (0–10000): ", 0, 10000), prices.Length); PrintCount(count); Pause(); break;
                    case "5": PrintValues(prices, count); Pause(); break;
                    case "6": RemoveAt(prices, ref count, ReadInt("Номер позиції для видалення: ", 1, prices.Length)); Pause(); break;
                    case "7": ShowStatistics(prices, count); Pause(); break;
                    case "8": ShowMax(prices, count); Pause(); break;
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
            Console.WriteLine("4 — додати ціну позиції");
            Console.WriteLine("5 — показати всі ціни");
            Console.WriteLine("6 — видалити за номером");
            Console.WriteLine("7 — статистика");
            Console.WriteLine("8 — найдорожча позиція");
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

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        // ===== Робота з масивом =====
        // Масив іде без ref (він посилальний), лічильник — з ref (int треба змінити в Main)
        static void AddValue(double[] values, ref int count, double newValue, int capacity)
        {
            if (count == capacity)
            {
                Console.WriteLine("Місця більше немає.");
                return;
            }
            values[count] = newValue;
            count++;
        }

        // Видаляє елемент за номером (з 1), зсуває решту вліво, зменшує лічильник
        static void RemoveAt(double[] values, ref int count, int index)
        {
            if (index < 1 || index > count)
            {
                Console.WriteLine("Немає позиції з таким номером.");
                return;
            }
            for (int i = index - 1; i < count - 1; i++)
            {
                values[i] = values[i + 1];
            }
            count--;
            values[count] = 0;
            Console.WriteLine($"Позицію №{index} видалено.");
        }

        // Таблиця з номерами до count; нічого не питає з клавіатури
        static void PrintValues(double[] values, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список порожній.");
                return;
            }
            Console.WriteLine($"{"№",4}{"Ціна, грн",14}");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1,4}{values[i],14:F2}");
            }
        }

        static void PrintCount(int count)
        {
            Console.WriteLine($"Записів тепер: {count}");
        }

        // ===== Статистика: два результати через out =====
        static void Statistics(double[] values, int count, out double sum, out double average)
        {
            sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += values[i];
            }
            average = count > 0 ? sum / count : 0;
        }

        static void ShowStatistics(double[] values, int count)
        {
            Statistics(values, count, out double sum, out double average);
            Console.WriteLine($"Кількість: {count}");
            Console.WriteLine($"Сума: {sum:F2} грн");
            Console.WriteLine($"Середня ціна: {average:F2} грн");
        }

        // Найбільше значення та його номер; порожній набір дає number = 0
        static void FindMax(double[] values, int count, out double max, out int number)
        {
            max = 0;
            number = 0;
            if (count == 0) return;

            max = values[0];
            number = 1;
            for (int i = 1; i < count; i++)
            {
                if (values[i] > max)
                {
                    max = values[i];
                    number = i + 1;
                }
            }
        }

        static void ShowMax(double[] values, int count)
        {
            FindMax(values, count, out double max, out int number);
            if (number == 0)
            {
                Console.WriteLine("Немає значень.");
                return;
            }
            Console.WriteLine($"Найбільша ціна: {max:F2} грн (позиція №{number}).");
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

        // ===== Обчислення =====
        static double CalculateArea(double diameter)
        {
            return Math.PI * diameter * diameter / 4;
        }

        static double CalculatePrice(double diameter)
        {
            return 0.12 * CalculateArea(diameter) + 40;
        }

        static int GetSold(int row, int col)
        {
            return 5 + (row * 3 + col * 2) % 7;
        }

        // ===== Пункт 2: таблиця вартості =====
        static void ShowCostTable()
        {
            double from = ReadDouble("Початковий діаметр, см (1–100): ", 1, 100);
            double to = ReadDouble($"Кінцевий діаметр, см ({from}–100): ", from, 100);
            double step = ReadDouble("Крок, см (0,1–100): ", 0.1, 100);
            Console.WriteLine();
            PrintCostTable(from, to, step);
        }

        static void PrintCostTable(double from, double to, double step)
        {
            Console.WriteLine($"{"Діаметр, см",12}{"Площа, см²",14}{"Ціна, грн",12}");
            for (double d = from; d <= to + 1e-9; d += step)
            {
                Console.WriteLine($"{d,12:F1}{CalculateArea(d),14:F2}{CalculatePrice(d),12:F2}");
            }
        }

        // ===== Пункт 3: продажі за днями тижня =====
        static void ShowSalesTable()
        {
            string[] pizzas = { "Маргарита", "Пепероні", "Чотири сири", "Гавайська", "Диявола" };
            string[] days = { "Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд" };

            int rows = ReadInt($"Кількість рядків (позицій), 1–{pizzas.Length}: ", 1, pizzas.Length);
            int cols = ReadInt($"Кількість стовпців (днів), 1–{days.Length}: ", 1, days.Length);
            Console.WriteLine();
            PrintSalesTable(pizzas, days, rows, cols);
        }

        static void PrintSalesTable(string[] pizzas, string[] days, int rows, int cols)
        {
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
                    int sold = GetSold(r, c);
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

        // ===== Безпечне введення =====
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