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