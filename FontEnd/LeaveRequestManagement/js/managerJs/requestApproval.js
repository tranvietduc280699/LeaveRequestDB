import { getApi } from "../api.js";
// Lấy thông tin người dùng đang đăng nhập
const user = JSON.parse(localStorage.getItem("currentUser"));
if (!user) {
    window.location.href = "../auth/login.html";
} else {
    // Map tên và chức vụ lên giao diện
    document.getElementById("fullName").textContent = user.fullName ?? "";
    document.getElementById("position").textContent = user.position ?? "";
    loadRequests();
}

// Lấy danh sách đơn thuộc phạm vi quản lý
async function loadRequests() {
    try {
        const result = await getApi(`/managerRequest?managerId=${user.id}`);
        if (!result.success) {
            alert(result.message);
            return;
        }
        const requests = result.data || [];
        // Kiểm tra dữ liệu trước khi bind vào bảng
        console.log("Danh sách đơn nghỉ:", requests);
        // Map số lượng theo trạng thái
        const pendingCount = requests.filter(item => Number(item.status) === 1).length;
        document.getElementById("pendingCount").textContent = pendingCount;
        document.getElementById("approvedCount").textContent =
            requests.filter(item => Number(item.status) === 2).length;
        document.getElementById("rejectedCount").textContent =
            requests.filter(item => Number(item.status) === 3).length;
        document.getElementById("requestDescription").textContent =
            `Có ${pendingCount} đơn đang chờ xử lý`;
    } catch (error) {
        console.error("Lỗi tải danh sách đơn:", error);
    }
}
