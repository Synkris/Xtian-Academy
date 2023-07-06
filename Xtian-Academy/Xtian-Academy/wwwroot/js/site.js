// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function ChangeColorToValid(selectedClass) {

    for (var i = 0; i < selectedClass.length; i++) {
        selectedClass[i].style.color = "green";
    }
}
function ChangeColorToInValid(selectedClass) {

    for (var i = 0; i < selectedClass.length; i++) {
        selectedClass[i].style.color = "gray";
    }
}

function HideFormPage11Next() {
    //FORM SECTION INDICATORS
    var registrationFormValidation = document.getElementsByClassName("tickcheck1");
    var personFormValidation = document.getElementsByClassName("tickcheck2");
    var skillFormValidation = document.getElementsByClassName("tickcheck3");

    // FORM INPUTS FOR VALIDATING SCRIPT
    var fName = document.getElementById("FirstName").value;
    var lName = document.getElementById("LastName").value;
    var noba = document.getElementById("PhoneNo").value;
    var userNam = document.getElementById("UserName").value;
    var email = document.getElementById("Email").value;
    var passwrd = document.getElementById("Passwrd").value;
    var cPasswrd = document.getElementById("Cpasswrd").value;
    var address = document.getElementById("Address").value;


    if (fName != "" && lName != "" && noba != "" && userNam != "" && email != "" && passwrd != "" && cPasswrd != "" && passwrd == cPasswrd && address != "") {

        document.getElementById("form11").style.display = "none";
        document.getElementById("form22").style.display = "block";
        document.getElementById("form33").style.display = "none";
        ChangeColorToValid(registrationFormValidation);
        ChangeColorToInValid(personFormValidation);
        ChangeColorToInValid(skillFormValidation);
    }
    else {
        document.getElementById("form11").style.display = "block";
        document.getElementById("form22").style.display = "none";
        document.getElementById("form33").style.display = "none";
        ChangeColorToInValid(registrationFormValidation);
        ChangeColorToInValid(personFormValidation);
        ChangeColorToInValid(skillFormValidation);
        if (fName == "") {
            document.getElementById("FNameValidation").style.display = "block";
        } else {
            document.getElementById("FNameValidation").style.display = "none";
        }
        if (lName == "") {
            document.getElementById("LNameValidation").style.display = "block";
        } else {
            document.getElementById("LNameValidation").style.display = "none";
        }
        if (noba == "") {
            document.getElementById("TelValidation").style.display = "block";
        } else {
            document.getElementById("TelValidation").style.display = "none";
        }
        if (userNam == "") {
            document.getElementById("UserNameValidation").style.display = "block";
        } else {
            document.getElementById("UserNameValidation").style.display = "none";
        }
        if (email == "") {
            document.getElementById("EmailValidation").style.display = "block";
        } else {
            document.getElementById("EmailValidation").style.display = "none";
        }
        if (passwrd == "") {
            document.getElementById("PasswordValidation").style.display = "block";
        } else {
            document.getElementById("PasswordValidation").style.display = "none";
        }
        if (cPasswrd != passwrd) {
            document.getElementById("ConfirmValidation").style.display = "block";
        } else {
            document.getElementById("ConfirmValidation").style.display = "none";
        }
        if (address == "") {
            document.getElementById("AddressValidation").style.display = "block";
        } else {
            document.getElementById("AddressValidation").style.display = "none";
        }
    }

}

