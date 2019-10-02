import { Component, OnInit } from '@angular/core';

import { BaseComponent } from 'src/app/shared/components/base/base.component';
import { UserService } from 'src/app/shared/services/user.service';
import { OrdersService } from 'src/app/shared/services/orders.service';
import { Role } from 'src/app/shared/models/user';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
})
export class OrdersComponent extends BaseComponent implements OnInit {

  constructor(
    public userSvc: UserService,
    public ordersSvc: OrdersService
  ) {
    super();
  }
  public orders: any = [];

  ngOnInit() {
    if (this.userSvc.user.role === Role.USER) {
      this.ordersSvc.getUserOrders()
        .subscribe(data => {
          this.orders = data;
        });
    }

    if (this.userSvc.user.role === Role.MANAGER) {
      this.ordersSvc.getRestaurantOrders()
        .subscribe(data => {
          this.orders = data;
        });
    }
  }
}
