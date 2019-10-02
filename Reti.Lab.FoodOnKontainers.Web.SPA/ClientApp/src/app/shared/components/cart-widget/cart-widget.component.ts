import { Component, AfterViewInit } from '@angular/core';
import { filter } from 'rxjs/operators';

import { CartService } from "../../services/cart.service";
import { ConfigurationService } from '../../services/configuration.service';

@Component({
  selector: 'app-cart-widget',
  templateUrl: './cart-widget.component.html',
  styleUrls: ['./cart-widget.component.css']
})
export class CartWidgetComponent implements AfterViewInit {
  constructor(
    private configSvc: ConfigurationService,
    public cartSvc: CartService
    ) { }

    ngAfterViewInit() {
      this.configSvc.config$.pipe(
        filter(conf => conf !== null)
      ).subscribe(() => {
          this.cartSvc.getBasket().subscribe();
      });
    }

    clear() {
      this.cartSvc.clearBasket().subscribe();
    }

    get quantity(): number {
      if (!this.cartSvc.basket) {
        return 0;
      }
      return this.cartSvc.basket.basketItems.reduce((prev, next) => {
        return prev + next.quantity;
      }, 0);
    }

    get total(): number {
      return this.cartSvc.basket.basketItems.reduce((prev, next) => {
        return prev + (next.quantity * next.price);
      }, 0);
    }
}
