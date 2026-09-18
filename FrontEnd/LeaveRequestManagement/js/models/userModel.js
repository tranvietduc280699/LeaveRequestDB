export default class User {
    constructor() {
        this.employeeCode = "";
        this.fullName = "";
        this.email = "";
        this.password = "";
        this.phone = "";
        this.address = "";
        this.position = "";
        this.departmentId = null;
        this.role = 1;
        this.status = 1;
        this.createdAt = new Date();
        this.updatedAt = new Date();
    }
}