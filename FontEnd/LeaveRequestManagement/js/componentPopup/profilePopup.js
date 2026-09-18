import { putApi } from "../api.js";

/* khởi tạo popup chỉnh sửa hồ sơ cá nhân */
export async function initProfilePopup(user) {
    // lấy file html popup
    const response = await fetch("../popups/profilePopup.html");
    // chuyển nội dung HTML thành chuỗi
    const popupHtml = await response.text();
    // gán nội dung HTML vào container
    document.getElementById("profilePopupContainer").innerHTML = popupHtml;

    // lấy các phần tử popup
    const editProfileButton =document.getElementById("editProfileButton");
    const profilePopup =document.getElementById("profilePopup");
    const cancelProfileButton =document.getElementById("cancelProfileButton");
    const closeProfilePopup =document.getElementById("closeProfilePopup");

    // mở popup
    editProfileButton.addEventListener("click", () => {
        const currentUser =JSON.parse(localStorage.getItem("currentUser"));
        if (!currentUser) {
            window.location.href = "../auth/login.html";
            return;
        }
        // Gán thông tin vào các ô input
        document.getElementById("editEmployeeCode").value =currentUser.employeeCode || "";
        document.getElementById("editFullName").value =currentUser.fullName || "";
        document.getElementById("editEmail").value =currentUser.email || "";
        document.getElementById("editPhone").value =currentUser.phone || "";
        document.getElementById("editAddress").value =currentUser.address || "";

        // Hiển thị popup
        profilePopup.classList.add("show");
    });

    // icon đóng popup
    closeProfilePopup.addEventListener("click", () => {
        profilePopup.classList.remove("show");
    });

    // nút hủy
    cancelProfileButton.addEventListener("click", () => {
        profilePopup.classList.remove("show");
    });

    // Lưu thông tin cá nhân được nhập trên popup
    document.getElementById("saveProfileButton")
        .addEventListener("click", async () => {

            // Chỉ gửi ba thông tin được chỉnh sửa
            const request = {
                fullName:document.getElementById("editFullName").value.trim(),
                phone:document.getElementById("editPhone").value.trim(),
                address:document.getElementById("editAddress").value.trim()
            };
            try {
                const result = await putApi(`/users?id=${user.id}`,request);
                if (!result.success) {
                    alert(result.message);
                    return;
                }

                // Cập nhật thông tin đang lưu trên trình duyệt
                Object.assign(user, request);
                localStorage.setItem( "currentUser",JSON.stringify(user));

                // Hiển thị thông tin mới trên trang hồ sơ
                document.getElementById("fullName").textContent =user.fullName;
                document.getElementById("profileFullName").textContent = user.fullName;
                document.getElementById("infoFullName").textContent =user.fullName;
                document.getElementById("infoPhone").textContent =user.phone;
                document.getElementById("infoAddress").textContent =user.address || "Chưa cập nhật";

                // Đóng popup sau khi lưu thành công
                profilePopup.classList.remove("show");
                alert(result.message);
            } catch (error) {
                console.error("Lỗi cập nhật hồ sơ:", error);
            }
        });
}