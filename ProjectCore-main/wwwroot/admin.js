document.addEventListener("DOMContentLoaded", function () {
    const token = localStorage.getItem("token");
    if (!token) {
        window.location.href = "login.html";
        return;
    }
    loadUsers();
    loadBakeries();
});

let users = [];
let bakeries = [];

function loadUsers() {
    const token = localStorage.getItem("token");
    fetch("/User", {
        headers: { "Authorization": `Bearer ${token}` }
    })
        .then(res => res.json())
        .then(usersData => {
            users = usersData;
            console.log(" משתמשים נטענו בהצלחה:", users);
            displayUsers(users);
            // ✅ אתחול פונקציונליות העריכה לאחר טעינת המשתמשים
            initializeEditFunctionality();
        });
}

function displayUsers(users) {
    const container = document.getElementById("customersList");
    container.innerHTML = "";

    if (users.length === 0) {
        container.innerHTML = "<p>לא נמצאו משתמשים</p>";
        return;
    }

    let table = `<table border="1">
        <tr><th>ID</th><th>שם משתמש</th><th>תפקיד</th><th>עריכה</th><th>מחיקה</th></tr>`;

    users.forEach(user => {
        table += `<tr>
            <td>${user.id}</td>
            <td>${user.username}</td>
            <td>${user.role}</td>
            <td><button class="edit-btn" data-id="${user.id}" data-username="${user.username}" data-role="${user.role}">ערוך</button></td>
            <td><button onclick="deleteUser(${user.id})">️ מחק</button></td>
        </tr>`;
    });

    table += "</table>";
    container.innerHTML = table;
}

// ✅ פונקציה לאתחול פונקציונליות העריכה
function initializeEditFunctionality() {
    const editButtons = document.querySelectorAll(".edit-btn");
    editButtons.forEach(button => {
        button.addEventListener("click", function () {
            const id = this.dataset.id;
            const username = this.dataset.username;
            const role = this.dataset.role;
            openEditForm(id, username, role);
        });
    });
}

function openEditForm(id, username, role) {
    document.getElementById("edit-id").value = id;
    document.getElementById("edit-username").value = username;
    document.getElementById("edit-role").value = role;
    document.getElementById("editForm").style.display = "block";
}

function updateUser() {
    const token = localStorage.getItem("token");
    const id = document.getElementById("edit-id").value;
    const username = document.getElementById("edit-username").value;
    const role = document.getElementById("edit-role").value;
    const password = document.getElementById("edit-password").value;

    console.log(" ID שנשלח לשרת:", id);

    if (!id || !username || !role || !password) {
        alert("⚠️ שגיאה: כל השדות חייבים להיות מלאים!");
        return;
    }

    const userData = { id: parseInt(id), username, role, password };
    console.log(" נתונים שנשלחים לשרת:", userData);

    fetch(`/User/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(userData)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    console.error("❌ שגיאות בשרת:", errorData.errors);
                    throw new Error("בעיה בעדכון הנתונים!");
                });
            }
            return response.json();
        })
        .then(data => {
            console.log(" תגובת השרת:", data);
            alert("✅ המשתמש עודכן בהצלחה!");
            location.reload();
        })
        .catch(error => {
            console.error("❌ שגיאה בעדכון המשתמש:", error);
            alert("❌ שגיאה בעדכון המשתמש! בדקי את הנתונים.");
        });
}

function closeEditForm() {
    document.getElementById("editForm").style.display = "none";
}

function deleteUser(userId) {
    const token = localStorage.getItem("token");
    if (!confirm("האם אתה בטוח שברצונך למחוק את המשתמש?")) return;

    fetch(`/User/${userId}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    })
        .then(response => {
            if (!response.ok) throw new Error("מחיקה נכשלה!");
            alert("✅ המשתמש נמחק בהצלחה!");
            location.reload();
        })
        .catch(error => alert("❌ שגיאה במחיקה: " + error));
}

function openAddUserForm() {
    document.getElementById("addUserForm").style.display = "block";
}

function closeAddUserForm() {
    document.getElementById("addUserForm").style.display = "none";
}

