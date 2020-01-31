using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }

        //TODO: Each line should be a separate method in this class
        // List
        public ActionResult List()
        {
            //what data do we need?
            List<Species> myspecies = db.Species.SqlQuery("Select * from species").ToList();

            return View(myspecies);
        }

        // Show
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            //string query = "select * from Species where SpeciesID=@SpeciesID";
            //SqlParameter sqlparam = new SqlParameter("@SpeciesID", id);
            //Species selectedSpecies = db.Species.SqlQuery(query, sqlparam).First();

            Species species = db.Species.SqlQuery("select * from Species where SpeciesID=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }
        // Add
        public ActionResult Add()
        {
            return View();
        }

        // [HttpPost] species/Add
        //goto views -> species -> add.cshtml
        [HttpPost]
        public ActionResult Add(string Name, string Breed)
        {
        
            string query = "insert into Species (Name, Breed) values (@Name,@Breed)";
            //to avoid sql injection we use sql parameterized query
            SqlParameter[] sqlparams = new SqlParameter[2]; //0,1 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@Name", Name);
            sqlparams[1] = new SqlParameter("@Breed", Breed);

            //DML operation used for inserting, updating, deleting
            db.Database.ExecuteSqlCommand(query,sqlparams);


            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
        }
        // Update
        // [HttpPost] Update
        public ActionResult Update(int id)
        {
            //need information about a particular pet
            Species selectedSpecies = db.Species.SqlQuery("select * from species where SpeciesID = @id", new SqlParameter("@id", id)).FirstOrDefault();

            return View(selectedSpecies);

        }
        [HttpPost]
        public ActionResult Update(int id, string Name, string Breed)
        {

            Debug.WriteLine("I am trying to edit a pet's name to " + Name);
            
            string query = "UPDATE Species SET Name = @Name, Breed = @Breed WHERE SpeciesID = @id";
            SqlParameter[] sqlparams = new SqlParameter[3]; //0,1 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@id", id);
            sqlparams[1] = new SqlParameter("@Name", Name);
            sqlparams[2] = new SqlParameter("@Breed", Breed);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }

        // (optional) delete
        public ActionResult Delete(int id)
        {

            string query = "Delete from Species WHERE speciesid=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);

            return RedirectToAction("List");
        }
        // [HttpPost] Delete
    }
}