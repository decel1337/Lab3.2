using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab3_2
{
    internal static class Program
    {
        private static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Student s1 = new Student("KB-003", "Коваленко Олена Петрівна", 2004, "ІПЗ-21", 2);
            Student s2 = new Student("KB-001", "Бойко Андрій Ігорович", 2003, "ІПЗ-31", 3);
            Student s3 = new Student("KB-002", "Шевченко Марія Олегівна", 2005, "ІПЗ-11", 1);

            Console.WriteLine("=== Методи класу Student ===");
            s1.PrintInfo();
            Console.WriteLine($"Вік: {s1.GetAge()}");
            s1.PromoteToNextCourse();
            Console.WriteLine($"Після переведення: курс {s1.Course}");

            Task2(s1, s2, s3);
            Task3And4(s1, s2, s3);
        }

        // колекції та масив
        private static void Task2(Student s1, Student s2, Student s3)
        {
            Console.WriteLine("\n=== ArrayList (non-generic) ===");
            ArrayList arrayList = new ArrayList();
            arrayList.Add(s1); arrayList.Add(s2); arrayList.Add(s3);   // додавання
            foreach (object o in arrayList)                              // прохід
                ((Student)o).PrintInfo();                                // потрібне приведення типу
            arrayList.Remove(s2);                                        // видалення
            ((Student)arrayList[0]).FullName = "Коваленко Олена Андріївна"; // оновлення
            int idx = arrayList.IndexOf(s3);                             // пошук
            Console.WriteLine($"s3 на позиції {idx}, елементів: {arrayList.Count}");
            // arrayList.Add(42)

            Console.WriteLine("\n=== List<Student> (generic) ===");
            List<Student> list = new List<Student> { s1, s2, s3 };
            foreach (Student s in list) s.PrintInfo();                   // без приведення типів
            list.Remove(s2);
            list[0].GroupNumber = "ІПЗ-31";
            Student found = list.Find(s => s.TicketNumber == "KB-002");
            Console.WriteLine($"Знайдено: {found?.FullName}, елементів: {list.Count}");
            list.Add(s2);

            Console.WriteLine("\n=== Масив Student[] ===");
            Student[] array = { s1, s2, s3 };
            foreach (Student s in array) s.PrintInfo();
            array[1] = null;                                             // «видалення» = обнулення
            array[0].BirthYear = 2004;                                   // оновлення
            int pos = Array.IndexOf(array, s3);                          // пошук
            Console.WriteLine($"s3 на позиції {pos}");
            Array.Resize(ref array, 4);                                  // розмір фіксований: новий масив
            array[3] = s2;
            Console.WriteLine($"Довжина масиву: {array.Length}");

            Console.WriteLine("\nВисновок: масив має фіксований розмір; ArrayList динамічний, " +
                              "але не типобезпечний (object, boxing); List<T> динамічний і типобезпечний.");
        }

        // дерево
        private static void Task3And4(Student s1, Student s2, Student s3)
        {
            Console.WriteLine("\n=== BinaryTree<Student> (IComparable<T>, за квитком) ===");
            BinaryTree<Student> tree = new BinaryTree<Student>();
            tree.Add(s1); tree.Add(s2); tree.Add(s3);
            Console.WriteLine($"Елементів: {tree.Count}, містить s2: {tree.Contains(s2)}");

            BinaryTree<string> strTree = new BinaryTree<string>();      // узагальненість
            strTree.Add("m"); strTree.Add("c"); strTree.Add("x");
            Console.WriteLine("Дерево рядків (preorder): " + string.Join(", ", strTree));
            // BinaryTree<int> не скомпілюється: where T : class

            Console.WriteLine("\n=== Прямий обхід (preorder), власний IEnumerator ===");
            foreach (Student s in tree) s.PrintInfo();

            Console.WriteLine("\n--- Явний IEnumerator<T> ---");
            using (IEnumerator<Student> e = tree.GetEnumerator())
                while (e.MoveNext()) Console.WriteLine(e.Current.FullName);

            Console.WriteLine("\n--- Дерево з IComparer<T>: за ПІБ ---");
            BinaryTree<Student> byName = new BinaryTree<Student>(new StudentByNameComparer());
            foreach (Student s in new[] { s1, s2, s3 }) byName.Add(s);
            foreach (Student s in byName) Console.WriteLine(s.FullName);

            Console.WriteLine("\n--- Дерево з IComparer<T>: за роком народження ---");
            BinaryTree<Student> byYear = new BinaryTree<Student>(new StudentByBirthYearComparer());
            foreach (Student s in new[] { s1, s2, s3 }) byYear.Add(s);
            foreach (Student s in byYear) Console.WriteLine($"{s.BirthYear} {s.FullName}");

            Console.WriteLine("\n--- Сортування через інтерфейси порівняння ---");
            List<Student> list = new List<Student> { s1, s2, s3 };
            list.Sort();                                                 // IComparable<T>
            Console.WriteLine("За квитком: " + string.Join("; ", list.ConvertAll(s => s.TicketNumber)));
            list.Sort(new StudentByNameComparer());                      // IComparer<T>
            Console.WriteLine("За ПІБ: " + string.Join("; ", list.ConvertAll(s => s.FullName)));
            list.Sort(new StudentByBirthYearComparer());
            Console.WriteLine("За роком: " + string.Join("; ", list.ConvertAll(s => s.BirthYear.ToString())));

            Console.WriteLine("\n--- Видалення з дерева ---");
            tree.Remove(s2);
            foreach (Student s in tree) Console.WriteLine(s.TicketNumber);
        }
    }
}