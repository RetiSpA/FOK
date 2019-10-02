import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

import { ConfigurationService } from './configuration.service';

@Injectable({
  providedIn: 'root'
})
export class RestaurantsService {

  constructor(
    private http: HttpClient,
    private configSvc: ConfigurationService
  ) { }

  private getApiUrl(action: string) {
    return `${this.configSvc.config['gatewayApiClient']}/api/restaurants/${action}`;
  }

  getRestaurants() {
    return this.http.get(this.getApiUrl('all'))
      .pipe(map((resp: Response) => {
        return resp;
      }));
  }

  getRestaurantDetail(restaurantId: string) {
    return this.http.get(this.getApiUrl(`detail/${restaurantId}`))
      .pipe(map((resp: Response) => {
        return resp;
      }));
  }
}
