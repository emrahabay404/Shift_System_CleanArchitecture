
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






// Global değişkenler
let currentPage = 1;
let totalPages = 1;

// Sayfa bilgisini güncelleme
function updatePaginationInfo(pageInfoSelectorId) {
    document.getElementById(pageInfoSelectorId).textContent = `Sayfa ${currentPage} / ${totalPages}`;
}

// Sayfa butonlarını kontrol etme
function togglePaginationButtons(hasPrevious, hasNext, prevPageBtnId, nextPageBtnId) {
    document.getElementById(prevPageBtnId).disabled = !hasPrevious;
    document.getElementById(nextPageBtnId).disabled = !hasNext;
}

// Önceki sayfaya gitme fonksiyonu
function prevPage(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId) {
    if (currentPage > 1) {
        currentPage--;
        loadShifts(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId);
    }
}

// Sonraki sayfaya gitme fonksiyonu
function nextPage(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId) {
    if (currentPage < totalPages) {
        currentPage++;
        loadShifts(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId);
    }
}

// Kayıt sayısı değiştiğinde tetiklenecek fonksiyon
function updatePageSize(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId) {
    currentPage = 1; // Sayfa numarasını 1 yap
    loadShifts(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId); // Yeni kayıt sayısına göre veriyi yükle
}

// Dinamik Tablo Başlıklarını ve Gövdesini Yükleme 
function loadShifts(url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId, editUrl, deleteUrl, addButtons = true) {
    const pageSize = document.getElementById(pageSizeSelectorId).value;
    const params = { pageNumber: currentPage, pageSize: pageSize };

    GetRequestSwal(url, params)
        .then(response => {
            if (response.succeeded) {
                updateTable(response.data, tableId, editUrl, deleteUrl);
                currentPage = response.currentPage;
                totalPages = response.totalPages;
                updatePaginationInfo(pageInfoSelectorId);
                togglePaginationButtons(response.hasPreviousPage, response.hasNextPage, prevPageBtnId, nextPageBtnId);

                CombinedTableFilter(tableId);

                if (addButtons) {
                    addExportButtons(tableId);
                }
            } else {
                console.error('Veri alınamadı', response.message);
            }
        })
        .catch(error => {
            console.error('Hata oluştu', error);
        });
}

function updateTable(data, tableId, editUrl, deleteUrl) {
    const table = document.getElementById(tableId);
    const thead = table.querySelector("thead");
    const tbody = table.querySelector("tbody");

    // Başlıkları oluşturma
    thead.innerHTML = ""; // Mevcut başlıkları temizle
    const headerRow = document.createElement("tr");

    if (data.length > 0) {
        const firstItem = data[0];
        Object.keys(firstItem).forEach(key => {
            const th = document.createElement("th");
            th.textContent = key; // Anahtar adını başlık olarak ekle
            headerRow.appendChild(th);
        });

        const actionTh = document.createElement("th");
        actionTh.textContent = "Actions";
        headerRow.appendChild(actionTh);
    }

    thead.appendChild(headerRow);
    tbody.innerHTML = ""; // Önceki satırları temizle

    data.forEach(item => {
        const row = document.createElement("tr");

        Object.keys(item).forEach(key => {
            const cell = document.createElement("td");
            cell.textContent = item[key] ? item[key] : "-";
            row.appendChild(cell);
        });

        // Edit ve Delete butonları
        const actionCell = document.createElement("td");

        const editButton = document.createElement("button");
        editButton.className = "btn btn-primary mr-2";
        editButton.innerHTML = '<i class="fas fa-edit"></i>';
        editButton.onclick = function () {
            editRow(item.id, editUrl); // Dinamik editUrl kullan
        };

        const deleteButton = document.createElement("button");
        deleteButton.className = "btn btn-danger";
        deleteButton.innerHTML = '<i class="fas fa-trash"></i>';
        deleteButton.onclick = function () {
            deleteRow(item.id, deleteUrl, url, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId, editUrl, deleteUrl); // Dinamik deleteUrl kullan
        };

        actionCell.appendChild(editButton);
        actionCell.appendChild(deleteButton);
        row.appendChild(actionCell);

        tbody.appendChild(row);
    });

    CombinedTableFilter(tableId);
}

