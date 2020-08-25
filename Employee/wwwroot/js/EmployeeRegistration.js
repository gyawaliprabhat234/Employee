function getReqArr(url, data, constr, arr) {
    return $.ajax({
        dataType: 'json',
        cache: false,
        async: false,
        url,
        data,
        contentType: 'application/json; charset=utf-8',
        success: result => {
            arr(result);
        },
        error: function () {
            alert(data.message, 'Error!!!');
        }
    });
}

function postReqArr(url, data, constr, arr) {
    $.ajaxSetup({ async: false });

    return $.post(url, data, function (result, status, xhr) {
        // console.log(xhr.status);
        // var res = JSON.parse(result);
        var res = (result);
        if (constr) {
            try {
                arr(res.Records.map(x => new constr(x)));
            } catch (e) {
                arr(new constr(res.Records));
            }
        } else arr(res.Records);
    }).done(() => {

    }).fail((xhr, status, message) => {
        alert(message, status);
 
    });
}


function AcademicInformation(data) {
    var self = this;
    self.Qualification = ko.observable(data.Qualification);
    self.MarksObtained = ko.observable(data.MarksObtained);

}

function EmployeeRegistrationViewModel() {
    var self = this;
    self.Qualifications = ko.observableArray([]);
    getReqArr('/Detail/GetAllQualifications', null, null, self.Qualifications)
    self.AllEmployeeList = ko.observableArray([]);
    getReqArr('/Detail/GetAllEmployeeDetail', null, null, self.AllEmployeeList)

   
    self.SelectedQualification = ko.observable();
    self.SelectedQualification1 = ko.observable();
    self.MarksObtained = ko.observable();
    self.SelectedAcademicInformation = ko.observable();
    self.EmployeeId = ko.observable();
    self.Name = ko.observable();
    self.Salary = ko.observable();
    self.DOB = ko.observable();
    self.EmailAddress = ko.observable();
    self.AcademicInformations = ko.observableArray([]);
    
    self.Add = function () {
        if (!self.SelectedQualification() || !self.MarksObtained()) {
            alert("PLease Emter all the required fields");
            return;
        }

        if (isNaN(self.MarksObtained())) {
            alert("please enter the numeric value");
            return;
        }
        if (self.MarksObtained() > 100 || self.MarksObtained() < 0) {
            alert("please enter marks between 0 and 100");
            return;
        }
 
        for (var i = 0; i < self.AcademicInformations().length; i++) {

            if (self.SelectedAcademicInformation()) {

               if( self.SelectedAcademicInformation().Qualification().QId != self.SelectedQualification().QId) {
                    if (self.SelectedQualification().QId == self.AcademicInformations()[i].Qualification().QId) {
                        alert(self.SelectedQualification().NameEng + " is already in the list");
                        return;
                    }
                }
            } else {
                if (self.SelectedQualification().QId == self.AcademicInformations()[i].Qualification().QId) {
                    alert(self.SelectedQualification().NameEng + " is already in the list");
                    return;
                }
            }
        }
    
        if (!self.SelectedAcademicInformation()) {
            var data = { Qualification: self.SelectedQualification(), MarksObtained: self.MarksObtained() };
            self.AcademicInformations.push(new AcademicInformation(data));
        } else {
            self.SelectedAcademicInformation().Qualification(self.SelectedQualification());
            self.SelectedAcademicInformation().MarksObtained(self.MarksObtained());
            self.SelectedAcademicInformation(null);
        }
        self.MarksObtained('');
        self.SelectedQualification(null);

    }
    self.Edit = function (data) {

        self.SelectedAcademicInformation(data);
        self.MarksObtained(data.MarksObtained());
        self.SelectedQualification(data.Qualification());

    }

    self.Delete = function (data) {
        if (self.SelectedAcademicInformation()) {
            alert("Please update the record first");
            return;
        }
        if (confirm("Are you sure to delete")) {
            var data = self.AcademicInformations().filter(function (item) {
                if (item.Qualification().QId != data.Qualification().QId) {
                    return item;
                }
            });
            self.AcademicInformations(data);
        }
    }

    self.Submit = function () {
        if (!self.Name() || !self.Salary() || !self.DOB()) {
            alert("Please Enter all the required fields");
            return;
        }

        if (isNaN(self.Salary())) {
            alert("please enter the Salary value");
            return;
        }
        if (self.Salary() > 1000000000 || self.Salary() < 0) {
            alert("please enter SAlary between 0 and 1000000000");
            return;
        }
        var data = {
            EmployeeId: self.EmployeeId(), 
            Name: self.Name(), 
            DateOfBirth: self.DOB(),
            EmailAddress: self.EmailAddress(), 
            Salary: self.Salary(),
            AcademicInformations: self.AcademicInformations()
        };

        $.ajaxSetup({ async: false });
        $.post("/Detail/SaveEmployee", ko.toJS(data), function (result) {

            if (result.IsSuccess) {
               
            } else {
              
            }


        }).done(() => {
        }).fail((x, y) => {
            DisplayErrorMessage('Something went wrong.');
            return null;

        });

    }

}
$(document).ready(function () {
    {
       
        ko.applyBindings(new EmployeeRegistrationViewModel());

    }})

