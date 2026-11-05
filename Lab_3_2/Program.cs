using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BinaryTreeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== 1. МАСИВИ ТА КОЛЕКЦІЇ (Варіант 4) ===");

            Circle[] arraySamples = CreateCircles();
            Circle[] circleArray = arraySamples.Take(5).ToArray();

            Console.WriteLine("\n-- Масив --");
            Array.Resize(ref circleArray, circleArray.Length + 1);
            circleArray[circleArray.Length - 1] = arraySamples[5];
            Console.WriteLine("Додано шосте коло: " + arraySamples[5]);

            circleArray[0].Radius = 5.5;
            Console.WriteLine("Оновлено радіус першого кола до 5.5.");

            Circle? arraySearchResult = Array.Find(circleArray, circle => circle.Radius == 6.0);
            Console.WriteLine(arraySearchResult != null
                ? "Знайдено: " + arraySearchResult
                : "Коло з радіусом 6 не знайдено.");

            int arrayRemoveIndex = Array.IndexOf(circleArray, arraySamples[1]);
            circleArray = RemoveAt(circleArray, arrayRemoveIndex);
            Console.WriteLine("Видалено друге коло. Обхід масиву:");
            foreach (Circle circle in circleArray)
                circle.DisplayInfo();

            Circle[] arrayListSamples = CreateCircles();
            ArrayList arrayList = new ArrayList(arrayListSamples.Take(5).ToArray());
            Console.WriteLine("\n-- Нетипізована колекція ArrayList --");
            arrayList.Add(arrayListSamples[5]);
            Console.WriteLine("Додано шосте коло.");

            ((Circle)arrayList[0]!).Radius = 5.5;
            Console.WriteLine("Оновлено радіус першого кола до 5.5.");

            int arrayListSearchIndex = arrayList.IndexOf(arrayListSamples[5]);
            Console.WriteLine(arrayListSearchIndex >= 0
                ? $"Коло знайдено за індексом {arrayListSearchIndex}."
                : "Коло не знайдено.");

            arrayList.Remove(arrayListSamples[1]);
            Console.WriteLine("Видалено друге коло. Обхід ArrayList:");
            foreach (object item in arrayList)
                ((Circle)item).DisplayInfo();

            Circle[] listSamples = CreateCircles();
            List<Circle> circleList = listSamples.Take(5).ToList();
            Console.WriteLine("\n-- Узагальнена колекція List<Circle> --");
            circleList.Add(listSamples[5]);
            Console.WriteLine("Додано шосте коло.");

            circleList[0].Radius = 5.5;
            Console.WriteLine("Оновлено радіус першого кола до 5.5.");

            Circle? listSearchResult = circleList.Find(circle => circle.Radius == 6.0);
            Console.WriteLine(listSearchResult != null
                ? "Знайдено: " + listSearchResult
                : "Коло з радіусом 6 не знайдено.");

            circleList.Remove(listSamples[1]);
            Console.WriteLine("Видалено друге коло. Обхід List<Circle>:");
            foreach (Circle circle in circleList)
                circle.DisplayInfo();

            circleList.Sort();
            Console.WriteLine("\nСортування List<Circle> за радіусом через IComparable<Circle>:");
            foreach (Circle circle in circleList)
                Console.WriteLine(circle);

            circleList.Sort(new CircleAreaComparer());
            Console.WriteLine("\nСортування List<Circle> за площею через IComparer<Circle>:");
            foreach (Circle circle in circleList)
                Console.WriteLine(circle);

            Console.WriteLine("\nПорівняння структур:");
            Console.WriteLine("- Масив має фіксований розмір; для додавання та видалення його потрібно копіювати або змінювати розмір.");
            Console.WriteLine("- ArrayList змінює розмір, але зберігає елементи як object і потребує приведення типів.");
            Console.WriteLine("- List<Circle> змінює розмір і перевіряє тип елементів під час компіляції.");

            Console.WriteLine("\n=== 2. УЗАГАЛЬНЕНЕ БІНАРНЕ ДЕРЕВО (Preorder) ===");

            Circle[] treeSamples = CreateCircles();
            BinaryTree<Circle> circleTree = new BinaryTree<Circle>();
            foreach (Circle circle in treeSamples)
                circleTree.Add(circle);

            Console.WriteLine("\nОбхід дерева у прямому порядку:");
            foreach (Circle circle in circleTree)
                Console.WriteLine(circle);

            BinaryTree<Circle> treeByArea = new BinaryTree<Circle>(new CircleAreaComparer());
            foreach (Circle circle in treeSamples)
                treeByArea.Add(circle);

            Console.WriteLine("\nОбхід дерева, вузли якого впорядковано за площею:");
            foreach (Circle circle in treeByArea)
                Console.WriteLine(circle);
        }

        private static Circle[] CreateCircles()
        {
            return new[]
            {
                new Circle("Червоний", "Чорний", 5.0),
                new Circle("Синій", "Білий", 3.0),
                new Circle("Зелений", "Жовтий", 7.0),
                new Circle("Жовтий", "Сірий", 2.0),
                new Circle("Фіолетовий", "Чорний", 4.0),
                new Circle("Помаранчевий", "Синій", 6.0)
            };
        }

        private static Circle[] RemoveAt(Circle[] source, int index)
        {
            if (index < 0 || index >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            Circle[] result = new Circle[source.Length - 1];
            Array.Copy(source, 0, result, 0, index);
            Array.Copy(source, index + 1, result, index, source.Length - index - 1);
            return result;
        }
    }
}
