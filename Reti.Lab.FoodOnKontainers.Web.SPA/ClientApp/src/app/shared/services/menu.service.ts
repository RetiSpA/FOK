import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

import { ConfigurationService } from './configuration.service';
import { ToastService, ToastType } from './toast.service';

@Injectable({
  providedIn: 'root'
})
export class MenuService {

  constructor(
    private http: HttpClient,
    private configSvc: ConfigurationService,
    private toastSvc: ToastService
  ) { }

  private getApiUrl(action: string) {
    return `${this.configSvc.config['gatewayApiClient']}/api/restaurantsmenu/${action}`;
  }

  updatePrice(item: any) {
    return this.http.put(this.getApiUrl('update'), item)
      .pipe(map((resp: Response) => {
        return resp;
      }),
      catchError((err) => {
        this.toastSvc.show("Errore nell'aggiornamento", "Attenzione", ToastType.danger);
        return of(null);
      }));
  }
}
