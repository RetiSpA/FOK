import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class GeolocationService {

  constructor(
    private http: HttpClient
  ) { }

  static readonly GEOCODE_APIKEY = 'AIzaSyA2zW8_-b_LuV-qNYLxaNZfjgqOXvemzXQ';
  
  private reverseGeocodingBaseUrl = "https://maps.googleapis.com/maps/api/geocode/json";
  private geolocationBaseUrl = "https://www.googleapis.com/geolocation/v1/geolocate";

  getPosition() {
    return this.http.post(`${this.geolocationBaseUrl}?key=${GeolocationService.GEOCODE_APIKEY}`, null)
      .pipe(map((resp: Response) => {
        return resp;
      }));
  }

  getAddress(lat: number, lng: number) {
    return this.http.get(`${this.reverseGeocodingBaseUrl}?latlng=${lat},${lng}&result_type=street_address&key=${GeolocationService.GEOCODE_APIKEY}`)
      .pipe(map((resp: Response) => {
        return resp;
      }));
  }
}
