import { getApi } from "../api.js";
// lấy thông tin quản lý đang đăng nhập
const user = JSON.parse(localStorage.getItem("currentUser"));
// hàm mở popup chi tiết đơn nghỉ
export async function leaveRequestPopup(requestId) {
    try {
        // gọi api lấy chi tiết đơn nghỉ
        const result = await getApi(`/managerRequest/${requestId}?managerId=${user.id}`);
        if (!result.success) {
            alert(result.message);
            return;
        }
        const request = result.data;
        // bind dữ liệu popup
        document.getElementById("detailRequestCode").textContent = request.requestCode ?? "";
        document.getElementById("detailEmployeeName").textContent = request.employeeName ?? "";
        document.getElementById("detailLeaveType").textContent = request.leaveTypeName ?? "";
        document.getElementById("detailDate").textContent = `${formatDate(request.startDate)} - ${formatDate(request.endDate)}`;
        document.getElementById("detailNumberOfDays").textContent = request.numberOfDays ?? "";
        document.getElementById("detailReason").textContent = request.reason || "Không có";
        document.getElementById("detailNote").textContent = request.note || "Không có";
        document.getElementById("detailHandoverPerson").textContent = request.handoverPerson || "Không có";
        document.getElementById("detailManagerResponse").textContent = request.managerResponse || "Chưa có phản hồi";
        document.getElementById("detailStatus").textContent = getStatusName(request.status);
        document.getElementById("detailCreatedAt").textContent = formatDate(request.createdAt);
        document.getElementById("detailProcessedAt").textContent = request.processedAt ? formatDate(request.processedAt) : "Chưa xử lý";
        // hiển thị popup
        document.getElementById("leaveRequestPopup").style.display = "flex";
    } catch (error) {
        console.error("Lỗi tải chi tiết đơn nghỉ:", error);
        alert("Không tải được chi tiết đơn nghỉ");
    }
}
// lấy tên trạng thái
function getStatusName(status) {
    if (Number(status) === 1) return "Chờ duyệt";
    if (Number(status) === 2) return "Đã duyệt";
    if (Number(status) === 3) return "Đã từ chối";
    if (Number(status) === 4) return "Đã hủy";
    return "Không xác định";
}
// đổi ngày sang dd/MM/yyyy
function formatDate(value) {
    if (!value) return "";
    const [year, month, day] = value.split("T")[0].split("-");
    return `${day}/${month}/${year}`;
}