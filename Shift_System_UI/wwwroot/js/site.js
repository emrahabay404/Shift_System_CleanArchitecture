
// Başarılı işlem swal
function SuccessSwal() {
    return swal({
        title: "Başarılı!",
        text: "İşleminiz başarıyla tamamlandı.",
        icon: "success",
        button: {
            text: "Tamam",
            className: "btn btn-success"
        }
    });
}

// Hata swal
function ErrorSwal() {
    return swal({
        title: "Hata!",
        text: "Bir hata oluştu, lütfen tekrar deneyin.",
        icon: "error",
        button: {
            text: "Tamam",
            className: "btn btn-danger"
        }
    });
}

// Uyarı swal
function WarningSwal() {
    return swal({
        title: "Uyarı!",
        text: "Lütfen kontrol edin!",
        icon: "warning",
        button: {
            text: "Tamam",
            className: "btn btn-warning"
        }
    });
}

// Soru swal (Emin misin?)
function ConfirmationSwal() {
    return swal({
        title: "UYARI",
        text: "Bu işlemi yapmak istediğinizden emin misiniz?",
        icon: "warning",
        buttons: {
            cancel: {
                text: "Hayır",
                visible: true,
                className: "btn btn-danger"
            },
            confirm: {
                text: "Evet",
                className: "btn btn-success"
            }
        },
        dangerMode: true
    }).then((isConfirmed) => {
        return isConfirmed; // Evet (true) veya Hayır (false) döndürür
    });
}

function TitleAndTextSwal(titleswal, textswal) {
    swal(titleswal, textswal, {
        buttons: {
            confirm: {
                text: "Tamam",
                className: "btn btn-success",
            },
        },
    });
}

function JustTextWithTimerSwal(text) {
    return swal(text, {
        buttons: false,
        timer: 3000,
    });
}

// URL'ye GET isteği atıp JSON verisi döndüren ve başarı/hata durumunu swal ile gösteren fonksiyon
function GetRequestSwal(url, params = {}) {
    // Parametreleri sorgu dizesine (query string) çeviriyoruz
    const queryString = new URLSearchParams(params).toString();
    const fullUrl = queryString ? `${url}?${queryString}` : url;
    return fetch(fullUrl)
        .then(response => response.json())
        .then(data => {
            return data; // JSON verisini döndürüyoruz
        })
        .catch(error => {
            // Hata oluşursa ErrorSwal göster
            ErrorSwal();
            throw error; // Hata durumunu dışarı döndürüyoruz
        });
}

// URL'ye POST isteği atıp JSON verisi döndüren ve başarı/hata durumunu swal ile gösteren fonksiyon
function PostRequestSwal(url, postData) {
    return fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(postData) // Gönderilen veri JSON formatına çevriliyor
    })
        .then(response => response.json())
        .then(data => {
            //return data; // JSON verisini döndürüyoruz
            window.location.reload();
        })
        .catch(error => {
            // Hata oluşursa ErrorSwal göster
            ErrorSwal();
            throw error; // Hata durumunu dışarı döndürüyoruz
        });
}

function SetHTMLtoDiv(id, content) {
    document.getElementById(id).innerHTML = content;
}

// Spinner'ı görünür hale getirir
function OpenSpinner() {
    //document.getElementById('layoutspinner').style.display = 'block'; // Spinner'ı göster
    document.getElementById('layoutspinner').style.visibility = 'visible';
}

// Spinner'ı gizler
function CloseSpinner() {
    document.getElementById('layoutspinner').style.visibility = 'hidden';
}


function ValidateTCKimlikNo(tcNo) {
    // TC Kimlik numarasının 11 haneli ve sadece rakamlardan oluştuğunun kontrolü
    if (!/^[1-9][0-9]{10}$/.test(tcNo)) {
        return false;
    }

    let digits = tcNo.split('').map(Number); // TC numarasını hanelerine ayır ve rakamlar haline getir

    // 1., 3., 5., 7. ve 9. haneler
    let oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];

    // 2., 4., 6. ve 8. haneler
    let evenSum = digits[1] + digits[3] + digits[5] + digits[7];

    // 10. hane doğrulama
    let tenthDigit = ((oddSum * 7) - evenSum) % 10;
    if (tenthDigit !== digits[9]) {
        return false;
    }

    // 11. hane doğrulama
    let eleventhDigit = (oddSum + evenSum + digits[9]) % 10;
    if (eleventhDigit !== digits[10]) {
        return false;
    }

    // TC Kimlik numarası geçerli
    return true;
}


function ValidateEmail(email) {
    // E-posta doğrulama regex'i
    const emailPattern = /^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$/;

    // E-posta adresinin regex'e uyup uymadığını kontrol ediyoruz
    if (!emailPattern.test(email)) {
        ErrorSwal("Geçersiz E-posta", "Lütfen geçerli bir e-posta adresi girin."); // Geçersiz e-posta adresi hatası göster
        return false; // Doğrulama başarısız
    }
    return true; // Doğrulama başarılı
}


function TableFilter(tableId, inputId) {
    document.getElementById(inputId).addEventListener("keyup", function () {
        var input, filter, table, tr, td, i, j, txtValue, visibleRowCount = 0;
        input = document.getElementById(inputId);
        filter = input.value.toLowerCase();
        table = document.getElementById(tableId);
        tr = table.getElementsByTagName("tr");

        // Tablodaki her satırı dolaş
        for (i = 1; i < tr.length; i++) {
            tr[i].style.display = "none"; // Satırı gizle (eğer arama eşleşmezse)

            // Her satırdaki sütunları kontrol et
            td = tr[i].getElementsByTagName("td");
            for (j = 0; j < td.length; j++) {
                if (td[j]) {
                    txtValue = td[j].textContent || td[j].innerText;
                    if (txtValue.toLowerCase().indexOf(filter) > -1) {
                        tr[i].style.display = ""; // Eğer eşleşme varsa satırı göster
                        visibleRowCount++; // Görünen satır sayısını artır
                        break; // Eşleşme bulduğunda diğer sütunları kontrol etmeye gerek yok
                    }
                }
            }
        }

        // Eğer hiçbir satır görünmüyorsa (visibleRowCount 0 ise) kayıt bulunamadı mesajını göster
        if (visibleRowCount === 0) {
            document.getElementById("noResultsMessage").style.display = "block";
        } else {
            document.getElementById("noResultsMessage").style.display = "none";
        }
    });

}




WebFont.load({
    google: { families: ["Public Sans:300,400,500,600,700"] },
    custom: {
        families: [
            "Font Awesome 5 Solid",
            "Font Awesome 5 Regular",
            "Font Awesome 5 Brands",
            "simple-line-icons",
        ],
        urls: ['/tema/assets/css/fonts.min.css'],
    },
    active: function () {
        sessionStorage.fonts = true;
    },
});