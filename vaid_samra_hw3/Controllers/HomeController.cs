using Microsoft.AspNetCore.Mvc;
using vaid_samra_hw3.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using vaid_samra_hw3.DAL;
using System.Linq;
using Microsoft.AspNetCore.OutputCaching;

namespace vaid_samra_hw3.Controllers
{
    public class HomeController : Controller
    {
        private AppDbContext _context;
        public HomeController(AppDbContext dbContext)
        {
            _context = dbContext;
        }
        public IActionResult Index(String SearchString)
        {
            
            var query = from b in _context.Books
                        select b;

            if (SearchString != null)
            {
                query = query.Where(b => b.Title.Contains(SearchString) ||
                                          b.Description.Contains(SearchString));
            }

            List<Book> SelectedBooks = query.Include(b => b.Genre).ToList();

            ViewBag.AllBooks = _context.Books.Count();
            ViewBag.SelectedBooks = SelectedBooks.Count;

            return View(SelectedBooks.OrderBy(s => s.Title));

            if (SearchString == null)
            {
                return View(_context.Books.Include(s => s.Genre).ToList());
            }

        }

        public IActionResult Details(int? id)
        {
            if (id== null)
            {
                return View("Error", new String[] { "BookID not specified - which book do you want to view?" });
            }
            Book book = _context.Books.Include(b => b.Genre).FirstOrDefault(b => b.BookID == id);
            if (book == null)
            {
                return View("Error", new String[] { "Book not found in database" });
            }
            return View(book);
        }
        public IActionResult DetailedSearch()
        {
            ViewBag.AllGenres = GetAllGenresSelectList();

            SearchViewModel svm = new SearchViewModel();
            svm.SearchGenreID = 0;
                

            return View(svm);
            
        }
        public IActionResult DisplaySearchResults(SearchViewModel svm)
            
        {
            var query = from c in _context.Books
                        select c;

            if (String.IsNullOrEmpty(svm.SearchName) == false)
            {
                query = query.Where(c => c.Title.Contains(svm.SearchName) || 
                                         c.Author.Contains(svm.SearchName));
            }

            if(String.IsNullOrEmpty(svm.SearchDescription) == false)
            {
                query = query.Where(c => c.Description.Contains(svm.SearchDescription));
            }

            if (svm.SearchFormat != null)
            {
                query = query.Where(c => c.BookFormat == svm.SearchFormat);
            }

            if(svm.SearchGenreID != 0 )
            {
                query = query.Where(c => c.Genre.GenreID == svm.SearchGenreID);
            } 

            if(svm.SearchPrice != 0 )
            {
                if (svm.SearchPriceType == PriceType.GreaterThan)
                {
                    query = query.Where(c => c.Price >= svm.SearchPrice);
                }

                if (svm.SearchPriceType == PriceType.LessThan)
                {
                    query = query.Where(c => c.Price <= svm.SearchPrice);
                }
            }
            
            if(svm.SearchPublished != null)
            {
                query = query.Where(c => c.PublishedDate >= svm.SearchPublished);
            }
           

            List<Book> SelectedBooks = query.Include(c => c.Genre).ToList();

            ViewBag.AllBooks = _context.Books.Count();
            ViewBag.SelectedBooks = SelectedBooks.Count;

            return View("Index", SelectedBooks.OrderBy(c => c.Title));


            

        }

        private SelectList GetAllGenresSelectList()
        {
            List<Genre> genreList = _context.Genres.ToList();

            Genre SelectNone = new Genre() { GenreID = 0, GenreName = "All Genres" };
            genreList.Add(SelectNone);

            SelectList genreSelectList = new SelectList(genreList.OrderBy(m => m.GenreID), "GenreID", "GenreName");

            return genreSelectList;
        }
    }
}
