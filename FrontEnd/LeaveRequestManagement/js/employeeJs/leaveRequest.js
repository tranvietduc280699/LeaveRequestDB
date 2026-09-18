import { getApi, postApi } from "../api.js";
import LeaveRequest from "../models/leaveRequestModel.js";
import "../authJs/logout.js";
// lấy thông tin trên local
const user = JSON.parse(localStorage.getItem("currentUser"));
if(!user){
    window.location.href = "../auth/login.html";
}else{
    // máp thông tin lên giao diện
    document.getElementById("employeeName").textContent = user.fullName;
    document.getElementById("employeeCode").textContent = user.employeeCode;
    document.getElementById("employeePosition").textContent = user.position;
}

// xử lý select thông tin loại nghỉ
var  leaveType = document.getElementById("leaveTypeId");
async function loadLeaveTypes() {
    try{
        const result = await getApi("/leaveTypes");
        result.data.forEach( types =>{
             // tạo option
            const option = document.createElement("option");
            option.value = types.id;
            option.textContent = types.name;
            // sét vào select
            leaveType.appendChild(option);
        })
    }catch (error) {
        console.error(error);
    }
}

// xử lý select người bàn giao
const handoverSelect = document.getElementById("handoverPerson");
async function loadManagers() {
    try {
        const result = await getApi("/users/managers");
        result.data.forEach(manager => {
            const option = document.createElement("option");
            option.value = manager.fullName;
            option.textContent = manager.fullName;
            handoverSelect.appendChild(option);
        });
    } catch (error) {
        console.error("Lỗi tải danh sách người bàn giao:", error);
    }
}


// xử lý số ngày nghỉ trên giao diện
var startDate = document.getElementById("startDate");
var endDate = document.getElementById("endDate");
var numberOfDays = document.getElementById("numberOfDays");

if (startDate && endDate && numberOfDays) {
    function calculateNumberOfDays() {
        // Chưa chọn đủ hai ngày
        if (!startDate.value || !endDate.value) {
            numberOfDays.value = "";
            return;
        }
        // Lấy thời gian theo mili giây
        const start = startDate.valueAsNumber;
        const end = endDate.valueAsNumber;

        // Kiểm tra ngày hợp lệ
        if (!Number.isFinite(start) || !Number.isFinite(end) || end < start) {
            numberOfDays.value = "";
            return;
        }
        // Tính khoảng cách và đổi sang số ngày
        const difference = end - start;
        numberOfDays.value = difference / (24 * 60 * 60 * 1000) + 1;
    }

    startDate.addEventListener("change", calculateNumberOfDays);
    endDate.addEventListener("change", calculateNumberOfDays);

    calculateNumberOfDays();
}

// xử lý lưu đơn nghỉ phép
const submitButton = document.getElementById("submitButton");

submitButton.addEventListener("click", async () => {
    // khởi tạo
    const request = new LeaveRequest();
    // máp dữ liệu
    request.leaveTypeId = Number(document.getElementById("leaveTypeId").value);
    request.startDate =document.getElementById("startDate").value;
    request.endDate =document.getElementById("endDate").value;
    request.reason = document.getElementById("reason").value.trim();
    request.note =document.getElementById("note").value.trim();
    request.handoverPerson =document.getElementById("handoverPerson").value;
    try {
        const result = await postApi(`/employeeRequest?employeeId=${user.id}`, request);
       if (result.success === false) {
            alert(result.message);
            return;
        }
        alert(result.message);
        window.location.href = "../employee/myRequest.html";

    } catch (error) {
        console.error(error);
        alert("Không thể kết nối đến máy chủ");
    }
});

// khởi tạo khi vào trang
loadLeaveTypes()
loadManagers()
