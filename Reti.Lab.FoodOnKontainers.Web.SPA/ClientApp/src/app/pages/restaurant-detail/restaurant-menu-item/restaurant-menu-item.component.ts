import { Component, Input, Output, EventEmitter } from '@angular/core';

import { BaseComponent } from 'src/app/shared/components/base/base.component';
import { CartService } from 'src/app/shared/services/cart.service';
import { UserService } from 'src/app/shared/services/user.service';
import { MenuService } from 'src/app/shared/services/menu.service';

@Component({
  selector: 'app-restaurant-menu-item',
  templateUrl: './restaurant-menu-item.component.html',
  styleUrls: ['./restaurant-menu-item.component.css']
})
export class RestaurantMenuItemComponent extends BaseComponent {
    @Input() item: any;
    @Output() addCart = new EventEmitter<any>();

    constructor(
      public cartSvc: CartService,
      public userSvc: UserService,
      private menuSvc: MenuService
      ) {
        super();
       }

    addToCart(item) {
      this.addCart.emit(item);        
    }

    updateItem(item) {
      this.menuSvc.updatePrice(item).subscribe();
    }
}