// ✅ פונקציה להוספת משתמש
function addUser() {
    const token = localStorage.getItem("token");
    const username = document.getElementById("add-username").value;
    const password = document.getElementById("add-password").value;
    const role = document.getElementById("add-role").value;

    if (!username || !password || !role) {
        alert("⚠️ שגיאה: כל השדות חייבים להיות מלאים!");
        return;
    }

    const userData = { username, password, role };

    fetch("/User", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(userData)
    })
        .then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    console.error("❌ שגיאות בשרת:", errorData.errors);
                    throw new Error("בעיה בהוספת המשתמש!");
                });
            }
            return response.json();
        })
        .then(data => {
            alert("✅ המשתמש נוסף בהצלחה!");
            location.reload();
        })
        .catch(error => {
            console.error("❌ שגיאה בהוספת המשתמש:", error);
            alert("❌ שגיאה בהוספת המשתמש! בדוק את הנתונים.");
        });
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

// ניהול עוגות
function loadBakeries() {
    const token = localStorage.getItem("token");
    fetch("/Bakerys", {
        headers: { "Authorization": `Bearer ${token}` }
    })
        .then(res => res.json())
        .then(bakeriesData => {
            bakeries = bakeriesData;
            displayBakeries(bakeries);
        });
}

function displayBakeries(bakeries) {
    const container = document.getElementById("bakeryList");
    container.innerHTML = "";

    if (bakeries.length === 0) {
        container.innerHTML = "<p>לא נמצאו עוגות</p>";
        return;
    }

    let table = `<table border="1">
        <tr><th>ID</th><th>שם עוגה</th><th>עריכה</th><th>מחיקה</th></tr>`;

    bakeries.forEach(bakery => {
        table += `<tr>
            <td>${bakery.id}</td>
            <td>${bakery.name}</td>
            <td><button onclick="openEditBakeryForm(${bakery.id}, '${bakery.name}')">ערוך</button></td>
            <td><button onclick="deleteBakery(${bakery.id})">מחק</button></td>
        </tr>`;
    });

    table += "</table>";
    container.innerHTML = table;
}

function openAddBakeryForm() {
    document.getElementById("addBakeryForm").style.display = "block";
}

function closeAddBakeryForm() {
    document.getElementById("addBakeryForm").style.display = "none";
}

function addBakery() {
    const token = localStorage.getItem("token");
    const name = document.getElementById("add-bakery-name").value;
    const userId = document.getElementById("add-bakery-user-id").value;

    fetch("/Bakerys", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify({ name, userId: parseInt(userId) }) // העברת userId ב-body
    })
        .then(res => res.json())
        .then(bakery => {
            bakeries.push(bakery);
            displayBakeries(bakeries);
            closeAddBakeryForm();
        });
}

function openEditBakeryForm(id, name) {
    document.getElementById("edit-bakery-id").value = id;
    document.getElementById("edit-bakery-name").value = name;
    document.getElementById("editBakeryForm").style.display = "block";
}

function closeEditBakeryForm() {
    document.getElementById("editBakeryForm").style.display = "none";
}

function updateBakery() {
    const token = localStorage.getItem("token");
    const id = document.getElementById("edit-bakery-id").value;
    const name = document.getElementById("edit-bakery-name").value;
    const userId = document.getElementById("edit-bakery-user-id").value;

    fetch(`/Bakerys/${id}?name=${encodeURIComponent(name)}&userId=${userId}`, {
        method: "PUT",
        headers: {
            "Authorization": `Bearer ${token}`
        }
    })
        .then(res => {
            if (!res.ok) {
                // טיפול בשגיאות HTTP
                return res.json().then(errorData => {
                    throw new Error(errorData.message || "שגיאה בעדכון מאפה");
                });
            }
            // טיפול בתגובה תקינה
            return res.json();
        })
        .then(updatedBakery => {
            const index = bakeries.findIndex(bakery => bakery.id === updatedBakery.id);
            if (index !== -1) {
                bakeries[index] = updatedBakery;
                displayBakeries(bakeries);
                closeEditBakeryForm();
            }
        })
        .catch(error => {
            console.error("שגיאה בעדכון מאפה:", error);
            alert(error.message || "שגיאה בעדכון מאפה. אנא נסה שוב.");
        });
}

function deleteBakery(id) {
    const token = localStorage.getItem("token");
    const userId = document.getElementById("edit-bakery-user-id").value;

    fetch(`/Bakerys/${id}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` },
        body: JSON.stringify({userId: parseInt(userId)}) //העברת userId ב body
    })
        .then(res => {
            if (res.ok) {
                bakeries = bakeries.filter(bakery => bakery.id !== id);
                displayBakeries(bakeries);
            }
        });
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}