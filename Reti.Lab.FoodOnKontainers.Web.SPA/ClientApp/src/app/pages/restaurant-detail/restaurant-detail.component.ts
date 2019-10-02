import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { RestaurantsService } from "../../shared/services/restaurants.service";
import { CartService } from 'src/app/shared/services/cart.service';
import { UserBasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-restaurants-detail',
  templateUrl: './restaurant-detail.component.html',
  styleUrls: ['./restaurant-detail.component.css']
})
export class RestaurantDetailComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private svc: RestaurantsService,
    public cartSvc: CartService
  ) { }

  ngOnInit() {
    this.route.params.subscribe(params => {
        let restaurantId = params['id'];
        this.getRestaurant(restaurantId);
    });
}

  public restaurant: any;

  getRestaurant(restaurantId: string) {
    this.svc.getRestaurantDetail(restaurantId)
      .subscribe(data => {
        this.restaurant = data;
      });
  }

  addToCart(item: any) {
    let newItem = new UserBasketItem();
    newItem.menuItemId = item.id;
    newItem.menuItemName = item.name;
    newItem.price = item.price;
    newItem.quantity = 1;
    this.cartSvc.addToBasket(newItem, this.restaurant.id, this.restaurant.name).subscribe();
  }
}
