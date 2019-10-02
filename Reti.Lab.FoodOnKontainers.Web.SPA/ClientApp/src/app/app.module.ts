import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgbPopoverModule, NgbToastModule, NgbTimepickerModule } from '@ng-bootstrap/ng-bootstrap';
import { AgmCoreModule } from '@agm/core';

import { AppComponent } from './app.component';

import { Role } from './shared/models/user';

import { RoleGuard } from './shared/guards/role.guard';
import { AuthGuard } from './shared/guards/auth.guard';

import { LoaderInterceptor } from './shared/components/loader/loader.interceptor';

import { OrderStatusPipe } from "./shared/pipes/order-status.pipe";

import { NavMenuComponent } from './shared/components/nav-menu/nav-menu.component';
import { LoaderComponent } from "./shared/components/loader/loader.component";
import { CartWidgetComponent } from "./shared/components/cart-widget/cart-widget.component";
import { OrdersTableComponent } from "./shared/components/orders-table/orders-table.component";
import { ToastComponent } from "./shared/components/toast/toast.component";
import { MapComponent } from "./shared/components/map/map.component";

import { HomeComponent } from './pages/home/home.component';
import { RestaurantsComponent } from "./pages/restaurants/restaurants.component";
import { RestaurantDetailComponent } from "./pages/restaurant-detail/restaurant-detail.component";
import { RestaurantMenuItemComponent } from "./pages/restaurant-detail/restaurant-menu-item/restaurant-menu-item.component";
import { CartComponent } from "./pages/cart/cart.component";
import { OrdersComponent } from "./pages/orders/orders.component";
import { OrderDetailComponent } from "./pages/order-detail/order-detail.component";

const AgmCoreAPIKEY = "AIzaSyDNbKfhiALgMO1yM996VZWAwoLZEoCj6-8";

@NgModule({
  declarations: [
    OrderStatusPipe,
    AppComponent,
    NavMenuComponent,
    LoaderComponent,
    OrdersTableComponent,
    ToastComponent,
    MapComponent,
    HomeComponent,
    CartWidgetComponent,
    RestaurantsComponent,
    RestaurantDetailComponent,
    RestaurantMenuItemComponent,
    CartComponent,
    OrdersComponent,
    OrderDetailComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'restaurants', component: RestaurantsComponent, canActivate: [AuthGuard, RoleGuard], data:{ roles: [Role.USER] } },
      { path: 'restaurant/:id', component: RestaurantDetailComponent, canActivate: [AuthGuard, RoleGuard], data:{ roles: [Role.USER, Role.MANAGER] } },
      { path: 'cart', component: CartComponent, canActivate: [AuthGuard, RoleGuard], data:{ roles: [Role.USER] } },
      { path: 'orders', component: OrdersComponent, canActivate: [AuthGuard, RoleGuard], data:{ roles: [Role.USER, Role.MANAGER] } },
      { path: 'order/:id', component: OrderDetailComponent, canActivate: [AuthGuard, RoleGuard], data:{ roles: [Role.USER, Role.MANAGER] } },
    ]),
    NgbPopoverModule,
    NgbToastModule,
    NgbTimepickerModule,
    AgmCoreModule.forRoot({
      apiKey: AgmCoreAPIKEY
    })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: LoaderInterceptor, multi: true },
    AuthGuard,
    RoleGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
