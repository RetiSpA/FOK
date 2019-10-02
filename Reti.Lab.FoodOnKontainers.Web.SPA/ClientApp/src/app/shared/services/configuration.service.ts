import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ConfigurationService {
  
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
    ) { }

    public config: any;
    public config$: BehaviorSubject<any> = new BehaviorSubject<any>(null);

    loadConfiguration() {
      this.http.get(`${this.baseUrl}api/Configuration`)
        .subscribe(resp => {
          this.config = resp;
          this.config$.next(this.config);
        });
    }
}
