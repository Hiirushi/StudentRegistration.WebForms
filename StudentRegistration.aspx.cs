using StudentRegistration.Data;
using StudentRegistration.Models;
using StudentRegistration.Repositories;
using StudentRegistration.Services;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentRegistration.WebForms
{
    public partial class StudentRegistration : System.Web.UI.Page
    {
        private StudentService studentService;
        private CourseRespository courseRespository;

        private void InitializeDependencies()
        {
            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory();
            StudentRepository studentRepository = new StudentRepository(dbConnectionFactory);
            studentService = new StudentService(studentRepository);
            courseRespository = new CourseRespository(dbConnectionFactory);
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            InitializeDependencies();
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            ddlCourse.DataSource = courseRespository.GetAllCourses();

            ddlCourse.DataTextField = "CourseName";
            ddlCourse.DataValueField = "CourseId";

            ddlCourse.DataBind();

            ddlCourse.Items.Insert(0, new ListItem("-- Select Course --", "0"));
            ddlCourse.SelectedIndex = 0;
        }

        private void ClearFields()
        {
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtAddress.Text = "";
            txtDateOfBirth.Text = DateTime.Now.ToString();
            ddlCourse.SelectedIndex = 0;
        }
        
        protected void btnViewStudents_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentDetails.aspx");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            DateTime? dateOfBirth = null;

            if (!string.IsNullOrWhiteSpace(txtDateOfBirth.Text))
            {
                dateOfBirth = DateTime.Parse(txtDateOfBirth.Text);
            }

            Student student = new Student
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                DateOfBirth = dateOfBirth,
                CourseId = Convert.ToInt32(ddlCourse.SelectedValue)
            };

            try
            {
                studentService.RegisterStudent(student);

                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-success alert-dismissable fade show";
                lblMessage.Text = "Student registered successfully";

                ClearFields();
            }
            catch(Exception ex)
            {
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-danger alert-dismissable fade show";
                lblMessage.Text = ex.Message;
            }
        }
    }
}