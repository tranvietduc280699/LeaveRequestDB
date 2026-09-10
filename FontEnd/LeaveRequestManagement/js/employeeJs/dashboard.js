// lấy thông tin người dùng hiện tại từ localStorage
const user = JSON.parse(localStorage.getItem("currentUser"));
if (!user) {
    window.location.href = "../login.html";
}else {
    document.getElementById("fullName").textContent = user.fullName;
    document.getElementById("infoFullName").textContent = user.fullName;
    document.getElementById("employeeCode").textContent = user.employeeCode;
    document.getElementById("position").textContent = user.position;
    document.getElementById("email").textContent = user.email;
    document.getElementById("phone").textContent = user.phone;
    document.getElementById("address").textContent = user.address;
}