// Registration for page 2 Button Next
function HideFormPage22Next() {
    //FORM SECTION INDICATORS
    var registrationFormValidation = document.getElementsByClassName("tickcheck1");
    var personFormValidation = document.getElementsByClassName("tickcheck2");
    var skillFormValidation = document.getElementsByClassName("tickcheck3");

    // FORM INPUTS FOR VALIDATING SCRIPT
    var jNysc = document.getElementById("nysc").value;
    var jLaptop = document.getElementById("laptop").value;
    var jProgrammingExp = document.getElementById("programmingExp").value;
    var jProgrammingExpList = document.getElementById("programmingExpList").value;
    var jStateOfResident = document.getElementById("stateOfResident").value;
    var jReasons = document.getElementById("reasons").value;

    if (jNysc != 0 && jLaptop != 0 && jProgrammingExp != 0 && jStateOfResident != 0 && jReasons != "") {
        if (jProgrammingExp == 1 && jProgrammingExpList == "") {
            document.getElementById("form11").style.display = "none";
            document.getElementById("form22").style.display = "block";
            document.getElementById("form33").style.display = "none";
            ChangeColorToValid(registrationFormValidation);
            ChangeColorToInValid(personFormValidation);
            ChangeColorToInValid(skillFormValidation);
            document.getElementById("ProgrammingExpListValidation").style.display = "block";
        } else {
            document.getElementById("form11").style.display = "none";
            document.getElementById("form22").style.display = "none";
            document.getElementById("form33").style.display = "block";
            ChangeColorToValid(registrationFormValidation);
            ChangeColorToValid(personFormValidation);
            ChangeColorToInValid(skillFormValidation);
        }
    }
    else {
        document.getElementById("form11").style.display = "none";
        document.getElementById("form22").style.display = "block";
        document.getElementById("form33").style.display = "none";
        ChangeColorToValid(registrationFormValidation);
        ChangeColorToInValid(personFormValidation);
        ChangeColorToInValid(skillFormValidation);
        if (jNysc == 0) {
            document.getElementById("NyscValidation").style.display = "block";
        } else {
            document.getElementById("NyscValidation").style.display = "none";
        }
        if (jLaptop == 0) {
            document.getElementById("LaptopValidation").style.display = "block";
        } else {
            document.getElementById("LaptopValidation").style.display = "none";
        }
        if (jProgrammingExp == 0) {
            document.getElementById("programmingExpValidation").style.display = "block";
        } else {
            document.getElementById("programmingExpValidation").style.display = "none";
        }
        if (jStateOfResident == 0) {
            document.getElementById("StateOfResidentValidation").style.display = "block";
        } else {
            document.getElementById("StateOfResidentValidation").style.display = "none";
        }
        if (jReasons == "") {
            document.getElementById("ReasonValidation").style.display = "block";
        } else {
            document.getElementById("ReasonValidation").style.display = "none";
        }
    }

}

// Registration for page 2 Button Previous
function HideFormPage22Previous() {
    document.getElementById("form11").style.display = "block";
    document.getElementById("form22").style.display = "none";
    document.getElementById("form33").style.display = "none";
    var registrationFormValidation = document.getElementsByClassName("tickcheck1");
    var personFormValidation = document.getElementsByClassName("tickcheck2");
    var skillFormValidation = document.getElementsByClassName("tickcheck3");

    ChangeColorToInValid(registrationFormValidation);
    ChangeColorToInValid(personFormValidation);
    ChangeColorToInValid(skillFormValidation);
}

// Registration for page 3 Button Previous
function HideFormPage33Previous() {

    document.getElementById("form11").style.display = "none";
    document.getElementById("form22").style.display = "block";
    document.getElementById("form33").style.display = "none";
    var registrationFormValidation = document.getElementsByClassName("tickcheck1");
    var personFormValidation = document.getElementsByClassName("tickcheck2");
    var skillFormValidation = document.getElementsByClassName("tickcheck3");

    ChangeColorToValid(registrationFormValidation);
    ChangeColorToInValid(personFormValidation);
    ChangeColorToInValid(skillFormValidation);
}

