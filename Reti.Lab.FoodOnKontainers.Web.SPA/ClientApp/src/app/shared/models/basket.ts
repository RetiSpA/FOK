import { GeoPosition } from "./common";

export class UserBasket {
    userId: number;
    restaurantId: number;
    restaurantName: string;
    basketItems: UserBasketItem[];

    constructor(userId: number) {
        this.userId = userId;
        this.basketItems = new Array<UserBasketItem>();
    }
}

export class UserBasketItem {
    menuItemId: number;
    menuItemName: string;
    quantity: number;
    price: number;
    available: boolean;
}

export class UserBasketItemToChange {
    id: number;
    itemId: number;
    quantity: number;

    constructor(id, itemId, quantity) {
        this.id = id;
        this.itemId = itemId;
        this.quantity = quantity;
    }
}

export class ConfirmBasketDto {
    userId: number;
    userName: string;
    deliveryAddress: string;
    deliveryPosition: GeoPosition;
    deliveryRequestedDate: Date;
}