// CombinedTableFilter: Tabloya genel ve sütun bazlı filtre ekleme
function CombinedTableFilter(tableId) {
    var table = document.getElementById(tableId);
    var thead = table.querySelector("thead");
    var tbody = table.querySelector("tbody");

    // Eğer genel arama ve noResultsMessage zaten eklenmişse, tekrar eklememek için kontrol et
    if (!document.getElementById(tableId + "generalSearchInput")) {
        // Genel filtreleme input'unu oluştur
        var generalFilterInput = document.createElement("input");
        generalFilterInput.type = "text";
        generalFilterInput.className = "form-control";
        generalFilterInput.placeholder = "Tabloda genel arama yapın...";
        generalFilterInput.id = tableId + "generalSearchInput";

        // noResultsMessage öğesini oluştur
        var noResultsMessage = document.createElement("p");
        noResultsMessage.id = tableId + "noResultsMessage";
        noResultsMessage.style.display = "none";
        noResultsMessage.style.color = "gray";
        noResultsMessage.textContent = "Kayıt bulunamadı.";

        // Tablonun üzerine genel filtre input'unu ve noResultsMessage öğesini ekle
        table.parentNode.insertBefore(generalFilterInput, table);
        table.parentNode.insertBefore(noResultsMessage, table.nextSibling);

        // Genel filtreleme işlevi
        generalFilterInput.addEventListener("keyup", function () {
            applyFilters();
        });
    }

    // Filtre satırını kontrol et, zaten var mı?
    if (!thead.querySelector(".filter-row")) {
        var headers = thead.getElementsByTagName("th");
        var filterRow = document.createElement("tr");
        filterRow.classList.add("filter-row"); // Filtre satırına sınıf ekliyoruz

        // Sütun bazlı filtreleme için filtre satırını oluştur
        for (var i = 0; i < headers.length; i++) {
            var th = document.createElement("th");
            var input = document.createElement("input");
            input.type = "text";
            input.className = "form-control";
            input.placeholder = "Sütunu filtrele...";
            input.setAttribute("data-column", i); // Her input'a sütun indexi ata
            th.appendChild(input);
            filterRow.appendChild(th);

            // Sütun bazlı filtreleme işlevi
            input.addEventListener("keyup", function () {
                applyFilters();
            });
        }

        // Filtre inputlarını tablo başlığının altına ekle
        thead.appendChild(filterRow);
    }

    // Filtreleme fonksiyonunu uygula
    function applyFilters() {
        var generalFilter = document.getElementById(tableId + "generalSearchInput").value.toLowerCase();
        var columnFilters = getFilters();
        var rows = tbody.getElementsByTagName("tr");
        var visibleRowCount = 0;

        // Her satırı kontrol et
        for (var i = 0; i < rows.length; i++) {
            var tr = rows[i];
            var tds = tr.getElementsByTagName("td");
            var showRow = true;

            // Sütun bazlı filtreyi kontrol et
            for (var j = 0; j < columnFilters.length; j++) {
                if (columnFilters[j]) {
                    var td = tds[j];
                    if (td) {
                        var txtValue = td.textContent || td.innerText;
                        if (txtValue.toLowerCase().indexOf(columnFilters[j].toLowerCase()) === -1) {
                            showRow = false;
                            break; // Sütun eşleşmezse satırı gizle
                        }
                    }
                }
            }

            // Genel filtreyi kontrol et
            if (showRow && generalFilter) {
                showRow = false;
                for (var k = 0; k < tds.length; k++) {
                    if (tds[k]) {
                        var txtValue = tds[k].textContent || tds[k].innerText;
                        if (txtValue.toLowerCase().indexOf(generalFilter) > -1) {
                            showRow = true;
                            break;
                        }
                    }
                }
            }

            // Satırı göster veya gizle
            tr.style.display = showRow ? "" : "none";
            if (showRow) visibleRowCount++;
        }

        // Eğer görünür satır yoksa, kayıt bulunamadı mesajını göster
        document.getElementById(tableId + "noResultsMessage").style.display = visibleRowCount === 0 ? "block" : "none";
    }

    // Sütun bazlı filtre değerlerini dönen fonksiyon
    function getFilters() {
        var filterInputs = thead.querySelector(".filter-row").getElementsByTagName("input");
        var filters = [];
        for (var i = 0; i < filterInputs.length; i++) {
            filters.push(filterInputs[i].value);
        }
        return filters;
    }
}

