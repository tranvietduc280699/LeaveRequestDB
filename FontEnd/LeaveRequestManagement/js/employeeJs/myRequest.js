import { getApi, putApi } from "../api.js";
// Lấy thông tin người dùng đang đăng nhập
const user = JSON.parse(localStorage.getItem("currentUser"));
if (!user) {
    window.location.href = "../auth/login.html";
} else {
    // Gán tên và chức vụ vào header
    document.getElementById("fullName").textContent = user.fullName;
    document.getElementById("position").textContent = user.position;
}


// xử lý select thông tin loại nghỉ
var  leaveTypeFilter = document.getElementById("leaveTypeFilter");
async function loadLeaveTypes() {
    try{
        const result = await getApi("/leaveTypes");
        result.data.forEach( types =>{
             // tạo option
            const option = document.createElement("option");
            option.value = types.id;
            option.textContent = types.name;
            // sét vào select
            leaveTypeFilter.appendChild(option);
        })
    }catch (error) {
        console.error(error);
    }
}

// xử lý khi vào trang và tìm kiếm bộ lọc
// Lấy các phần tử trên giao diện
const requestTableBody = document.getElementById("requestTableBody");
const emptyRequest = document.getElementById("emptyRequest");
const keyword = document.getElementById("keyword");
const statusFilter = document.getElementById("statusFilter");
// Lấy danh sách đơn theo bộ lọc hiện tại
async function loadRequests() {
    try {
        // Mỗi lần gọi mới lấy giá trị đang chọn
        const params = new URLSearchParams();
        params.set("employeeId", user.id);
        if (keyword.value.trim()) params.set("requestCode", keyword.value.trim());
        if (leaveTypeFilter.value) params.set("leaveTypeId", leaveTypeFilter.value);
        if (statusFilter.value) params.set("status", statusFilter.value);
        // Gọi API một lần để lấy danh sách
        const result = await getApi(`/employeeRequest?${params.toString()}`);
        if (!result.success) {
            alert(result.message);
            return;
        }
        const requests = result.data || [];
        // Xóa dữ liệu cũ trước khi vẽ lại bảng
        requestTableBody.replaceChildren();
        emptyRequest.style.display = requests.length ? "none" : "block";
        requests.forEach(request => {
            // Tìm tên loại nghỉ và trạng thái từ các option
            const typeOption = Array.from(leaveTypeFilter.options)
                .find(option => option.value === String(request.leaveTypeId));
            const statusOption = Array.from(statusFilter.options)
                .find(option => option.value === String(request.status));
            const row = document.createElement("tr");
            const values = [
                request.requestCode,
                request.leaveTypeName || typeOption?.textContent || "Không xác định",
                `${formatDate(request.startDate)} - ${formatDate(request.endDate)}`,
                request.numberOfDays,
                formatDate(request.createdAt),
                statusOption?.textContent || "Không xác định"
            ];
            // Gán dữ liệu vào từng cột
            values.forEach(value => {
                const cell = document.createElement("td");
                cell.textContent = value ?? "";
                row.appendChild(cell);
            });
            requestTableBody.appendChild(row);

            // Tạo cột thao tác bên trong vòng lặp
            const actionCell = document.createElement("td");
            if (Number(request.status) === 1) {
                const cancelButton = document.createElement("button");
                cancelButton.type = "button";
                cancelButton.className = "cancel-request-button";
                cancelButton.textContent = "Hủy đơn";
                cancelButton.dataset.id = request.id;
                actionCell.appendChild(cancelButton);
                
                // hủy đơn nghỉ phép trên bản ghi
                cancelButton.addEventListener("click", async () => {
                    try {
                        // Khóa nút trong lúc gửi yêu cầu
                        cancelButton.disabled = true;
                        const result = await putApi(
                            `/employeeRequest/${request.id}/cancel?employeeId=${user.id}`,
                            {}
                        );
                        if (!result.success) {
                            alert(result.message);
                            return;
                        }
                        alert(result.message);
                        // Tải lại bảng để hiển thị trạng thái mới
                        await loadRequests();
                    } catch (error) {
                        console.error("Lỗi hủy đơn:", error);
                        alert(error.message || "Không thể hủy đơn nghỉ phép");
                    } finally {
                        cancelButton.disabled = false;
                    }
                });
            }
            row.appendChild(actionCell);
            requestTableBody.appendChild(row);
        });
    } catch (error) {
        console.error("Lỗi tải đơn nghỉ:", error);
        alert("Không tải được danh sách đơn nghỉ phép");
    }
}
// Đổi ngày từ API sang dd/MM/yyyy
function formatDate(value) {
    if (!value) return "";
    const [year, month, day] = value.split("T")[0].split("-");
    return `${day}/${month}/${year}`;
}
// Bấm Tìm kiếm mới gọi API theo bộ lọc
document.getElementById("searchButton").addEventListener("click", () => {
    loadRequests();
});
// Đặt lại chỉ xóa lựa chọn, bấm Tìm kiếm để tải lại
document.getElementById("resetButton").addEventListener("click", () => {
    keyword.value = "";
    leaveTypeFilter.value = "";
    statusFilter.value = "";
});


// khi vào trang
 loadLeaveTypes();
 loadRequests()