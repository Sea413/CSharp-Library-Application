using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Patron
  {
    private int _id;
    private string _name;

    public Patron(string Name, int Id = 0)
    {
      _id = Id;
      _name = Name;
    }

    public override bool Equals(System.Object otherPatron)
  {
    if (!(otherPatron is Patron))
    {
      return false;
    }
    else {
      Patron newPatron = (Patron) otherPatron;
      bool idEquality = this.GetId() == newPatron.GetId();
      bool nameEquality = this.GetName() == newPatron.GetName();
      return (idEquality && nameEquality);
    }
  }

  public int GetId()
  {
    return _id;
  }
  public string GetName()
  {
    return _name;
  }
  public void SetName(string newName)
  {
    _name = newName;
  }

  public static List<Patron> GetAll()
{
  List<Patron> AllPatrons = new List<Patron>{};

  SqlConnection conn = DB.Connection();
  SqlDataReader rdr = null;
  conn.Open();

  SqlCommand cmd = new SqlCommand("SELECT * FROM patrons", conn);
  rdr = cmd.ExecuteReader();

  while(rdr.Read())
  {
    int patronId = rdr.GetInt32(0);
    string patronName = rdr.GetString(1);
    Patron newPatron = new Patron(patronName, patronId);
    AllPatrons.Add(newPatron);
  }
  if (rdr != null)
  {
    rdr.Close();
  }
  if (conn != null)
  {
    conn.Close();
  }
  return AllPatrons;
}


public void Save()
{
  SqlConnection conn = DB.Connection();
  SqlDataReader rdr;
  conn.Open();

  SqlCommand cmd = new SqlCommand("INSERT INTO patrons (Name) OUTPUT INSERTED.id VALUES (@PatronName)", conn);

  SqlParameter nameParam = new SqlParameter();
  nameParam.ParameterName = "@PatronName";
  nameParam.Value = this.GetName();

  cmd.Parameters.Add(nameParam);

  rdr = cmd.ExecuteReader();

  while(rdr.Read())
  {
    this._id = rdr.GetInt32(0);
  }
  if (rdr != null)
  {
    rdr.Close();
  }
  if (conn != null)
  {
    conn.Close();
  }
}

public List<Book> GetBooks()
{
  SqlConnection conn = DB.Connection();
  SqlDataReader rdr = null;
  conn.Open();

  SqlCommand cmd = new SqlCommand("SELECT book_id FROM patrons_books WHERE patron_id = @PatronId;", conn);

  SqlParameter PatronIdParameter = new SqlParameter();
  PatronIdParameter.ParameterName = "@PatronId";
  PatronIdParameter.Value = this.GetId();
  cmd.Parameters.Add(PatronIdParameter);

  rdr = cmd.ExecuteReader();

  List<int> BookIds = new List<int> {};

  while (rdr.Read())
  {
    int BookId = rdr.GetInt32(0);
    BookIds.Add(BookId);
  }
  if (rdr != null)
  {
    rdr.Close();
  }

  List<Book> Books = new List<Book> {};

  foreach (int book_id in BookIds)
  {
    SqlDataReader queryReader = null;
    SqlCommand classQuery = new SqlCommand("SELECT * FROM books WHERE Id = @BookId;", conn);

    SqlParameter BookIdParameter = new SqlParameter();
    BookIdParameter.ParameterName = "@BookId";
    BookIdParameter.Value = book_id;
    classQuery.Parameters.Add(BookIdParameter);

    queryReader = classQuery.ExecuteReader();
    while (queryReader.Read())
    {
      int thisBookID = queryReader.GetInt32(0);
      string bookTitle =queryReader.GetString(1);
      int authorId = queryReader.GetInt32(2);
      int copies = queryReader.GetInt32(3);
      Book foundBook = new Book(bookTitle, authorId, copies, thisBookID);
      Books.Add(foundBook);
    }
    if (queryReader != null)
    {
      queryReader.Close();
    }
  }
  if (conn != null)
  {
    conn.Close();
  }
  return Books;
}

public static void DeleteAll()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();
    SqlCommand cmd = new SqlCommand("DELETE FROM patrons;", conn);
    cmd.ExecuteNonQuery();
  }

  public static Patron Find(int id)
  {
    SqlConnection conn = DB.Connection();
    SqlDataReader rdr = null;
    conn.Open();

    SqlCommand cmd = new SqlCommand("SELECT * FROM patrons WHERE Id = @PatronName", conn);
    SqlParameter PatronNameParameter = new SqlParameter();
    PatronNameParameter.ParameterName = "@PatronName";
    PatronNameParameter.Value = id.ToString();
    cmd.Parameters.Add(PatronNameParameter);
    rdr = cmd.ExecuteReader();

    int foundPatronId = 0;
    string foundPatronName = null;

    while(rdr.Read())
    {
      foundPatronId = rdr.GetInt32(0);
      foundPatronName = rdr.GetString(1);
    }
    Patron foundPatron = new Patron(foundPatronName,foundPatronId);

    if (rdr != null)
    {
      rdr.Close();
    }
    if (conn != null)
    {
      conn.Close();
    }
    return foundPatron;
  }
  public void AddBook(Book newBook)
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("INSERT INTO patrons_books (patron_id, book_id) VALUES (@PatronId, @BookId);", conn);

    SqlParameter bookIdParameter = new SqlParameter();
    bookIdParameter.ParameterName = "@BookId";
    bookIdParameter.Value = newBook.GetId();
    cmd.Parameters.Add(bookIdParameter);

    SqlParameter PatronNameParameter = new SqlParameter();
    PatronNameParameter.ParameterName = "@PatronId";
    PatronNameParameter.Value = this.GetId();
    cmd.Parameters.Add(PatronNameParameter);

    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }

  public void Delete()
  {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("DELETE FROM patrons WHERE id = @PatronId;", conn);
    SqlParameter PatronNameParameter = new SqlParameter();
    PatronNameParameter.ParameterName = "@PatronId";
    PatronNameParameter.Value = this.GetId();

    cmd.Parameters.Add(PatronNameParameter);
    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
  }
}
}