// Registration for page 3 Button Submt
function ApplicationRequest() {

    var registrationFormValidation = document.getElementsByClassName("tickcheck1");
    var personFormValidation = document.getElementsByClassName("tickcheck2");
    var skillFormValidation = document.getElementsByClassName("tickcheck3");
    var skll = document.getElementById("skills").value;
    var surv = document.getElementById("survive").value;
    var cv = document.getElementById("cv").value;


    if (skll != "" && surv != "" && cv != "") {
        ChangeColorToValid(registrationFormValidation);
        ChangeColorToValid(personFormValidation);
        ChangeColorToValid(skillFormValidation);

        ApplicationRequestMain();
    }
    else {
        ChangeColorToValid(registrationFormValidation);
        ChangeColorToValid(personFormValidation);
        ChangeColorToInValid(skillFormValidation);

        if (skll == "") {
            document.getElementById("SkillsValidation").style.display = "block";
        } else {
            document.getElementById("SkillsValidation").style.display = "none";
        }
        if (surv == "") {
            document.getElementById("SurviveValidation").style.display = "block";
        } else {
            document.getElementById("SurviveValidation").style.display = "none";
        }
        if (cv == "") {
            document.getElementById("CvValidation").style.display = "block";
        } else {
            document.getElementById("CvValidation").style.display = "none";
        }
    }

}

function EnableOrDisableInput() {
    var inputControlla = document.getElementById("programmingExp").value;
    var inputToChange = document.getElementById("programmingExpList");

    if (inputControlla == 1) {
        inputToChange.disabled = false;
    } else {
        inputToChange.disabled = true;
    }
}

// APPLICATION REQUEST 
function ApplicationRequestMain() {

    $('#loader').show();
    $('#loader-wrapper').show();

    var file = document.getElementById("cv").files;

    if (file[0] != null) {
        const reader = new FileReader();
        reader.readAsDataURL(file[0]);

        reader.onload = function () {
            var data = {};
            // section 1
            data.FirstName = $('#FirstName').val();
            data.LastName = $('#LastName').val();
            data.PhoneNumber = $('#PhoneNo').val();
            data.UserName = $('#UserName').val();
            data.Email = $('#Email').val();
            data.Password = $('#Passwrd').val();
            data.ConfirmPassword = $('#Cpasswrd').val();
            data.Address = $('#Address').val();

            // section 2
            data.HasCompletedNysc = $('#nysc').val();
            data.HasLaptop = $('#laptop').val();
            data.HasAnyProgrammingExp = $('#programmingExp').val();
            data.ApplicantResideInEnugu = $('#stateOfResident').val();
            data.ProgrammingLanguagesExps = $('#programmingExpList').val();
            data.ReasonForProgramming = $('#reasons').val();

            // section 3
            data.AboutYourSkills = $('#skills').val();
            data.HowDoYouIntendToCopeStatement = $('#survive').val();
            data.CV = reader.result;
            data.fileExtensionChecker = data.CV.includes("application/pdf");

            if (data.fileExtensionChecker) {
                let applicationViewModel = JSON.stringify(data);
                $.ajax({
                    type: 'Post',
                    dataType: 'json',
                    url: '/Accounts/Application',
                    data:
                    {
                        applicationUserViewModel: applicationViewModel,
                    },
                    success: function (result) {

                        if (!result.isError) {
                            $("#loader").fadeOut(3000);

                            var url = '/Accounts/Login';
                            successAlertWithRedirect(result.msg, url)
                        }
                        else {
                            $("#loader").fadeOut(3000);
                            errorAlert(result.msg)
                        }
                    },
                    error: function (ex) {
                        $("#loader").fadeOut(3000);
                        errorAlert("Error occured try again");
                    }
                });
            }
            else {
                $("#loader").fadeOut(3000);
                errorAlert("Only PDF file is allowed");
            }
        }
    }
}

