
//$(document).ready(function () {
//    Swal.fire("SweetAlert2 is working!");
//});


$("#loginBtn").on("click", function () {
    document.getElementById("spinnerlogin").style.display = "block";
    let _data = {
        username: $("#username").val(),
        password: $("#password").val()
    };
    $.ajax({
        type: "POST",
        url: "/Auth/Login/",
        //contentType: 'application/json',
        //data: JSON.stringify(_data),
        data: _data,
        //crossDomain: true,
        //xhrFields: {
        //    withCredentials: true
        //},
        success: function (funk) {
            if (funk == false) {
                Swal.fire("Geçersiz kimlik bilgileri");
                document.getElementById("spinnerlogin").style.display = "none";
            } else {
                Swal.fire("Başarılı!");
                //Swal.fire(funk);
                //localStorage.setItem('token', funk);
                window.location.href = "/Home/Index";
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            //alert("Error: " + errorThrown);
        }
    });
});

