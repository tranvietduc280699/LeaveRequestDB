// lấy thông tin người dùng hiện tại từ localStorage máp các field vào các phần tử HTML tương ứng
const user = JSON.parse(localStorage.getItem("currentUser"));
if (!user) {
    window.location.href = "../auth/login.html";
}else {
    // xửu lý đăng xuất
    const logoutButton = document.getElementById("logoutButton");
   logoutButton.addEventListener("click", () => {
        localStorage.removeItem("currentUser");
        window.location.href = "../auth/login.html";
    });
}
