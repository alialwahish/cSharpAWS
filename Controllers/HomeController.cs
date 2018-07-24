using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cSharpBelt.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;



namespace cSharpBelt.Controllers
{
    public class HomeController : Controller
    {

        private MyContext _context;
        public HomeController(MyContext context)
        {


            _context = context;
            _context.SaveChanges();
        }




        [HttpGet("")]
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            return View();
        }


        [HttpGet("Registerd")]
        public IActionResult SignIn()
        {
            Console.WriteLine("Got inside registerd");


            return View("SignIn");
        }


        [HttpPost("Home/create")]

        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {

                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                user.Confirm_Password = Hasher.HashPassword(user, user.Confirm_Password);



                _context.Add(user);
                _context.SaveChanges();
                ViewBag.user = user;

                HttpContext.Session.SetString("Email", user.Email);
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Index");
            }


        }




        [HttpPost("Home/login")]

        public IActionResult LogingMethod(string Email, string Password)
        {

            User logUser = _context.users.SingleOrDefault(usr => usr.Email == Email);




            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            if (logUser != null && Password != null)
            {

                if (0 != Hasher.VerifyHashedPassword(logUser, logUser.Password, Password))
                {

                    HttpContext.Session.SetString("Email", logUser.Email);

                    return RedirectToAction("Dashboard");

                }
                else
                {

                    ViewBag.err = "Password or Username is not valid";
                    return View("SignIn");

                }


            }
            else
            {

                ViewBag.err = "Email or Password can't be empty";
                return View("SignIn");
            }


        }




        public IActionResult Dashboard() ////////////////// loading the dash board
        {

            string Email = HttpContext.Session.GetString("Email");
            User logUser = _context.users.SingleOrDefault(usr => usr.Email == Email);
            List<User> allUsers = _context.users.ToList();
            List<Ideas> allIdeas = _context.ideas.Include(a => a.Likes).OrderByDescending(a => a.NumOfLikes).ToList();

            


            ViewBag.allActs = allIdeas;
            ViewBag.user = logUser;
            ViewBag.allUsers = allUsers;


            return View("Dashboard");
        }

        [HttpPost("Home/createNewAct")]////////////////////////////////// creating new avtivity
        public IActionResult IdeaDetails(Ideas ide)
        {


            string Email = HttpContext.Session.GetString("Email");
            User logUser = _context.users.SingleOrDefault(usr => usr.Email == Email);


            if (!ModelState.IsValid)
            {
                List<Ideas> allIdeas = _context.ideas.Include(a => a.Likes).OrderByDescending(a => a.NumOfLikes).ToList();
                List<User> allUsers = _context.users.Include(u=>u.Likes).ToList();

                ViewBag.allUsers = allUsers;
                ViewBag.user = logUser;
                ViewBag.allActs = allIdeas;

                return View("Dashboard");
            }

            ide.UserId = logUser.UserId;

            logUser.Ideas.Add(ide);



            _context.SaveChanges();

            Ideas idea = _context.ideas.Include(a => a.Likes).FirstOrDefault(a => a.LikesId == ide.LikesId);
            ViewBag.user = logUser;

            ViewBag.idea = idea;

            return RedirectToAction("Dashboard");
            // return RedirectToAction("ViewActDetails",new {act.ActivitiesId});

        }


        [HttpGet("/Home/users/Liked/{id}")]
        public IActionResult usersLikes(int id)
        {

            Ideas idea = _context.ideas.Include(i => i.Likes).SingleOrDefault(i => i.IdeasId == id);

            List<User> likedUsers = new List<User>();

            List<Likes>postLikes = _context.likes.Include(l=>l.User).Include(l=>l.Ideas).Where(l=>l.IdeasId==idea.IdeasId).ToList();

            foreach(var u in postLikes){
                if(!likedUsers.Contains(u.User)){
                    likedUsers.Add(u.User);
                }
            }
            User creator=_context.users.SingleOrDefault(u=>u.UserId==idea.UserId);
            ViewBag.creator=creator;
            ViewBag.idea=idea;
            ViewBag.likedUsers=likedUsers;

            return View("UsersLikes");
        }











        [HttpGet("Home/userDetails/{id}")]
        public IActionResult userDetails(int id)
        {
            User userDet = _context.users.Include(u => u.Likes).Include(u => u.Ideas).SingleOrDefault(u => u.UserId == id);



            ViewBag.userDet = userDet;

            return View("UserDetails");
        }



        [HttpGet("Home/join/{id}")] ////////////////// joining Event
        public IActionResult join(int id)
        {
            string Email = HttpContext.Session.GetString("Email");
            User logUser = _context.users.Include(u => u.Ideas).SingleOrDefault(usr => usr.Email == Email);

            Ideas idea = _context.ideas.Include(i => i.Likes).SingleOrDefault(i => i.IdeasId == id);

            Likes like = new Likes()
            {
                User = logUser,
                Ideas = idea
            };
            idea.NumOfLikes++;
            logUser.Likes.Add(like);
            idea.Likes.Add(like);
            _context.Add(like);

            _context.SaveChanges();


            return RedirectToAction("Dashboard");


        }




        [HttpGet("Home/Delete/{id}")] ////////////////////// deleting event
        public IActionResult Delete(int id)
        {
            Ideas ide = _context.ideas.FirstOrDefault(a => a.IdeasId == id);


            _context.Remove(ide);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }


       

        [HttpGet("Home/navToDashboard")] ///////////////////////////// return to Dashboard
        public IActionResult NavToDash()
        {

            return RedirectToAction("Dashboard");
        }





     
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
