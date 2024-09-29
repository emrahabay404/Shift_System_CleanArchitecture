
var _passwordapi = $("#passwordapi").val();

$("#logoutbtn").click(function () {
    $.ajax({
        type: "GET",
        url: "/Auth/Logout", // Logout action
        success: function (data) {
            if (data === true) {
                window.location.href = "/Account/Login";
            }
        },
        error: function (err) {
            console.error("Logout failed: ", err);
        }
    });
});

$("#apiBtn").click(function () {
    swal({
        title: "Lütfen Şifrenizi Girin",
        content: {
            element: "input",
            attributes: {
                placeholder: "Şifrenizi Girin",
                type: "password",
                id: "passwordapi",
                value: "De*1wrlCRo04Y@Ph==R?T",
                className: "form-control",
            },
        },
        buttons: {
            cancel: {
                visible: true,
                text: "İptal",
                className: "btn btn-danger",
            },
            confirm: {
                text: "Giriş Yap",
                className: "btn btn-success",
                closeModal: false // SweetAlert'in kapanmasını engeller
            },
        },
        closeOnClickOutside: false,
    }).then(function (value) {
        // Kullanıcı 'İptal' butonuna tıkladıysa
        if (value === null) {
            swal.close(); // SweetAlert kutusunu kapat
            return;
        }

        var _passwordapi = $("#passwordapi").val(); // Şifreyi input'tan alın
        if (!_passwordapi) {
            swal("Hata", "Şifre girilmedi!", "error");
            return;
        }

        // Input ve butonları devre dışı bırak
        $("#passwordapi").attr("disabled", true);
        $(".swal-button--confirm").attr("disabled", true);
        $(".swal-button--cancel").attr("disabled", true);

        // Spinner'ı SweetAlert içeriğine butonların altına ekle
        var spinnerHtml = `
    <div id="spinner-container" style="text-align: center; margin-top: 10px;">
        <div id="spinnerloginapi" class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;">
            <span class="sr-only">Loading...</span>
        </div>
    </div>`;

        $(".swal-content").append(spinnerHtml); // Spinner HTML'ini butonların altına ekle

        // Spinner'ı göster
        $("#spinnerloginapi").show();

        $.ajax({
            type: "POST",
            url: "/Auth/ApiLogin",
            data: { password: _passwordapi }, // Şifreyi gönderin
            success: function (response) {
                // Spinner'ı gizle
                $("#spinnerloginapi").hide();

                // Input ve butonları tekrar etkinleştir
                $("#passwordapi").attr("disabled", false);
                $(".swal-button--confirm").attr("disabled", false);
                $(".swal-button--cancel").attr("disabled", false);

                if (response.success) {
                    // "Bağlantı Başarılı" mesajını göster ve ardından sayfayı yenile
                    swal(response.message, {
                        icon: "success",
                        buttons: {
                            confirm: {
                                className: "btn btn-success",
                            },
                        },
                    }).then(() => {
                        window.location.reload(); // SweetAlert kapandıktan sonra sayfayı yenile
                    });
                } else {
                    swal("Hata", response.message, "warning");
                }
            },
            error: function (response) {
                // Spinner'ı gizle
                $("#spinnerloginapi").hide();

                // Input ve butonları tekrar etkinleştir
                $("#passwordapi").attr("disabled", false);
                $(".swal-button--confirm").attr("disabled", false);
                $(".swal-button--cancel").attr("disabled", false);

                swal("Hata", "Bir hata oluştu.", "error");
            }
        });
    });
});

$(document).ready(function () {
    //// Sayfa yüklendiğinde AJAX isteği yap
    //$.ajax({
    //    type: "GET",
    //    url: "/Auth/ApiConnectionStatus", // API metoduna istek
    //    success: function (response) {
    //        if (response.success) {
    //            // JWT Token mevcutsa, menü öğesini "Bağlanıldı" olarak değiştir ve check ikonu ekle
    //            $("#apiBtn")
    //                .html('<i class="fas fa-check"></i><p>Servis Bağlantısı : Bağlı</p>')
    //                .removeAttr('href') // Bağlantıyı kaldır
    //                .css('cursor', 'default') // Farenin üzerine gelince göstergeyi değiştir
    //                .off('click'); // Tıklama olayını kaldır
    //        } else {
    //            // JWT Token yoksa, menü öğesi "Servise Bağlan" olarak kalır
    //            $("#apiBtn")
    //                .html('<i class="fas fa-link"></i><p>Servise Bağlan</p><span class="badge badge-secondary">1</span>')
    //                .attr('href', '#') // Bağlantıyı tekrar ekle
    //                .css('cursor', 'pointer'); // Farenin üzerine gelince göstergeyi değiştir
    //        }
    //    },
    //    error: function () {
    //        // AJAX hatası durumunda menü öğesi "Servise Bağlan" olarak kalır
    //        $("#apiBtn")
    //            .html('<i class="fas fa-link"></i><p>Servise Bağlan</p><span class="badge badge-secondary">1</span>')
    //            .attr('href', '#') // Bağlantıyı tekrar ekle
    //            .css('cursor', 'pointer'); // Farenin üzerine gelince göstergeyi değiştir
    //    }
    //});

    $(".changeSideBarColor").on("click", function () {
        if ($(this).attr("data-color") == "default") {
            $(".sidebar").removeAttr("data-background-color");
        } else {
            $(".sidebar").attr("data-background-color", $(this).attr("data-color"));
        }
        $(this).parent().find(".changeSideBarColor").removeClass("selected");
        $(this).addClass("selected");
        layoutsColors();
        getCheckmark();
    });

});

