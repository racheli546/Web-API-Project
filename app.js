const apiUrl = 'http://localhost:5025/Bakery';  // כתובת ה-API שלך

// פונקציה לטעינת המאפיות
async function loadBakeries() {
    const response = await fetch(apiUrl);
    const bakeries = await response.json();
    const bakeryList = document.getElementById('bakery-list');
    bakeryList.innerHTML = '';
    bakeries.forEach(bakery => {
        const li = document.createElement('li');
        li.innerHTML = `
    <div>
        ${bakery.name} ${bakery.isItWithChocolate ? "(With Chocolate)" : "(No Chocolate)"}
        <button onclick="editBakery(${bakery.id})">Edit</button>
        <button onclick="deleteBakery(${bakery.id})">Delete</button>
    </div>
`;
        bakeryList.appendChild(li);
    });
}

// פונקציה להוספת מאפה
document.getElementById('add-bakery-form').addEventListener('submit', async (e) => {
    e.preventDefault();
    const name = document.getElementById('bakery-name').value;
    const isWithChocolate = document.getElementById('is-with-chocolate').checked;

    const newBakery = {
        name: name,
        isItWithChocolate: isWithChocolate
    };

    const response = await fetch(apiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(newBakery)
    });

    if (response.ok) {
        loadBakeries();  // עדכון הרשימה לאחר ההוספה
        document.getElementById('bakery-name').value = '';  // ניקוי שדה שם
        document.getElementById('is-with-chocolate').checked = false;  // ניקוי checkbox
    } else {
        alert("Failed to add bakery!");
    }
});

// פונקציה למחיקת מאפה
async function deleteBakery(id) {
    const response = await fetch(`${apiUrl}/${id}`, {
        method: 'DELETE'
    });

    if (response.ok) {
        loadBakeries();  // עדכון הרשימה לאחר המחיקה
    } else {
        alert("Failed to delete bakery!");
    }
}

// פונקציה לעריכת מאפה
async function editBakery(id) {
    const newName = prompt("Enter new bakery name");
    const isWithChocolate = confirm("Is it with chocolate?");

    if (newName) {
        const updatedBakery = {
            id: id,
            name: newName,
            isItWithChocolate: isWithChocolate
        };

        const response = await fetch(`${apiUrl}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedBakery)
        });

        if (response.ok) {
            loadBakeries();  // עדכון הרשימה לאחר העריכה
        } else {
            alert("Failed to update bakery!");
        }
    }
}

// טעינת המאפיות כאשר הדף נטען
loadBakeries();
