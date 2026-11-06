<%@ Register Assembly="Microsoft.ReportViewer.WebForms"
    Namespace="Microsoft.Reporting.WebForms"
    TagPrefix="rsweb" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentDetails.aspx.cs" Inherits="StudentRegistration.WebForms.StudentDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student List</title>
    <link
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
        rel="stylesheet" />

    <link rel="stylesheet"
      href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css" />

    <style>
        /* Pagination container */
        #gvStudents tr:last-child td {
            padding: 10px;
        }

        /* Disabled pagination buttons */
        #gvStudents .aspNetDisabled {
            opacity: 0.5;
            pointer-events: none;
        }
    </style>
</head>
<body>
    <form id="studentDetailsForm" runat="server">

        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <div class="container mt-5">
            <div class="d-flex align-items-center mb-4">
                <h3>Student Details</h3>
                <div class="d-flex gap-2 ms-auto">
                    <asp:Button
                        ID="btnAddStudent"
                        runat="server"
                        Text="Add New Student"
                        CssClass="btn btn-primary"
                        CausesValidation="false"
                        OnClick="btnAddStudent_Click" />

                    <asp:Button
                        ID="btnGenerateReport"
                        runat="server"
                        Text="Generate Report"
                        CssClass="btn btn-success"
                        CausesValidation="False"
                        OnClick="btnGenerateReport_Click" 
                        ToolTip="Generate student details report"/>
                </div>
            </div>

            <div class="card-body p-4">
                <asp:Panel
                    ID="pnlMessage"
                    runat="server"
                    Visible="false"
                    CssClass="alert alert-dismissible fade show position-relative"
                    role="alert">

                    <asp:Label ID="lblMessage" runat="server" />

                    <button type="button"
                        class="btn-close position-absolute top-0 end-0"
                        style="transform: scale(0.7);"
                        data-bs-dismiss="alert">
                    </button>

                </asp:Panel>

                <div class="table-responsive">

                    <!-- Search -->
                    <div class="d-flex gap-2 mb-3 align-items-center">
                        <asp:TextBox
                            ID="txtSearch"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Search students..." />

                        <asp:DropDownList
                            ID="ddlSearchCourse"
                            runat="server"
                            CssClass="form-select"
                            Style="width: 200px;">
                        </asp:DropDownList>

                        <asp:Button
                            ID="btnSearch"
                            runat="server"
                            Text="Search"
                            CssClass="btn btn-primary"
                            OnClick="btnSearch_Click"
                            CausesValidation="false" />

                        <asp:Button
                            ID="btnClearSearch"
                            runat="server"
                            Text="Clear"
                            CssClass="btn btn-outline-secondary"
                            OnClick="btnClearSearch_Click"
                            CausesValidation="false" />
                    </div>

                    <asp:GridView
                        ID="gvStudents"
                        runat="server"
                        AutoGenerateColumns="false"
                        DataKeyNames="StudentID"
                        AllowPaging="true"
                        PageSize="5"
                        OnRowCreated="gvStudents_RowCreated"
                        OnPageIndexChanging="gvStudents_PageIndexChanging"
                        OnRowCommand="gvStudents_RowCommand"
                        EmptyDataText="No students registered yet"
                        CssClass="table table-striped table-hover table-bordered align-middle"
                        GridLines="none">

                        <Columns>
                            <asp:BoundField
                                DataField="Full Name"
                                HeaderText="Full Name" />

                            <asp:BoundField
                                DataField="Address"
                                HeaderText="Address" />

                            <asp:BoundField
                                DataField="Date of Birth"
                                HeaderText="Date of Birth"
                                DataFormatString="{0:yyyy-MM-dd}"
                                HtmlEncode="false" />

                            <asp:BoundField
                                DataField="Course"
                                HeaderText="Course" />

                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:LinkButton
                                        ID="btnEdit"
                                        runat="server"
                                        CommandName="EditStudent"
                                        CssClass="btn btn-primary btn-sm"
                                        CommandArgument='<%# Eval("StudentID") %>'
                                        CausesValidation="false" 
                                        ToolTip="Edit Student">
                                    <i class="bi bi-pencil-square"></i>
                                        </asp:LinkButton>

                                    <asp:LinkButton
                                        ID="btnDelete"
                                        runat="server"
                                        CommandName="ConfirmDelete"
                                        CssClass="btn btn-danger btn-sm"
                                        CommandArgument='<%# Eval("StudentID") %>'
                                        CausesValidation="false" 
                                        ToolTip="Delete Student">
                                    <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <PagerStyle HorizontalAlign="Center" />
                        <PagerTemplate>
                            <div class="d-flex justify-content-center align-items-center gap-2 mt-3">
                                <asp:LinkButton
                                    ID="btnPrevious"
                                    runat="server"
                                    Text="Previous"
                                    CommandName="Page"
                                    CommandArgument="Prev"
                                    CssClass="btn btn-outline-primary btn-sm" />

                                <asp:Repeater
                                    ID="rptPages"
                                    runat="server"
                                    OnItemCommand="rptPages_ItemCommand" >

                                    <ItemTemplate>
                                        <asp:LinkButton
                                            ID="btnPage"
                                            runat="server"
                                            Text='<%# Convert.ToInt32(Container.DataItem) + 1 %>'
                                            CommandName="SelectPage"
                                            CommandArgument='<%# Container.DataItem %>'
                                            CssClass='<%# Convert.ToInt32(Container.DataItem) == gvStudents.PageIndex ? "btn btn-primary btn-sm" : "btn btn-outline-primary btn-sm" %>' />
                                    </ItemTemplate>

                                </asp:Repeater>

                                <asp:LinkButton
                                    ID="btnNext"
                                    runat="server"
                                    Text="Next"
                                    CommandName="Page"
                                    CommandArgument="Next"
                                    CssClass="btn btn-outline-primary btn-sm" />
                            </div>

                        </PagerTemplate>
                    </asp:GridView>

                    <div class="mt-4">
                        <rsweb:ReportViewer
                            ID="ReportViewer1"
                            runat="server"
                            Width="100%"
                            Height="600px"
                            ProcessingMode="Local"
                            Visible="false">
                        </rsweb:ReportViewer>
                    </div>

                    <div class="modal fade" id="editStudentModal" tabindex="-1">
                        <div class="modal-dialog modal-dialog-centered">
                            <div class="modal-content">

                                <div class="modal-header">
                                    <h5 class="modal-title">Edit Student Details</h5>

                                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                                </div>

                                <div class="modal-body">
                                    <asp:HiddenField
                                        ID="hfStudentID"
                                        runat="server" />

                                    <div class="mb-3">
                                        <label>First Name</label>
                                        <asp:TextBox
                                            ID="txtEditFirstName"
                                            runat="server"
                                            CssClass="form-control" />
                                        <asp:RequiredFieldValidator
                                            ID="rfvEditFirstName"
                                            runat="server"
                                            ControlToValidate="txtEditFirstName"
                                            ErrorMessage="First name is required"
                                            ForeColor="Red"
                                            Display="Dynamic"
                                            ValidationGroup="EditStudent" />
                                    </div>

                                    <div class="mb-3">
                                        <label>Last Name</label>
                                        <asp:TextBox
                                            ID="txtEditLastName"
                                            runat="server"
                                            CssClass="form-control" />
                                        <asp:RequiredFieldValidator
                                            ID="rfrEditLastName"
                                            runat="server"
                                            ControlToValidate="txtEditLastName"
                                            ErrorMessage="Last name is required"
                                            ForeColor="Red"
                                            Display="Dynamic"
                                            ValidationGroup="EditStudent" />
                                    </div>

                                    <div class="mb-3">
                                        <label>Address</label>
                                        <asp:TextBox
                                            ID="txtEditAddress"
                                            runat="server"
                                            CssClass="form-control" />
                                    </div>

                                    <div class="mb-3">
                                        <label>Date of Birth</label>
                                        <asp:TextBox
                                            ID="txtEditDateOfBirth"
                                            runat="server"
                                            TextMode="Date"
                                            CssClass="form-control" />
                                        <asp:CompareValidator
                                            ID="cvEditDateOfBirth"
                                            runat="server"
                                            ControlToValidate="txtEditDateOfBirth"
                                            Operator="DataTypeCheck"
                                            Type="Date"
                                            ErrorMessage="Enter a Valid date"
                                            ForeColor="Red"
                                            Display="Dynamic"
                                            ValidationGroup="EditStudent" />
                                    </div>

                                    <div class="mb-3">
                                        <label>Course</label>
                                        <asp:DropDownList
                                            ID="ddlEditCourse"
                                            runat="server"
                                            CssClass="form-select" />
                                        <asp:RequiredFieldValidator
                                            ID="rfvEditCourse"
                                            runat="server"
                                            ControlToValidate="ddlEditCourse"
                                            InitialValue="0"
                                            ErrorMessage="Please select a course"
                                            ForeColor="Red"
                                            Display="Dynamic"
                                            ValidationGroup="EditStudent" />
                                    </div>

                                    <div class="modal-footer">
                                        <button
                                            type="button"
                                            class="btn btn-secondary"
                                            data-bs-dismiss="modal">
                                            Cancel
                                        </button>

                                        <asp:Button
                                            ID="btnUpdateStudent"
                                            runat="server"
                                            Text="Update"
                                            CssClass="btn btn-primary"
                                            OnClick="btnUpdateStudent_Click"
                                            CausesValidation="true"
                                            ValidationGroup="EditStudent" />
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal fade" id="deleteStudentModal" tabindex="-1">
                        <div class="modal-dialog modal-dialog-centered modal-sm">
                            <div class="modal-content">

                                <div class="modal-header">
                                    <h5 class="modal-title">Confirm Delete</h5>

                                    <button
                                        type="button"
                                        class="btn-close"
                                        data-bs-dismiss="modal">
                                    </button>
                                </div>

                                <div class="modal-body">
                                    Are you sure you want to delete this student?
                                </div>

                                <div class="modal-footer">
                                    <button
                                        type="button"
                                        class="btn btn-secondary"
                                        data-bs-dismiss="modal">
                                        Cancel
                                    </button>

                                    <asp:Button
                                        ID="btnConfirmDelete"
                                        runat="server"
                                        Text="Delete"
                                        CssClass="btn btn-danger"
                                        CausesValidation="false"
                                        OnClick="btnConfirmDelete_Click" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <asp:HiddenField
                        ID="hfDeleteStudentID"
                        runat="server" />

                    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
