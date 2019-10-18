function ViewModel() {
	var self = this;
	self.fileName = ko.observable("");
	self.fileData = ko.observable("");

	self.FirstName = ko.observable();
	self.LastName = ko.observable();
	self.EmailId = ko.observable();
	self.ContactNumber = ko.observable();
	self.IsActive = ko.observable(true);
	self.IsActive_Enable = ko.observable(true);
	self.EmailId_enable = ko.observable(true);
	self.errors = ko.validation.group(self);

	self.IsAdd = ko.observable(true);
	self.IsUpdate = ko.observable(true);

	self.AddEmployeeDetails = function () {
		self.IsAdd(true);
		self.IsUpdate(false);
		AddEmployeeDetailsPopup();
	}
	self.ClosePopup = function () {
		self.Reset();
		self.errors.showAllMessages(false);
		$("#ContactInformationDetailsPopup").css("display", "none");
	};
	self.UpdateEmpDetails = function () {	
		self.IsUpdate(true);
		self.IsAdd(false);
		sessionStorage.setItem('Id', $(".rowSelect.active").find("label").text());
		var Id = sessionStorage.getItem("Id");
		if (sessionStorage.getItem("Id") != "") {

			var url = 'api/ContactInformation/GetContactInformationDetailsById';
			var data = { RowId: Id };
			Commonmethods.GetAjaxData(url, 'GET', false, '', data, 'Issue with function GetContactInformationById', function (result) {
				if (result != null) {
					self.FirstName(result[0].FirstName);
					self.LastName(result[0].LastName);
					self.EmailId(result[0].EmailId);
					self.ContactNumber(result[0].ContactNo);
					self.IsActive(result[0].IsActive);
					if (result[0].IsActive != false)
						self.IsActive_Enable(false);
					$("#ContactInformationDetailsPopup").show();
					$("#ContactInformationDetailsPopup").css("display", "block");
				}
			});
		}
	}
	self.DeleteEmpDetails = function () {
		var datalist = {
			Id: sessionStorage.getItem("Id")
		}

		var result = deleteEmployee(datalist);
		if (result.status == true) {
			self.FillGridData();		
			//ShowMessageBox("User deleted successfully.");
			alert("User deleted successfully.");
		}
		else if (result.status == false) {
			alert('Error while getting data');
			return;
		}

	}
	self.FillGridData = function () {
		GetAllUserDetails();
	}
	self.ConfirmEmpDelete = function () {
		if ($(".rowSelect.active").find("label").text().trim() != "") {
			sessionStorage.setItem('Id', $(".rowSelect.active").find("label").text());
			self.DeleteEmpDetails();
		}
	}

	self.Create = function () {
		$('#lblEmailId').css("display", "block");
		$('#lblEmailIdMsg').css("display", "none");
		$('#lblContactNo').css("display", "block");
		$('#lblContactNoMsg').css("display", "none");
		var er = self.errors();
		if (self.errors().length == 0) {
			var EmployeeData = {
				FirstName: self.FirstName().trim(),
				LastName: self.LastName().trim(),
				EmailId: self.EmailId(),
				ContactNo: self.ContactNumber(),
				IsActive: self.IsActive()
			};
			var data = create(EmployeeData);
			var status = Constants.MasterRecordMessageStatus.Added;
			var DuplicateEmpID = Constants.MasterRecordMessageStatus.DuplicateEmpID;
			var DuplicateEmailID = Constants.MasterRecordMessageStatus.DuplicateEmailID;
			var DuplicateContactID = Constants.MasterRecordMessageStatus.DuplicateContactID;
			if (data) {
				if (data == status) {
					alert('User added successfully!!');
					self.ClosePopup();
					self.FillGridData();
				} else {
					if (data.toUpperCase() == DuplicateEmpID.toUpperCase()) {
						$('#lblEmpId').css("display", "none");
						$('#lblEmpIdMsg').css("display", "block");
					}
					else if (data.toUpperCase() == DuplicateEmailID.toUpperCase()) {
						$('#lblEmailId').css("display", "none");
						$('#lblEmailIdMsg').css("display", "block");
					}
					else if (data.toUpperCase() == DuplicateContactID.toUpperCase()) {
						$('#lblContactNo').css("display", "none");
						$('#lblContactNoMsg').css("display", "block");
					}
					else {
						alert('Failed to add user!!');						
					}
				}
			}
			else if (data == null) {
				alert('Error while getting data');
				return;
			}
		} else {
			self.errors.showAllMessages();
			return;
		}		
	}
	//Update Employees
	self.Update = function () {
		var Id = sessionStorage.getItem("Id");
		if (self.errors().length == 0) {
			var Emplist = {
				Id: Id,

				FirstName: self.FirstName().trim(),
				LastName: self.LastName().trim(),
				EmailId: self.EmailId(),
				ContactNo: self.ContactNumber(),
				IsActive: self.IsActive()
			}

			var result = update(Emplist);

			var status = Constants.MasterRecordMessageStatus.Updated;
			var DuplicateEmailID = Constants.MasterRecordMessageStatus.DuplicateEmailID;
			var DuplicateContactID = Constants.MasterRecordMessageStatus.DuplicateContactID;
			var DuplicateEmpID = Constants.MasterRecordMessageStatus.DuplicateEmpID;
			if (result) {
				if (result == status) {
					alert('User updated successfully!!');			
					self.ClosePopup();
					self.FillGridData();
				} else {
					if (result.toUpperCase() == DuplicateEmailID.toUpperCase()) {
						$('#lblEmailId').css("display", "none");
						$('#lblEmailIdMsg').css("display", "block");
					}
					else if (result.toUpperCase() == DuplicateContactID.toUpperCase()) {
						$('#lblContactNo').css("display", "none");
						$('#lblContactNoMsg').css("display", "block");
					}
					else if (result.toUpperCase() == DuplicateEmpID.toUpperCase()) {
						$('#lblEmpId').css("display", "none");
						$('#lblEmpIdMsg').css("display", "block");
					}
					else {
						alert('Failed to update user!!');					
					}
				}
			}			
		} else {
			self.errors.showAllMessages();
			return;
		}
	}
	self.Reset = function () {
		self.FirstName('');
		self.LastName('');
		self.EmailId('');
		self.ContactNumber('');
		self.IsActive('');
		self.IsAdd(true);
		self.IsUpdate(true);
	}

	self.FirstName.extend({
		required: {
			param: true,
			message: "First name is required."
		},
		minlength: 1,
		maxlength: 200,
	});
	self.LastName.extend({
		required: {
			param: true,
			message: "Last name is required."
		},
		minlength: 1,
		maxlength: 200,
	});

	self.ContactNumber.extend({
		required: {
			param: true,
			message: "Contact Number name is required."
		}
	});

	self.EmailId = ko.observable().extend({
		required: true,
		pattern: {
			message: 'Please enter valid email Id',
			params: /^([\d\w-\.]+@([\d\w-]+\.)+[\w]{2,4})?$/,
		},
		minlength: 1,
		maxlength: 200,
	});
}

