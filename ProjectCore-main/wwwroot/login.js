document.addEventListener("DOMContentLoaded", function() {
    // בדוק אם יש טופס התחברות
    const loginForm = document.getElementById("loginForm");
    
    loginForm.addEventListener("submit", function(event) {
        event.preventDefault();  // מניעת שליחת הטופס בדרך הרגילה

        // קח את פרטי המשתמש
        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;

        // שלח את פרטי המשתמש לשרת כדי לבדוק את ההתחברות
        fetch("/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ username, password })
        })
        .then(response => response.json())
        .then(data => {
            if (data.token) {
                // אם יש טוקן, שמור אותו ב-localStorage
                localStorage.setItem("token", data.token);

                // בדוק אם המשתמש הוא מנהל לפי הטוקן
                const tokenData = JSON.parse(atob(data.token.split('.')[1]));  // פרק את הטוקן כדי לקבל את המידע הפנימי
                const role = tokenData.role;  // נניח שהתפקיד נמצא בטוקן כ-"role"

                if (role === "Admin") {
                    // אם זה מנהל, הפנה לדף admin.html
                    window.location.href = "admin.html";
                } else {
                    // אם לא מנהל, הפנה לדף רגיל
                    window.location.href = "index.html";
                }
            } else {
                // הצג הודעת שגיאה אם אין טוקן
                alert("ההתחברות נכשלה. נסה שוב.");
            }
        })
        .catch(error => {
            console.error("שגיאה בהתחברות:", error);
            alert("שגיאה בהתחברות. נסה שוב מאוחר יותר.");
        });
    });
});
