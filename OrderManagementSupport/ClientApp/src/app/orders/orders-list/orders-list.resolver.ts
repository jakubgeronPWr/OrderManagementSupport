import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Order } from '../../shared/model/order';
import { Observable } from 'rxjs';
import { OrdersRestService } from '../../shared/services/orders-rest.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class OrdersListResolver implements Resolve<Order[] | null> {

  constructor(private readonly ordersService: OrdersRestService) {
  }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Order[] | null> | Order[] | null {
    return this.ordersService.findAll();
  }

}
