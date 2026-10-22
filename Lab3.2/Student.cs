using System;
using System.Collections.Generic;

namespace Lab3_2
{
    public class Student : IComparable<Student>
    {
        public string TicketNumber { get; set; }
        public string FullName { get; set; }
        public int BirthYear { get; set; }
        public string GroupNumber { get; set; }
        public int Course { get; private set; }

        public Student(string ticketNumber, string fullName, int birthYear, string groupNumber, int course = 1)
        {
            TicketNumber = ticketNumber;
            FullName = fullName;
            BirthYear = birthYear;
            GroupNumber = groupNumber;
            Course = course;
        }

        // Переведення на наступний курс
        public void PromoteToNextCourse()
        {
            if (Course >= 6)
            {
                Console.WriteLine($"{FullName}: вже на останньому курсі.");
                return;
            }
            Course++;
        }

        // Обчислення віку
        public int GetAge() => DateTime.Now.Year - BirthYear;

        // Виведення інформації
        public void PrintInfo() => Console.WriteLine(this);

        public override string ToString() =>
            $"Квиток: {TicketNumber}, ПІБ: {FullName}, рік народження: {BirthYear} (вік {GetAge()}), " +
            $"група: {GroupNumber}, курс: {Course}";

        // Природний порядок: за номером квитка
        public int CompareTo(Student other)
        {
            if (other is null) return 1;
            return string.Compare(TicketNumber, other.TicketNumber, StringComparison.Ordinal);
        }
    }

    // Порівнювачі IComparer<T>
    public class StudentByNameComparer : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            return string.Compare(x.FullName, y.FullName, StringComparison.CurrentCulture);
        }
    }

    public class StudentByBirthYearComparer : IComparer<Student>
    {
        public int Compare(Student x, Student y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x is null) return -1;
            if (y is null) return 1;
            return x.BirthYear.CompareTo(y.BirthYear);
        }
    }
}