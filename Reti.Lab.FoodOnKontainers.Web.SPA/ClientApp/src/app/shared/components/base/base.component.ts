import { Role } from "../../models/user";

export abstract class BaseComponent {
    USER = Role.USER;
    MANAGER = Role.MANAGER;
}