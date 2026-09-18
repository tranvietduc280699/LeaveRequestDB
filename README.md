# LeaveRequestManagement

Hệ thống quản lý đơn nghỉ phép dành cho **nhân viên** và **quản lý**.

## Chức năng

### Nhân viên
- Đăng nhập hệ thống.
- Xem trang tổng quan.
- Tạo đơn nghỉ phép.
- Xem danh sách đơn nghỉ của bản thân.
- Lọc đơn theo mã đơn, loại nghỉ và trạng thái.
- Hủy đơn khi đơn đang ở trạng thái chờ duyệt.
- Xem và cập nhật hồ sơ cá nhân.

### Quản lý
- Xem trang tổng quan quản lý.
- Theo dõi số lượng đơn chờ duyệt, đã duyệt và đã từ chối.
- Xem danh sách đơn nghỉ của nhân viên.
- Lọc đơn theo từ khóa, loại nghỉ và trạng thái.
- Xem chi tiết đơn nghỉ.
- Duyệt hoặc từ chối đơn đang chờ xử lý.
- Xem và quản lý danh sách nhân viên.
- Lọc nhân viên theo từ khóa, phòng ban và trạng thái.
- Xem và cập nhật hồ sơ cá nhân.

## Công nghệ sử dụng

### BackEnd
- ASP.NET Core Web API
- C#
- MySQL
- ADO.NET
- MySqlConnector
- Swagger

### FrontEnd
- HTML
- CSS
- JavaScript
- Fetch API
- LocalStorage
- Live Server

## Database

Tên database:

```text
LeaveRequestDB
```

Các bảng chính:

- `DEPARTMENTS`: quản lý phòng ban.
- `USERS`: quản lý tài khoản nhân viên và quản lý.
- `LEAVE_TYPES`: quản lý loại nghỉ phép.
- `LEAVE_REQUESTS`: quản lý đơn xin nghỉ phép.

## Phân quyền

| Giá trị | Vai trò |
|---|---|
| 1 | Nhân viên |
| 2 | Quản lý |

## Trạng thái tài khoản

| Giá trị | Trạng thái |
|---|---|
| 1 | Đang hoạt động |
| 2 | Ngừng hoạt động |

## Trạng thái đơn nghỉ

| Giá trị | Trạng thái |
|---|---|
| 1 | Chờ duyệt |
| 2 | Đã duyệt |
| 3 | Đã từ chối |
| 4 | Đã hủy |

## Cấu trúc project

```text
LeaveRequestManagement
├── BackEnd
│   └── LeaveRequestManagement
│       ├── API
│       │   ├── Controllers
│       │   ├── Authentication
│       │   ├── Program.cs
│       │   └── appsettings.json
│       ├── BL
│       ├── DL
│       └── Model
├── Database
└── FrontEnd
    └── LeaveRequestManagement
        ├── assets
        ├── css
        ├── js
        └── pages
```

## Kiến trúc BackEnd

BackEnd được chia thành các tầng:

```text
Controller
    ↓
Business Logic - BL
    ↓
Data Layer - DL
    ↓
MySQL
```

### API
Nhận request từ FrontEnd và trả response về client.

### BL
Xử lý nghiệp vụ, kiểm tra dữ liệu và trả về `ApiResponse<T>`.

### DL
Thực hiện truy vấn dữ liệu với MySQL.

### Model
Chứa entity, enum, request model và các model dùng chung.

## Response dùng chung

Hệ thống sử dụng cấu trúc response:

```json
{
  "success": true,
  "code": "SUCCESS",
  "message": "Thành công",
  "data": {}
}
```

Model tương ứng:

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
}
```

## Một số API chính

### Đăng nhập

```http
POST /auth/login
```

### Phòng ban

```http
GET /api/departments
```

### Loại nghỉ

```http
GET /api/leaveTypes
```

### Đơn nghỉ của nhân viên

```http
GET /api/employeeRequest
POST /api/employeeRequest
PUT /api/employeeRequest/{id}/cancel
```

### Quản lý đơn nghỉ

```http
GET /api/managerRequest
GET /api/managerRequest/{id}?managerId={managerId}
PUT /api/managerRequest/{id}/process?managerId={managerId}
```

Ví dụ body khi duyệt đơn:

```json
{
  "status": 2,
  "managerResponse": "Đơn nghỉ đã được duyệt"
}
```

Ví dụ body khi từ chối đơn:

```json
{
  "status": 3,
  "managerResponse": "Đơn nghỉ đã bị từ chối"
}
```

### Quản lý nhân viên

```http
GET /api/User/employees
```

Có thể lọc theo:

```text
departmentId
keyword
status
```

Ví dụ:

```http
GET /api/User/employees?departmentId=1&keyword=duc&status=1
```

## Cách chạy project

### 1. Tạo Database

Chạy file SQL trong thư mục:

```text
Database
```

Database được sử dụng:

```text
LeaveRequestDB
```

### 2. Cấu hình BackEnd

Mở solution BackEnd bằng Visual Studio.

Kiểm tra cấu hình kết nối MySQL trong:

```text
appsettings.json
```

Sau đó chạy API.

URL API hiện tại:

```text
https://localhost:7063
```

Swagger:

```text
https://localhost:7063/swagger
```

### 3. Chạy FrontEnd

Mở thư mục FrontEnd bằng Visual Studio Code.

Khuyến nghị sử dụng extension **Live Server**.

Ví dụ FrontEnd chạy tại:

```text
http://127.0.0.1:5500
```

Trong file `api.js`, URL BackEnd được cấu hình:

```javascript
const API_BASE_URL = "https://localhost:7063/api";
```

## Luồng hoạt động

### Nhân viên

```text
Đăng nhập
→ Tổng quan
→ Tạo đơn nghỉ
→ Theo dõi đơn của tôi
→ Hủy đơn nếu đang chờ duyệt
```

### Quản lý

```text
Đăng nhập
→ Tổng quan quản lý
→ Xem danh sách đơn nghỉ
→ Xem chi tiết
→ Duyệt / Từ chối
→ Quản lý nhân viên
```

## Giao diện chính

### Nhân viên
- Tổng quan
- Tạo đơn nghỉ
- Đơn của tôi
- Hồ sơ cá nhân

### Quản lý
- Tổng quan
- Duyệt đơn nghỉ
- Quản lý nhân viên
- Hồ sơ cá nhân

## Ghi chú

- Thông tin người dùng đăng nhập được lưu trong `localStorage` với key:

```text
currentUser
```

- FrontEnd gọi BackEnd thông qua các hàm dùng chung trong `api.js`:
  - `getApi`
  - `postApi`
  - `putApi`

- Quản lý chỉ được duyệt hoặc từ chối đơn đang ở trạng thái `Pending`.

## Tác giả

**TenCuaBan**
