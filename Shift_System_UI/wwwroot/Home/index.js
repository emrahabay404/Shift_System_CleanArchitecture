let originalData = []; // Tüm verileri saklamak için

// Shifts veri yükleme fonksiyonu
async function loadShifts(pageNumber = 1) {
    const pageSize = document.getElementById("pageSize").value;

    try {
        // API çağrısı
        const response = await fetch(`/Home/GetShiftsWithPagination?pageNumber=${pageNumber}&pageSize=${pageSize}`);

        // Hata kontrolü
        if (!response.ok) {

            throw new Error('Veri çekme sırasında hata oluştu');
        }

        const data = await response.json();


        originalData = data.data;

        if (originalData && originalData.length > 0) {
            renderTable(originalData); // Tabloyu çiz
            renderPagination(data.currentPage, data.totalPages); // Sayfalamayı oluştur
        } else {

            document.querySelector("#shiftsTable tbody").innerHTML = "<tr><td colspan='3'>Kayıt yok</td></tr>";
        }

    } catch (error) {

    }
}

function renderTable(data) {
    const tableBody = document.querySelector("#shiftsTable tbody");

    if (data.length > 0) {
        // Satırları doldurma
        tableBody.innerHTML = data.map(row => `
            <tr>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${row.shift_Name}</td>
                <td>${new Date(row.createdDate).toLocaleDateString()}</td>
                <td>
                    <div class="d-flex">
                        <button class="btn btn-primary me-2" onclick="editShift('${row.id}')">
                            <i class="fas fa-edit"></i>
                        </button>
                        <button class="btn btn-danger" onclick="deleteShift('${row.id}')">
                            <i class="fas fa-trash"></i>
                        </button>
                    </div>
                </td>
            </tr>
        `).join('');
    } else {
        tableBody.innerHTML = "<tr><td colspan='3'>Kayıt yok</td></tr>";
    }
}

// Sayfalama butonlarını oluşturma
function renderPagination(currentPage, totalPages) {
    const paginationDiv = document.getElementById("pagination");
    paginationDiv.innerHTML = '';

    for (let i = 1; i <= totalPages; i++) {
        paginationDiv.innerHTML += `<button class="btn ${i === currentPage ? 'btn-secondary' : 'btn-light'}" onclick="loadShifts(${i})">${i}</button>`;
    }
}

// Tabloda filtreleme fonksiyonu
function filterTable() {
    const filterInput = document.getElementById("filterInput").value.toLowerCase();
    const filteredData = originalData.filter(row => {
        return row.shift_Name.toLowerCase().includes(filterInput) ||
            new Date(row.createdDate).toLocaleDateString().includes(filterInput);
    });

    // Tabloyu filtrelenmiş verilerle yeniden çiz
    renderTable(filteredData);

    // Eğer hiçbir satır görünmezse "Kayıt bulunamadı" mesajını göster
    if (filteredData.length === 0) {
        document.querySelector("#shiftsTable tbody").innerHTML = "<tr><td colspan='3'>Kayıt bulunamadı</td></tr>";
    }
}

// Düzenleme işlemi için yönlendirme
function editShift(id) {
    window.location.href = `/home/edit-shift/${id}`;
}

// Silme işlemi için ConfirmSwal ile onay al ve isteği gönder
function deleteShift(id) {
    ConfirmationSwal().then((isConfirmed) => {
        if (isConfirmed) {
            // Silme işlemini gerçekleştirme
            fetch(`/home/deleteshift/${id}`, { method: 'GET' })
                .then(response => {
                    if (!response.ok) {

                        throw new Error('Silme işlemi sırasında hata oluştu');
                    }
                    return response.json();
                })
                .then(() => {
                    SuccessSwal(); // Başarılı mesajı göster
                    loadShifts(); // Tablonun yeniden yüklenmesi
                })
                .catch((error) => {

                    ErrorSwal(); // Hata mesajı göster
                });
        }
    });
}

window.onload = function () {
    loadShifts(); // Verileri yükler 
};


