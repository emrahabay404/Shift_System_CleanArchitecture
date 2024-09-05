$(document).ready(function () {


    $("#multi-filter-select").DataTable({
        pageLength: 5,
        initComplete: function () {
            this.api()
                .columns()
                .every(function () {
                    var column = this;
                    var select = $(
                        '<select class="form-select"><option value=""></option></select>'
                    )
                        .appendTo($(column.footer()).empty())
                        .on("change", function () {
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());

                            column
                                .search(val ? "^" + val + "$" : "", true, false)
                                .draw();
                        });

                    column
                        .data()
                        .unique()
                        .sort()
                        .each(function (d, j) {
                            select.append(
                                '<option value="' + d + '">' + d + "</option>"
                            );
                        });
                });
        },
    });

});


// Dosya Seçildiğinde Önizleme Gösterme
$('#fileInput').on('change', function () {
    var file = this.files[0];
    var preview = $('#filePreview');

    if (file) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var fileType = file.type;
            preview.empty(); // Önizleme alanını temizle

            // Resim Dosyası İçin Önizleme
            if (fileType.startsWith('image/')) {
                var img = $('<img />', {
                    src: e.target.result,
                    width: '200px',
                    class: 'img-thumbnail'
                });
                preview.append(img);
            }
            // PDF Dosyası İçin Önizleme
            else if (fileType === 'application/pdf') {
                var pdfEmbed = $('<embed />', {
                    src: e.target.result,
                    type: 'application/pdf',
                    width: '100%',
                    height: '500px'
                });
                preview.append(pdfEmbed);
            }
            // Diğer Dosya Türleri İçin Bilgi Mesajı
            else {
                preview.append('<p>Bu dosya türü için önizleme desteklenmiyor: ' + fileType + '</p>');
            }
        };
        reader.readAsDataURL(file);
    }
});

// Dosya Yükleme İşlemi
function uploadFile() {
    var formData = new FormData();
    var fileInput = $('#fileInput')[0].files[0];
    formData.append('file', fileInput);

    $.ajax({
        url: '/Home/Upload/',
        type: 'POST',
        data: formData,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response.success) {
                $('#message').html('<div class="alert alert-success">' + response.message + '</div>');
                resetForm(); // Form ve önizlemeyi sıfırlama işlemi
                scrollToTop(); // Sayfanın en üstüne kaydır
            } else {
                $('#message').html('<div class="alert alert-danger">' + response.message + '</div>');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#message').html('<div class="alert alert-danger">Bir hata oluştu: ' + textStatus + '</div>');
        }
    });
}

// Form ve Önizleme Alanını Sıfırlama İşlemi
function resetForm() {
    $('#fileInput').val(''); // Dosya girişini sıfırla
    $('#filePreview').html('<p>Dosya seçildiğinde burada önizleme gözükecek.</p>'); // Önizleme alanını sıfırla
}

// Sayfanın En Üstüne Kaydırma İşlemi
function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

