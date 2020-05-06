import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Order } from '../model/order';

@Injectable({
  providedIn: 'root'
})
export class OrdersRestService {

  constructor(private readonly http: HttpClient) {

  }

  findAll(): Observable<Order[]> {
    return this.http.get<Order[]>('/api/orders');
  }

  findBookById(id: string): Observable<Order> {
    return this.http.get<Order>(`/api/orders/${id}`);
  }

  delete(bookId: string): Observable<Order> {
    return this.http.delete<Order>(`/api/orders/${bookId}`);
  }

}
