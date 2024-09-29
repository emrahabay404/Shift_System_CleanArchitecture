
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

function setHTML(id, content) {
    document.getElementById(id).innerHTML = content;
}

// Spinner'ı görünür hale getirir
function OpenSpinner() {
    document.getElementById('layoutspinner').style.display = 'block'; // Spinner'ı göster
}

// Spinner'ı gizler
function CloseSpinner() {
    document.getElementById('layoutspinner').style.display = 'none'; // Spinner'ı gizle
}
