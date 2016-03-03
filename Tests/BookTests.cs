using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Library
{
  public class BookTest : IDisposable
  {
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Athens_Library;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_BookEmptyAtFirst()
    {
      int result = Book.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_BookReturnTrueForSameName()
    {
      Book firstClass = new Book("Philosophy", 12,5);
      Book secondClass = new Book("Philosophy", 12,5);
      Assert.Equal(firstClass, secondClass);
    }


        [Fact]
    public void Test_Save_SavesClassToDatabase()
    {
      //Arrange
      Book testBook = new Book("Illiad",12,4);
      testBook.Save();

      //Act
      List<Book> result = Book.GetAll();
      List<Book> testList = new List<Book>{testBook};

      //Assert
      Assert.Equal(testList, result);
    }


    [Fact]
    public void Test_Save_AssignsIdToBookObject()
    {
      //Arrange
      Book testBook = new Book("Illiad",12,4);
      testBook.Save();

      //Act
      Book savedBook = Book.GetAll()[0];
      int result = savedBook.GetId();
      int testId = testBook.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsBookInDatabase()
    {
      //Arrange
      Book testBook = new Book("Illiad",12,4);
      testBook.Save();

      //Act
      Book foundBook = Book.Find(testBook.GetId());

      //Assert
      Assert.Equal(testBook, foundBook);
    }

        // [Fact]
        // public void Test_AddStudent_AddsStudentToClass()
        // {
        //   //Arrange
        //   Class testClass = new Class("Fiction", "CRWT001");
        //   testClass.Save();
        //
        //   Student testStudent = new Student("Veronica Alley", new DateTime (2009,10,01));
        //   testStudent.Save();
        //
        //   Student testStudent2 = new Student("Sean Peerenboom", new DateTime (2009,10,01));
        //   testStudent2.Save();
        //
        //   //Act
        //   testClass.AddStudent(testStudent);
        //   testClass.AddStudent(testStudent2);
        //
        //   List<Student> result = testClass.GetStudents();
        //   List<Student> testList = new List<Student>{testStudent, testStudent2};
        //
        //   //Assert
        //   Assert.Equal(testList, result);
        // }

        // [Fact]
        // public void Test_GetStudents_ReturnsAllClassStudents()
        // {
        //   //Arrange
        //   Class testClass = new Class("Fiction", "CRWT001");
        //   testClass.Save();
        //
        //   Student testStudent1 = new Student("Veronica Alley", new DateTime (2009,10,01));
        //   testStudent1.Save();
        //
        //   // Student testStudent2 = new Student("Sean Peerenboom", new DateTime (2009,10,01));
        //   // testStudent2.Save();
        //
        //   //Act
        //   testClass.AddStudent(testStudent1);
        //   List<Student> savedStudents = testClass.GetStudents();
        //   List<Student> testList = new List<Student> {testStudent1};
        //
        //   //Assert
        //   Assert.Equal(testList, savedStudents);
        // }

    //   public void Test_GetClasses_ReturnsAllStudentClasses()
    //   {
    //   //Arrange
    //   Student testStudent = new Student("Veronica Alley", new DateTime (2009,10,01));
    //   testStudent.Save();
    //
    //   Class testClass1 = new Class("Fiction", "CRWT001");
    //   testClass1.Save();
    //
    //   Class testClass2 = new Class("Philosophy", "PHIL002");
    //   testClass2.Save();
    //
    //   //Act
    //   testStudent.AddClass(testClass1);
    //   List<Class> result = testStudent.GetClasses();
    //   List<Class> testList = new List<Class> {testClass1};
    //
    //   //Assert
    //   Assert.Equal(testList, result);
    // }
    // [Fact]
    //     public void Test_Delete_DeletesClassAssociationsFromDatabase()
    //     {
    //       //Arrange
    //       Student testStudent = new Student("Ted Mosley", new DateTime (2009,10,01));
    //       testStudent.Save();
    //
    //       string testName = "Ted Mosley";
    //       Class testClass = new Class(testName,"Gred1001");
    //       testClass.Save();
    //
    //       //Act
    //       testClass.AddStudent(testStudent);
    //       testClass.Delete();
    //
    //       List<Class> resultStudentClasses = testStudent.GetClasses();
    //       List<Class> testStudentClasses = new List<Class> {};
    //
    //       //Assert
    //       Assert.Equal(testStudentClasses, resultStudentClasses);
    //     }
    // [Fact]
    // public void Test_Delete_DeletesBookFromDatabase()
    // {
    //   //Arrange
    //   string name1 = "Buddhism";
    //   Book testBook1 = new Class(name1, 3,4);
    //   testBook1.Save();
    //
    //   string name2 = "Basket Weaving";
    //   Class testBook2 = new Class(name2,4,6);
    //   testBook2.Save();
    //
    //   //Act
    //   testBook1.Delete();
    //   List<Book> resultBooks = Book.GetAll();
    //   List<Book> testBookList = new List<Book> {testBook2};
    //
    //   //Assert
    //   Assert.Equal(testBookList, resultBooks);
    // }

      [Fact]
        public void Dispose()
        {
          Book.DeleteAll();
        }
      }
    }
