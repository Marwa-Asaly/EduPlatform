using EduPlatformProject.Classes;
using EduPlatformProject.Models;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Globalization;

namespace EduPlatformProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var adp = new SqlDataAdapter("SELECT * FROM categories;", Database.ConnStr);
            DataTable tbl = new();
            adp.Fill(tbl);

            var adpCourses = new SqlDataAdapter("SELECT * FROM courses WHERE in_home = 1;", Database.ConnStr);
            DataTable tblC = new();
            adpCourses.Fill(tblC);

            ViewBag.Categories = tbl;
            ViewBag.Courses = tblC;
            return View();
        }

        public IActionResult Assignments()
        {
            var adp = new SqlDataAdapter("SELECT * FROM assignments;", Database.ConnStr);
            DataTable tbl = new();
            adp.Fill(tbl);

            ViewBag.Assignments = tbl;
            return View();
        }

        public IActionResult SubmitAssignment(IFormFile file, string assignment_id)
        {
            var conn = new SqlConnection(Database.ConnStr);
            var cmd = new SqlCommand("INSERT INTO assignment_submittions (id, [file], submitted_at, submitted_by, assignment_id) VALUES(NEWID(), @a, @b, @c, @d)", conn);
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                cmd.Parameters.Add(new SqlParameter("@a", Convert.ToBase64String(fileBytes)));
            }
            cmd.Parameters.Add(new SqlParameter("@b", DateTime.Now));
            cmd.Parameters.Add(new SqlParameter("@c", HttpContext.Session.GetString("id")));
            cmd.Parameters.Add(new SqlParameter("@d", assignment_id));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("Assignments");
        }

        public IActionResult Exams()
        {
            var adp = new SqlDataAdapter("SELECT * FROM exams;", Database.ConnStr);
            DataTable tbl = new();
            adp.Fill(tbl);

            ViewBag.Exams = tbl;
            return View();
        }

        public IActionResult SwitchLanguage(string lang, string returnUrl)
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return Redirect(!string.IsNullOrWhiteSpace(returnUrl) ? returnUrl : "/");
        }

        public IActionResult CreateExam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateExam(string title_ar, string title_tr, string content_ar, string content_tr,
            string q1, string q11, string q12, string q13, string q2, string q21, string q22, string q23, string q3, string q31, string q4, string q41)
        {
            var conn = new SqlConnection(Database.ConnStr);
            var cmd = new SqlCommand("INSERT INTO exams (id, title_ar, title_tr, content_ar, content_tr) VALUES(@id, @a, @b, @c, @d)", conn);
            string examId = Guid.NewGuid().ToString();
            cmd.Parameters.Add(new SqlParameter("@id", examId));
            cmd.Parameters.Add(new SqlParameter("@a", title_ar));
            cmd.Parameters.Add(new SqlParameter("@b", title_tr));
            cmd.Parameters.Add(new SqlParameter("@c", content_ar));
            cmd.Parameters.Add(new SqlParameter("@d", content_tr));

            var question = new SqlCommand(@"INSERT INTO exam_questions 
            (id, question_ar, question_tr, exam_id, answer_1_ar, answer_2_ar, answer_3_ar, answer_1_tr, answer_2_tr, answer_3_tr, is_multiple_choice) 
            VALUES(NEWID(), @a, @b, @c, @q1, @q2, @q3, @q11, @q22, @q33, 1)", conn);
            question.Parameters.Add(new SqlParameter("@a", q1));
            question.Parameters.Add(new SqlParameter("@b", q2));
            question.Parameters.Add(new SqlParameter("@c", examId));
            question.Parameters.Add(new SqlParameter("@q1", q11));
            question.Parameters.Add(new SqlParameter("@q2", q12));
            question.Parameters.Add(new SqlParameter("@q3", q13));
            question.Parameters.Add(new SqlParameter("@q11", q21));
            question.Parameters.Add(new SqlParameter("@q22", q22));
            question.Parameters.Add(new SqlParameter("@q33", q23));

            var question2 = new SqlCommand(@"INSERT INTO exam_questions 
            (id, question_ar, question_tr, exam_id, answer_1_ar, answer_1_tr, is_multiple_choice) 
            VALUES(NEWID(), @a, @b, @c, @q1, @q2, 0)", conn);
            question2.Parameters.Add(new SqlParameter("@a", q3));
            question2.Parameters.Add(new SqlParameter("@b", q4));
            question2.Parameters.Add(new SqlParameter("@c", examId));
            question2.Parameters.Add(new SqlParameter("@q1", q31));
            question2.Parameters.Add(new SqlParameter("@q2", q41));

            conn.Open();
            cmd.ExecuteNonQuery();
            question.ExecuteNonQuery();
            question2.ExecuteNonQuery();
            conn.Close();
            return Redirect("/");
        }

        public IActionResult CreateAssignment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SubmitExam(string exam_id, string answer_1, string answer_2)
        {
            int grade = 0;
            var exam = new SqlDataAdapter($"SELECT * FROM exam_questions WHERE exam_id = '{exam_id}' ORDER BY is_multiple_choice DESC", Database.ConnStr);
            bool isArabic = CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "ar";
            var tbl = new DataTable();
            exam.Fill(tbl);
            if (answer_1 == (tbl.Rows[0][isArabic ? "answer_3_ar" : "answer_3_tr"]).ToString())
            {
                grade++;
            }
            if (answer_2 == (tbl.Rows[1][isArabic ? "answer_1_ar" : "answer_1_tr"]).ToString())
            {
                grade++;
            }
            var conn = new SqlConnection(Database.ConnStr);
            var cmd = new SqlCommand("INSERT INTO exam_submittions (id, user_id, exam_id, grade) VALUES(NEWID(), @a, @a1, @a3)", conn);
            cmd.Parameters.Add(new SqlParameter("@a1", exam_id));
            cmd.Parameters.Add(new SqlParameter("@a", HttpContext.Session.GetString("id")));
            cmd.Parameters.Add(new SqlParameter("@a3", grade));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return RedirectToAction("exams");
        }

        [HttpPost]
        public IActionResult CreateAssignment(string title_ar, string title_tr, string content_ar, string content_tr, DateTime deadline)
        {
            var conn = new SqlConnection(Database.ConnStr);
            var cmd = new SqlCommand("INSERT INTO assignments (id, title_ar, title_tr, content_ar, content_tr, deadline) VALUES(NEWID(), @a, @b, @c, @d, @e)", conn);
            cmd.Parameters.Add(new SqlParameter("@a", title_ar));
            cmd.Parameters.Add(new SqlParameter("@b", title_tr));
            cmd.Parameters.Add(new SqlParameter("@c", content_ar));
            cmd.Parameters.Add(new SqlParameter("@d", content_tr));
            cmd.Parameters.Add(new SqlParameter("@e", deadline));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(string title_ar, string title_tr, IFormFile photo)
        {
            var conn = new SqlConnection(Database.ConnStr);
            var cmd = new SqlCommand("INSERT INTO categories (id, title_ar, title_tr, photo) VALUES(NEWID(), @a, @b, @c)", conn);
            cmd.Parameters.Add(new SqlParameter("@a", title_ar));
            cmd.Parameters.Add(new SqlParameter("@b", title_tr));
            using (var ms = new MemoryStream())
            {
                photo.CopyTo(ms);
                var fileBytes = ms.ToArray();
                cmd.Parameters.Add(new SqlParameter("@c", Convert.ToBase64String(fileBytes)));
            }

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return Redirect("/");
        }

        [HttpGet]
        public IActionResult CreateCourse()
        {
            var adp = new SqlDataAdapter("SELECT * FROM categories;", Database.ConnStr);
            DataTable tbl = new();
            adp.Fill(tbl);

            ViewBag.Categories = tbl;
            return View();
        }

        [HttpPost]
        public IActionResult CreateCourse(string title_ar, string title_tr, string desc_ar, string desc_tr, string category_id, string inHome)
        {
            bool in_home = inHome == "on";
            var adp = new SqlDataAdapter("SELECT * FROM categories;", Database.ConnStr);
            DataTable tbl = new();
            adp.Fill(tbl);

            ViewBag.Categories = tbl;

            var conn = new SqlConnection(Database.ConnStr);
            var cmd = new SqlCommand("INSERT INTO courses (id, title_ar, title_tr, content_ar, content_tr, category_id, in_home) VALUES(NEWID(), @a, @b, @c, @d, @e, @ih)", conn);
            cmd.Parameters.Add(new SqlParameter("@a", title_ar));
            cmd.Parameters.Add(new SqlParameter("@b", title_tr));
            cmd.Parameters.Add(new SqlParameter("@c", desc_ar ?? ""));
            cmd.Parameters.Add(new SqlParameter("@d", desc_tr ?? ""));
            cmd.Parameters.Add(new SqlParameter("@e", category_id));
            cmd.Parameters.Add(new SqlParameter("@ih", in_home));

            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            return Redirect("/");
        }

        public IActionResult Category(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect("/");
            }

            var adp = new SqlDataAdapter("SELECT * FROM categories WHERE id = @i;", Database.ConnStr);
            adp.SelectCommand.Parameters.Add(new SqlParameter("@i", id));
            DataTable tbl = new();
            adp.Fill(tbl);

            var adpCourses = new SqlDataAdapter("SELECT * FROM courses WHERE category_id = @i;", Database.ConnStr);
            adpCourses.SelectCommand.Parameters.Add(new SqlParameter("@i", id));
            DataTable tblCourses = new();
            adpCourses.Fill(tblCourses);

            ViewBag.Courses = tblCourses;
            ViewBag.Category = tbl.Rows[0];

            return View();
        }

        public IActionResult Course(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Redirect("/");
            }

            var adp = new SqlDataAdapter("SELECT * FROM courses WHERE id = @i;", Database.ConnStr);
            adp.SelectCommand.Parameters.Add(new SqlParameter("@i", id));
            DataTable tbl = new();
            adp.Fill(tbl);
            ViewBag.Course = tbl.Rows[0];

            return View();
        }

        public IActionResult AddToFav(string id, string returnUrl)
        {
            var adp = new SqlDataAdapter("SELECT * FROM favorites WHERE course_id = @i AND user_id = @ii;", Database.ConnStr);
            adp.SelectCommand.Parameters.Add(new SqlParameter("@i", id));
            adp.SelectCommand.Parameters.Add(new SqlParameter("@ii", HttpContext.Session.GetString("id")));
            DataTable tbl = new();
            adp.Fill(tbl);
            if (tbl.Rows.Count == 0)
            {
                var conn = new SqlConnection(Database.ConnStr);
                var cmd = new SqlCommand("INSERT INTO favorites (id, course_id, user_id) VALUES(NEWID(), @a, @b)", conn);
                cmd.Parameters.Add(new SqlParameter("@a", id));
                cmd.Parameters.Add(new SqlParameter("@b", HttpContext.Session.GetString("id")));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return Redirect(!string.IsNullOrWhiteSpace(returnUrl) ? returnUrl : "/");
        }

        public IActionResult RemoveFromFav(string id, string returnUrl)
        {
            var adp = new SqlDataAdapter("SELECT * FROM favorites WHERE course_id = @i AND user_id = @ii;", Database.ConnStr);
            adp.SelectCommand.Parameters.Add(new SqlParameter("@i", id));
            adp.SelectCommand.Parameters.Add(new SqlParameter("@ii", HttpContext.Session.GetString("id")));
            DataTable tbl = new();
            adp.Fill(tbl);
            if (tbl.Rows.Count > 0)
            {
                var conn = new SqlConnection(Database.ConnStr);
                var cmd = new SqlCommand("DELETE FROM favorites WHERE course_id = @a AND user_id = @b;", conn);
                cmd.Parameters.Add(new SqlParameter("@a", id));
                cmd.Parameters.Add(new SqlParameter("@b", HttpContext.Session.GetString("id")));

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return Redirect(!string.IsNullOrWhiteSpace(returnUrl) ? returnUrl : "/");
        }

        public IActionResult Favorites()
        {
            if (HttpContext.Session.GetString("id") == null)
            {
                return Redirect("/");
            }

            var query = @"SELECT * FROM courses a LEFT JOIN favorites b
                    ON a.id = b.course_id
                    WHERE b.user_id = @userId";

            var adp = new SqlDataAdapter(query, Database.ConnStr);
            adp.SelectCommand.Parameters.Add(new SqlParameter("@userId", HttpContext.Session.GetString("id")));
            DataTable tbl = new();
            adp.Fill(tbl);

            ViewBag.Courses = tbl;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
