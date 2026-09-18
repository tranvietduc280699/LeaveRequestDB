import { getApi } from "../api.js";
import "../authJs/logout.js";

// Lấy thông tin người dùng đang đăng nhập
const user = JSON.parse(localStorage.getItem("currentUser"));

if (!user) {
    window.location.href = "../auth/login.html";
} else {
    // Gán tên và chức vụ vào header
    document.getElementById("fullName").textContent = user.fullName;
    document.getElementById("position").textContent = user.position;
}


// xử lý select phòng ban
var departmentFilter = document.getElementById("departmentFilter");
async function loadDepartments() {
    try {
        const result = await getApi("/departments");
        if (!result.success) {
            alert(result.message);
            return;
        }
        result.data.forEach(department => {
            // tạo option
            const option = document.createElement("option");
            option.value = department.id;
            option.textContent = department.name;

            // set vào select
            departmentFilter.appendChild(option);
        });

    } catch (error) {
        console.error("Lỗi tải phòng ban:", error);
    }
}


// xử lý khi vào trang và tìm kiếm bộ lọc
// Lấy các phần tử trên giao diện
const employeeTableBody = document.getElementById("employeeTableBody");
const emptyEmployee = document.getElementById("emptyEmployee");
const keyword = document.getElementById("keyword");
const statusFilter = document.getElementById("statusFilter");


// Lấy danh sách nhân viên theo bộ lọc hiện tại
async function loadEmployees() {
    try {
        // Mỗi lần gọi mới lấy giá trị đang chọn
        const params = new URLSearchParams();
        if (keyword.value.trim()) {
            params.set("keyword", keyword.value.trim());
        }

        if (departmentFilter.value) {
            params.set("departmentId", departmentFilter.value);
        }

        if (statusFilter.value) {
            params.set("status", statusFilter.value);
        }
        // Gọi API lấy danh sách nhân viên
        const result = await getApi(`/User/employees${params.toString() ? `?${params.toString()}` : ""}`);
        if (!result.success) {
            alert(result.message);
            return;
        }
        const employees = result.data || [];

        // thống kê tổng nhân viên
        document.getElementById("employeeCount").textContent =employees.length;

        // thống kê nhân viên đang hoạt động
        document.getElementById("activeCount").textContent =employees.filter(employee => Number(employee.status) === 1).length;

        // thống kê nhân viên ngừng hoạt động
        document.getElementById("inactiveCount").textContent = employees.filter(employee => Number(employee.status) === 2).length;

        // Xóa dữ liệu cũ 
        employeeTableBody.replaceChildren();

        // Bind danh sách nhân viên
        employees.forEach(employee => {
            const row = document.createElement("tr");

            // Tạo ô nhân viên
            const employeeCell = document.createElement("td");
            const employeeInfo = document.createElement("div");
            employeeInfo.className = "employee";
            const avatar = document.createElement("img");
            avatar.src = "../../assets/images/cute.jpg";
            avatar.alt = "Ảnh nhân viên";

            const employeeName = document.createElement("div");
            employeeName.className = "employee-name";
            employeeName.textContent = employee.fullName ?? "";

            employeeInfo.appendChild(avatar);
            employeeInfo.appendChild(employeeName);

            employeeCell.appendChild(employeeInfo);
            row.appendChild(employeeCell);

            // Các dữ liệu còn lại
            const values = [
                employee.employeeCode,
                employee.email,
                employee.phone,
                employee.position,
                employee.departmentName
            ];

            // Gán dữ liệu vào từng cột
            values.forEach(value => {
                const cell = document.createElement("td");
                cell.textContent = value ?? "";
                row.appendChild(cell);
            });

            // Tạo cột trạng thái
            const statusCell = document.createElement("td");
            const status = document.createElement("div");
            if (Number(employee.status) === 1) {
                status.className = "status active";
                status.textContent = "Đang hoạt động";
            } else if (Number(employee.status) === 2) {
                status.className = "status inactive";
                status.textContent = "Ngừng hoạt động";
            }
            statusCell.appendChild(status);
            row.appendChild(statusCell);

            // Tạo cột thao tác
            const actionCell = document.createElement("td");
            const actions = document.createElement("div");
            actions.className = "actions";

            /*
                Phần thao tác phát triển sau

                const detailButton = document.createElement("button");
                detailButton.type = "button";
                detailButton.className = "profile-button";
                detailButton.textContent = "Chi tiết";
                detailButton.dataset.id = employee.id;

                actions.appendChild(detailButton);
            */

            actionCell.appendChild(actions);
            row.appendChild(actionCell);

            // Thêm dòng vào bảng
            employeeTableBody.appendChild(row);
        });

    } catch (error) {
        console.error("Lỗi tải danh sách nhân viên:", error);
        alert("Không tải được danh sách nhân viên");
    }
}

// Bấm Tìm kiếm mới gọi API theo bộ lọc
document.getElementById("searchButton").addEventListener("click", () => {
    loadEmployees();
});

// Đặt lại chỉ xóa lựa chọn, bấm Tìm kiếm để tải lại
document.getElementById("resetButton").addEventListener("click", () => {
    keyword.value = "";
    departmentFilter.value = "";
    statusFilter.value = "";
});

// khi vào trang
loadDepartments();
loadEmployees();