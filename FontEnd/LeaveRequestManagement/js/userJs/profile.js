import { getApi, putApi } from "../api.js";

/*mở popup chỉnh sửa hồ sơ cá nhân*/
const response=await fetch("../popups/profilePopup.html");
const popupHtml=await response.text();   // chuyển đổi nội dung HTML thành chuỗi
document.getElementById("profilePopupContainer").innerHTML=popupHtml; //gán nội dung HTML vào phần tử container

const editProfileButton=document.getElementById("editProfileButton");
const profilePopup=document.getElementById("profilePopup");

editProfileButton.addEventListener("click",()=>{
    profilePopup.classList.add("show");
});

// cancel button và icon đóng popup
const cancelProfileButton=document.getElementById("cancelProfileButton");
const closeProfilePopup = document.getElementById("closeProfilePopup");
closeProfilePopup.addEventListener("click",()=>{
    profilePopup.classList.remove("show");
});
cancelProfileButton.addEventListener("click",()=>{
    profilePopup.classList.remove("show");
});

// sét button trên sidebar
const currentUser=JSON.parse(localStorage.getItem("currentUser"));
const dashboardLink=document.getElementById("dashboardLink");
const requestLink=document.getElementById("requestLink");
const managementLink=document.getElementById("managementLink");

if(Number(currentUser.role)===2){
    dashboardLink.href="../manager/managerDashboard.html";
    requestLink.href="../manager/requestApproval.html";
    requestLink.textContent="Duyệt đơn nghỉ";
    managementLink.href="../manager/employeeManagement.html";
    managementLink.textContent="Quản lý nhân viên";
}else{
    dashboardLink.href="../employee/dashboard.html";
    requestLink.href="../employee/leaveRequest.html";
    requestLink.textContent="Tạo đơn nghỉ";
    managementLink.href="../employee/myRequest.html";
    managementLink.textContent="Đơn của tôi";
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
        const department = (result.data || []).find(item =>
            String(item.id) === String(user.departmentId)
        );
        document.getElementById("infoDepartment").textContent =
            department ? department.name : "Chưa có thông tin";
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
    document.getElementById("position").textContent = user.position || "Chưa cập nhật";
    // Thông tin bên dưới ảnh đại diện
    document.getElementById("profileFullName").textContent = user.fullName;
    document.getElementById("profileEmployeeCode").textContent = user.employeeCode;
    // Thông tin trong các ô hồ sơ
    document.getElementById("infoFullName").textContent = user.fullName;
    document.getElementById("infoEmployeeCode").textContent = user.employeeCode;
    document.getElementById("infoEmail").textContent = user.email;
    document.getElementById("infoPhone").textContent = user.phone || "Chưa cập nhật";
    document.getElementById("infoPosition").textContent = user.position || "Chưa cập nhật";
    document.getElementById("infoAddress").textContent = user.address || "Chưa cập nhật";
    document.getElementById("infoDepartment").textContent = "Chưa có thông tin";
    // Hiển thị trạng thái tài khoản
    const isActive = Number(user.status) === 1;
    const profileStatus = document.getElementById("profileStatus");
    profileStatus.classList.toggle("inactive", !isActive);
    profileStatus.querySelector(".status-text").textContent =
        isActive ? "Đang hoạt động" : "Ngừng hoạt động";
    // Hiển thị vai trò tài khoản
    document.getElementById("accountInformation").textContent =
    `Chức vụ: ${user.position || "Chưa cập nhật"}`;
    // Chức vụ
    const roleNames = {
        1: "Nhân viên",
        2: "Quản lý"
    };
    document.getElementById("accountInformation").textContent =
    `Vai trò: ${roleNames[Number(user.role)] ?? "Không xác định"}`;
    // Tải tên phòng ban
    loadDepartment();
}

// cập nhật thông tin lên form chỉnh sửa hồ sơ cá nhân (popup)
document.getElementById("editProfileButton").addEventListener("click", () => {
    const user = JSON.parse(localStorage.getItem("currentUser"));
    if (!user) {
        window.location.href = "../auth/login.html";
        return;
    }
    // Gán thông tin vào các ô input
    document.getElementById("editEmployeeCode").value = user.employeeCode || "";
    document.getElementById("editFullName").value = user.fullName || "";
    document.getElementById("editEmail").value = user.email || "";
    document.getElementById("editPhone").value = user.phone || "";
    document.getElementById("editAddress").value = user.address || "";
    
    // Hiển thị popup
    document.getElementById("profilePopup").classList.add("show");
});

// Lưu thông tin cá nhân được nhập trên popup
document.getElementById("saveProfileButton").addEventListener("click", async () => {
    
    // Chỉ gửi ba thông tin được chỉnh sửa
    const request = {
        fullName: document.getElementById("editFullName").value.trim(),
        phone: document.getElementById("editPhone").value.trim(),
        address: document.getElementById("editAddress").value.trim()
    };
    try {
        const result = await putApi(`/users?id=${user.id}`, request);
        if (!result.success) {
           alert(result.message);
            return;
        }
        // Cập nhật thông tin đang lưu trên trình duyệt
        Object.assign(user, request);
        localStorage.setItem("currentUser", JSON.stringify(user));
        
        // Hiển thị thông tin mới trên trang hồ sơ
        document.getElementById("fullName").textContent = user.fullName;
        document.getElementById("profileFullName").textContent = user.fullName;
        document.getElementById("infoFullName").textContent = user.fullName;
        document.getElementById("infoPhone").textContent = user.phone;
        document.getElementById("infoAddress").textContent = user.address || "Chưa cập nhật";
        // Đóng popup sau khi lưu thành công
        document.getElementById("profilePopup").classList.remove("show");
        alert(result.message);
    } catch (error) {
        console.error("Lỗi cập nhật hồ sơ:", error);
    }
});
