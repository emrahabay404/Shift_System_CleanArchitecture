window.onload = function () {
    const defaultPageSize = 250;  // Varsayılan sayfa boyutu

    // pageSize select kutusundaki varsayılan değeri seç
    document.getElementById('pageSize').value = defaultPageSize;

    // İlk veri yükleme çağrısını yap
    loadShifts(
        '/Home/GetShiftsWithPagination',  // Tablo verileri için API URL'si
        'pageSize',                       // Sayfa boyutunu seçtiğiniz elementin ID'si
        'pageInfo',                       // Sayfa bilgisini göstermek için elementin ID'si
        'prevPageBtn',                    // Önceki sayfa butonunun ID'si
        'nextPageBtn',                    // Sonraki sayfa butonunun ID'si
        'shiftsTable',                    // Tablo elementinin ID'si
        '/Home/EditShift',                // Edit işlemi için URL
        '/Home/DeleteShift',              // Delete işlemi için URL
        true                              // Export butonlarını tabloya ekle
    );

    populatePageSizeOptions('pageSize');  // Sayfa boyutunu dinamik olarak yükle
};


// Butona tıklama ile modalı açmak
document.getElementById("openModalButton").addEventListener("click", function () {
    createAndShowModal('Todo Detayı', 'https://localhost:7161/api/Teams/paged');
});


