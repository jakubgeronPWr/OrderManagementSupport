import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { OrderAddComponent } from './orders/order-add/order-add.component';
import { OrdersListResolver } from './orders/orders-list/orders-list.resolver';

const routes: Routes = [
  {
    path: 'orders',
    component: OrdersListComponent,
    resolve: {
      orders: OrdersListResolver
    }
  },
  {
    path: 'order',
    component: OrderAddComponent
  }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
