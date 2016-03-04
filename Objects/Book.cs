using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Library
{
  public class Book
  {
    private int _id;
    private string _title;
    private int _copies;
    private int _authorId;

    public Book(string Title, int authorID, int Copies, int Id = 0)
    {
      _id = Id;
      _title = Title;
      _copies = Copies;
      _authorId = authorID;
    }

    public override bool Equals(System.Object otherbooks)
    {
        if (!(otherbooks is Book))
        {
          return false;
        }
        else
        {
          Book newBook = (Book) otherbooks;
          bool idEquality = this.GetId() == newBook.GetId();
          bool titleEquality = this.GetTitle() == newBook.GetTitle();
          bool copiesEquality = this.GetCopies() ==newBook.GetCopies();
          bool authoridEquality =this.GetAuthorId()==newBook.GetAuthorId();
          return (idEquality && titleEquality && copiesEquality && authoridEquality);
        }
    }
    public int GetId()
    {
      return _id;
    }
    public string GetTitle()
    {
      return _title;
    }
    public void SetTitle(string newTitle)
    {
      _title = newTitle;
    }
    public int GetAuthorId()
    {
      return _authorId;
    }
    public int GetCopies()
    {
      return _copies;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr;
      conn.Open();

      SqlCommand cmd = new SqlCommand ("INSERT INTO Books (Title, Author_id, copies) OUTPUT INSERTED.id VALUES (@BookTitle, @AuthorID, @Copies);", conn);

        SqlParameter titleParameter = new SqlParameter();
        titleParameter.ParameterName = "@BookTitle";
        titleParameter.Value = this.GetTitle();

        cmd.Parameters.Add(titleParameter);

        SqlParameter AuthorIDParameter = new SqlParameter();
        AuthorIDParameter.ParameterName = "@AuthorID";
        AuthorIDParameter.Value = this.GetAuthorId();

        cmd.Parameters.Add(AuthorIDParameter);

        SqlParameter copiesParameter = new SqlParameter();
        copiesParameter.ParameterName = "@Copies";
        copiesParameter.Value = this.GetCopies();

        cmd.Parameters.Add(copiesParameter);

        rdr = cmd.ExecuteReader();

        while(rdr.Read())
        {
          this._id = rdr.GetInt32(0);
        }
        if (rdr != null)
        {
          rdr.Close();
        }
        if(conn != null)
        {
          conn.Close();
        }
      }
      public static void DeleteAll()
      {
        SqlConnection conn = DB.Connection();
        conn.Open();
        SqlCommand cmd = new SqlCommand("DELETE FROM Books;", conn);
        cmd.ExecuteNonQuery();
      }

      public static Book Find(int id)
      {
      SqlConnection conn = DB.Connection();
      SqlDataReader rdr = null;
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM Books WHERE id = @BookId;", conn);
      SqlParameter BookIdParameter = new SqlParameter();
      BookIdParameter.ParameterName = "@BookId";
      BookIdParameter.Value = id.ToString();
      cmd.Parameters.Add(BookIdParameter);
      rdr = cmd.ExecuteReader();

      int foundBookId = 0;
      string foundTitle = null;
      int foundAuthorId = 0;
      int foundCopies = 0;


      while(rdr.Read())
      {
        foundBookId = rdr.GetInt32(0);
        foundTitle = rdr.GetString(1);
        foundAuthorId = rdr.GetInt32(2);
        foundCopies = rdr.GetInt32(3);
      }
      Book foundBook = new Book(foundTitle, foundAuthorId, foundCopies, foundBookId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
      return foundBook;
      }

      public static List<Book> GetAll()
          {
            List<Book> allBooks = new List<Book>{};

            SqlConnection conn = DB.Connection();
            SqlDataReader rdr = null;
            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Books;", conn);
            rdr = cmd.ExecuteReader();

            while(rdr.Read())
            {
              int BookId = rdr.GetInt32(0);
              string BookName = rdr.GetString(1);
              int AuthorId = rdr.GetInt32(2);
              int copies = rdr.GetInt32(3);
              Book newBook = new Book(BookName, AuthorId, copies, BookId);
              allBooks.Add(newBook);
            }
            if (rdr != null)
            {
              rdr.Close();
            }
            if (conn != null)
            {
              conn.Close();
            }
            return allBooks;
          }
      //     public void Update(string newTitle, int newauthorId,int copies)
      //     {
      //     SqlConnection conn = DB.Connection();
      //     SqlDataReader rdr;
      //     conn.Open();
      //
      //     SqlCommand cmd = new SqlCommand("UPDATE class SET Title = @NewTitle, Author_Id = @authorId, copies = @copies,  OUTPUT INSERTED.Titles WHERE id = @BookId;", conn);
      //
      //     SqlParameter newTitleParameter = new SqlParameter();
      //     newTitleParameter.ParameterTitle = "@NewTitle";
      //     newTitleParameter.Value = newTitle;
      //     cmd.Parameters.Add(newTitleParameter);
      //
      //
      //     SqlParameter ClassIdParameter = new SqlParameter();
      //     ClassIdParameter.ParameterName = "@BookId";
      //     ClassIdParameter.Value = this.GetId();
      //     cmd.Parameters.Add(ClassIdParameter);
      //     rdr = cmd.ExecuteReader();
      //
      //     while(rdr.Read())
      //     {
      //       this._name = rdr.GetString(0);
      //     }
      //
      //     if (rdr != null)
      //     {
      //       rdr.Close();
      //     }
      //
      //     if (conn != null)
      //     {
      //       conn.Close();
      // }
    // }
    public void Delete()
    {
    SqlConnection conn = DB.Connection();
    conn.Open();

    SqlCommand cmd = new SqlCommand("DELETE FROM Book WHERE id = @BookId; DELETE FROM checkout WHERE Book_Id = @BookId;", conn);
    SqlParameter BookIdParameter = new SqlParameter();
    BookIdParameter.ParameterName = "@BookId";
    BookIdParameter.Value = this.GetId();

    cmd.Parameters.Add(BookIdParameter);
    cmd.ExecuteNonQuery();

    if (conn != null)
    {
      conn.Close();
    }
    }
  }
  }
