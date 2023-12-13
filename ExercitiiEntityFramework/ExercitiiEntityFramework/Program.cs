using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExercitiiEntityFramework
{
    internal class Program
    {
        static pubsEntities pe;
        static void Main(string[] args)
        {
            //getPublishers();
            //addBook();
            //updateBook("AB6789", 100);
            //deleteBook("AB6789");
            getRelations();
            Console.ReadKey();
        }
        public static void getPublishers()
        {
            pe = new pubsEntities();
            IEnumerable<publisher> pubs = from p in pe.publishers
                                          orderby p.pub_name
                                          select p;
            foreach (publisher publisher in pubs)
            {
                Console.WriteLine($"{publisher.pub_name} \t {publisher.country} \t {publisher.city} \t {publisher.employees.First().fname}");
            }
        }
        public static void addBook()
        {
            title t = new title
            {
                title_id = "AB6789",
                title1 = "Capra cu 3 iezi",
                pubdate = DateTime.Now,
                type = "poveste",
                pub_id = "1389"
            };
            pe.titles.Add(t);
            pe.SaveChanges();
            Console.WriteLine("Titlu adaugat!\n");
            author a = new author
            {
                au_fname = "Creanga",
                au_lname = "Ion",
                contract = false,
                phone = "1234-1234",
                au_id = "998-72-3569"
            };
            pe.authors.Add(a);
            pe.SaveChanges();
            Console.WriteLine("Autor adaugat!\n");
            titleauthor ta = new titleauthor
            {
                au_id = a.au_id,
                title_id = t.title_id,
            };
            pe.titleauthors.Add(ta);
            pe.SaveChanges();
            Console.WriteLine("Legatura adaugata!\n");
        }
        public static void updateBook(string id, decimal price)
        {
            pe = new pubsEntities();
            //title title = pe.titles.Where(x=>x.title_id==id).FirstOrDefault();
            title title = (from t in pe.titles where t.title_id == id select t).FirstOrDefault();
            if(title != null)
            {
                title.price = price;
                pe.SaveChanges();
                Console.WriteLine("Pret actualizat!\n");
            }
        }
        public static void deleteBook(string id)
        {
            pe = new pubsEntities();
            title title = pe.titles.Where(x => x.title_id == id).FirstOrDefault();
            if(title != null)
            {
                IEnumerable<titleauthor> ta = pe.titleauthors.Where(x => x.title_id == id);
                pe.titleauthors.RemoveRange(ta);
                pe.titles.Remove(title);
                pe.SaveChanges();
                Console.WriteLine("Carte stearsa!\n");
                return;
            }
            Console.WriteLine("Titlul cautat nu exist!\n");
        }
        public static void getRelations()
        {
            pe = new pubsEntities();
            var titles = from t in pe.titles
                                        join a in pe.titleauthors on t.title_id equals a.title_id
                                        join autor in pe.authors on a.au_id equals autor.au_id
                                        select new
                                        {
                                            Titlu = t.title1,
                                            Fname = autor.au_fname,
                                            Lname = autor.au_lname
                                        };
            foreach(var title in titles)
            {
                Console.WriteLine(title.Titlu + "\t" + title.Fname + "\t" + title.Lname);
            }
        }
    }
}
