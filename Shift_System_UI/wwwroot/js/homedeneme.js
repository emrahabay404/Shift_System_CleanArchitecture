$(document).ready(function () {
    var currentPage = 1;
    var pageSize = 2500;


    function loadShifts(pageNumber) {
        $.ajax({
            url: `/Home/GetShiftsWithPagination?pageNumber=${pageNumber}&pageSize=${pageSize}`,
            method: 'GET',
            success: function (response) {
                renderTable(response.data); // Tabloyu güncelle
                updatePaginationControls(response.currentPage, response.totalPages); // Sayfalandırma düğmelerini güncelle
                document.getElementById("spinnervardiya").style.display = "none";
            },
            error: function (error) {
                console.error("Hata oluştu:", error);
                alert("Veriler yüklenirken bir hata oluştu.");
            }
        });

    }

    function renderTable(data) {
        var tbody = $('#vardiyalist');
        tbody.empty(); // Mevcut tablo içeriğini temizle

        data.forEach(function (item) {
            tbody.append(`
                        <tr>
                            <td>${item.id}</td>
                            <td>${item.shift_Name}</td>
                         <td>${item.url1}</td> 
                            <td>${item.url2}</td> 
                            <td>${item.url3}</td> 
                            <td>${item.url4}</td> 
                            <td>${item.url5}</td> 
                        </tr>
                    `);
        });
    }

    function updatePaginationControls(currentPage, totalPages) {
        var paginationControls = $('#pagination-controls');
        paginationControls.empty(); // Mevcut düğmeleri temizle

        // Önceki sayfa düğmesi
        if (currentPage > 1) {
            paginationControls.append(`<button class="page-button" data-page="${currentPage - 1}">Önceki</button>`);
        }

        // Sayfa numarası düğmeleri
        for (var i = 1; i <= totalPages; i++) {
            var activeClass = (i === currentPage) ? 'active' : '';
            paginationControls.append(`<button class="page-button ${activeClass}" data-page="${i}">${i}</button>`);
        }

        // Sonraki sayfa düğmesi
        if (currentPage < totalPages) {
            paginationControls.append(`<button class="page-button" data-page="${currentPage + 1}">Sonraki</button>`);
        }

        // Sayfa düğmelerine tıklama olayı ekleyin
        $('.page-button').click(function () {
            var pageNumber = $(this).data('page');
            currentPage = pageNumber; // Mevcut sayfayı güncelle
            loadShifts(pageNumber);
        });
    }

    document.getElementById("spinnervardiya").style.display = "block";
    // İlk sayfa verilerini yükle
    loadShifts(currentPage);

});

