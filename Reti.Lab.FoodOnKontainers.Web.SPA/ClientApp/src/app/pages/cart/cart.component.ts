import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { CartService } from 'src/app/shared/services/cart.service';
import { ToastService, ToastType } from 'src/app/shared/services/toast.service';
import { GeolocationService } from 'src/app/shared/services/geolocation.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  constructor(
    public cartSvc: CartService,
    private toastSvc: ToastService,
    private mapsSvc: GeolocationService,
    private router: Router
  ) { }

  time = { hour: 0, minute: 0 };
  address: string;
  // Coordinate Reti default
  lat: number = 45.6085428;
  lng: number = 8.8478331;

  ngOnInit() {
    this.cartSvc.getBasket().subscribe();
    const now = new Date();
    this.time.hour = now.getHours() + 1;
    this.time.minute = now.getMinutes();

    this.mapsSvc.getPosition()
      .subscribe((resp: any) => {
        if (resp && resp.location && resp.location.lat && resp.location.lng) {
          this.lat = resp.location.lat;
          this.lng = resp.location.lng;
          this.getAddress();
        }
      });
  }

  getAddress() {
    this.mapsSvc.getAddress(this.lat, this.lng).subscribe((resp: any) => {
      if (resp && resp.results && resp.results && resp.results.length > 0 && resp.results[0].formatted_address)
        this.address = resp.results[0].formatted_address;
    })
  }

  updateAddress(event) {
    this.lat = event.coords.lat;
    this.lng = event.coords.lng;
    this.getAddress();
  }

  confirm() {
    let deliveryDate = new Date();
    deliveryDate.setHours(this.time.hour, this.time.minute);
    this.cartSvc.confirmBasket(deliveryDate, this.address, this.lat, this.lng).subscribe((res) => {
      if (res) {
        this.toastSvc.show("Ordine partito, verifica lo stato nel riepilogo ordini", "", ToastType.success);
        this.router.navigate(["/orders"]);
      }
    });
  }
}