// Dinamik olarak pageSize seçeneklerini doldur
function populatePageSizeOptions(selectId) {

    const pageSizeOptions = [5, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100];
    const selectElement = document.getElementById(selectId);
    // Mevcut seçenekleri temizle
    selectElement.innerHTML = '';

    // Yeni seçenekleri ekle
    pageSizeOptions.forEach(optionValue => {
        const option = document.createElement('option');
        option.value = optionValue;
        option.textContent = optionValue;
        selectElement.appendChild(option);
    });
}
// Dinamik düzenleme işlemi için
function deleteRow(id, deleteUrl, loadUrl, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId, editUrl, deleteUrlAfter) {
    ConfirmationSwal().then((isConfirmed) => {
        if (isConfirmed) {
            const fullUrl = `${deleteUrl}?id=${id}`;
            PostRequestSwal(fullUrl)
                .then(() => {
                    SuccessSwal();
                    loadShifts(loadUrl, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId, editUrl, deleteUrlAfter); // Dinamik URL'leri kullanarak tabloyu yeniden yükle
                })
                .catch(() => {
                    ErrorSwal();
                });
        }
    });
}

// Dinamik silme işlemi için
function deleteRow(id, deleteUrl, loadUrl, pageSizeSelectorId, pageInfoSelectorId, prevPageBtnId, nextPageBtnId, tableId, editUrl, deleteUrlAfter) {
    // Silme işlemi öncesi onay alıyoruz
    ConfirmationSwal().then((isConfirmed) => {
        if (isConfirmed) {
            const fullUrl = `${deleteUrl}?id=${id}`;
            // Silme işlemini burada yapın
            PostRequestSwal(fullUrl)
                .then(() => {
                    SuccessSwal(); // Silme işlemi başarılı olduysa göster
                    // Silme işleminden sonra tabloyu yeniden yüklemek için dinamik URL kullanılıyor
                    loadShifts(
                        loadUrl,              // Dinamik olarak gelen tablo yükleme URL'si
                        pageSizeSelectorId,   // Dinamik olarak gelen sayfa boyutu seçici ID
                        pageInfoSelectorId,   // Dinamik olarak gelen sayfa bilgisi ID
                        prevPageBtnId,        // Önceki sayfa butonu ID'si
                        nextPageBtnId,        // Sonraki sayfa butonu ID'si
                        tableId,              // Tablo ID'si
                        editUrl,              // Edit URL'si
                        deleteUrlAfter        // Delete URL'si
                    );
                })
                .catch(() => {
                    ErrorSwal(); // Hata oluşursa göster
                });
        }
    });
}





