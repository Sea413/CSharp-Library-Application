using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Library
{
  public class PatronTest : IDisposable
  {
    public PatronTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Athens_Library;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_EmptyAtFirst()
    {
      //Arrange, Act
      int result = Patron.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_EqualOverrideTrueForSameDescription()
    {
      //Arrange, Act
      Patron firstPatron = new Patron("David Turley");
      Patron secondPatron = new Patron("David Turley");

      //Assert
      Assert.Equal(firstPatron, secondPatron);
    }

    [Fact]
    public void Test_Save()
    {
      //Arrange
      Patron testPatron = new Patron("David Turley");
      testPatron.Save();

      //Act
      List<Patron> result = Patron.GetAll();
      List<Patron> testList = new List<Patron>{testPatron};

      //Assert
      Assert.Equal(testList, result);
    }



    [Fact]
    public void Test_SaveAssignsIdToObject()
    {
      //Arrange
      Patron testPatron = new Patron("Hai Lam");
      testPatron.Save();

      //Act
      Patron savedPatron = Patron.GetAll()[0];

      int result = savedPatron.GetId();
      int testId = testPatron.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_FindFindsPatronInDatabase()
    {
      //Arrange
      Patron testPatron = new Patron("Zach Black");
      testPatron.Save();

      //Act
      Patron result = Patron.Find(testPatron.GetId());

      //Assert
      Assert.Equal(testPatron, result);
    }

    public void Dispose()
    {
      Patron.DeleteAll();
    }
  }
}
