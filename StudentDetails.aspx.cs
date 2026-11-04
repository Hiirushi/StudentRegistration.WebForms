using Microsoft.Reporting.WebForms;
using StudentRegistration.Data;
using StudentRegistration.Models;
using StudentRegistration.Repositories;
using StudentRegistration.Services;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentRegistration.WebForms
{
    public partial class StudentDetails : System.Web.UI.Page
    {
        private StudentService studentService;
        private CourseRespository courseRespository;
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeDependencies();
            if (!IsPostBack)
            {
                LoadStudents();
            }
        }

        private void InitializeDependencies()
        {
            DbConnectionFactory dbConnectionFactory = new DbConnectionFactory();
            StudentRepository studentRepository = new StudentRepository(dbConnectionFactory);
            studentService = new StudentService(studentRepository);
            courseRespository = new CourseRespository(dbConnectionFactory);
        }

        private void LoadStudents()
        {
            gvStudents.DataSource = studentService.GetAllStudents();
            gvStudents.DataBind();
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EditStudent")
            {
                int studentID = Convert.ToInt32(e.CommandArgument);
                Student student = studentService.GetStudentByID(studentID);

                if (student != null)
                {
                    hfStudentID.Value = student.StudentId.ToString();

                    txtEditFirstName.Text = student.FirstName;
                    txtEditLastName.Text = student.LastName;
                    txtEditAddress.Text = student.Address;
                    txtEditDateOfBirth.Text = student.DateOfBirth.HasValue ?
                        student.DateOfBirth.Value.ToString("yyyy-MM-dd") : "";

                    LoadEditCourses();

                    ddlEditCourse.SelectedValue = student.CourseId.ToString();

                    ScriptManager.RegisterStartupScript(
                        this,
                        GetType(),
                        "ShowEditModal",
                        "var modal = new bootstrap.Modal(document.getElementById('editStudentModal')); modal.show();",
                        true);
                }
            }
            else if (e.CommandName == "ConfirmDelete")
            {
                hfDeleteStudentID.Value = e.CommandArgument.ToString();

                ClientScript.RegisterStartupScript(
                    GetType(),
                    "ShowDeleteModal",
                    "var modal = new bootstrap.Modal(document.getElementById('deleteStudentModal')); modal.show();",
                    true);
            }
        }

        private void LoadEditCourses()
        {
            ddlEditCourse.DataSource = courseRespository.GetAllCourses();

            ddlEditCourse.DataTextField = "CourseName";
            ddlEditCourse.DataValueField = "CourseID";

            ddlEditCourse.DataBind();
        }

        protected void btnAddStudent_Click(object sender, EventArgs e)
        {
            Response.Redirect("StudentRegistration.aspx");
        }
        protected void btnUpdateStudent_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? dateOfBirth = null;

                if (!string.IsNullOrWhiteSpace(txtEditDateOfBirth.Text))
                {
                    dateOfBirth = DateTime.Parse(txtEditDateOfBirth.Text);
                }

                Student student = new Student
                {
                    StudentId = Convert.ToInt32(hfStudentID.Value),
                    FirstName = txtEditFirstName.Text.Trim(),
                    LastName = txtEditLastName.Text.Trim(),
                    Address = txtEditAddress.Text.Trim(),
                    DateOfBirth = dateOfBirth,
                    CourseId = Convert.ToInt32(ddlEditCourse.SelectedValue)
                };

                studentService.UpdateStudent(student);

                lblMessage.Text = "Student updated successfully";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                LoadStudents();

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "CloseEditModal",
                    "var modal = bootstrap.Modal.getInstance(document.getElementById('editStudentModal')); if (modal) modal.hide();",
                    true
                  );
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;

                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "ShowEditModal",
                    "var modal = bootstrap.Modal.getInstance(document.getElementById('editStudentModal')); if (modal) modal.show();",
                    true
                  );
            }
        }

        protected void btnConfirmDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int studentID = Convert.ToInt32(hfDeleteStudentID.Value);
                studentService.DeleteStudent(studentID);

                lblMessage.Text = "Student deleted successfully";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                LoadStudents();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;

                ClientScript.RegisterStartupScript(
                    GetType(),
                    "ShowDeleteModal",
                    "var modal = new bootstrap.Modal(document.getElementById('deleteStudentModal')); modal.show();",
                    true
                 );
            }
        }
        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable students = studentService.GetStudentsForReport();

                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/StudentReport.rdlc");

                ReportViewer1.LocalReport.DataSources.Clear();

                ReportDataSource reportDataSource = new ReportDataSource("StudentReport", students);

                ReportViewer1.LocalReport.DataSources.Add(reportDataSource);
                ReportViewer1.LocalReport.Refresh();
                ReportViewer1.Visible = true;
            }
            catch(Exception ex)
            {
                lblMessage.Text = ex.Message;
                lblMessage.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}