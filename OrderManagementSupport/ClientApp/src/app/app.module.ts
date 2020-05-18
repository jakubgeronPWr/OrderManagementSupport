import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { OrderAddComponent } from './orders/order-add/order-add.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavBarComponent } from './shared/nav-bar/nav-bar.component';
import { OrdersListResolver } from './orders/orders-list/orders-list.resolver';
import { HttpClientModule } from '@angular/common/http';
import { OrderPanelComponent } from './orders/orders-list/order-panel/order-panel.component';

@NgModule({
  declarations: [
    AppComponent,
    OrdersListComponent,
    OrderAddComponent,
    NavBarComponent,
    OrderPanelComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule
  ],
  providers: [
    OrdersListResolver
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
