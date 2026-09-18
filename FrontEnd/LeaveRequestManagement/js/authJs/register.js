import { getApi, postApi } from "../api.js";
import User from "../models/userModel.js";

const departmentSelect = document.getElementById("departmentId");
const registerForm = document.getElementById("registerForm");
/*load departments*/
async function loadDepartments() {
    try {
        const result = await getApi("/departments");
        result.data.forEach(department => {
            // tạo options
            const option = document.createElement("option");
            // lấy id và name của department để tạo option
            option.value = department.id;
            option.textContent = department.name;
            // đưa vào câu select
            departmentSelect.appendChild(option);
        });
    } catch (error) {
        console.error(error);
    }
}
/*đăng ký tài khoản*/
registerForm.addEventListener("submit", async (event) => {
    event.preventDefault();
    const user = new User();
    user.employeeCode = document.getElementById("employeeCode").value;
    user.fullName = document.getElementById("fullName").value;
    user.email = document.getElementById("email").value;
    user.password = document.getElementById("password").value;
    user.phone = document.getElementById("phone").value;
    user.address = document.getElementById("address").value;
    user.position = document.getElementById("position").value;
    user.departmentId = Number(departmentSelect.value);
    try{
        const result = await postApi("/auth/register", user);
         if (result.success) {
            alert("Đăng ký thành công!");
            registerForm.reset();
            window.location.href = "login.html"; // Chuyển hướng đến trang đăng nhập
        }
    }catch (error) {
        alert(error.message);
    }
})  
loadDepartments();