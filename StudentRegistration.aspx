<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StudentRegistration.aspx.cs" Inherits="StudentRegistration.WebForms.StudentRegistration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Student Registration</title>

    <link
        href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
        rel="stylesheet" />

    <style>
        body {
            background-color: #f8f9fa;
            font-family: Arial, sans-serif;
        }

        .card {
            max-width: 500px;
            margin: 0px auto;
            border-radius: 12px;
        }

        .card-body {
            padding: 15px !important;
        }

        .form-label {
            font-weight: 600;
            margin-bottom: 4px;
            font-size: 14px;
        }

        .mb-3 {
            margin-bottom: 7px !important;
        }

        .form-control,
        .form-select {
            width: 100%;
            padding: 7px 10px;
            border-radius: 8px;
            border: 1px solid #ced4da;
            font-size: 14px;
        }

        .form-control:focus,
        .form-select:focus {
            border-color: #86b7fe;
            box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.15);
        }

    </style>
</head>
<body>
    <form id="studentRegistrationForm" runat="server">

        <div class="container mt-5 mb-3">
            <div class="card shadow-sm border rounded-3 pt-2 pb-2">
                <div class="card-body p-2 text-center">
                    <h4 class="mb-1 fw-bold">Student Registration</h4>
                    <small>Enter the student details below</small>
                </div>
                <div class="px-5 pt-2 pb-3">
                    <asp:Panel ID="pnlMessage" runat="server"
                        Visible="false"
                        CssClass="alert alert-dismissible fade show"
                        role="alert">

                        <asp:Label ID="lblMessage" runat="server" />

                        <button type="button" class="btn-close position-absolute top-0 end-0 m-2"
                            style="transform: scale(0.7);"
                            data-bs-dismiss="alert">
                        </button>
                    </asp:Panel>

                    <div class="mb-3">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name:" CssClass="form-label" />
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator
                            ID="rfvFirstName"
                            runat="server"
                            ControlToValidate="txtFirstName"
                            ErrorMessage="First name is required"
                            ForeColor="Red" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name:" CssClass="form-label" />
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator
                            ID="rfrLastName"
                            runat="server"
                            ControlToValidate="txtLastName"
                            ErrorMessage="Last name is required"
                            ForeColor="Red" />
                    </div>

                    <div class="mb-4">
                        <asp:Label ID="lblAddress" runat="server" Text="Address:" CssClass="form-label" />
                        <asp:TextBox ID="txtAddress" TextMode="MultiLine" Rows="2" runat="server" CssClass="form-control" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblDateOfBirth" runat="server" Text="Date Of Birth:" CssClass="form-label" />
                        <asp:TextBox ID="txtDateOfBirth" TextMode="Date" runat="server" CssClass="form-control" />
                        <asp:CompareValidator
                            ID="cvDateOfBirth"
                            runat="server"
                            ControlToValidate="txtDateOfBirth"
                            Operator="DataTypeCheck"
                            Type="Date"
                            ErrorMessage="Enter a Valid date"
                            ForeColor="Red" />
                    </div>

                    <div class="mb-3">
                        <asp:Label ID="lblCourse" runat="server" Text="Course:" CssClass="form-label" />
                        <asp:DropDownList ID="ddlCourse" runat="server" CssClass="form-control">
                            <asp:ListItem Text="-- Select Course --" Value="0" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rfvCourse"
                            runat="server"
                            ControlToValidate="ddlCourse"
                            InitialValue="0"
                            ErrorMessage="Please select a course"
                            ForeColor="Red" />
                    </div>

                    <div class="d-flex justify-content-center gap-3 mt-3">
                        <asp:Button
                            ID="btnSave"
                            runat="server"
                            Text="Save"
                            OnClick="btnSave_Click"
                            CssClass="btn btn-primary px-4" />

                        <asp:Button
                            ID="btnClear"
                            runat="server"
                            Text="Reset"
                            CssClass="btn btn-outline-primary px-4"
                            CausesValidation="false"
                            OnClick="btnReset_Click" />

                        <asp:Button
                            ID="btnViewStudents"
                            runat="server"
                            Text="View Students"
                            CssClass="btn btn-success px-4"
                            CausesValidation="false"
                            OnClick="btnViewStudents_Click" />
                    </div>
                </div>
            </div>
        </div>

        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    </form>
</body>
</html>
