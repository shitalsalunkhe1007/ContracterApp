var Commonmethods = {
    GetAjaxData: function GetData(url, reqType, isAsync, contentType, data, ErrorMsg, successCallback) {
        var result = null;
        if (contentType.trim() == '')
            contentType = "application/x-www-form-urlencoded; charset=UTF-8";

        $.ajax({
            url: url,
            type: reqType,
            async: isAsync,
            dataType: "json",
            contentType: contentType,
            data: data,
            success: successCallback,
			error: function (r, s, e) {
                //logAjaxError(r, s, e, ErrorMsg);
            }
        });
    },  
    //add record
    AddData: function AddData(url, data) {
        var result = null;

        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            dataType: "json",
            //headers: { 'Authorization': 'Bearer ' + token },
            data: data,
            success: function (data) {
                result = data;
            },
            //Commented By $Girish.
            error: function (returnVal) {
                Commonmethods.CustomAlert('Failed', returnVal.responseJSON, 'Danger');
            },
            //Added new code below to display custom error message
            //error: Commonmethods.OnError
        });
        return result;
    },
    //update record
    UpdateData: function UpdateData(url, data) {
        var result = null;

        $.ajax({
            url: url,
            type: 'POST',
            async: false,
            dataType: "json",
            //headers: { 'Authorization': 'Bearer ' + token }, 
            data: data,
            success: function (data) {
                result = data;
            },
			error: function (data) {
                Commonmethods.CustomAlert('Failed', data, 'Danger');
            },
        });
        return result;
    },
    //delete Process record
    DeleteData: function DeleteData(url, data) {
        var result = null;

        $.ajax({
            url: url,
            type: 'DELETE',
            async: false,
            dataType: "json",
            //headers: {
            //    'UserSession': sessionStorage.getItem("SingleUserToken"), 'UserId': sessionStorage.getItem("UserId"), 'UserTimeZone': sessionStorage.getItem("UserTimeZone"), 'DeptId': sessionStorage.getItem('DeptId'), 'IsSuperAdmin': sessionStorage.getItem('IsSuperAdmin')
            //},
            //data: data,
            success: function (data) {
                result =
                    {
                        status: true,
                        message: data
                    };
            },
            error: function (data) {
                result =
                    {
                        status: false,
                        message: data.responseText
                    };
            },
        });
        return result;
    }   
}