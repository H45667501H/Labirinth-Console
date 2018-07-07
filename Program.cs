using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labirinth_Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //Запрос данных
            Console.WriteLine("Укажите ширину лабиринта:");
            int labirinthWidth = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Укажите длину лабиринта:");
            int labirinthLength = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Укажите сложность лабиринта (0 - 100):");
            int labirinthDifficulty = Convert.ToInt32(Console.ReadLine());

            //Инициализация пространства координат
            int[,] map = new int[labirinthWidth, labirinthLength];
            int[,] mapOfDistance = new int[labirinthWidth, labirinthLength];

            //Генерация мира

            //Параметры для одной клетки
            Random cell = new Random();
            int cellWallChance;

            //Генерация карты
            for (int i = 0; i < labirinthWidth; i++)
            {
                for (int j = 0; j < labirinthLength; j++)
                {
                    if (i == 0 | j == 0 | i == labirinthWidth - 1 | j == labirinthLength - 1) map[i, j] = 0;
                    else
                    {
                        cellWallChance = Convert.ToInt32(cell.Next(0, 100));

                        if (i == 1 & j == 1) map[1, 1] = 2;
                        else
                        {
                            if (i == labirinthWidth - 2 & j == labirinthLength - 2) map[labirinthWidth - 2, labirinthLength - 2] = 1;
                            else
                            {
                                if (cellWallChance < labirinthDifficulty) map[i, j] = 0;
                                else map[i, j] = 1;
                            }
                        }
                    }
                }
            }

            //Генерация карты расстояний до выхода

            //Точка выхода
            int exit = 0;

            //Генерация карты расстояний
            for (int i = labirinthWidth - 1; i >= 0; i--)
            {
                for (int j = labirinthLength - 1; j >= 0; j--)
                {
                    mapOfDistance[i, j] = exit;
                    exit++;
                }
                exit = labirinthWidth - i;
            }

            ////Визуализация карты расстояний до контрольной точки
            //for (int i = 0; i < labirinthWidth; i++)
            //{
            //    for (int j = 0; j < labirinthLength; j++)
            //    {
            //        Console.Write(mapOfDistance[i, j]);
            //        Console.Write(" ");
            //        if (j == labirinthLength - 1) Console.WriteLine();
            //    }
            //}

            //Алгоритм построения пути от старта до выхода

            //Начальное расстояние до выхода
            int cellId = (labirinthWidth - 2) + (labirinthLength - 2);

            //Координаты текущего положения
            int X = 1;
            int Y = 1;

            //Параметры сторон относительно текущего положения
            int[] sides = new int[4];
            int freeSideAmount;

            //Журналы координат пути до выхода
            Stack<int> coordWayX = new Stack<int>();
            Stack<int> coordWayY = new Stack<int>();

            //Запись начальных координат в журнал
            coordWayX.Push(X);
            coordWayY.Push(Y);

            //Порядковый номер первой свободной стороны
            int numberFirstFreeSide = 0;

            //Построение пути
            do
            {
                freeSideAmount = 0;

                //Анализ сторон относительно текущего положения
                sides[0] = map[X, Y + 1];
                sides[1] = map[X + 1, Y];
                sides[2] = map[X, Y - 1];
                sides[3] = map[X - 1, Y];

                //Вычисление количества свободных сторон
                for (int i = 0; i <= 3; i++) if (sides[i] == 1) freeSideAmount++;

                switch (freeSideAmount)
                {
                    case 0:
                        map[X, Y] = 3;

                        X = coordWayX.Pop();
                        Y = coordWayY.Pop();

                        break;
                    case 1:
                        for (int i = 0; i <= 3; i++)
                        {
                            if (sides[i] == 1)
                            {
                                map[X, Y] = 2;

                                coordWayX.Push(X);
                                coordWayY.Push(Y);

                                switch (i)
                                {
                                    case 0: Y++; break;
                                    case 1: X++; break;
                                    case 2: Y--; break;
                                    case 3: X--; break;
                                }
                            }
                        }

                        break;
                    default:
                        for (int i = 3; i >= 0; i--)
                        {
                            if (sides[i] == 1)
                            {
                                map[X, Y] = 2;

                                numberFirstFreeSide = i;

                                coordWayX.Push(X);
                                coordWayY.Push(Y);
                            }
                        }

                        switch (numberFirstFreeSide)
                        {
                            case 0: Y++; break;
                            case 1: X++; break;
                            case 2: Y--; break;
                            case 3: X--; break;
                        }

                        break;
                }

                //Текущее расстояние до выхода
                cellId = mapOfDistance[X, Y];

                //Визуализация мира
                for (int i = 0; i < labirinthWidth; i++)
                {
                    for (int j = 0; j < labirinthLength; j++)
                    {
                        if (i == labirinthWidth - 2 & j == labirinthLength - 2) Console.Write("╬");
                        else
                        {
                            if (map[i, j] == 0) Console.Write("█");
                            if (map[i, j] == 1) Console.Write(" ");
                            if (map[i, j] == 2) Console.Write("X");
                            if (map[i, j] == 3) Console.Write("░");
                            if (j == labirinthLength - 1) Console.WriteLine();
                        }
                    }
                }

                //Проверка на наличие координат в журнале
                if (coordWayX.Count == 0)
                {
                    Console.WriteLine("Выхода нет");
                    break;
                }

            } while (cellId != 2);

            Console.ReadLine();
        }
    }
}
