using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace week6_fileIO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TestFileIO();
        }

        static void TestFileIO()
        {
            Library lib1 = new Library("L1_01_T203", "Under the tree");
            Console.WriteLine($"lib1 : {lib1}");

            //lib1.AddNewBook(); //these will ask the user 2 times for abook you wanna add to the library.
            //lib1.AddNewBook();

            //shows all books
            lib1.ShowBooks();

            //show book with specific title
            Console.WriteLine($"\n showing book from library 1 with the title spring day");
            lib1.ShowBooks("spring day");

            Library lib2 = new Library("L2_03_M203", "The Sacred Space");
            Console.WriteLine($"lib2 : {lib2}");

            //lib2.AddNewBook();
            lib2.ShowBooks();

            //show book from specific genre
            Console.WriteLine($"\n showing book from library 2 with the genre fiction");
            lib2.ShowBooks(BookGenre.Fiction);

            //show book from specific genre and title
            Console.WriteLine($"\n showing book from library 2 with genre fiction title The Jungle Book");
            lib2.ShowBooks("The Jungle Book",BookGenre.Fiction);

            Library lib3 = new Library("L3_02_M203", "Over the Cloud");
            Console.WriteLine($"lib3 : {lib3}");
            lib3.ShowBooks();
        }

        public enum BookGenre
        {
            Fiction, NonFiction, Biography, Mythology
        }

        public class Book
        {
            public string Title { get; }
            public string Author { get; }
            public BookGenre Genre { get; }

            public Book(string title, string author, BookGenre genre)
            {
                this.Title = title;
                this.Author = author;
                this.Genre = genre;
            }

            public override string ToString()
            {
                return $"Book title: {this.Title} ----- Author : {this.Author} ----- Genre : {this.Genre}";
            }
        }

        public class Library
        {
            private static string FILENAME = "Library_Data_txt";
            private string LibraryID { get; }
            public string LibraryName { get; }

            public List<Book>booklist { get; }

            public Library(string ID, string name)
            {
                this.LibraryID = ID;
                this.LibraryName = name;

                //class relationship- HAS-A
                //one class contains object of another. we are using the book class in this class.
                this.booklist = new List<Book>(); //empty list of books

                //READ BOOK INFO FROM FILE AND SAVE THE OBJECTS IN LIST
                using (StreamReader reader = new StreamReader(FILENAME))
                {
                    string recordline;
                    while ((recordline= reader.ReadLine()) !=null) { 
                        //Console.WriteLine($"record line: {recordline}");

                        //extract individual fields from recordline by splitting all the fields by comma since the file is in cvs format (comma-seperated values)
                        string[] fields=recordline.Split(',');

                        if (fields[0]==this.LibraryID)
                        {
                            //fields[0] will give u the library id, 1 will give u title, and so on
                            //author and title is in string byt genre is in enumerations. so how do we convert it into string.
                            string title = fields[1];
                            string author = fields[2];
                            BookGenre genre = (BookGenre)Enum.Parse(typeof(BookGenre), fields[3]);
                            Book newBook = new Book(title, author, genre);
                            booklist.Add(newBook);
                        }
                    
                    }
                    reader.Close();
                }

            }

            public void AddNewBook()
            {
                Console.WriteLine("Please enter book title : ");
                string title = Console.ReadLine();

                Console.WriteLine("Please enter book author : ");
                string author = Console.ReadLine();

                Console.WriteLine("Please choose book genre [0: Fiction, 1: NonFiction, 2: Biography, 3: Mythology] : ");
                int genre = Convert.ToInt16(Console.ReadLine());

                if (genre < 0 || genre > 4)
                {
                    genre = 0;
                }
                BookGenre selectedGenre = (BookGenre)genre;
                Book newBook = new Book(title, author, selectedGenre);

                //add object to he list
                this.booklist.Add(newBook);

                //save the book info in a text file for data persistence
                //to store permanently 
                string dataline = $"{this.LibraryID},{newBook.Title},{newBook.Author},{newBook.Genre}";
                Console.WriteLine(dataline);

                //append: true- will add new data at the end of existing ones
                //append: false- overwrites/replaces existing data to add new one
                StreamWriter writer = new StreamWriter(FILENAME, append: true);
                writer.WriteLine(dataline); //this will write new data in a new line.
                //writer.Write(dataline); //this will write everthing in one line because there is now writelines, just write.
                writer.Close();
            }

            //method overloading: having multipke fuctions in the same class with the same name, but different number of parameter and different types of parameters

            public void ShowBooks()
            {
                Console.WriteLine($"\nShowing books from library {this.LibraryID}");

                foreach(Book book in this.booklist)
                {
                    Console.WriteLine($"book: {book}");
                }
            }

            //task-show appropriate message to user if no book is available matching their criteria

            public void ShowBooks(string title)
            {
                Console.WriteLine($"\nShowing books from library {this.LibraryID}");

                foreach (Book book in this.booklist)
                {
                    if (book.Title == title)
                    {
                        Console.WriteLine($"book: {book}");
                    }
                    
                }
            }

            public void ShowBooks(BookGenre genre)
            {
                Console.WriteLine($"\nShowing books from library {this.LibraryID}");

                foreach (Book book in this.booklist)
                {
                    if (book.Genre==genre)
                    {
                        Console.WriteLine($"book: {book}");
                    }
                    
                }
            }

            public void ShowBooks(string title, string author)
            {
                Console.WriteLine($"\nShowing books from library {this.LibraryID}");

                foreach (Book book in this.booklist)
                {
                    if (book.Title==title && book.Author==author)
                    {
                        Console.WriteLine($"book: {book}");
                    }

                    
                }
            }

            public void ShowBooks(string title, BookGenre genre)
            {
                Console.WriteLine($"\nShowing books from library {this.LibraryID}");

                foreach (Book book in this.booklist)
                {
                    if (book.Title == title && book.Genre == genre)
                    {
                        Console.WriteLine($"book: {book}");
                    }
                        
                }
            }

            public override string ToString()
            {
                return $"Library ID : {this.LibraryID} ---- Library Name : {this.LibraryName}";
            }
        }
    }


}
