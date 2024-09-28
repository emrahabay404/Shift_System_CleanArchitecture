
$(document).ready(function () {
    //Swal.fire("SweetAlert2 is working!");
    //alert("ssaddsa");
});


$("#loginBtn").on("click", function (event) {

    document.getElementById("spinnerlogin").style.display = "block";

    let _data = {
        username: $("#username").val(),
        password: $("#password").val()
    };

    $.ajax({
        type: "POST",
        url: "/Auth/Login/",
        data: _data,
        success: function (response) {
            if (response == true) {
                window.location.href = "/Home/Index";
            } else {
                // Hata mesajını swal ile göster
                swal(response, "", {
                    icon: "warning",
                    buttons: {
                        confirm: {
                            className: "btn btn-warning",
                        },
                    },
                });
                document.getElementById("spinnerlogin").style.display = "none";
            }
        },
        error: function (xhr, textStatus, errorThrown) {
            swal(errorThrown, "", {
                icon: "warning",
                buttons: {
                    confirm: {
                        className: "btn btn-warning",
                    },
                },
            });
            document.getElementById("spinnerlogin").style.display = "none";
        }
    });
});


