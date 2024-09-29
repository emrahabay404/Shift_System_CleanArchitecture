

$(document).ready(function () {

    OpenSpinner();

    GetRequestSwal("https://fake-json-api.mock.beeceptor.com/users", {})
        .then(data => {
            createTableFromJSON(data);
        })
        .catch(error => {
            console.error("Hata:", error);
        });

    function createTableFromJSON(data) {
        // Tabloyu oluşturma
        let table = `<table border="1" class="table table-striped">
                    <thead>
                        <tr>
                            <th>ID</th>
                            <th>Name</th>
                            <th>Username</th>
                            <th>Email</th>
                            <th>Company</th>
                            <th>Address</th>
                            <th>Phone</th>
                            <th>State</th>
                            <th>Country</th>
                            <th>Photo</th>
                        </tr>
                    </thead>
                    <tbody>`;

        // JSON verisini tablo satırlarına dönüştürme
        data.forEach(item => {
            table += `<tr>
                    <td>${item.id}</td>
                    <td>${item.name}</td>
                    <td>${item.username}</td>
                    <td>${item.email}</td>
                    <td>${item.company}</td>
                    <td>${item.address}</td>
                    <td>${item.phone}</td>
                    <td>${item.state}</td>
                    <td>${item.country}</td>
                    <td><img src="${item.photo}" width="50" height="50"/></td>
                  </tr>`;
        });

        table += `</tbody></table>`;

        // Tablonun DOM'a eklenmesi
        document.getElementById("tablediv").innerHTML = table;
    }

    CloseSpinner();
});


document.getElementById('showSuccess').addEventListener('click', function () {
    TitleAndTextSwal("baslık", "");
});

