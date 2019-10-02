import { Component } from '@angular/core';

import { UserService } from '../../shared/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  
  constructor(
    public userSvc: UserService
  ) {
  }

  public username: string;
  public password: string;

  login() {
    this.userSvc.login(this.username, this.password).subscribe((resp) => {
      if (resp) {
        this.username = null;
        this.password = null;
      }
    });
  }
}