// Dinamik modal oluşturma ve içeriği güncelleme fonksiyonu
function createAndShowModal(modalTitle, fetchUrl) {
    // Modal div yapısı (Bootstrap 5)
    const modalHtml = `
        <!-- Modal -->
        <div
          class="modal fade"
          id="exampleModal"
          tabindex="-1"
          role="dialog"
          aria-labelledby="exampleModalLabel"
          aria-hidden="true"
        >
          <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
            <div class="modal-content">
              <div class="modal-header">
                <h5 class="modal-title fw-bold" id="exampleModalLabel">${modalTitle}</h5>
                <button
                  type="button"
                  class="btn-close"
                  data-bs-dismiss="modal"
                  aria-label="Close"
                ></button>
              </div>
              <div class="modal-body">
                <div class="spinner-border text-primary" id="loadingSpinner" role="status">
                  <span class="visually-hidden">Loading...</span>
                </div>
                <div class="table-responsive d-none" id="modalTableWrapper"> <!-- Yatay kaydırma için kapsayıcı div -->
                  <table class="table table-hover" id="modalTable">
                    <thead>
                      <tr id="modalTableHead"></tr>
                    </thead>
                    <tbody id="modalTableBody">
                      <!-- Dinamik olarak doldurulacak -->
                    </tbody>
                  </table>
                </div>
              </div>
              <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                  Close
                </button>
                <button type="button" class="btn btn-primary" id="saveChangesButton">Save changes</button>
              </div>
            </div>
          </div>
        </div>
    `;

    // Önceki modalı kaldır (Eğer varsa)
    const existingModal = document.getElementById('exampleModal');
    if (existingModal) {
        existingModal.remove();
    }

    // Modalı body'ye ekle
    document.body.insertAdjacentHTML('beforeend', modalHtml);

    // Modal'ı hemen aç
    const modal = new bootstrap.Modal(document.getElementById('exampleModal'));
    modal.show();

    // Dinamik olarak verileri getir ve modal'daki tabloyu güncelle
    fetch(fetchUrl)
        .then(response => response.json())
        .then(data => {
            const tableHead = document.getElementById("modalTableHead");
            const tableBody = document.getElementById("modalTableBody");
            const spinner = document.getElementById("loadingSpinner");
            const modalTableWrapper = document.getElementById("modalTableWrapper");

            // Tabloyu temizle (önceden var olan satırları kaldır)
            tableHead.innerHTML = "";
            tableBody.innerHTML = "";

            // Eğer gelen veri bir object ise "data" anahtarı ile başlayan array olabilir
            if (data.data && Array.isArray(data.data)) {
                data = data.data;  // data içinde veri varsa oraya iniyoruz
            }

            // Eğer veri array ise, tabloyu doldur
            if (Array.isArray(data)) {
                if (data.length > 0) {
                    const keys = Object.keys(data[0]);

                    // Başlıkları ekle
                    keys.forEach(key => {
                        const th = document.createElement('th');
                        th.innerText = key.toUpperCase();
                        tableHead.appendChild(th);
                    });

                    // Verileri tabloya ekle
                    data.forEach(item => {
                        const tr = document.createElement('tr');
                        keys.forEach(key => {
                            const td = document.createElement('td');
                            // Eğer veri bir boolean ise true/false olarak göster
                            if (typeof item[key] === 'boolean') {
                                td.innerText = item[key] ? 'Evet' : 'Hayır';
                            }
                            // Eğer veri bir nesne ise JSON.stringify ile göster (ve uzunluğu kontrol et)
                            else if (typeof item[key] === 'object' && item[key] !== null) {
                                td.innerText = JSON.stringify(item[key], null, 2).slice(0, 100) + (JSON.stringify(item[key]).length > 100 ? '...' : '');
                            }
                            // Eğer veri bir string ise uzunluk kontrolü yap
                            else if (typeof item[key] === 'string' && item[key].length > 50) {
                                td.innerText = item[key].slice(0, 50) + '...';
                            } else {
                                td.innerText = item[key] !== null && item[key] !== undefined ? item[key] : '-'; // Değer yoksa "-" göster
                            }
                            tr.appendChild(td);
                        });
                        tableBody.appendChild(tr);
                    });
                } else {
                    // Eğer boş array geldiyse
                    tableBody.innerHTML = '<tr><td colspan="5">Kayıt bulunamadı.</td></tr>';
                }
            } else {
                // Eğer veri object ise, tek bir satır olarak ekleyelim
                const keys = Object.keys(data);

                // Başlıkları ekle
                keys.forEach(key => {
                    const th = document.createElement('th');
                    th.innerText = key.toUpperCase();
                    tableHead.appendChild(th);
                });

                // Veriyi tabloya ekle
                const tr = document.createElement('tr');
                keys.forEach(key => {
                    const td = document.createElement('td');
                    // Boolean değerler için kontrol
                    if (typeof data[key] === 'boolean') {
                        td.innerText = data[key] ? 'Evet' : 'Hayır';
                    }
                    // Eğer veri bir nesne ise JSON.stringify ile göster
                    else if (typeof data[key] === 'object' && data[key] !== null) {
                        td.innerText = JSON.stringify(data[key], null, 2).slice(0, 100) + (JSON.stringify(data[key]).length > 100 ? '...' : '');
                    }
                    // Eğer veri bir string ise uzunluk kontrolü yap
                    else if (typeof data[key] === 'string' && data[key].length > 50) {
                        td.innerText = data[key].slice(0, 50) + '...';
                    } else {
                        td.innerText = data[key] !== null && data[key] !== undefined ? data[key] : '-'; // Değer yoksa "-" göster
                    }
                    tr.appendChild(td);
                });
                tableBody.appendChild(tr);
            }

            // Spinner'ı gizle ve tabloyu göster
            spinner.classList.add('d-none');
            modalTableWrapper.classList.remove('d-none');
        })
        .catch(error => {
            console.error('Hata oluştu:', error);
            document.getElementById("modalTableBody").innerHTML = "<tr><td>Veri alınamadı.</td></tr>";
        });
}




