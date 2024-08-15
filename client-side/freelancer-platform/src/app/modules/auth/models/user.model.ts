export class User {
    userName: string = '';
    userId: string = '';
    role: string = '';

    constructor(userName: string, userId: string, role: string) {
        this.userName = userName;
        this.userId = userId;
        this.role = role;
    }
}