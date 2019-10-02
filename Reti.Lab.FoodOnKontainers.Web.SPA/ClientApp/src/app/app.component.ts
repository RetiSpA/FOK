import { Component } from '@angular/core';

import { ConfigurationService } from './shared/services/configuration.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {

  constructor(
    private configSvc: ConfigurationService
    ) { 
      this.configSvc.loadConfiguration();
    }
}