function deleteEmployee(data) {
    var result = null;
	result = Commonmethods.DeleteData('api/ContactInformation/DelteContactInformationDetails?Id=' + data.Id, data);
    return result;
}
function BindAllUserDetails(result) {
    $('#FilteredInfoGrid').dataTable({
        "aaData": result,
        "order": [],
        scrollX: true,
        destroy: true,
        dom: 'lBfrtip',
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="fa fa-file-excel-o" title="Export to excel"></i>',
                title: 'EmployeeDetails'
            },
        ],
        "aoColumns": [
            {
				sTitle: "Id", sWidth: "15%", sClass: "cHide",
                "render": function (data, type, full, meta) {
                    return "<label id='btnAddAction' class='lnk' style='font-weight:normal;' >" + full.Id + "</label></a>";
                }
            },           
            {
                sTitle: "First Name", sWidth: "20%",
                "render": function (data, type, full, meta) {
                    return $('<div/>').text(full.FirstName).html();

                }
            },           
            {
                sTitle: "Last Name", sWidth: "20%",
                "render": function (data, type, full, meta) {
                    return $('<div/>').text(full.LastName).html();

                }
            },
            {
                sTitle: "Email Id", sWidth: "25%",
                "render": function (data, type, full, meta) {
                    return $('<div/>').text(full.EmailId).html();

                }
			}, 
			{ sTitle: "Contact No", mDataProp: "ContactNo", sWidth: "15%" },   
            { sTitle: "IsActive", mDataProp: "IsActive", sWidth: "5%" }
            ],

        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $(nRow).addClass("rowSelect");
        },
        "fnInitComplete": function (settings, json) {
            $("#FilteredInfoGrid").children("tbody").children("tr:first").addClass("active");
        },
        "fnInfoCallback": function (oSettings, iStart, iEnd, iMax, iTotal, sPre) {
            var rows = $("#FilteredInfoGrid").dataTable().fnGetNodes();
            for (var i = 0; i < rows.length; i++) {
                $(rows[i]).removeClass("active")
            }
            $("#FilteredInfoGrid").children("tbody").children("tr").children("td").unbind("click");
            $("#FilteredInfoGrid").children("tbody").children("tr").children("td").click(function (event) {
                $(this.parentNode).toggleClass("active");

                $("#FilteredInfoGrid").children("tbody").children("tr").not(this.parentNode).removeClass("active");
            });
            return "Showing " + iStart + " to " + iEnd + " of " + iTotal + " entries";

        }
	});
}

var EmployeeDetailViewModel;
$(document).ready(function () {
        EmployeeDetailViewModel = new ViewModel();
	ko.applyBindings(EmployeeDetailViewModel, document.getElementById('dvContactInformationsDetails'));
        EmployeeDetailViewModel.FillGridData();
});

function GetAllUserDetails() {
		var url = '/api/ContactInformation/GetAllUserDetails';
        var data = { IncludeInactive: true };
        Commonmethods.GetAjaxData(url, 'GET', false, '', data, 'Issue with function GetAllUserDetails', function (result) {
            BindAllUserDetails(result);
        });
}

function AddEmployeeDetailsPopup() {
    sessionStorage.setItem('Id', null);   
	$("#ContactInformationDetailsPopup").css("display", "block"); 
	$("#ContactInformationDetailsPopup").show();
}
function create(data) {
	var result = null;
	var url = '/api/ContactInformation/AddContactInformation';
	Commonmethods.GetAjaxData(url, 'POST', false, '', data, 'Issue with function GetRolesList', function (data1) {
		result = data1;
	});
	return result;
}
function update(data) {
	var result = null;
	var url = '/api/ContactInformation/UpdateContactInformation';
	Commonmethods.GetAjaxData(url, 'POST', false, '', data, 'Issue with function GetRolesList', function (data1) {
		result = data1;
	});
	return result;
}