// LOGIN POST ACTION
function loginPost() {

    $('#loader').show();
    $('#loader-wrapper').show();

    var email = document.getElementById("Email").value;
    var Passwrd = document.getElementById("Passwrd").value;
    if (email != null && Passwrd != null) {

        var data = {};
        data.Email = $("#Email").val();
        data.Password = $("#Passwrd").val();
        let loginViewModel = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            dataType: 'Json',
            url: '/Accounts/Login',
            data:
            {
                loginViewModel: loginViewModel
            },
            success: function (result) {


                if (result.isNotVerified) {
                    $("#loader").fadeOut(3000);
                    errorAlert(result.msg)
                }
                else if (!result.isError) {
                    $("#loader").fadeOut(3000);
                    successAlertWithRedirect(result.msg, result.dashboard)
                }
                else {
                    $("#loader").fadeOut(3000);
                    errorAlert(result.msg)
                }
            },
            Error: function (ex) {
                $("#loader").fadeOut(3000);
                errorAlert(ex);
            }
        });
    }
}



// DOCUMENTATION SUBMISSION 
function ApplicantDocumentation() {

    var file = {};
    file.FirstGuarantor = document.getElementById("FirstGuarantor").files;
    file.SecondGuarantor = document.getElementById("SecondGuarantor").files;
    file.NepaBill = document.getElementById("NepaBill").files;
    file.Contractforms = document.getElementById("Contractforms").files;
    var BVN = $('#BVN').val();
    if (file.FirstGuarantor[0] != null && file.SecondGuarantor[0] != null && file.NepaBill[0] != null && file.Contractforms[0] != null && BVN != null) {

        $('#loader').show();
        $('#loader-wrapper').show();
        if (file.FirstGuarantor[0] != null) {
            const reader = new FileReader();
            reader.readAsDataURL(file.FirstGuarantor[0]);
            var base64FirstGuarantor;
            reader.onload = function () {
                base64FirstGuarantor = reader.result;


                if (file.SecondGuarantor[0] != null) {
                    const reader = new FileReader();
                    reader.readAsDataURL(file.SecondGuarantor[0]);
                    var base64SecondGuarantor;
                    reader.onload = function () {
                        base64SecondGuarantor = reader.result;

                        if (file.NepaBill[0] != null) {
                            const reader = new FileReader();
                            reader.readAsDataURL(file.NepaBill[0]);
                            var base64NepaBill;
                            reader.onload = function () {
                                base64NepaBill = reader.result;

                                if (file.Contractforms[0] != null) {
                                    const reader = new FileReader();
                                    reader.readAsDataURL(file.Contractforms[0]);
                                    var base64Contractforms;
                                    reader.onload = function () {
                                        base64Contractforms = reader.result;
                                        var allDocument = {};
                                        allDocument.BVN = BVN;
                                        allDocument.FirstGuarantor = base64FirstGuarantor;
                                        allDocument.SecondGuarantor = base64SecondGuarantor;
                                        allDocument.NepaBill = base64NepaBill;
                                        allDocument.SignedContract = base64Contractforms;

                                        if (BVN != "" || BVN != 0) {
                                            let rawData = JSON.stringify(allDocument);
                                            $.ajax({
                                                type: 'Post',
                                                dataType: 'Json',
                                                url: '/Student/SubmitApplicantDocument',
                                                data:
                                                {
                                                    applicantDetailedDocuments: rawData,
                                                },
                                                success: function (result) {

                                                    if (result.isError) {
                                                        $("#loader").fadeOut(3000);
                                                        errorAlert(result.msg)
                                                    }
                                                    else {
                                                        $("#loader").fadeOut(3000);
                                                        var url = '/Student/ViewApplicantsDocumentation';
                                                        newSuccessAlert(result.msg, url);
                                                    }
                                                },
                                                error: function (ex) {
                                                    $("#loader").fadeOut(3000);
                                                    errorAlert("Error occured try again");
                                                }
                                            })
                                        }
                                        else {
                                            $("#loader").fadeOut(3000);
                                            errorAlert("Incorrect Details");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }
    }
    else {

        if (BVN == "") {
            document.querySelector("#BVNVDT").style.display = "block";
        } else {
            document.querySelector("#BVNVDT").style.display = "none";
        }
        if (file.FirstGuarantor[0] == null) {
            document.querySelector("#FirstGuarantorVDT").style.display = "block";
        } else {
            document.querySelector("#FirstGuarantorVDT").style.display = "none";
        }
        if (file.SecondGuarantor[0] == null) {
            document.querySelector("#SecondGuarantorVDT").style.display = "block";
        } else {
            document.querySelector("#SecondGuarantorVDT").style.display = "none";
        }
        if (file.NepaBill[0] == null) {
            document.querySelector("#NepaBillVDT").style.display = "block";
        } else {
            document.querySelector("#NepaBillVDT").style.display = "none";
        }
        if (file.Contractforms[0] == null) {
            document.querySelector("#ContractformsVDT").style.display = "block";
        } else {
            document.querySelector("#ContractformsVDT").style.display = "none";
        }
    }
}

// CHANGE PASSWORD POST ACTION
function ChangePasswordPost() {

    $('#loader').show();
    $('#loader-wrapper').show();
    var data = {};
    data.OldPassword = $("#OldPasswrd").val();
    data.NewPassword = $("#NewPasswrd").val();
    data.ConfirmPassword = $("#Cpasswrd").val();
    let changePasswordDetails = JSON.stringify(data);
    $.ajax({
        type: 'POST',
        dataType: 'Json',
        url: '/Accounts/ChangePasswordPost',
        data:
        {
            userPasswordDetails: changePasswordDetails
        },
        success: function (result) {

            if (!result.isError) {
                $("#loader").fadeOut(3000);
                successAlertWithRedirect(result.msg, result.url)
            }
            else {
                $("#loader").fadeOut(3000);
                errorAlert(result.msg)
            }
        },
        Error: function (ex) {
            $("#loader").fadeOut(3000);
            errorAlert(ex);
        }
    });
}

// FORGOT PASSWORD POST ACTION
function ForgotPasswordReset() {
    debugger;
    var email = $("#Email").val();
    if (email != "") {
        let emailAccount = email;
        $.ajax({
            type: 'POST',
            url: '/Accounts/ForgotPassword', // we are calling json method
            dataType: 'json',
            data:
            {
                email: emailAccount,
            },
            success: function (result) {

                if (!result.isNotVerified) {
                    var url = '/Accounts/Login';
                    successAlertWithRedirect(result.msg, url)
                }
                else {
                    errorAlert(result.msg);
                }
            },
            Error: function (ex) {
                errorAlert(ex);
            }
        });
    }
    else {
        errorAlert("Enter your email");
    }
}

// SET NEW PASSWORD POST ACTION
function resetPassword() {

    $('#loader').show();
    $('#loader-wrapper').show();
    var data = {};
    data.Password = $("#newPasswordId").val();
    data.ConfirmPassword = $("#confirmPasswordId").val();
    data.Token = $("#Token").val();
    let resetPasswordViewModel = JSON.stringify(data);
    if (data.Password != "" && data.ConfirmPassword != "" && data.Password == data.ConfirmPassword) {
        $.ajax({
            type: 'POST',
            url: '/Accounts/ResetUserPassword', // we are calling json method
            dataType: 'json',
            data:
            {
                viewmodel: resetPasswordViewModel,
            },
            success: function (result) {

                if (!result.isError) {
                    $("#loader").fadeOut(3000);
                    successAlert(result.msg)
                    window.location.reload;
                }
                else {
                    $("#loader").fadeOut(3000);
                    errorAlert(result.msg);
                }
            },
            error: function (ex) {
                $("#loader").fadeOut(3000);
                errorAlert(ex);
            }
        });
    } else {
        if (data.Password == "") {
            errorAlert("Enter password");
        }
        if (data.Password != data.ConfirmPassword) {
            errorAlert("Password and password confirm not matched");
        }
    }
}

// Admin Registration POST ACTION
function RegisterAdmin() {
    var data = {};
    data.FirstName = $('#FirstName').val();
    data.LastName = $('#LastName').val();
    data.Email = $('#Email').val();
    data.Password = $('#Passwrd').val();
    data.ConfirmPassword = $('#ConfirmPasswrd').val();
    if (data.FirstName != "" && data.LastName != "" && data.Email != "" && data.Password != "" && data.Password == data.ConfirmPassword) {
        let applicationViewModel = JSON.stringify(data);
        $.ajax({
            type: 'POST',
            dataType: 'Json',
            url: '/Accounts/AdminRegisteration',
            data:
            {
                applicationUserViewModel: applicationViewModel,
            },
            success: function (result) {

                if (!result.isError) {
                    $("#loader").fadeOut(3000);
                    var url = '/Accounts/Login'
                    successAlertWithRedirect(result.msg, url)
                }
                else {
                    $("#loader").fadeOut(3000);
                    errorAlert(result.msg)
                }
            },
            Error: function (ex) {
                $("#loader").fadeOut(3000);
                errorAlert(ex);
            }
        });
    }
    else {
        if (data.FirstName == "") {
            document.querySelector("#fNameVDT").style.display = "block";
        } else {
            document.querySelector("#fNameVDT").style.display = "none";
        }
        if (data.LastName == "") {
            document.querySelector("#lNameVDT").style.display = "block";
        } else {
            document.querySelector("#lNameVDT").style.display = "none";
        }
        if (data.Email == "") {
            document.querySelector("#emailVDT").style.display = "block";
        } else {
            document.querySelector("#emailVDT").style.display = "none";
        }
        if (data.Password == "") {
            document.querySelector("#passwrdVDT").style.display = "block";
        } else {
            document.querySelector("#passwrdVDT").style.display = "none";
            if (data.Password != data.ConfirmPassword) {
                document.querySelector("#cpasswrdVDT").style.display = "block";
            } else {
                document.querySelector("#cpasswrdVDT").style.display = "none";
            }
        }
    }
}

function viewApplcantsData(HasCompletedNysc, HasLaptop, HasAnyProgrammingExp, ApplicantResideInEnugu, ProgrammingLanguagesExps, ReasonForProgramming) {

    var nyscCheck = HasCompletedNysc;
    var laptopCheck = HasLaptop;
    var programmingExpCheck = HasAnyProgrammingExp;
    var ResideInEnuguCheck = ApplicantResideInEnugu;
    if (nyscCheck == "Yes") {
        document.querySelector("#nyscChecker1").style.display = "block";
        document.querySelector("#nyscChecker2").style.display = "none";
    } else {
        document.querySelector("#nyscChecker1").style.display = "none";
        document.querySelector("#nyscChecker2").style.display = "block";
    }
    if (laptopCheck == "Yes") {
        document.querySelector("#laptopChecker1").style.display = "block";
        document.querySelector("#laptopChecker2").style.display = "none";

    } else {
        document.querySelector("#laptopChecker1").style.display = "none";
        document.querySelector("#laptopChecker2").style.display = "block";
    }
    if (programmingExpCheck == "Yes") {
        document.querySelector("#programmingExpChecker1").style.display = "block";
        document.querySelector("#programmingExpChecker2").style.display = "none";
    } else {
        document.querySelector("#programmingExpChecker1").style.display = "none";
        document.querySelector("#programmingExpChecker2").style.display = "block";
    }
    if (ResideInEnuguCheck == "Yes") {
        document.querySelector("#resideInEnuguChecker1").style.display = "block";
        document.querySelector("#resideInEnuguChecker2").style.display = "none";
    } else {
        document.querySelector("#resideInEnuguChecker1").style.display = "none";
        document.querySelector("#resideInEnuguChecker2").style.display = "block";
    }

    $("#nysc").val(nyscCheck);
    $("#laptop").val(laptopCheck);
    $("#programmingExp").val(programmingExpCheck);
    $("#ApplicantResideInEnugu").val(ResideInEnuguCheck);
    $("#programmingExpList").val(ProgrammingLanguagesExps);
    $("#reasons").val(ReasonForProgramming);
}
