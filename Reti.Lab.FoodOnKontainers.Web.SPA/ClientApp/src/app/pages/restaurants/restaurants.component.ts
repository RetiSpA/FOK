import { Component, OnInit } from '@angular/core';

import { RestaurantsService } from "../../shared/services/restaurants.service";

@Component({
  selector: 'app-restaurants',
  templateUrl: './restaurants.component.html'
})
export class RestaurantsComponent implements OnInit {

  constructor(
    private svc: RestaurantsService
  ) { }

  ngOnInit() {
    this.getRestaurants();
  }

  public restaurants: any = [];

  getRestaurants() {
    this.svc.getRestaurants()
      .subscribe(data => {
        this.restaurants = data;
      });
  }
}
