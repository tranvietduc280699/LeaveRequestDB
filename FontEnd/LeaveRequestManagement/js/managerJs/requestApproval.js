import { getApi } from "../api.js";
// Lấy thông tin người dùng đang đăng nhập
const user = JSON.parse(localStorage.getItem("currentUser"));

// Map tên và chức vụ lên giao diện
function bindUser() {
    document.getElementById("fullName").textContent = user.fullName ?? "";
    document.getElementById("position").textContent = user.position ?? "";
}
// Tải danh sách loại nghỉ vào bộ lọc
async function loadLeaveTypes() {
    try {
        const result = await getApi("/leaveTypes");
        if (!result.success) {
            alert(result.message);
            return;
        }
        const leaveTypeFilter = document.getElementById("leaveTypeFilter");
        // Giữ option "Tất cả loại nghỉ"
        leaveTypeFilter.length = 1;
        (result.data || []).forEach(type => {
            const option = document.createElement("option");
            option.value = type.id;
            option.textContent = type.name;
            leaveTypeFilter.appendChild(option);
        });
    } catch (error) {
        console.error("Lỗi tải loại nghỉ:", error);
    }
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
        // Map số lượng theo trạng thái
        const pendingCount = requests.filter(item => Number(item.status) === 1).length;
        document.getElementById("pendingCount").textContent = pendingCount;
        document.getElementById("approvedCount").textContent =
            requests.filter(item => Number(item.status) === 2).length;
        document.getElementById("rejectedCount").textContent =
            requests.filter(item => Number(item.status) === 3).length;
        document.getElementById("requestDescription").textContent =
            `Có ${pendingCount} đơn đang chờ xử lý`;
        // Map danh sách đơn vào bảng
        renderRequests(requests);
    } catch (error) {
        console.error("Lỗi tải danh sách đơn:", error);
    }
}

// Map dữ liệu đơn vào bảng
function renderRequests(requests) {
    const tableBody = document.getElementById("requestTableBody");
    const leaveTypeFilter = document.getElementById("leaveTypeFilter");
    tableBody.replaceChildren();
    document.getElementById("emptyRequest").hidden = requests.length > 0;
    const statuses = {
        1: { name: "Chờ duyệt", className: "pending" },
        2: { name: "Đã duyệt", className: "approved" },
        3: { name: "Đã từ chối", className: "rejected" },
        4: { name: "Đã hủy", className: "cancelled" }
    };
    requests.forEach(request => {
        const row = document.createElement("tr");
        // Tạo ô và thêm vào cuối dòng
        function addCell(text) {
            const cell = document.createElement("td");
            cell.textContent = text ?? "";
            row.appendChild(cell);
            return cell;
        }
        
        // Cột mã đơn
        addCell(request.requestCode);
        
        // Cột nhân viên
        const employeeCell = addCell("");
        const employee = document.createElement("div");
        employee.className = "employee";
        const information = document.createElement("div");
        const name = document.createElement("div");
        name.className = "employee-name";
        name.textContent = request.employeeName ?? "Chưa có tên";
        const code = document.createElement("div");
        code.className = "employee-code";
        code.textContent = request.employeeCode ?? "";
        information.append(name, code);
        employee.appendChild(information);
        employeeCell.appendChild(employee);
        
        // Cột loại nghỉ: lấy tên từ API hoặc option đã tải
        const typeOption = Array.from(leaveTypeFilter.options)
            .find(option => option.value === String(request.leaveTypeId));
        addCell(request.leaveTypeName || typeOption?.textContent || "Không xác định");
        
        // Cột thời gian nghỉ
        const dateCell = addCell(formatDate(request.startDate));
        const endDate = document.createElement("div");
        endDate.className = "end-date";
        endDate.textContent = `đến ${formatDate(request.endDate)}`;
        dateCell.appendChild(endDate);
        
        // Cột số ngày
        addCell(request.numberOfDays);
        
        // Cột lý do
        const reasonCell = addCell("");
        const reason = document.createElement("div");
        reason.className = "reason";
        reason.textContent = request.reason ?? "";
        reason.title = request.reason ?? "";
        reasonCell.appendChild(reason);
        
        // Cột trạng thái
        const statusCell = addCell("");
        const status = statuses[request.status];
        const badge = document.createElement("div");
        badge.className = `status ${status?.className ?? ""}`;
        badge.textContent = status?.name ?? "Không xác định";
        statusCell.appendChild(badge);
        
        // Cột thao tác
        const actionCell = addCell("");
        const actions = document.createElement("div");
        actions.className = "actions";
        // Nút chi tiết hiển thị ở mọi trạng thái
        const detailButton = document.createElement("button");
        detailButton.type = "button";
        detailButton.className = "detail-button";
        detailButton.textContent = "Chi tiết";
        detailButton.dataset.id = request.id;
        actions.appendChild(detailButton);
        // Chỉ cho duyệt hoặc từ chối đơn đang chờ duyệt
        if (Number(request.status) === 1) {
            const approveButton = document.createElement("button");
            approveButton.type = "button";
            approveButton.className = "approve-button";
            approveButton.textContent = "Duyệt";
            approveButton.dataset.id = request.id;
            const rejectButton = document.createElement("button");
            rejectButton.type = "button";
            rejectButton.className = "reject-button";
            rejectButton.textContent = "Từ chối";
            rejectButton.dataset.id = request.id;
            actions.append(approveButton, rejectButton);
        }
        actionCell.appendChild(actions);
    });
}
// Định dạng ngày thành dd/MM/yyyy
function formatDate(value) {
    if (!value) return "";
    const [year, month, day] = value.split("T")[0].split("-");
    return `${day}/${month}/${year}`;
}

// Gọi các hàm khi vào trang
if (!user) {
    window.location.href = "../auth/login.html";
} else {
    bindUser();
    await loadLeaveTypes();
    await loadRequests();
}
