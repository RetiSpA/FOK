export class User {
    id: number;
    name: string;
    lastname: string;
    username: string;
    password: string;
    token: string;
    budget: number;
    role: string;
    restaurantId: number;
}

export class Role {
    static MANAGER = 'Manager';
    static USER = 'User';
}