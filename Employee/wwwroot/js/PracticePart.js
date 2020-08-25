

function EmployeeInformation(data) {
    var self = this;
    self.EName = ko.observable(data.EName);
    self.EAddress = ko.observable(data.EAddress);;
    self.EAge = ko.observable(data.EAge);
    self.ESelectedPost = ko.observable(data.ESelectedPost);
}
function EmployeeDetail() {
    var self = this;
    self.AllPost = [{ PostId: '1', PostName: 'Project Leader', NEpaliConvert: 'शिक्षक' },
        { PostId: '2', PostName: 'Project Manager', NEpaliConvert: 'प्रिंसिपल' },
        { PostId: '3', PostName: 'Project Sponsor', NEpaliConvert: 'प्रबन्धक' },
        { PostId: '4', PostName: 'Executive Sponsor', NEpaliConvert: 'लेखापाल' },
        { PostId: '5', PostName: 'Business Analyst', NEpaliConvert: 'शिक्षक' }
        
    ]
    self.AllDepartment = [{ DepartmentId: '1', DepartmentName: 'Civil' },
        { DepartmentId: '2', DepartmentName: 'Computer' },
        { DepartmentId: '3', DepartmentName: 'Electrical' },
        { DepartmentId: '4', DepartmentName: 'Mechanical' }]
    self.SelectedDepartment = ko.observable();
    self.AllGroup = [{ GroupId: '1', GroupName: 'A' },
        { GroupId: '2', GroupName: 'B' },
        { GroupId: '3', GroupName: 'C' },
        { GroupId: '4', GroupName: 'D' }]
    self.SelectedGroup = ko.observable();
    
    self.Name = ko.observable();
    self.Address = ko.observable();
    self.Age = ko.observable();
    self.CopyName = ko.observable();
    self.CopyName(self.Name());
  
    self.SelectedPost = ko.observable();
    self.SelectedPostInNepali = ko.observable();
    self.SelectedEmployeeDetail = ko.observable();

    self.AllDetail = ko.observableArray([]);
    
    self.Add = function () {
        if (!self.Name() || !self.Address() || !self.Age() || !self.SelectedPost()) {
            alert("PLease Emter all the required fields");
            return;
        }
        if (isNaN(self.Age())) {
            alert("Age  must be in number");
            return;
        }
        if (self.Age() > 40 || self.Age() < 20) {
            alert("Employee Age must be between 20 to 40");
            return;
        }

        for (var i = 0; i < self.AllDetail().length; i++) {
            if (self.SelectedEmployeeDetail()) {
                if (self.SelectedEmployeeDetail().ESelectedPost().PostId != self.SelectedPost().PostId) {
                    if (self.SelectedPost().PostId == self.AllDetail()[i].ESelectedPost().PostId) {
                        alert('Post Name Must Be Different ');
                        return;
                    }
                }

            }
            else {
                if (self.SelectedPost().PostId == self.AllDetail()[i].ESelectedPost().PostId) {
                    alert('Post Name Must Be Different ');
                    return;
                }
            }
           
        }
        if (!self.SelectedEmployeeDetail()) {
            var data = { EName: self.Name(), EAddress: self.Address(), EAge: self.Age(), ESelectedPost: self.SelectedPost() };
            self.AllDetail.push(new EmployeeInformation(data));
        }
        else {
            self.SelectedEmployeeDetail().EName(self.Name());
            self.SelectedEmployeeDetail().EAddress(self.Address());
            self.SelectedEmployeeDetail().EAge(self.Age());
            self.SelectedEmployeeDetail().ESelectedPost(self.SelectedPost());
            self.SelectedEmployeeDetail(null);
        }
        self.Name('');
        self.Address('');
        self.Age('');
        self.SelectedPost(null);
        alert('Data has been successfully added');
    }
    self.Edit = function (data) {
        self.SelectedEmployeeDetail(data);
        self.Name(data.EName());
        self.Address(data.EAddress());
        self.Age(data.EAge());
        self.SelectedPost(data.ESelectedPost());
      
    }
    self.Delete = function (data) {

        if (confirm('Are you sure want to delete data?')) {
            self.AllDetail.remove(data);
           
            return;
        }

    }

}
ko.applyBindings(new EmployeeDetail());