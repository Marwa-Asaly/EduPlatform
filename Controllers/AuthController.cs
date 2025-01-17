using EduPlatformProject.Classes;
using EduPlatformProject.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using System.Data;

namespace EduPlatformProject.Controllers
{
    public class AuthController : Controller
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public AuthController(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            bool succeeded = false;
            var cmd = new SqlDataAdapter("SELECT * FROM users WHERE email = @e;", Database.ConnStr);
            cmd.SelectCommand.Parameters.Add(new SqlParameter("@e", email));


            DataTable tbl = new();
            cmd.Fill(tbl);

            string hpwd = Hasher.Hash(password);
            if (tbl.Rows.Count > 0)
            {
                var user = tbl.Rows[0];
                if (user["password"].ToString() == hpwd)
                {
                    HttpContext.Session.SetString("id", user["id"].ToString());
                    HttpContext.Session.SetString("first_name", user["first_name"].ToString());
                    HttpContext.Session.SetString("last_name", user["last_name"].ToString());
                    HttpContext.Session.SetString("email", user["email"].ToString());
                    succeeded = true;
                }
            }

            if (succeeded)
            {
                return Redirect("/");
            }
            else
            {
                ViewBag.Error = _localizer["InvalidLogin"];
                return View();
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(string firstName, string lastName, string email, string password)
        {
            var conn = new SqlConnection(Database.ConnStr);
            conn.Open();

            var checkCmd = new SqlCommand("SELECT 1 FROM users WHERE email = @e;", conn);
            checkCmd.Parameters.AddWithValue("@e", email);

            var reader = checkCmd.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Close();
                ViewBag.Error = _localizer["AccountExists"];
                return View();
            }
            reader.Close();

            var cmd = new SqlCommand("INSERT INTO users (id, first_name, last_name, email, password) VALUES (NEWID(), @firstName, @lastName, @Email, @Password)", conn);

            cmd.Parameters.AddWithValue("@firstName", firstName);
            cmd.Parameters.AddWithValue("@lastName", lastName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", Hasher.Hash(password));

            cmd.ExecuteNonQuery();
            conn.Close();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string emailAddress)
        {
            ViewBag.msg = _localizer["MessageSent"];
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
