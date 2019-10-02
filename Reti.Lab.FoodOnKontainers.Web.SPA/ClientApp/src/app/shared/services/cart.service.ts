import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import { ConfigurationService } from './configuration.service';
import { UserService } from './user.service';
import { UserBasket, UserBasketItem, UserBasketItemToChange, ConfirmBasketDto } from '../models/basket';
import { ToastService, ToastType } from './toast.service';
import { GeoPosition } from '../models/common';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  constructor(
    private http: HttpClient,
    private userSvc: UserService,
    private configSvc: ConfigurationService,
    private toastSvc: ToastService
    ) { }

    public basket: UserBasket;

    private getApiUrl(action: string) {
      return `${this.configSvc.config['gatewayApiClient']}/api/basket/user/${action}`;
    }

    private getApiFlowUrl(action: string) {
      return `${this.configSvc.config['gatewayApiClient']}/api/flow/user/${action}`;
    }

    getBasket() {
      return this.http.get(this.getApiUrl(`${this.userSvc.user.id}`))
        .pipe(map((resp: UserBasket) => {
          if (this.basket && this.basket.basketItems && this.basket.basketItems.length > 0 && resp && resp.basketItems && this.basket.basketItems.length > 0) {
            const localBasketTotal = this.basket.basketItems.reduce((prev, next) => { return prev + (next.quantity * next.price); }, 0);
            const newBasketTotal = resp.basketItems.reduce((prev, next) => { return prev + (next.quantity * next.price); }, 0);
            if (localBasketTotal !== newBasketTotal) {
              this.toastSvc.show("Verifica il carrello, qualcosa potrebbe essere cambiato dall'ultima volta...", "Attenzione", ToastType.info);
            }
          }
          this.basket = resp;
          return resp;
        }),
          catchError((err) => {
            console.log("Empty basket");
            this.basket = new UserBasket(this.userSvc.user.id);
            return of(null);
          })
      );
    }

    addToBasket(userBasketItem: UserBasketItem, restaurantId: number, restaurantName: string) {
      this.basket.restaurantId = restaurantId;
      this.basket.restaurantName = restaurantName;

      let basketItem = this.basket.basketItems.find(el => el.menuItemId === userBasketItem.menuItemId);
      if (basketItem) {        
        return this.http.put(this.getApiUrl('update'), new UserBasketItemToChange(this.basket.userId, basketItem.menuItemId, basketItem.quantity + 1))
            .pipe(map((resp: Response) => {
              basketItem.quantity++;
              return resp;
        }));
      } else {        
        let update = new UserBasket(this.basket.userId);
        update.restaurantId = this.basket.restaurantId;
        update.restaurantName = this.basket.restaurantName;
        update.basketItems.push(userBasketItem)
        return this.http.post(this.getApiUrl('additem'), update)
            .pipe(map((resp: Response) => {
              this.basket.basketItems.push(userBasketItem);
              return resp;
        }));
      }      
    }

    clearBasket() {
      return this.http.delete(this.getApiUrl(`clear/${this.userSvc.user.id}`))
          .pipe(map((resp: Response) => {
            this.basket = new UserBasket(this.userSvc.user.id);
            return resp;
      }));
    }

    confirmBasket(deliveryDate: Date, deliveryAddress: string, deliveryLat: number, deliveryLng: number) {
      let request = new ConfirmBasketDto();
      request.deliveryAddress = deliveryAddress;
      request.deliveryRequestedDate = deliveryDate;
      request.deliveryPosition = new GeoPosition(deliveryLat, deliveryLng);
      request.userId = this.basket.userId;
      request.userName = this.userSvc.user.username;

      return this.http.post(this.getApiFlowUrl("confirmbasket"), request)
          .pipe(map((resp: Response) => {
            this.basket = new UserBasket(this.userSvc.user.id);
            return resp;
      }),
      catchError((err) => {
        this.toastSvc.show("Errore nella creazione dell'ordine", "Attenzione", ToastType.danger);
        return of(null);
      }));
    }
}
