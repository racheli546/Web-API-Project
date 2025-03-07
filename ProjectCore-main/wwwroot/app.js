// 🔐 התחברות ושליחת טוקן לשרת
function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    fetch("/Auth/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    })
    .then(res => res.json())
    .then(data => {
    if (data.token) {
        localStorage.setItem("token", data.token);
        
        const tokenData = JSON.parse(atob(data.token.split('.')[1]));
        console.log(tokenData);  // הוסף כאן את הדפסת הטוקן
        
        const role = tokenData["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];  // נניח שהתפקיד נמצא בטוקן כ-"role"
        console.log("role: ", role);  // הדפסת התפקיד שנמצא בטוקן

        if (role === "Admin") {
            console.log("הולך למנהל");
            window.location.href = "admin.html";
        } else {
            console.log("הולך ללקוח");
            window.location.href = "index.html";
        }
    } else {
        document.getElementById("errorMessage").innerText = "⚠️ שם משתמש או סיסמה שגויים!";
    }
});

}

// 🏠 בדיקת התחברות והצגת שם המשתמש
function checkLogin() {
    const token = localStorage.getItem("token");
    if (!token) {
        window.location.href = "login.html"; // אם אין טוקן, מעבר להתחברות
    } else {
        fetch("/User/currentUser", {
            headers: { "Authorization": `Bearer ${token}` }
        })
        .then(res => res.json())
        .then(user => {
            document.getElementById("username").innerText = user.username;
            loadBakeries();
            loadUserPurchases();
        })
        .catch(() => {
            localStorage.removeItem("token");
            window.location.href = "login.html";
        });
    }
}

// 📦 טעינת מאפים זמינים לרכישה
function loadBakeries() {
    const token = localStorage.getItem("token");

    fetch("/Bakerys", {
        headers: { "Authorization": `Bearer ${token}` }
    })
    .then(res => res.json())
    .then(bakeries => {
        const list = document.getElementById("bakeryList");
        list.innerHTML = "";
        bakeries.forEach(bakery => {
            list.innerHTML += `<li>${bakery.name} 
                <button onclick="buyBakery(${bakery.id})">קנה</button></li>`;
        });
    });
}

// 🛒 קניית מאפה
function buyBakery(bakeryId) {
    const token = localStorage.getItem("token");

    fetch(`/User/buy/${bakeryId}`, {
        method: "POST",
        headers: { "Authorization": `Bearer ${token}` }
    })
    .then(() => {
        alert("✅ נרכש בהצלחה!");
        loadBakeries();
        loadUserPurchases();
    });
}

// 🏷️ טעינת המאפים שנרכשו
function loadUserPurchases() {
    const token = localStorage.getItem("token");

    fetch("/User/purchases", {
        headers: { "Authorization": `Bearer ${token}` }
    })
    .then(res => res.json())
    .then(purchases => {
        const list = document.getElementById("purchasedList");
        list.innerHTML = "";
        purchases.forEach(bakery => {
            list.innerHTML += `<li>${bakery.name} 
                <button onclick="removeBakery(${bakery.id})">הסר</button></li>`;
        });
    });
}

// ❌ מחיקת מאפה שנרכש
function removeBakery(bakeryId) {
    const token = localStorage.getItem("token");

    fetch(`/User/remove/${bakeryId}`, {
        method: "DELETE",
        headers: { "Authorization": `Bearer ${token}` }
    })
    .then(() => {
        alert("✅ הוסר בהצלחה!");
        loadUserPurchases();
    });
}

// 🚪 התנתקות
function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

// ✅ הפעלת טעינה של הנתונים
if (window.location.pathname === "/index.html") {
    checkLogin();
}
