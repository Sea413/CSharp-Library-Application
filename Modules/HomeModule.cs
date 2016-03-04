using System;
using System.Collections.Generic;
using Nancy;
using Nancy.ViewEngines.Razor;

namespace Library
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };

      Get["/books"] = _ => {
      List<Book> AllBooks = Book.GetAll();
      return View["books.cshtml", AllBooks];
      };

      Get["/patrons"] = _ => {
      List<Patron> AllPatrons = Patron.GetAll();
      return View["patron.cshtml", AllPatrons];
      };


      Get["/patrons/{id}"] = parameters => {
         Dictionary<string, object> model = new Dictionary<string, object>();
         Patron selectedPatron = Patron.Find(parameters.id);
         List<Book> allBooks = Book.GetAll();
         List<Book> bookpatrons = selectedPatron.GetBooks();
         model.Add("patrons", selectedPatron);
         model.Add("books", allBooks);
         model.Add("checkedout", bookpatrons);
         return View["patrons.cshtml", model];
         };

        //  Get["/books/{id}"] = parameters => {
        //     Dictionary<string, object> model = new Dictionary<string, object>();
        //     Book selectedBook = Book.Find(parameters.id);
        //     model.Add("book", selectedBook);
        //     return View["patrons.cshtml", model];
        //     };

      //    Post["patron/add_book"] = _ => {
      //    Book selectedBook = Book.Find(Request.Form["book-id"]);
      //    Patron selectedpatron = Patron.Find(Request.Form["patron-id"]);
      //    selectedpatron.AddBook(selectedBook);
      //    List<Patron> AllPatron = Patron.GetAll();
      //    return View["classes.cshtml", AllPatron];
      //  };

         Post["/patrons"] = _ => {
         Patron newPatron = new Patron(Request.Form["patron_name"]);
         newPatron.Save();
         List<Patron> AllPatrons = Patron.GetAll();
         return View["patron.cshtml", AllPatrons];
         };

         Post["/books"] = _ => {
         Book newBook = new Book(Request.Form["Book_Title"], Request.Form["Author_Id"],Request.Form["copies"]);
         newBook.Save();
         List<Book> AllBooks = Book.GetAll();
         return View["books.cshtml", AllBooks];
         };
       }
     }
   }
