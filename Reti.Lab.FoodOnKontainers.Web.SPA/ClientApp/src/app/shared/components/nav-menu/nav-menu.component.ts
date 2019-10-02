import { Component } from '@angular/core';

import { BaseComponent } from '../base/base.component';
import { UserService } from 'src/app/shared/services/user.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent extends BaseComponent {

  constructor(
    public userSvc: UserService
  ) {
    super();
  }

  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
