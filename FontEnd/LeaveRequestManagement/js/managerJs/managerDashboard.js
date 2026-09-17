import { getApi } from "../api.js";
import "../authJs/logout.js";

// lấy thông tin người dùng hiện tại từ localStorage 
const user = JSON.parse(localStorage.getItem("currentUser"));
if (!user) {
    window.location.href = "../auth/login.html";
}else {
     // Hiển thị họ tên và chức vụ
    document.getElementById("fullName").textContent = user.fullName ?? "";
    document.getElementById("position").textContent = user.position ?? "";
    
}

// Lấy danh sách đơn và và thống kê tổng theo trạng thái
async function loadStatistics() {
    try {
        const result = await getApi(`/managerRequest?managerId=${user.id}`);
        if (!result.success) {
            alert(result.message);
            return;
        }
        const requests = result.data || [];
        document.getElementById("pendingCount").textContent =
            requests.filter(item => Number(item.status) === 1).length;
        document.getElementById("approvedCount").textContent =
            requests.filter(item => Number(item.status) === 2).length;
        document.getElementById("rejectedCount").textContent =
            requests.filter(item => Number(item.status) === 3).length;

        // Gọi hiển thị danh sách đơn mới nhất
        renderRecentRequests(requests);
    } catch (error) {
        console.error("Lỗi tải thống kê:", error);
    }
}

// hiển thị đơn mới nhất trên giao diện (3 bản ghi)
function renderRecentRequests(requests) {
    const requestList = document.getElementById("requestList");
    requestList.replaceChildren();
    const recentRequests = requests
        .filter(item => Number(item.status) === 1)
        .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
        .slice(0, 3);
    if (!recentRequests.length) {
        const row = document.createElement("tr");
        const cell = document.createElement("td");
        cell.colSpan = 3;
        cell.className = "empty-data";
        cell.textContent = "Chưa có đơn nghỉ cần xử lý";
        row.appendChild(cell);
        requestList.appendChild(row);
        return;
    }
    recentRequests.forEach(request => {
        const row = document.createElement("tr");
        const values = [
            request.requestCode,
            request.employeeName ?? "Chưa có tên",
            `${formatDate(request.startDate)} - ${formatDate(request.endDate)}`
        ];
        values.forEach(value => {
            const cell = document.createElement("td");
            cell.textContent = value ?? "";
            row.appendChild(cell);
        });
        requestList.appendChild(row);
    });
}
// format thời gian hiển thị
function formatDate(value) {
    if (!value) return "";
    const [year, month, day] = value.split("T")[0].split("-");
    return `${day}/${month}/${year}`;
}

// Khi vào trang tải dữ liệu
loadStatistics();
