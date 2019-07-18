using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRdWr
{
    class Student	// Класс студент
    {
        string firstname;		// Имя
        string lastname;		// Фамилия
        string address;		// Адрес
        string phone;			// Телефон
        DateTime birthday;		// Дата рождения
        int number;			// Номер зачетки

        // Поверхностное копирование объекта
        public Student Clone()
        {
            // Вызываем функцию базового класса (Object) для поверхностного копирования объекта
            return (Student)MemberwiseClone();
        }
        // Ввод данных
        public void Input()
        {
            Console.WriteLine("*****Ввод данных о студенте:******");
            Console.Write("Имя: ");
            firstname = Console.ReadLine();
            Console.Write("Фамилия: ");
            lastname = Console.ReadLine();
            Console.Write("Адрес: ");
            address = Console.ReadLine();
            Console.Write("Телефон: ");
            phone = Console.ReadLine();
            Console.Write("Дата рождения: ");
            try
            {
                // Считывание даты
                birthday = Convert.ToDateTime(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Ошибка ввода, используем текущую дату");
                birthday = DateTime.Now;
            }
            Console.Write("Номер зачетки: ");
            try
            {
                number = Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Ошибка ввода, используется номер 0");
                number = 0;
            }
            Console.WriteLine("**********************************");
        }
        // Вывод данных
        public void Print()
        {
            Console.WriteLine("*****Вывод данных о студенте:*****");
            Console.WriteLine("Имя: {0}", firstname);
            Console.WriteLine("Фамилия: {0}", lastname);
            Console.WriteLine("Адрес: {0}", address);
            Console.WriteLine("Телефон: {0}", phone);
            Console.WriteLine("Дата рождения: {0}.{1}.{2}",
                birthday.Day, birthday.Month, birthday.Year);
            Console.WriteLine("Номер зачетки: {0}", number);
            Console.WriteLine("**********************************");
        }

        // Запись в файл
        public void Write(BinaryWriter bw)
        {
            // Все данные записываются по отдельности
            bw.Write(firstname);
            bw.Write(lastname);
            bw.Write(address);
            bw.Write(phone);
            bw.Write(birthday.Year);
            bw.Write(birthday.Month);
            bw.Write(birthday.Day);
            bw.Write(number);
        }

        // Статический метод для чтения из файла информации
        // и создания нового объекта на ее основе
        public static Student Read(BinaryReader br)
        {
            // Считывание производится в порядке, 
            // соответствующем записи
            Student st = new Student();
            st.firstname = br.ReadString();
            st.lastname = br.ReadString();
            st.address = br.ReadString();
            st.phone = br.ReadString();
            int year = br.ReadInt32();
            int month = br.ReadInt32();
            int day = br.ReadInt32();
            st.birthday = new DateTime(year, month, day);
            st.number = br.ReadInt32();

            return st;
        }
    }
    // Класс Group
    class Group : ICloneable
    {
        // Название группы
        string groupname;
        // Массив студентов
        Student[] st;

        // Свойства
        public string GroupName
        {
            get
            {
                return groupname;
            }
            set
            {
                groupname = value;
            }
        }
        public Student[] Students
        {
            get
            {
                return st;
            }
            set
            {
                st = value;
            }
        }

        // Конструктор, получающий название группы и количество студентов
        public Group(string gn, int n)
        {
            groupname = gn;
            // По умолчанию в группе 10 студентов
            if (n < 0 || n > 10)
                n = 10;
            st = new Student[n];
            // Создаем студентов
            for (int i = 0; i < n; i++)
                st[i] = new Student();
        }

        // Аналог конструктора копирования
        public Group(Group gr)
        {
            // Создаем массив студентов 
            st = new Student[gr.st.Length];
            // Передираем название группы
            groupname = gr.groupname;
            // Передираем каждого индивидуума
            for (int i = 0; i < gr.st.Length; i++)
                st[i] = gr.st[i].Clone();
        }

        // Заполняем группу
        public void Input()
        {
            for (int i = 0; i < st.Length; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                st[i].Input();
            }
        }
        // Изменение данных конкретного студента
        public void InputAt(int n)
        {
            if (st == null || n >= st.Length || n < 0)
                return;

            st[n].Input();
        }

        // Вывод списка группы
        public void Print()
        {
            Console.WriteLine("Группа {0}:", groupname);

            for (int i = 0; i < st.Length; i++)
            {
                Console.WriteLine("{0}.", i + 1);
                st[i].Print();
            }
        }

        // Вывод информации о конкретном студенте
        public void PrintAt(int n)
        {
            if (st == null || n >= st.Length || n < 0)
                return;

            st[n].Print();
        }

        // "Глубокое" копирование, реализация функции из интерфейса IClonable
        public object Clone()
        {
            // Создание новой группы
            Group gr = new Group(groupname, st.Length);
            // Передираем каждого индивидуума
            for (int i = 0; i < st.Length; i++)
                gr.st[i] = st[i].Clone();
            // Возврат независимой копии группы
            return gr;
        }

        // Запись в файл
        public void Write(BinaryWriter bw)
        {
            // Сохраняем название группы
            bw.Write(groupname);
            // Сохраняем количество студентов
            bw.Write(st.Length);

            // Для сохранения студента вызывается
            // соответствующий метод из класса Student
            for (int i = 0; i < st.Length; i++)
                st[i].Write(bw);
        }

        // Статический метод для чтения из файла информации
        // и создания нового объекта на ее основе
        public static Group Read(BinaryReader br)
        {
            string gn = br.ReadString();
            int n = br.ReadInt32();

            Student[] st = new Student[n];

            // Для считывания студента вызывается соотв. метод из класса Student
            for (int i = 0; i < n; i++)
                st[i] = Student.Read(br);

            // Создаем пустую группу
            Group gr = new Group(gn, 0);
            // Записываем в нее студентов
            gr.st = st;

            return gr;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Группа
            Group gr = new Group("Group-192", 5);

            gr.Input();
            gr.Print();

            // Создаем поток для создания файла и/или записи в него
            FileStream fs = new FileStream("group.bin", FileMode.OpenOrCreate, FileAccess.Write);

            // Создаем двоичный поток для записи
            BinaryWriter bw = new BinaryWriter(fs, Encoding.Default);

            // Пишем данные
            gr.Write(bw);
            // Закрываем потоки
            bw.Close();
            fs.Close();

            // Создаем поток для чтения из файла
            fs = new FileStream("group.bin", FileMode.Open, FileAccess.Read);
            // Создаем двоичный поток для чтения
            BinaryReader br = new BinaryReader(fs, Encoding.Default);

            // Читаем данные
            gr = Group.Read(br);
            // Закрываем потоки
            br.Close();
            fs.Close();

            gr.Print();
        }
    }
}
