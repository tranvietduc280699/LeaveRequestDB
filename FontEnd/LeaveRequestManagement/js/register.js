import { getApi, postApi } from "./api.js";
import User from "./models/userModel.js";

const departmentSelect = document.getElementById("departmentId");

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
loadDepartments();