function addExportButtons(tableId) {
    const table = document.getElementById(tableId);

    // Eğer daha önce buton eklenmişse, tekrar eklemeyelim
    if (document.getElementById("exportButtonsContainer")) {
        return; // Butonlar zaten varsa, çık
    }

    // Butonlar için bir kapsayıcı div oluştur
    const buttonContainer = document.createElement("div");
    buttonContainer.id = "exportButtonsContainer"; // ID verdik ki tekrar tekrar oluşturulmasın
    buttonContainer.classList.add("mb-3"); // Margin-bottom ekle

    // PDF Butonu
    const pdfButton = document.createElement("button");
    pdfButton.className = "btn btn-primary mr-2";
    pdfButton.innerText = "PDF";
    pdfButton.onclick = function () {
        exportTableToPDF(tableId);
    };

    // Excel Butonu
    const excelButton = document.createElement("button");
    excelButton.className = "btn btn-success mr-2";
    excelButton.innerText = "Excel";
    excelButton.onclick = function () {
        exportTableToExcel(tableId);
    };

    // CSV Butonu
    const csvButton = document.createElement("button");
    csvButton.className = "btn btn-info mr-2";
    csvButton.innerText = "CSV";
    csvButton.onclick = function () {
        exportTableToCSV(tableId);
    };

    // Yazdırma Butonu
    const printButton = document.createElement("button");
    printButton.className = "btn btn-warning";
    printButton.innerText = "Yazdır";
    printButton.onclick = function () {
        printTable(tableId);
    };

    // Butonları buttonContainer içine ekleyelim
    buttonContainer.appendChild(pdfButton);
    buttonContainer.appendChild(excelButton);
    buttonContainer.appendChild(csvButton);
    buttonContainer.appendChild(printButton);

    // Buton kapsayıcısını tablonun üstüne ekleyelim
    table.parentNode.insertBefore(buttonContainer, table);
}
function printTable(tableId) {
    const tableContent = document.getElementById(tableId).outerHTML;
    const printWindow = window.open('', '', 'height=600,width=800');
    printWindow.document.write('<html><head><title>Yazdır</title></head><body>');
    printWindow.document.write(tableContent);
    printWindow.document.write('</body></html>');
    printWindow.document.close();
    printWindow.print();
}
function exportTableToPDF(tableId) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Tablo verilerini al
    const table = document.getElementById(tableId);
    const rows = [];
    const headers = [];

    // Başlıkları al
    const headerCells = table.querySelectorAll('thead tr th');
    headerCells.forEach(th => {
        headers.push(th.textContent);
    });

    // Verileri al
    const tableRows = table.querySelectorAll('tbody tr');
    tableRows.forEach(row => {
        const rowData = [];
        row.querySelectorAll('td').forEach(td => {
            rowData.push(td.textContent);
        });
        rows.push(rowData);
    });

    // PDF'e tabloyu ekle
    doc.autoTable({
        head: [headers],
        body: rows
    });

    // PDF'i kaydet
    //doc.save('table_data.pdf');
    doc.save(tableId + '.pdf');
}
function exportTableToExcel(tableId) {
    const table = document.getElementById(tableId);
    const wb = XLSX.utils.book_new();
    const ws = XLSX.utils.table_to_sheet(table);

    XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
    XLSX.writeFile(wb, 'table_data.xlsx');
}
function exportTableToCSV(tableId) {
    const table = document.getElementById(tableId);
    let csvContent = "";

    // Başlıkları al
    const headers = table.querySelectorAll('thead tr th');
    let headerRow = [];
    headers.forEach(th => {
        headerRow.push(th.textContent);
    });
    csvContent += headerRow.join(",") + "\n";

    // Verileri al
    const rows = table.querySelectorAll('tbody tr');
    rows.forEach(row => {
        const rowData = [];
        row.querySelectorAll('td').forEach(td => {
            rowData.push(td.textContent);
        });
        csvContent += rowData.join(",") + "\n";
    });

    // CSV dosyasını oluştur ve indir
    const blob = new Blob([csvContent], { type: 'text/csv;charset=utf-8;' });
    const link = document.createElement('a');
    if (link.download !== undefined) {
        const url = URL.createObjectURL(blob);
        link.setAttribute('href', url);
        link.setAttribute('download', 'table_data.csv');
        link.style.visibility = 'hidden';
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}
