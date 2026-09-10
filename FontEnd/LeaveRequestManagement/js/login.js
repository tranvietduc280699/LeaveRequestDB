import { getApi, postApi } from "./api.js";

/* lấy thông tin form đăng nhập */
const loginForm  = document.getElementById("loginForm");
loginForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;
    try {
        const result = await postApi("/auth/login", { email, password });
         if (!result.success) {
            alert(result.message);
            return;
        }
        if (result.success) {
            const user = result.data;
            localStorage.setItem("currentUser",JSON.stringify(user));

            alert(result.message);

            if (user.role === 1) {
                // Nhân viên
                window.location.href = "./employee/dashboard.html";
            } else if (user.role === 2) {
                // Quản lý
                window.location.href = "./manager/dashboard.html";
            } else {
                alert("Vai trò không hợp lệ");
            }
        }
    } catch (error) {
        alert(error.message);
    }
});
