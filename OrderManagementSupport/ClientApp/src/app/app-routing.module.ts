import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { OrdersListComponent } from './orders/orders-list/orders-list.component';
import { OrdersListResolver } from './orders/orders-list/orders-list.resolver';

const routes: Routes = [
  { path: '', redirectTo: '/orders', pathMatch: 'full' },
  {
    path: 'orders',
    component: OrdersListComponent,
    resolve: {
      orders: OrdersListResolver
    }
  }
  ];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
