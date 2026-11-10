
// LOGIN FORM
const loginForm = document.getElementById("loginForm");

if (loginForm) {
    loginForm.addEventListener("submit", function (event) {
        event.preventDefault();

        const role = document.getElementById("role").value;

        const dashboards = {
            admin: "dashboards/admin.html",
            hr: "dashboards/hr.html",
            "project-manager": "dashboards/project-manager.html",
            employee: "dashboards/employee.html",
            accountant: "dashboards/accountant.html"
        };

        if (dashboards[role]) {
            window.location.href = dashboards[role];
        } else {
            alert("Please select a role.");
        }
    });
}
