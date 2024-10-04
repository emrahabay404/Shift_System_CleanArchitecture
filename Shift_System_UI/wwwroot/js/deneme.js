
$(document).ready(function () {

    OpenSpinner();

    GetRequestSwal("https://api.github.com/users/hadley/repos", {})
        .then(data => {
            createTableFromJSON(data);
        })
        .catch(error => {
            TitleAndTextSwal(error, "HATA");
        });

    function createTableFromJSON(data) {
        // Tabloyu oluşturma
        let table = `<table border="1" class="table table-striped">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Repo Name</th>
                        <th>Full Name</th>
                        <th>Full Name</th>
                        <th>Full Name</th>
                        <th>Full Name</th>
                        <th>Full Name</th>
                        <th>Owner Username</th>
                        <th>Owner Avatar</th>
                        <th>Repo URL</th>
                    </tr>
                </thead>
                <tbody>`;

        // JSON verisini tablo satırlarına dönüştürme
        data.forEach(item => {
            table += `<tr>
                <td>${item.id}</td>
                <td>${item.name}</td>
                <td>${item.name}</td>
                <td>${item.name}</td>
                <td>${item.name}</td>
                <td>${item.name}</td>
                <td>${item.full_name}</td>
                <td>${item.owner.login}</td>
                <td><img src="${item.owner.avatar_url}" width="50" height="50"/></td>
                <td><a href="${item.html_url}" target="_blank">${item.html_url}</a></td>
              </tr>`;
        });

        table += `</tbody></table>`;
         
        SetHTMLtoDiv("tablediv", table);
    }

    CloseSpinner();
});



