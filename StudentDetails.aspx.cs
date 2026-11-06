using Microsoft.Reporting.WebForms;
using StudentRegistration.Data;
using StudentRegistration.Models;
using StudentRegistration.Repositories;
using StudentRegistration.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentRegistration.WebForms
{
    public partial class StudentDetails : System.Web.UI.Page
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
                LoadStudents();
                LoadSearchCourses();
            }
        }

        private void LoadStudents(string searchText = "", string courseID = "0")
        {
            DataTable students = studentService.GetAllStudents();

            DataView dataView = new DataView(students);

            string filter = "";

            if (!string.IsNullOrWhiteSpace(searchText))
            { 
                string search = searchText.Trim().Replace("'", "''");

                filter =
                    $"[Full Name] LIKE '%{search}%' OR " +
                    $"[Address] LIKE '%{search}%' ";
            }
            if(courseID != "0")
            {
                if (!string.IsNullOrEmpty(filter)){
                    filter = $"({filter}) AND ";
                }

                filter += $"[CourseID] = {Convert.ToInt32(courseID)}";
            }
            dataView.RowFilter = filter;
            gvStudents.DataSource = dataView;
            gvStudents.DataBind();
        }

        private void LoadSearchCourses()
        {
            ddlSearchCourse.DataSource = courseRespository.GetAllCourses();

            ddlSearchCourse.DataTextField = "CourseName";
            ddlSearchCourse.DataValueField = "CourseID";

            ddlSearchCourse.DataBind();

            ddlSearchCourse.Items.Insert(0, new ListItem("All Courses", "0"));
        }

        protected void gvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvStudents.PageIndex = e.NewPageIndex;
            LoadStudents();
        }

        protected void gvStudents_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                Repeater rptPages = (Repeater)e.Row.FindControl("rptPages");

                List<int> pages = new List<int>();

                for (int i=0; i< gvStudents.PageCount; i++)
                {
                    pages.Add(i);
                }

                rptPages.DataSource = pages;
                rptPages.DataBind();

                LinkButton btnPrevious = (LinkButton)e.Row.FindControl("btnPrevious");
                LinkButton btnNext = (LinkButton)e.Row.FindControl("btnNext");

                btnPrevious.Enabled = gvStudents.PageIndex > 0;
                btnNext.Enabled = gvStudents.PageIndex < gvStudents.PageCount - 1;
            }
        }

        protected void rptPages_ItemCommand(object sender, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SelectPage")
            {
                int pageIndex = Convert.ToInt32(e.CommandArgument);
                gvStudents.PageIndex = pageIndex;
                LoadStudents();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadStudents(txtSearch.Text.Trim(), ddlSearchCourse.SelectedValue);
        }

        protected void btnClearSearch_Click(Object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlSearchCourse.SelectedValue = "0";
            LoadStudents();
        }

        protected void gvStudents_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            pnlMessage.Visible = false;
            lblMessage.Text = "";
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
            Page.Validate("EditStudent");
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(
                    this,
                    GetType(),
                    "ShowEditModal",
                    "bootstrap.Modal.getOrCreateInstance(document.getElementById('editStudentModal')).show();",
                    true
            );
                return;
            }
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

                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-success alert-dismissible fade show position-relative";

                lblMessage.Text = "Student updated successfully";

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
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-danger alert-dismissible fade show position-relative";

                lblMessage.Text = ex.Message;

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

                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-success alert-dismissible fade show position-relative";
                lblMessage.Text = "Student deleted successfully";

                LoadStudents();
            }
            catch (Exception ex)
            {
                pnlMessage.Visible = true;
                pnlMessage.CssClass = "alert alert-danger alert-dismissible fade show position-relative";
                lblMessage.Text = ex.Message;

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
            pnlMessage.Visible = false;
            lblMessage.Text = "";
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