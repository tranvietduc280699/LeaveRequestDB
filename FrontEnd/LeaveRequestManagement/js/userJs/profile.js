import { getApi } from "../api.js";
import { initProfilePopup } from "../componentPopup/profilePopup.js";

// sét button trên sidebar
const currentUser = JSON.parse(localStorage.getItem("currentUser"));
const dashboardLink = document.getElementById("dashboardLink");
const requestLink = document.getElementById("requestLink");
const managementLink = document.getElementById("managementLink");

if (Number(currentUser.role) === 2) {
    dashboardLink.href = "../manager/managerDashboard.html";
    requestLink.href = "../manager/requestApproval.html";
    requestLink.textContent = "Duyệt đơn nghỉ";
    managementLink.href = "../manager/employeeManagement.html";
    managementLink.textContent = "Quản lý nhân viên";
} else {
    dashboardLink.href = "../employee/dashboard.html";
    requestLink.href = "../employee/leaveRequest.html";
    requestLink.textContent = "Tạo đơn nghỉ";
    managementLink.href = "../employee/myRequest.html";
    managementLink.textContent = "Đơn của tôi";
}

// Lấy thông tin người dùng đã lưu khi đăng nhập
const user = JSON.parse(localStorage.getItem("currentUser"));

// Lấy tên phòng ban theo departmentId
async function loadDepartment() {
    try {
        const result = await getApi("/departments");
        if (!result.success) {
            console.error(result.message);
            return;
        }
        const department = (result.data || []).find(item =>String(item.id) === String(user.departmentId));
        document.getElementById("infoDepartment").textContent =department ? department.name : "Chưa có thông tin";

    } catch (error) {
        console.error("Lỗi tải phòng ban:", error);
    }
}

// Chưa đăng nhập thì chuyển về trang đăng nhập
if (!user) {
    window.location.href = "../auth/login.html";
} else {
    // Thông tin trên header
    document.getElementById("fullName").textContent = user.fullName;
    document.getElementById("position").textContent =user.position || "Chưa cập nhật";

    // Thông tin bên dưới ảnh đại diện
    document.getElementById("profileFullName").textContent = user.fullName;
    document.getElementById("profileEmployeeCode").textContent =user.employeeCode;

    // Thông tin trong các ô hồ sơ
    document.getElementById("infoFullName").textContent = user.fullName;
    document.getElementById("infoEmployeeCode").textContent =user.employeeCode;
    document.getElementById("infoEmail").textContent = user.email;
    document.getElementById("infoPhone").textContent =user.phone || "Chưa cập nhật";
    document.getElementById("infoPosition").textContent =user.position || "Chưa cập nhật";
    document.getElementById("infoAddress").textContent =user.address || "Chưa cập nhật";
    document.getElementById("infoDepartment").textContent ="Chưa có thông tin";

    // Hiển thị trạng thái tài khoản
    const isActive = Number(user.status) === 1;
    const profileStatus = document.getElementById("profileStatus");
    profileStatus.classList.toggle("inactive", !isActive);
    profileStatus.querySelector(".status-text").textContent =
        isActive ? "Đang hoạt động" : "Ngừng hoạt động";

    // Chức vụ
    document.getElementById("accountInformation").textContent =`Chức vụ: ${user.position || "Chưa cập nhật"}`;

    // Vai trò
    const roleNames = {
        1: "Nhân viên",
        2: "Quản lý"
    };
    document.getElementById("accountInformation").textContent =`Vai trò: ${roleNames[Number(user.role)] ?? "Không xác định"}`;

    // Tải tên phòng ban
    loadDepartment();
    // Khởi tạo popup chỉnh sửa hồ sơ
    initProfilePopup(user);
}