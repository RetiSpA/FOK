import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { BaseComponent } from 'src/app/shared/components/base/base.component';
import { OrdersService } from 'src/app/shared/services/orders.service';
import { UserService } from 'src/app/shared/services/user.service';
import { OrderStatus } from 'src/app/shared/models/order';

@Component({
  selector: 'app-order-detail',
  templateUrl: './order-detail.component.html',
})
export class OrderDetailComponent extends BaseComponent implements OnInit {
  
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private ordersSvc: OrdersService,
    public userSvc: UserService
  ) {
    super();
  }

  public order: any;
  public workableOrder = OrderStatus.Inserito;

  ngOnInit() {
    this.route.params.subscribe(params => {
        let orderId = params['id'];
        this.getOrder(orderId);
    });
  }

  getOrder(orderId: number) {
    this.ordersSvc.getOrder(orderId)
      .subscribe(data => {
        this.order = data;
      });
  }

  reject(orderId: number) {
    this.ordersSvc.updateOrderStatus(orderId, OrderStatus.Rifiutato).subscribe(() => this.router.navigate(["/orders"]));
  }

  accept(orderId: number) {
    this.ordersSvc.updateOrderStatus(orderId, OrderStatus.Accettato).subscribe(() => this.router.navigate(["/orders"]));
  }
}
