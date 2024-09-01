
$(document).ready(function () {
    //Swal.fire("SweetAlert2 is working!");
    //alert("ssaddsa");
});


$("#loginBtn").on("click", function () {
    document.getElementById("spinnerlogin").style.display = "block";
    let _data = {
        username: $("#username").val(),
        password: $("#password").val()
    };
    $.ajax({
        type: "POST",
        url: "/Auth/Login/",
        data: _data,
        success: function (funk) {
            if (funk == false) {
                swal("Kullanıcı | Şifre Hatalı", "Kalan giriş denemesi: 5", {
                    icon: "warning",
                    buttons: {
                        confirm: {
                            className: "btn btn-warning",
                        },
                    },
                });
                document.getElementById("spinnerlogin").style.display = "none";
            } else {
                //Swal.fire("Başarılı!");
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

