import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/internal/operators/map';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import { ConfigurationService } from './configuration.service';
import { UserService } from './user.service';
import { ToastService, ToastType } from './toast.service';
import { OrderStatus } from '../models/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  constructor(
    private http: HttpClient,
    private userSvc: UserService,
    private configSvc: ConfigurationService,
    private toastSvc: ToastService
  ) {
   }

   private getApiUrl(action: string) {
    return `${this.configSvc.config['gatewayApiClient']}/api/orders/${action}`;
  }

  getUserOrders() {
    return this.http.get(this.getApiUrl(`user/${this.userSvc.user.id}`))
      .pipe(map((resp: Response) => {
        return resp;
    }));
  }

  getRestaurantOrders() {
    return this.http.get(this.getApiUrl(`restaurant/${this.userSvc.user.restaurantId}`))
      .pipe(map((resp: Response) => {
        return resp;
    }));
  }

  getOrder(orderId: number) {
    return this.http.get(this.getApiUrl(`${orderId}`))
      .pipe(map((resp: Response) => {
        return resp;
    }));
  }

  updateOrderStatus(orderId: number, status: OrderStatus) {
    return this.http.put(this.getApiUrl(`update/order/${orderId}/${status}`), null)
      .pipe(map((resp: Response) => {
        return resp;
    }),
    catchError((err) => {
      this.toastSvc.show("Errore nell'aggiornamento dell'ordine", "Attenzione", ToastType.danger);
      return of(null);
    }));
  }
}
